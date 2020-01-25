using System;
using System.Collections.Generic;
using System.Linq;
using Memoyed.Cards.Domain.CardBoxes;
using Memoyed.Cards.Domain.CardBoxSets;
using Memoyed.Cards.Domain.LearningCards;
using Memoyed.Cards.Domain.RevisionSessions.SessionCards;
using Memoyed.Cards.Domain.Shared;

namespace Memoyed.Cards.Domain.RevisionSessions
{
    public class RevisionSession
    {
        private readonly List<SessionCard> _sessionCards;
        private readonly CardBoxSet _set;

        /// <summary>
        /// Creates a revision session with cards from the cardBox specified in the arguments
        /// </summary>
        /// <param name="id">Id of the revision session</param>
        /// <param name="cardBoxSet">Set of card boxes for which the session is created</param>
        /// <param name="cardBoxId">ID of the card box for which the session is created</param>
        /// <param name="now">Current time in UTC</param>
        public RevisionSession(RevisionSessionId id, CardBoxSet cardBoxSet, CardBoxId cardBoxId, UtcTime now)
            : this(id, cardBoxSet, GetSessionCardsFromSet(id, now, cardBoxSet, cardBoxId))
        {
        }

        /// <summary>
        /// Creates a revision session with cards from the entire set (which are ready for revision)
        /// </summary>
        /// <param name="id">Id of the revision session</param>
        /// <param name="cardBoxSet">Set of card boxes for which the session is created</param>
        /// <param name="now">Current time in UTC</param>
        public RevisionSession(RevisionSessionId id, CardBoxSet cardBoxSet, UtcTime now)
            : this(id, cardBoxSet, GetSessionCardsFromSet(id, now, cardBoxSet))
        {
        }

        private RevisionSession(RevisionSessionId id, CardBoxSet cardBoxSet, List<SessionCard> sessionCards)
        {
            if (sessionCards.Count == 0)
            {
                throw new DomainException.NoCardsForRevisionException();
            }
            
            Id = id;
            _set = cardBoxSet;
            _sessionCards = sessionCards;
        }

        public RevisionSessionId Id { get; }
        public IEnumerable<SessionCard> SessionCards => _sessionCards;
        public RevisionSessionStatus Status { get; private set; }

        public void CardAnswered(LearningCardId cardId, AnswerType answerType, string word)
        {
            var sessionCard = _sessionCards.FirstOrDefault(sc => sc.LearningCardId == cardId);
            if (sessionCard == null)
            {
                throw new DomainException.SessionCardNotFoundException();
            }

            var correctWord = answerType switch
            {
                AnswerType.NativeLanguage => sessionCard.NativeLanguageWord,
                AnswerType.TargetLanguage => sessionCard.TargetLanguageWord,
                _ => throw new ArgumentException("Unknown AnswerType")
            };
            
            // TODO: Maybe do a more light check
            sessionCard.Status = correctWord != word
                ? SessionCardStatus.AnsweredWrong
                : SessionCardStatus.AnsweredCorrectly;
        }

        public void CompleteSession(UtcTime? now = null)
        {
            if (Status == RevisionSessionStatus.Completed)
            {
                throw new DomainException.SessionAlreadyCompletedException();
            }
            var cardIdsByStatus = _sessionCards
                .GroupBy(sc => sc.Status)
                .ToDictionary(k => k.Key,
                    v => v.Select(c => c.LearningCardId)
                    .ToList());
            if (cardIdsByStatus.ContainsKey(SessionCardStatus.NotAnswered))
            {
                throw new DomainException.NotAllCardsAnsweredException();
            }

            if (cardIdsByStatus.TryGetValue(SessionCardStatus.AnsweredCorrectly, out var cardsToPromote))
            {
                foreach (var card in cardsToPromote)
                {
                    _set.PromoteCard(card, now);
                }
            }

            if (cardIdsByStatus.TryGetValue(SessionCardStatus.AnsweredWrong, out var cardsToDemote))
            {
                foreach (var card in cardsToDemote)
                {
                    _set.DemoteCard(card, now);
                }
            }

            Status = RevisionSessionStatus.Completed;
        }

        private static List<SessionCard> GetSessionCardsFromSet(RevisionSessionId sessionId, DateTime now,
            CardBoxSet set, CardBoxId boxId = null)
        {
            var cardsForRevision = new List<LearningCard>();
            var boxes = set.CardBoxes;
            if (boxId != null)
            {
                var singleBox = boxes.FirstOrDefault(b => b.Id == boxId);
                if (singleBox == null)
                {
                    throw new DomainException.CardBoxNotFoundInSetException();
                }
                
                boxes = new List<CardBox> {singleBox};
            }
            
            foreach (var box in boxes)
            {
                cardsForRevision.AddRange(box.LearningCards
                    .Where(lc => lc.CardBoxChangedDate != null
                                 && lc.CardBoxChangedDate.Value.AddDays(box.RevisionDelay) < now));
            }

            return cardsForRevision
                .Select(c => new SessionCard(sessionId, c))
                .ToList();
        }
    }
}
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

        /// <summary>
        /// Creates a revision session with cards from the cardBox specified in the arguments
        /// </summary>
        /// <param name="id">Id of the revision session</param>
        /// <param name="cardBoxSet">Set of card boxes for which the session is created</param>
        /// <param name="cardBoxId">ID of the card box for which the session is created</param>
        /// <param name="now">Current time in UTC</param>
        public RevisionSession(RevisionSessionId id, CardBoxSet cardBoxSet, CardBoxId cardBoxId, UtcTime now)
        {
            Id = id;
            _sessionCards = GetSessionCardsFromSet(now, cardBoxSet, cardBoxId);
        }


        /// <summary>
        /// Creates a revision session with cards from the entire set (which are ready for revision)
        /// </summary>
        /// <param name="id">Id of the revision session</param>
        /// <param name="cardBoxSet">Set of card boxes for which the session is created</param>
        /// <param name="now">Current time in UTC</param>
        public RevisionSession(RevisionSessionId id, CardBoxSet cardBoxSet, UtcTime now)
        {
            Id = id;
            _sessionCards = GetSessionCardsFromSet(now, cardBoxSet);
        }

        public RevisionSessionId Id { get; }
        public IEnumerable<SessionCard> SessionCards => _sessionCards;

        public void CardAnswered(LearningCardId cardId, AnswerType targetLanguage, string word)
        {
            throw new System.NotImplementedException();
        }

        public void CompleteSession()
        {
            throw new System.NotImplementedException();
        }

        private List<SessionCard> GetSessionCardsFromSet(DateTime now, CardBoxSet set, CardBoxId boxId = null)
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
                .Select(c => new SessionCard(Id, c))
                .ToList();
        }
    }
}
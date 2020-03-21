using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Memoyed.Cards.Domain.CardBoxes;
using Memoyed.Cards.Domain.CardBoxSets;
using Memoyed.Cards.Domain.Cards;
using Memoyed.Cards.Domain.RevisionSessions.SessionCards;
using Memoyed.Cards.Domain.Shared;
using Memoyed.DomainFramework;

namespace Memoyed.Cards.Domain.RevisionSessions
{
    public class RevisionSession : AggregateRoot
    {

        internal RevisionSession(RevisionSessionId id, CardBoxSetId cardBoxSetId, List<SessionCard> sessionCards)
        {
            if (sessionCards.Count == 0)
            {
                throw new DomainException.NoCardsForRevisionException();
            }
            
            Id = id;
            CardBoxSetId = cardBoxSetId;
            _sessionCards = sessionCards;
        }

        private RevisionSession()
        {
        }

        public RevisionSessionId Id { get; }
        public CardBoxSetId CardBoxSetId { get; }
        public ReadOnlyCollection<SessionCard> SessionCards => _sessionCards.AsReadOnly();
        private readonly List<SessionCard> _sessionCards;
        public RevisionSessionStatus Status { get; private set; }

        public void CardAnswered(CardId cardId, AnswerType answerType, string word)
        {
            var sessionCard = _sessionCards.FirstOrDefault(sc => sc.CardId == cardId);
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
                    v => v.Select(c => c.CardId)
                    .ToList());
            
            if (cardIdsByStatus.ContainsKey(SessionCardStatus.NotAnswered))
            {
                throw new DomainException.NotAllCardsAnsweredException();
            }

            Status = RevisionSessionStatus.Completed;

            if (!cardIdsByStatus.TryGetValue(SessionCardStatus.AnsweredCorrectly, out var answeredCorrectlyCards))
            {
                answeredCorrectlyCards = new List<CardId>();
            }
            
            if (!cardIdsByStatus.TryGetValue(SessionCardStatus.AnsweredWrong, out var answeredWrongCards))
            {
                answeredWrongCards = new List<CardId>();
            }
            
            EventPublisher.Publish(new RevisionSessionEvents.RevisionSessionCompleted(Id, CardBoxSetId,
                answeredCorrectlyCards, answeredWrongCards, now ?? new UtcTime(DateTime.UtcNow)));
        }
    }
}
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Memoyed.Domain.Cards.CardBoxSets;
using Memoyed.Domain.Cards.Cards;
using Memoyed.Domain.Cards.RevisionSessions.SessionCards;
using Memoyed.Domain.Cards.Shared;
using Memoyed.DomainFramework;

namespace Memoyed.Domain.Cards.RevisionSessions
{
    public class RevisionSession : AggregateRoot
    {
        private readonly List<SessionCard> _sessionCards;

        internal RevisionSession(RevisionSessionId id, CardBoxSetId cardBoxSetId, List<SessionCard> sessionCards)
        {
            if (sessionCards.Count == 0) throw new DomainException.NoCardsForRevisionException();

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
        public RevisionSessionStatus Status { get; private set; }

        public void CardAnsweredCorrectly(CardId cardId)
        {
            CardAnswered(cardId, SessionCardStatus.AnsweredCorrectly);
        }

        public void CardAnsweredWrong(CardId cardId)
        {
            CardAnswered(cardId, SessionCardStatus.AnsweredWrong);
        }

        private void CardAnswered(CardId cardId, SessionCardStatus status)
        {
            var sessionCard = _sessionCards.FirstOrDefault(sc => sc.CardId == cardId);
            if (sessionCard == null) throw new DomainException.SessionCardNotFoundException();

            if (sessionCard.Status != SessionCardStatus.NotAnswered)
                throw new DomainException.CardAlreadyAnsweredException();

            sessionCard.Status = status;
        }

        public void CompleteSession(UtcTime? now = null)
        {
            if (Status == RevisionSessionStatus.Completed) throw new DomainException.SessionAlreadyCompletedException();

            var cardIdsByStatus = _sessionCards
                .GroupBy(sc => sc.Status)
                .ToDictionary(k => k.Key,
                    v => v.Select(c => c.CardId)
                        .ToList());

            if (cardIdsByStatus.ContainsKey(SessionCardStatus.NotAnswered))
                throw new DomainException.NotAllCardsAnsweredException();

            Status = RevisionSessionStatus.Completed;
        }
    }
}
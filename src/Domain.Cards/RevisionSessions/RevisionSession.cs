using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Memoyed.Domain.Cards.CardBoxSets;
using Memoyed.Domain.Cards.Cards;
using Memoyed.Domain.Cards.RevisionSessions.SessionCards;
using Memoyed.Domain.Cards.Services;
using Memoyed.DomainFramework;

namespace Memoyed.Domain.Cards.RevisionSessions
{
    public class RevisionSession : AggregateRoot
    {
        private readonly List<SessionCard> _sessionCards = new List<SessionCard>();

        internal RevisionSession(Guid id, Guid cardBoxSetId, List<SessionCard> sessionCards)
        {
            if (sessionCards.Count == 0) throw new DomainException.NoCardsForRevisionException();

            Id = id;
            CardBoxSetId = cardBoxSetId;
            _sessionCards = sessionCards;
        }

        private RevisionSession()
        {
        }

        public Guid Id { get; }
        public Guid CardBoxSetId { get; }
        public IReadOnlyCollection<SessionCard> SessionCards => _sessionCards.AsReadOnly();
        public RevisionSessionStatus Status { get; private set; }

        public void CardAnswered(Guid cardId, SessionCardAnswerType answerType, string answer,
            ICardAnswerCheckService answerCheckService)
        {
            var card = SessionCards.FirstOrDefault(sc => sc.CardId == cardId);
            if (card == null)
            {
                throw new InvalidOperationException("Couldn't find a card with the id in the revision session");
            }

            CardAnswered(cardId, answerCheckService.CheckAnswer(
                answerType == SessionCardAnswerType.NativeLanguage ? card.NativeLanguageWord : card.TargetLanguageWord,
                answer)
                ? SessionCardStatus.AnsweredCorrectly
                : SessionCardStatus.AnsweredWrong);
        }

        private void CardAnswered(Guid cardId, SessionCardStatus status)
        {
            var sessionCard = _sessionCards.FirstOrDefault(sc => sc.CardId == cardId);
            if (sessionCard == null) throw new DomainException.SessionCardNotFoundException();

            if (sessionCard.Status != SessionCardStatus.NotAnswered)
                throw new DomainException.CardAlreadyAnsweredException();

            sessionCard.Status = status;
        }

        public void CompleteSession(DateTimeOffset? now = null)
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
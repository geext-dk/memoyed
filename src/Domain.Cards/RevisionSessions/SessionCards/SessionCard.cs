using System;
using Memoyed.Domain.Cards.Cards;
using Memoyed.DomainFramework;

namespace Memoyed.Domain.Cards.RevisionSessions.SessionCards
{
    public class SessionCard : Entity
    {
        public SessionCard(Guid sessionId, Card card)
        {
            SessionId = sessionId;
            CardId = card.Id;
            NativeLanguageWord = card.NativeLanguageWord;
            TargetLanguageWord = card.TargetLanguageWord;
            Status = SessionCardStatus.NotAnswered;
        }

        private SessionCard()
        {
        }

        public Guid SessionId { get; }
        public Guid CardId { get; }
        public SessionCardStatus Status { get; internal set; }
        public CardWord NativeLanguageWord { get; }
        public CardWord TargetLanguageWord { get; }
    }
}
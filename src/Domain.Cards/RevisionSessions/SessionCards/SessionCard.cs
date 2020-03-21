using Memoyed.Domain.Cards.Cards;
using Memoyed.DomainFramework;

namespace Memoyed.Domain.Cards.RevisionSessions.SessionCards
{
    public class SessionCard : Entity
    {
        public SessionCard(RevisionSessionId sessionId, Card card)
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

        public RevisionSessionId SessionId { get; }
        public CardId CardId { get; }
        public SessionCardStatus Status { get; internal set; }
        public CardWord NativeLanguageWord { get; }
        public CardWord TargetLanguageWord { get; }
    }
}
using Memoyed.Cards.Domain.LearningCards;

namespace Memoyed.Cards.Domain.RevisionSessions.SessionCards
{
    public class SessionCard
    {
        public RevisionSessionId RevisionSessionId { get; }
        public LearningCardId LearningCardId { get; }
        public SessionCardStatus Status { get; }
    }
}
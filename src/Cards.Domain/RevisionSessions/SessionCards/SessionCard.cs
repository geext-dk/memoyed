using Memoyed.Cards.Domain.LearningCards;
using Memoyed.DomainFramework;

namespace Memoyed.Cards.Domain.RevisionSessions.SessionCards
{
    public class SessionCard : Entity
    {
        public SessionCard(RevisionSessionId sessionId, LearningCard card)
        {
            SessionId = sessionId;
            LearningCardId = card.Id;
            NativeLanguageWord = card.NativeLanguageWord;
            TargetLanguageWord = card.TargetLanguageWord;
            Status = SessionCardStatus.NotAnswered;
        }

        private SessionCard()
        {
        }

        public RevisionSessionId SessionId { get; }
        public LearningCardId LearningCardId { get; }
        public SessionCardStatus Status { get; internal set; }
        public LearningCardWord NativeLanguageWord { get; }
        public LearningCardWord TargetLanguageWord { get; }
    }
}
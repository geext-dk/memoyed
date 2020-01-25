using Memoyed.Cards.Domain.LearningCards;

namespace Memoyed.Cards.Domain.RevisionSessions.SessionCards
{
    public class SessionCard
    {
        public SessionCard(RevisionSessionId sessionId, LearningCard card)
        {
            SessionId = sessionId;
            LearningCardId = card.Id;
            NativeLanguageWord = card.NativeLanguageWord;
            TargetLanguageWord = card.TargetLanguageWord;
            Status = SessionCardStatus.NotAnswered;
        }
        
        public RevisionSessionId SessionId { get; }
        public LearningCardId LearningCardId { get; }
        public SessionCardStatus Status { get; }
        public LearningCardWord NativeLanguageWord { get; }
        public LearningCardWord TargetLanguageWord { get; }
    }
}
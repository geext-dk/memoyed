using Memoyed.DomainFramework;

namespace Memoyed.Cards.Domain.LearningCards
{
    public class LearningCardComment : DomainValue<string>
    {
        public LearningCardComment(string comment)
        {
            Value = comment?.Trim();
        }
    }
}
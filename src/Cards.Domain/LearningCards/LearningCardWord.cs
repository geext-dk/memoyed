using Memoyed.DomainFramework;

namespace Memoyed.Cards.Domain.LearningCards
{
    public class LearningCardWord : DomainValue<string>
    {
        public LearningCardWord(string value)
        {
            value = value?.Trim();
            if (string.IsNullOrEmpty(value))
            {
                throw new DomainException.EmptyWordException();
            }

            Value = value;
        }
    }
}
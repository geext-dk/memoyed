using Memoyed.DomainFramework;

namespace Memoyed.Cards.Domain.LearningCards
{
    public class LearningCardWord : DomainValue<string>
    {
        public LearningCardWord(string word)
        {
            var value = word?.Trim();
            if (string.IsNullOrEmpty(value))
            {
                throw new DomainException.EmptyWordException();
            }

            Value = value;
        }
    }
}
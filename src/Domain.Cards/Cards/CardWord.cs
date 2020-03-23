using Memoyed.DomainFramework;

namespace Memoyed.Domain.Cards.Cards
{
    public class CardWord : DomainValue<string>
    {
        public CardWord(string value)
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
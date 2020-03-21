using Memoyed.DomainFramework;

namespace Memoyed.Cards.Domain.Cards
{
    public class CardComment : DomainValue<string>
    {
        public CardComment(string value)
        {
            Value = value?.Trim();
        }
    }
}
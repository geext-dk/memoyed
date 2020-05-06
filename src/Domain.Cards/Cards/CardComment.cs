using Memoyed.DomainFramework;

namespace Memoyed.Domain.Cards.Cards
{
    public class CardComment : DomainValue<string?>
    {
        public CardComment(string? value)
        {
            Value = value?.Trim();
        }
    }
}
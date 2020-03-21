using Memoyed.DomainFramework;

namespace Memoyed.Domain.Cards.CardBoxes
{
    public class CardBoxLevel : OrderedDomainValue<int>
    {
        public CardBoxLevel(int value)
        {
            if (value < 0) throw new DomainException.InvalidCardBoxLevelException();

            Value = value;
        }

        protected override int Position => Value;
    }
}
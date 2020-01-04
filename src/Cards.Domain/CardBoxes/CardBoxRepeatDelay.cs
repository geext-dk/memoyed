using Memoyed.DomainFramework;

namespace Memoyed.Cards.Domain.CardBoxes
{
    public class CardBoxRepeatDelay : OrderedDomainValue<int>
    {
        public CardBoxRepeatDelay(int value)
        {
            if (value < 1 || value > 30)
            {
                throw new DomainException.InvalidRepeatDelayException();
            }

            Value = value;
        }

        protected override int Position => Value;
    }
}
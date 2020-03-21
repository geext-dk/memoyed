using Memoyed.DomainFramework;

namespace Memoyed.Cards.Domain.CardBoxes
{
    public class CardBoxRevisionDelay : OrderedDomainValue<int>
    {
        public CardBoxRevisionDelay(int value)
        {
            if (value < 1 || value > 30)
            {
                throw new DomainException.InvalidRevisionDelayException();
            }

            Value = value;
        }

        protected override int Position => Value;
    }
}
using Memoyed.DomainFramework;

namespace Memoyed.Domain.Cards.CardBoxes
{
    public class CardBoxRevisionDelay : OrderedDomainValue<int>
    {
        public CardBoxRevisionDelay(int delay)
        {
            if (delay < 1 || delay > 30)
            {
                throw new DomainException.InvalidRevisionDelayException();
            }

            Delay = delay;
        }

        public int Delay { get; }

        protected override int Position => Delay;
    }
}
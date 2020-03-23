using Memoyed.DomainFramework;

namespace Memoyed.Domain.Cards.CardBoxes
{
    public class CardBoxLevel : OrderedDomainValue<int>
    {
        public CardBoxLevel(int level)
        {
            if (level < 0)
            {
                throw new DomainException.InvalidCardBoxLevelException();
            }

            Level = level;
        }

        protected override int Position => Level;
        public int Level { get; }
    }
}
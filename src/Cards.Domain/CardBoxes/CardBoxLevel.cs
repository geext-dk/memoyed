using System;
using Memoyed.DomainFramework;

namespace Memoyed.Cards.Domain.CardBoxes
{
    public class CardBoxLevel : OrderedDomainValue<int>
    {
        public CardBoxLevel(int value)
        {
            if (value < 0)
            {
                throw new DomainException.InvalidCardBoxLevelException();
            }

            Value = value;
        }

        protected override int Position => Value;
    }
}
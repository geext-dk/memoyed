using System;
using Memoyed.DomainFramework;

namespace Memoyed.Domain.Cards.CardBoxes
{
    public class CardBoxId : DomainValue<Guid>
    {
        public CardBoxId(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new DomainException.EmptyIdException();
            }

            Value = value;
        }
    }
}
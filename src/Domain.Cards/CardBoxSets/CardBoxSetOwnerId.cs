using System;
using Memoyed.DomainFramework;

namespace Memoyed.Domain.Cards.CardBoxSets
{
    public class CardBoxSetOwnerId : DomainValue<Guid>
    {
        public CardBoxSetOwnerId(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new DomainException.EmptyIdException();
            }

            Value = value;
        }
    }
}
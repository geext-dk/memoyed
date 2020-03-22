using System;
using Memoyed.DomainFramework;

namespace Memoyed.Domain.Cards.CardBoxSets
{
    public class CardBoxSetId : DomainValue<Guid>
    {
        public CardBoxSetId(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new DomainException.EmptyIdException();
            }

            Value = value;
        }
    }
}
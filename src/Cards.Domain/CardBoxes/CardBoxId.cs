using System;
using Memoyed.DomainFramework;

namespace Memoyed.Cards.Domain.CardBoxes
{
    public class CardBoxId : DomainValue<Guid>
    {
        public CardBoxId(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new DomainException.EmptyIdException();
            }

            Value = id;
        }
    }
}
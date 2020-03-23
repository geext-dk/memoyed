using System;
using Memoyed.DomainFramework;

namespace Memoyed.Domain.Cards.CardBoxes
{
    public class CardBoxId : DomainValue
    {
        public CardBoxId(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new DomainException.EmptyIdException();
            }

            Id = id;
        }
        
        public Guid Id { get; }
    }
}
using System;
using Memoyed.DomainFramework;

namespace Memoyed.Domain.Cards.CardBoxSets
{
    public class CardBoxSetOwnerId : DomainValue
    {
        public CardBoxSetOwnerId(Guid id)
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
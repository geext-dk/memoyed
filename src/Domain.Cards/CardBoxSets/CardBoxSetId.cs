using System;
using Memoyed.DomainFramework;

namespace Memoyed.Domain.Cards.CardBoxSets
{
    public class CardBoxSetId : DomainValue
    {
        public CardBoxSetId(Guid id)
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
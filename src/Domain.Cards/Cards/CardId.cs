using System;
using Memoyed.DomainFramework;

namespace Memoyed.Domain.Cards.Cards
{
    public class CardId : DomainValue
    {
        public CardId(Guid id)
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
using System;
using Memoyed.DomainFramework;

namespace Memoyed.Domain.Cards.Cards
{
    public class CardId : DomainValue<Guid>
    {
        public CardId(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new DomainException.EmptyIdException();
            }

            Value = value;
        }
    }
}
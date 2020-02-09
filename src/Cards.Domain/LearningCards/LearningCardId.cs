using System;
using Memoyed.DomainFramework;

namespace Memoyed.Cards.Domain.LearningCards
{
    public class LearningCardId : DomainValue<Guid>
    {
        public LearningCardId(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new DomainException.EmptyIdException();
            }

            Value = value;
        }
    }
}
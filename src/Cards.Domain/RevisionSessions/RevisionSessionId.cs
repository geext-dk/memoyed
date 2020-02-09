using System;
using Memoyed.DomainFramework;

namespace Memoyed.Cards.Domain.RevisionSessions
{
    public class RevisionSessionId : DomainValue<Guid>
    {
        public RevisionSessionId(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new DomainException.EmptyIdException();
            }

            Value = value;
        }
    }
}
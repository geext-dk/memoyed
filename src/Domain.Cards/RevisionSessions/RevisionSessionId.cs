using System;
using Memoyed.DomainFramework;

namespace Memoyed.Domain.Cards.RevisionSessions
{
    public class RevisionSessionId : DomainValue<Guid>
    {
        public RevisionSessionId(Guid value)
        {
            if (value == Guid.Empty) throw new DomainException.EmptyIdException();

            Value = value;
        }
    }
}
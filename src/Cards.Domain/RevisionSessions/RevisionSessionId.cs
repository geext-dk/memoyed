using System;
using Memoyed.DomainFramework;

namespace Memoyed.Cards.Domain.RevisionSessions
{
    public class RevisionSessionId : DomainValue<Guid>
    {
        public RevisionSessionId(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new DomainException.EmptyIdException();
            }

            Value = id;
        }
    }
}
using System;
using Memoyed.DomainFramework;

namespace Memoyed.Domain.Cards.RevisionSessions
{
    public class RevisionSessionId : DomainValue
    {
        public RevisionSessionId(Guid id)
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
using System;
using Memoyed.DomainFramework;

namespace Memoyed.Domain.Cards.RevisionSessions.SessionCards
{
    public class SessionCardId : DomainValue
    {
        public SessionCardId(Guid id)
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
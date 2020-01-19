using System;
using Memoyed.DomainFramework;

namespace Memoyed.Cards.Domain.RevisionSessions.SessionCards
{
    public class SessionCardId : DomainValue<Guid>
    {
        public SessionCardId(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new DomainException.EmptyIdException();
            }

            Value = id;
        }
    }
}
using System;
using Memoyed.DomainFramework;

namespace Memoyed.Domain.Cards.RevisionSessions.SessionCards
{
    public class SessionCardId : DomainValue<Guid>
    {
        public SessionCardId(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new DomainException.EmptyIdException();
            }

            Value = value;
        }
    }
}
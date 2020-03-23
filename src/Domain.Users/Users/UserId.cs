using System;
using Memoyed.DomainFramework;

namespace Memoyed.Domain.Users.Users
{
    public class UserId : DomainValue
    {
        public UserId(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new InvalidOperationException();
            }

            Id = id;
        }
        
        public Guid Id { get; }
    }
}
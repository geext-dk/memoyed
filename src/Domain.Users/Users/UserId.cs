using System;
using Memoyed.DomainFramework;

namespace Memoyed.Domain.Users
{
    public class UserId : DomainValue<Guid>
    {
        public UserId(Guid value)
        {
            if (value == Guid.Empty) throw new InvalidOperationException();

            Value = value;
        }
    }
}
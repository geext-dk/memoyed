using System;
using Memoyed.DomainFramework;

namespace Memoyed.Domain.Users.Users
{
    public class User : AggregateRoot
    {
        public Guid Id { get; set; }
    }
}
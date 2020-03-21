using Memoyed.DomainFramework;

namespace Memoyed.Domain.Users.Users
{
    public class User : AggregateRoot
    {
        public UserId Id { get; set; }
    }
}
using Memoyed.DomainFramework;

namespace Users.Domain.Users
{
    public class User : AggregateRoot
    {
        public UserId Id { get; set; }
    }
}
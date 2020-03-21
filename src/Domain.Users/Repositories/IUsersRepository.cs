using System.Threading.Tasks;
using Memoyed.Domain.Users.Users;

namespace Memoyed.Domain.Users.Repositories
{
    public interface IUsersRepository
    {
        Task<User> Get(UserId id);
        void AddNew(User user);
    }
}
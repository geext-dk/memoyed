using System.Threading.Tasks;

namespace Memoyed.DomainFramework
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}
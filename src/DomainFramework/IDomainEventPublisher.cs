using System.Threading.Tasks;

namespace Memoyed.DomainFramework
{
    public interface IDomainEventPublisher
    {
        Task Publish<T>(T @event) where T : class;
    }
}
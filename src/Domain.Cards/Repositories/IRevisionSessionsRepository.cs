using System.Threading.Tasks;
using Memoyed.Cards.Domain.RevisionSessions;

namespace Memoyed.Cards.Domain.Repositories
{
    public interface IRevisionSessionsRepository
    {
        Task<RevisionSession> Get(RevisionSessionId id);
        void AddNew(RevisionSession revisionSession);
    }
}
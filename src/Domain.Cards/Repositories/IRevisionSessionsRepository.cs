using System;
using System.Threading.Tasks;
using Memoyed.Domain.Cards.RevisionSessions;

namespace Memoyed.Domain.Cards.Repositories
{
    public interface IRevisionSessionsRepository
    {
        Task<RevisionSession> Get(Guid id);
        void AddNew(RevisionSession revisionSession);
    }
}
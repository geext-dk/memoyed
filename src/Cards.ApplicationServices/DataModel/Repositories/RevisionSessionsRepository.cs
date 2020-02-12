using System.Threading.Tasks;
using Memoyed.Cards.Domain.Repositories;
using Memoyed.Cards.Domain.RevisionSessions;
using Microsoft.EntityFrameworkCore;

namespace Memoyed.Cards.ApplicationServices.DataModel.Repositories
{
    public class RevisionSessionsRepository : IRevisionSessionsRepository
    {
        private readonly CardsContext _db;

        public RevisionSessionsRepository(CardsContext db)
        {
            _db = db;
        }

        public async Task<RevisionSession> Get(RevisionSessionId id)
        {
            return await _db.RevisionSessions
                .Include(s => s.SessionCards)
                .FirstOrDefaultAsync(s => s.Id == id)
                .ConfigureAwait(false);
        }

        public Task AddNew(RevisionSession revisionSession)
        {
            _db.RevisionSessions.Add(revisionSession);

            return Task.CompletedTask;
        }
    }
}
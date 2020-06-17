using System;
using System.Threading.Tasks;
using Memoyed.Domain.Cards.Repositories;
using Memoyed.Domain.Cards.RevisionSessions;
using Microsoft.EntityFrameworkCore;

namespace Memoyed.Application.EntityFramework.Repositories
{
    public class RevisionSessionsRepository : IRevisionSessionsRepository
    {
        private readonly CardsContext _db;

        public RevisionSessionsRepository(CardsContext db)
        {
            _db = db;
        }

        public async Task<RevisionSession> Get(Guid id)
        {
            return await _db.RevisionSessions
                .Include(s => s.SessionCards)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public void AddNew(RevisionSession revisionSession)
        {
            _db.RevisionSessions.Add(revisionSession);
        }
    }
}
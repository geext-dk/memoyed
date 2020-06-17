using System;
using System.Threading.Tasks;
using Memoyed.Domain.Cards.CardBoxSets;
using Memoyed.Domain.Cards.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Memoyed.Application.EntityFramework.Repositories
{
    public class CardBoxSetsRepository : ICardBoxSetsRepository
    {
        private readonly CardsContext _cardsContext;

        public CardBoxSetsRepository(CardsContext cardsContext)
        {
            _cardsContext = cardsContext;
        }

        public async Task<CardBoxSet> Get(Guid id)
        {
            return await _cardsContext.CardBoxSets
                .Include(s => s.CardBoxes)
                .ThenInclude(b => b.Cards)
                .Include(s => s.CompletedRevisionSessionIds)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public void AddNew(CardBoxSet set)
        {
            _cardsContext.CardBoxSets.Add(set);
        }
    }
}
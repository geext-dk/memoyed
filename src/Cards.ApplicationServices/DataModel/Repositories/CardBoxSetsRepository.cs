using System.Threading.Tasks;
using Memoyed.Cards.Domain.CardBoxSets;
using Memoyed.Cards.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Memoyed.Cards.ApplicationServices.DataModel.Repositories
{
    public class CardBoxSetsRepository : ICardBoxSetsRepository
    {
        private readonly CardsContext _cardsContext;

        public CardBoxSetsRepository(CardsContext cardsContext)
        {
            _cardsContext = cardsContext;
        }

        public async Task<CardBoxSet> Get(CardBoxSetId id)
        {
            return await _cardsContext.CardBoxSets
                .FirstOrDefaultAsync(c => c.Id == id)
                .ConfigureAwait(false);
        }

        public Task AddNew(CardBoxSet set)
        {
            _cardsContext.CardBoxSets.Add(set);
            return Task.CompletedTask;
        }
    }
}
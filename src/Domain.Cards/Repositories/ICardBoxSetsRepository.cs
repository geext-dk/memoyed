using System.Threading.Tasks;
using Memoyed.Cards.Domain.CardBoxSets;

namespace Memoyed.Cards.Domain.Repositories
{
    public interface ICardBoxSetsRepository
    {
        Task<CardBoxSet> Get(CardBoxSetId id);
        Task AddNew(CardBoxSet set);
    }
}
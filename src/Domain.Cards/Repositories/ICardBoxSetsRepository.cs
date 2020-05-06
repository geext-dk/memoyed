using System;
using System.Threading.Tasks;
using Memoyed.Domain.Cards.CardBoxSets;

namespace Memoyed.Domain.Cards.Repositories
{
    public interface ICardBoxSetsRepository
    {
        Task<CardBoxSet> Get(Guid id);
        void AddNew(CardBoxSet set);
    }
}
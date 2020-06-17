using System;
using System.Threading.Tasks;
using Memoyed.Application.EntityFramework;
using Memoyed.Domain.Cards.Repositories;
using Memoyed.DomainFramework;

namespace Memoyed.WebApi.DataModel
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly CardsContext _cardsContext;

        public UnitOfWork(CardsContext cardsContext)
        {
            _cardsContext = cardsContext;
        }


        public async Task Commit()
        {
            await _cardsContext.SaveChangesAsync();
        }
    }
}
using System;
using System.Threading.Tasks;
using Memoyed.Application.DataModel;
using Memoyed.Application.DataModel.Repositories;
using Memoyed.Domain.Cards.Repositories;

namespace Memoyed.Application
{
    public sealed class UnitOfWork
    {
        private readonly Lazy<ICardBoxSetsRepository> _cardBoxSetsRepository;
        private readonly CardsContext _cardsContext;
        private readonly Lazy<IRevisionSessionsRepository> _revisionSessionsRepository;

        public UnitOfWork(CardsContext cardsContext)
        {
            _cardsContext = cardsContext;
            _cardBoxSetsRepository = new Lazy<ICardBoxSetsRepository>(
                () => new CardBoxSetsRepository(_cardsContext));
            _revisionSessionsRepository = new Lazy<IRevisionSessionsRepository>(
                () => new RevisionSessionsRepository(_cardsContext));
        }

        public ICardBoxSetsRepository CardBoxSetsRepository => _cardBoxSetsRepository.Value;
        public IRevisionSessionsRepository RevisionSessionsRepository => _revisionSessionsRepository.Value;

        public async Task Commit()
        {
            await _cardsContext.SaveChangesAsync();
        }
    }
}
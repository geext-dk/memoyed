using System;
using System.Threading.Tasks;
using Memoyed.Cards.ApplicationServices.DataModel;
using Memoyed.Cards.ApplicationServices.DataModel.Repositories;
using Memoyed.Cards.Domain.Repositories;

namespace Memoyed.Cards.ApplicationServices
{
    public sealed class UnitOfWork
    {
        private readonly CardsContext _cardsContext;
        private readonly Lazy<ICardBoxSetsRepository> _cardBoxSetsRepository;
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
            await _cardsContext.SaveChangesAsync()
                .ConfigureAwait(false);
        }
    }
}
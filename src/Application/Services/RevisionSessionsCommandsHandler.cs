using System.Linq;
using System.Threading.Tasks;
using Memoyed.Application.Dto;
using Memoyed.Domain.Cards.Cards;
using Memoyed.Domain.Cards.RevisionSessions;
using Memoyed.Domain.Cards.RevisionSessions.SessionCards;
using Memoyed.Domain.Cards.Services;
using Memoyed.DomainFramework;

namespace Memoyed.Application.Services
{
    public class RevisionSessionsCommandsHandler
    {
        private readonly IDomainEventPublisher _eventPublisher;
        private readonly UnitOfWork _unitOfWork;
        private readonly ICardAnswerCheckService _cardAnswerCheckService;

        public RevisionSessionsCommandsHandler(UnitOfWork unitOfWork, IDomainEventPublisher eventPublisher,
            ICardAnswerCheckService cardAnswerCheckService)
        {
            _unitOfWork = unitOfWork;
            _eventPublisher = eventPublisher;
            _cardAnswerCheckService = cardAnswerCheckService;
        }

        public async Task SetCardAnswer(Commands.SetCardAnswerCommand command)
        {
            var session = await _unitOfWork.RevisionSessionsRepository
                .Get(new RevisionSessionId(command.RevisionSessionId));

            var cardId = new CardId(command.CardId);
            session.CardAnswered(cardId, command.AnswerType, command.Answer, _cardAnswerCheckService);

            await _unitOfWork.Commit();
        }

        public async Task CompleteSession(Commands.CompleteRevisionSessionCommand command)
        {
            var session = await _unitOfWork.RevisionSessionsRepository
                .Get(new RevisionSessionId(command.RevisionSessionId));

            session.CompleteSession();

            await _eventPublisher.Publish(new RevisionSessionEvents.RevisionSessionCompleted(session.Id,
                session.CardBoxSetId));

            await _unitOfWork.Commit();
        }
    }
}
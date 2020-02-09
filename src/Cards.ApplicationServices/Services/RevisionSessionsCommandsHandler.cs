using System.Threading.Tasks;
using Memoyed.Cards.ApplicationServices.Dto;
using Memoyed.Cards.Domain.LearningCards;
using Memoyed.Cards.Domain.RevisionSessions;

namespace Memoyed.Cards.ApplicationServices.Services
{
    public class RevisionSessionsCommandsHandler
    {
        private readonly UnitOfWork _unitOfWork;

        public RevisionSessionsCommandsHandler(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task SetCardAnswer(Commands.SetCardAnswerCommand command)
        {
            var session = await _unitOfWork.RevisionSessionsRepository
                .Get(new RevisionSessionId(command.RevisionSessionId))
                .ConfigureAwait(false);
                
            session.CardAnswered(new LearningCardId(command.LearningCardId), AnswerType.TargetLanguage, command.Answer);

            await _unitOfWork.Commit()
                .ConfigureAwait(false);
        }

        public async Task CompleteSession(Commands.SetCardAnswerCommand command)
        {
            var session = await _unitOfWork.RevisionSessionsRepository
                .Get(new RevisionSessionId(command.RevisionSessionId))
                .ConfigureAwait(false);
            
            session.CompleteSession();

            await _unitOfWork.Commit()
                .ConfigureAwait(false);
        }
    }
}
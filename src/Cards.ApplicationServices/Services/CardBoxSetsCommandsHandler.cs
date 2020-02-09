using System;
using System.Threading.Tasks;
using Memoyed.Cards.ApplicationServices.Dto;
using Memoyed.Cards.Domain.CardBoxes;
using Memoyed.Cards.Domain.CardBoxSets;
using Memoyed.Cards.Domain.LearningCards;
using Memoyed.Cards.Domain.Shared;

namespace Memoyed.Cards.ApplicationServices.Services
{
    public class CardBoxSetsCommandsHandler
    {
        private readonly UnitOfWork _unitOfWork;

        public CardBoxSetsCommandsHandler(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateCardBoxSet(Commands.CreateCardBoxSetCommand command)
        {
            var cardBoxSet = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()),
                new CardBoxSetName(command.Name), 
                new CardBoxSetLanguage(command.NativeLanguage, DomainChecksImpl.ValidateLanguage),
                new CardBoxSetLanguage(command.TargetLanguage, DomainChecksImpl.ValidateLanguage));

            await _unitOfWork.CardBoxSetsRepository.AddNew(cardBoxSet)
                .ConfigureAwait(false);

            await _unitOfWork.Commit()
                .ConfigureAwait(false);
        }

        public async Task CreateCardBox(Commands.CreateCardBoxCommand command)
        {
            var cardBoxSet = await _unitOfWork.CardBoxSetsRepository.Get(new CardBoxSetId(command.CardBoxSetId))
                .ConfigureAwait(false);
            
            var box = new CardBox(new CardBoxId(Guid.NewGuid()),
                cardBoxSet.Id,
                new CardBoxLevel(command.CardBoxLevel), 
                new CardBoxRevisionDelay(command.RevisionDelay));
            
            cardBoxSet.AddCardBox(box);

            await _unitOfWork.Commit()
                .ConfigureAwait(false);
        }

        public async Task AddNewLearningCard(Commands.AddNewLearningCardCommand dto)
        {
            var cardBoxSet = await _unitOfWork.CardBoxSetsRepository.Get(new CardBoxSetId(dto.CardBoxSetId))
                .ConfigureAwait(false);
            
            var learningCard = new LearningCard(new LearningCardId(Guid.NewGuid()), 
                new LearningCardWord(dto.NativeLanguageWord), 
                new LearningCardWord(dto.TargetLanguageWord), 
                new LearningCardComment(dto.Comment));

            cardBoxSet.AddNewCard(learningCard, new UtcTime(DateTime.UtcNow));

            await _unitOfWork.Commit()
                .ConfigureAwait(false);
        }

        public async Task RemoveLearningCard(Commands.RemoveLearningCardCommand dto)
        {
            var cardBoxSet = await _unitOfWork.CardBoxSetsRepository.Get(new CardBoxSetId(dto.CardBoxSetId))
                .ConfigureAwait(false);
            
            cardBoxSet.RemoveCard(new LearningCardId(dto.LearningCardId));

            await _unitOfWork.Commit()
                .ConfigureAwait(false);
        }

        public async Task RenameCardBoxSet(Commands.RenameCardBoxSetCommand command)
        {
            var cardBoxSet = await _unitOfWork.CardBoxSetsRepository.Get(new CardBoxSetId(command.CardBoxSetId))
                .ConfigureAwait(false);
            
            cardBoxSet.Rename(new CardBoxSetName(command.CardBoxSetName));

            await _unitOfWork.Commit()
                .ConfigureAwait(false);
        }

        public async Task StartRevisionSession(Commands.StartRevisionSessionCommand command)
        {
            var cardBoxSet = await _unitOfWork.CardBoxSetsRepository.Get(new CardBoxSetId(command.CardBoxSetId))
                .ConfigureAwait(false);

            var revisionSession = cardBoxSet.StartRevisionSession();

            await _unitOfWork.RevisionSessionsRepository.AddNew(revisionSession)
                .ConfigureAwait(false);

            await _unitOfWork.Commit()
                .ConfigureAwait(false);
        }
    }
}
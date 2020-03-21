using System;
using System.Threading.Tasks;
using Memoyed.Cards.ApplicationServices.Dto;
using Memoyed.Cards.Domain.CardBoxes;
using Memoyed.Cards.Domain.CardBoxSets;
using Memoyed.Cards.Domain.Cards;
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

        public async Task Handle(object command, Guid ownerId)
        {
            switch (command)
            {
                case Commands.CreateCardBoxSetCommand createCardBoxSetCommand:
                {
                    await HandleCreate(createCardBoxSetCommand, ownerId);
                    break;
                }
                case Commands.CreateCardBoxCommand createCardBoxCommand:
                {
                    await HandleUpdate(createCardBoxCommand.CardBoxSetId, s =>
                    {
                        var box = new CardBox(new CardBoxId(Guid.NewGuid()), s.Id,
                            new CardBoxLevel(createCardBoxCommand.CardBoxLevel),
                            new CardBoxRevisionDelay(createCardBoxCommand.RevisionDelay));

                        s.AddCardBox(box);
                    });
                    break;
                }
                case Commands.AddNewCardCommand addNewCardCommand:
                {
                    await HandleUpdate(addNewCardCommand.CardBoxSetId, s =>
                    {
                        var card = new Card(new CardId(Guid.NewGuid()),
                            new CardWord(addNewCardCommand.NativeLanguageWord),
                            new CardWord(addNewCardCommand.TargetLanguageWord),
                            new CardComment(addNewCardCommand.Comment));

                        s.AddNewCard(card, new UtcTime(DateTime.UtcNow));
                    });
                    break;
                }
                case Commands.RemoveCardCommand removeCardCommand:
                {
                    await HandleUpdate(removeCardCommand.CardBoxSetId, s =>
                    {
                        s.RemoveCard(new CardId(removeCardCommand.CardId));
                    });
                    break;
                }
                case Commands.RenameCardBoxSetCommand renameCardBoxSetCommand:
                {
                    await HandleUpdate(renameCardBoxSetCommand.CardBoxSetId,
                        s => { s.Rename(new CardBoxSetName(renameCardBoxSetCommand.CardBoxSetName)); });
                    break;
                }
                case Commands.StartRevisionSessionCommand startRevisionSessionCommand:
                {
                    await HandleUpdate(startRevisionSessionCommand.CardBoxSetId, s =>
                    {
                        var revisionSession = s.StartRevisionSession();
                        _unitOfWork.RevisionSessionsRepository.AddNew(revisionSession);
                    });
                    break;
                }
                default:
                    throw new NotSupportedException();
            }
        }

        private async Task HandleUpdate(Guid cardBoxSetId, Action<CardBoxSet> update)
        {
            var cardBoxSet = await _unitOfWork.CardBoxSetsRepository.Get(new CardBoxSetId(cardBoxSetId));

            if (cardBoxSet == null)
            {
                throw new InvalidOperationException("Couldn't find a card box set with the given identity");
            }

            update(cardBoxSet);

            await _unitOfWork.Commit();
        }

        private async Task HandleCreate(Commands.CreateCardBoxSetCommand command, Guid ownerId)
        {
            var cardBoxSet = new CardBoxSet(new CardBoxSetId(Guid.NewGuid()), new CardBoxSetOwnerId(ownerId), 
                new CardBoxSetName(command.Name),
                new CardBoxSetLanguage(command.NativeLanguage, DomainChecksImpl.ValidateLanguage),
                new CardBoxSetLanguage(command.TargetLanguage, DomainChecksImpl.ValidateLanguage));

            await _unitOfWork.CardBoxSetsRepository.AddNew(cardBoxSet);

            await _unitOfWork.Commit();
        }
    }
}
﻿using System;
using System.Threading.Tasks;
using Memoyed.Application.Dto;
using Memoyed.Domain.Cards.CardBoxes;
using Memoyed.Domain.Cards.CardBoxSets;
using Memoyed.Domain.Cards.Cards;
using Memoyed.Domain.Cards.Repositories;
using Memoyed.DomainFramework;

namespace Memoyed.Application.Services
{
    public class CardBoxSetsCommandsHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICardBoxSetsRepository _cardBoxSetsRepository;
        private readonly IRevisionSessionsRepository _revisionSessionsRepository;
        
        public CardBoxSetsCommandsHandler(IUnitOfWork unitOfWork, ICardBoxSetsRepository cardBoxSetsRepository,
            IRevisionSessionsRepository revisionSessionsRepository)
        {
            _unitOfWork = unitOfWork;
            _cardBoxSetsRepository = cardBoxSetsRepository;
            _revisionSessionsRepository = revisionSessionsRepository;
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
                        var box = new CardBox(Guid.NewGuid(), s.Id,
                            new CardBoxLevel(createCardBoxCommand.Level),
                            new CardBoxRevisionDelay(createCardBoxCommand.RevisionDelay));

                        s.AddCardBox(box);
                    });
                    break;
                }
                case Commands.CreateCardCommand addNewCardCommand:
                {
                    await HandleUpdate(addNewCardCommand.CardBoxSetId, s =>
                    {
                        var card = new Card(Guid.NewGuid(),
                            new CardWord(addNewCardCommand.NativeLanguageWord),
                            new CardWord(addNewCardCommand.TargetLanguageWord));

                        if (addNewCardCommand.Comment != null)
                            card.ChangeComment(new CardComment(addNewCardCommand.Comment));

                        s.AddNewCard(card, DateTimeOffset.UtcNow);
                    });
                    break;
                }
                case Commands.RemoveCardCommand removeCardCommand:
                {
                    await HandleUpdate(removeCardCommand.CardBoxSetId,
                        s => { s.RemoveCard(removeCardCommand.CardId); });
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
                        _revisionSessionsRepository.AddNew(revisionSession);
                    });
                    break;
                }
                default:
                    throw new NotSupportedException();
            }
        }

        private async Task HandleUpdate(Guid cardBoxSetId, Action<CardBoxSet> update)
        {
            var cardBoxSet = await _cardBoxSetsRepository.Get(cardBoxSetId);

            if (cardBoxSet == null)
                throw new InvalidOperationException("Couldn't find a card box set with the given identity");

            update(cardBoxSet);

            await _unitOfWork.Commit();
        }

        private async Task HandleCreate(Commands.CreateCardBoxSetCommand command, Guid ownerId)
        {
            var cardBoxSet = new CardBoxSet(Guid.NewGuid(), new CardBoxSetOwnerId(ownerId),
                new CardBoxSetName(command.Name),
                new CardBoxSetLanguage(command.NativeLanguage, DomainChecksImpl.ValidateLanguage),
                new CardBoxSetLanguage(command.TargetLanguage, DomainChecksImpl.ValidateLanguage));

            _cardBoxSetsRepository.AddNew(cardBoxSet);

            await _unitOfWork.Commit();
        }
    }
}
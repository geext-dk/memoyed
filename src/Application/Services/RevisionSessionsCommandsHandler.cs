﻿using System.Linq;
using System.Threading.Tasks;
using Memoyed.Application.Dto;
using Memoyed.Domain.Cards.Cards;
using Memoyed.Domain.Cards.RevisionSessions;
using Memoyed.DomainFramework;

namespace Memoyed.Application.Services
{
    public class RevisionSessionsCommandsHandler
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IDomainEventPublisher _eventPublisher;
        
        public RevisionSessionsCommandsHandler(UnitOfWork unitOfWork, IDomainEventPublisher eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _eventPublisher = eventPublisher;
        }

        public async Task SetCardAnswer(Commands.SetCardAnswerCommand command)
        {
            var session = await _unitOfWork.RevisionSessionsRepository
                .Get(new RevisionSessionId(command.RevisionSessionId));
            
            var cardId = new CardId(command.CardId);
            var card = session.SessionCards.First(sc => sc.CardId == cardId);

            if (command.Answer == card.TargetLanguageWord.Value)
            {
                session.CardAnsweredCorrectly(cardId);
            }
            else
            {
                session.CardAnsweredWrong(cardId);
            }

            await _unitOfWork.Commit();
        }

        public async Task CompleteSession(Commands.CompleteSessionCommand command)
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
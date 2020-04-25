﻿using System;
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

        public async Task Handle(object command, Guid userId)
        {
            switch (command)
            {
                case Commands.SetCardAnswerCommand setCardAnswerCommand:
                {
                    await HandleUpdate(setCardAnswerCommand.RevisionSessionId,
                        session => session.CardAnswered(
                            new CardId(setCardAnswerCommand.CardId), setCardAnswerCommand.AnswerType,
                            setCardAnswerCommand.Answer,
                            _cardAnswerCheckService));
                    break;
                }
                case Commands.CompleteRevisionSessionCommand completeRevisionSessionCommand:
                {
                    await HandleUpdateAsync(completeRevisionSessionCommand.RevisionSessionId,
                        async session =>
                        {
                            session.CompleteSession();
                            await _eventPublisher.Publish(new RevisionSessionEvents.RevisionSessionCompleted(
                                session.Id, session.CardBoxSetId));
                        });

                    break;
                }
                default:
                    throw new NotSupportedException();
            }
        }

        private async Task HandleUpdate(Guid revisionSessionId, Action<RevisionSession> update)
        {
            var revisionSession = await _unitOfWork.RevisionSessionsRepository.Get(new RevisionSessionId(revisionSessionId));

            if (revisionSession == null)
                throw new InvalidOperationException("Couldn't find a revision session with the given identity");

            update(revisionSession);

            await _unitOfWork.Commit();
        }

        private async Task HandleUpdateAsync(Guid revisionSessionId, Func<RevisionSession, Task> updateAsync)
        {
            var revisionSession = await _unitOfWork.RevisionSessionsRepository.Get(new RevisionSessionId(revisionSessionId));

            if (revisionSession == null)
                throw new InvalidOperationException("Couldn't find a revision session with the given identity");

            await updateAsync(revisionSession);

            await _unitOfWork.Commit();
        }
    }
}
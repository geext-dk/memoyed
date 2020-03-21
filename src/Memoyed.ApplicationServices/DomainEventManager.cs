using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Memoyed.Cards.Domain.CardBoxSets;
using Memoyed.Cards.Domain.Cards;
using Memoyed.Cards.Domain.Repositories;
using Memoyed.Cards.Domain.RevisionSessions;
using Memoyed.Cards.Domain.Shared;
using Memoyed.DomainFramework;
using Microsoft.Extensions.DependencyInjection;

namespace Memoyed.Cards.ApplicationServices
{
    public sealed class DomainEventManager : IDomainEventPublisher, IDisposable
    {
        private readonly Channel<object> _eventsChannel = Channel.CreateBounded<object>(100);
        private Task _processEventsTask;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly IServiceProvider _serviceProvider;

        public DomainEventManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            
            InitializeEventHandler();
        }

        private void InitializeEventHandler()
        {
            var token = _cts.Token;
            _processEventsTask = Task.Factory
                .StartNew(EventProcessingLoop, token, TaskCreationOptions.LongRunning, TaskScheduler.Default)
                .Unwrap();
        }

        public async Task Publish<T>(T @event) where T : class
        {
            await _eventsChannel.Writer.WriteAsync(@event);
        }

        private async Task EventProcessingLoop()
        {
            try
            {
                while (true)
                {
                    try
                    {
                        var @event = await _eventsChannel.Reader.ReadAsync();
                        await HandleDomainEvent(@event);
                    }
                    catch (ChannelClosedException)
                    {
                        break;
                    }
                }
            }
            catch (TaskCanceledException)
            {
            }
        }

        private async Task HandleDomainEvent(object domainEvent)
        {
            using var scope = _serviceProvider.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetService<UnitOfWork>();
            switch (domainEvent)
            {
                case RevisionSessionEvents.RevisionSessionCompleted revisionSessionCompleted:
                {
                    var cardBoxSetRepository = scope.ServiceProvider.GetService<ICardBoxSetsRepository>();
                    var cardBoxSet = await cardBoxSetRepository.Get(
                        new CardBoxSetId(revisionSessionCompleted.CardBoxSetId));
                    
                    cardBoxSet.ProcessCardsFromRevisionSession(
                        new RevisionSessionId(revisionSessionCompleted.RevisionSessionId),
                        revisionSessionCompleted.AnsweredSuccessfullyCardIds
                            .Select(c => new CardId(c)),
                        revisionSessionCompleted.AnsweredWrongCardsId
                            .Select(c => new CardId(c)),
                        new UtcTime(revisionSessionCompleted.DateTime));
                    
                    break;
                }
            }

            await unitOfWork.Commit();
        }

        public void Dispose()
        {
            _cts.Cancel();
        }
    }
}
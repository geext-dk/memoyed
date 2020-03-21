using System;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Memoyed.Domain.Cards.CardBoxSets;
using Memoyed.Domain.Cards.Cards;
using Memoyed.Domain.Cards.Repositories;
using Memoyed.Domain.Cards.RevisionSessions;
using Memoyed.Domain.Cards.Shared;
using Memoyed.DomainFramework;
using Microsoft.Extensions.DependencyInjection;

namespace Memoyed.Application
{
    public sealed class DomainEventManager : IDomainEventPublisher, IDisposable
    {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly Channel<object> _eventsChannel = Channel.CreateBounded<object>(100);
        private readonly IServiceProvider _serviceProvider;
        private Task _processEventsTask;

        public DomainEventManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            InitializeEventHandler();
        }

        public void Dispose()
        {
            _cts.Cancel();
        }

        public async Task Publish<T>(T @event) where T : class
        {
            await _eventsChannel.Writer.WriteAsync(@event);
        }

        private void InitializeEventHandler()
        {
            var token = _cts.Token;
            _processEventsTask = Task.Factory
                .StartNew(EventProcessingLoop, token, TaskCreationOptions.LongRunning, TaskScheduler.Default)
                .Unwrap();
        }

        private async Task EventProcessingLoop()
        {
            try
            {
                while (true)
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
    }
}
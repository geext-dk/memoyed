using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Memoyed.Domain.Cards.CardBoxSets;
using Memoyed.Domain.Cards.Repositories;
using Memoyed.Domain.Cards.RevisionSessions;
using Memoyed.DomainFramework;
using Microsoft.Extensions.DependencyInjection;

namespace Memoyed.Application
{
    public sealed class DomainEventManager : IDomainEventPublisher, IDisposable
    {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly Channel<object> _eventsChannel = Channel.CreateBounded<object>(100);
        private readonly IServiceProvider _serviceProvider;

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
            Task.Factory
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
                    var revisionSessionRepository = scope.ServiceProvider.GetService<IRevisionSessionsRepository>();
                    var cardBoxSet = await cardBoxSetRepository.Get(
                        revisionSessionCompleted.CardBoxSetId);
                    var revisionSession = await revisionSessionRepository.Get(revisionSessionCompleted.RevisionSessionId);

                    cardBoxSet.ProcessCardsFromRevisionSession(revisionSession, revisionSessionCompleted.DateTime);

                    break;
                }
            }

            await unitOfWork.Commit();
        }
    }
}
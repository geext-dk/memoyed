using Memoyed.Application.Services;
using Memoyed.Domain.Cards.Services;
using Memoyed.DomainFramework;
using Microsoft.Extensions.DependencyInjection;

namespace Memoyed.Application
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCardsApplicationServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IDomainEventPublisher, DomainEventManager>();
            serviceCollection.AddSingleton<ICardAnswerCheckService, SimpleCardAnswerCheckService>();

            serviceCollection.AddTransient<CardBoxSetsCommandsHandler>();
            serviceCollection.AddTransient<RevisionSessionsCommandsHandler>();

            serviceCollection.AddTransient<CardBoxSetsCommandsHandler>();
            serviceCollection.AddTransient<RevisionSessionsCommandsHandler>();
        }
    }
}
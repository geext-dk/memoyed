using System;
using System.Data;
using Memoyed.Application.DataModel;
using Memoyed.Application.DataModel.Repositories;
using Memoyed.Application.Services;
using Memoyed.Domain.Cards.Repositories;
using Memoyed.DomainFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Memoyed.Application
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCardsApplicationServices(this IServiceCollection serviceCollection,
            Action<DbContextOptionsBuilder> dbOptionsBuilder)
        {
            serviceCollection.AddDbContext<CardsContext>(dbOptionsBuilder);

            serviceCollection.AddTransient<UnitOfWork>();
            serviceCollection.AddTransient<ICardBoxSetsRepository, CardBoxSetsRepository>();
            serviceCollection.AddTransient<IRevisionSessionsRepository, RevisionSessionsRepository>();
            serviceCollection.AddTransient<IDomainEventPublisher, DomainEventManager>();

            serviceCollection.AddTransient<CardBoxSetsCommandsHandler>();
            serviceCollection.AddTransient<RevisionSessionsCommandsHandler>();

            serviceCollection.AddTransient<CardBoxSetsCommandsHandler>();
            serviceCollection.AddTransient<RevisionSessionsCommandsHandler>();
            serviceCollection.AddScoped<IDbConnection>(provider =>
            {
                var context = provider.GetRequiredService<CardsContext>();
                return context.Database.GetDbConnection();
            });
        }
    }
}
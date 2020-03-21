using System;
using Dapper;
using Memoyed.Cards.ApplicationServices.DataModel;
using Memoyed.Cards.ApplicationServices.DataModel.Repositories;
using Memoyed.Cards.ApplicationServices.Extensions;
using Memoyed.Cards.ApplicationServices.Services;
using Memoyed.Cards.Domain.Repositories;
using Memoyed.DomainFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Memoyed.Cards.ApplicationServices
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
            serviceCollection.AddTransient<CardBoxSetsQueriesHandler>();
            serviceCollection.AddTransient<RevisionSessionsCommandsHandler>();
            
            SqlMapper.AddTypeHandler(new SqlMapperExtensions.GuidSqlHandler());
        }
    }
}
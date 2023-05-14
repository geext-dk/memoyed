using System;
using System.Data;
using Memoyed.Application.EntityFramework.Repositories;
using Memoyed.Domain.Cards.Repositories;
using Memoyed.DomainFramework;
using Memoyed.WebApi.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Memoyed.Application.EntityFramework
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCardsDataServices(this IServiceCollection serviceCollection,
            Action<DbContextOptionsBuilder> dbOptionsBuilder)
        {
            serviceCollection.AddTransient<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddScoped<ICardBoxSetsRepository, CardBoxSetsRepository>();
            serviceCollection.AddScoped<IRevisionSessionsRepository, RevisionSessionsRepository>();
            serviceCollection.AddDbContext<CardsContext>(dbOptionsBuilder);
            serviceCollection.AddScoped<IDbConnection>(provider =>
            {
                var context = provider.GetRequiredService<CardsContext>();
                return context.Database.GetDbConnection();
            });
        }
    }
}

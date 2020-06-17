using System;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Memoyed.Application.EntityFramework
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCardsDataServices(this IServiceCollection serviceCollection,
            Action<DbContextOptionsBuilder> dbOptionsBuilder)
        {
            serviceCollection.AddDbContext<CardsContext>(dbOptionsBuilder);
            serviceCollection.AddScoped<IDbConnection>(provider =>
            {
                var context = provider.GetRequiredService<CardsContext>();
                return context.Database.GetDbConnection();
            });
        }
    }
}
using System;
using GraphQL.Types;
using GraphQL.Utilities;

namespace Memoyed.WebApi.GraphQL
{
    public class CardsSchema : Schema
    {
        public CardsSchema(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Services = serviceProvider;
            Query = serviceProvider.GetRequiredService<CardsQuery>();
            Mutation = serviceProvider.GetRequiredService<CardsMutation>();
        }
    }
}
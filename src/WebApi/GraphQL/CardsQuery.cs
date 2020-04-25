using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using GraphQL;
using GraphQL.Types;
using Memoyed.Application.Dto;
using Memoyed.WebApi.GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Memoyed.WebApi.GraphQL
{
    public class CardsQuery : ObjectGraphType
    {
        public CardsQuery(IServiceProvider serviceProvider)
        {
            Name = "CardsQuery";

            FieldAsync<ListGraphType<CardBoxSetType>, IEnumerable<ReturnModels.CardBoxSetModel>>("cardBoxSets",
                arguments: new QueryArguments(
                    new QueryArgument<IdGraphType>
                    {
                        Name = "id",
                        Description = "Id of a set to query"
                    }),
                resolve: async c =>
                {
                    using var scope = serviceProvider.CreateScope();
                    var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
                    var sql = @"SELECT s.id, s.name, s.native_language, s.target_language
                                FROM card_box_sets AS s";

                    var id = c.GetArgument<Guid?>("id");
                    if (id.HasValue) sql += " WHERE @Id = s.Id";
                    return await connection.QueryAsync<ReturnModels.CardBoxSetModel>(sql, new
                    {
                        Id = id
                    });
                });
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using GraphQL;
using GraphQL.Types;
using Memoyed.Application.DataModel;
using Memoyed.Application.Dto;
using Memoyed.WebApi.GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Memoyed.WebApi.GraphQL
{
    public class CardsQuery : ObjectGraphType<object>
    {
        public CardsQuery(CardsContext cardsDb)
        {
            var connection = cardsDb.Database.GetDbConnection();
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
                    var sql = @"SELECT s.Id, s.Name, s.NativeLanguage, s.TargetLanguage
                                                 FROM CardBoxSets AS s";

                    var id = c.GetArgument<Guid?>("id");
                    if (id.HasValue)
                    {
                        sql += " WHERE @Id = s.Id";
                    }
                    return await connection.QueryAsync<ReturnModels.CardBoxSetModel>(sql, new
                    {
                        Id = id
                    });
                });
        }
        
    }
}
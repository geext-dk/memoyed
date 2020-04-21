using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
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
                resolve: async c =>
                {

                    const string sql = @"SELECT s.Id, s.Name, s.NativeLanguage, s.TargetLanguage
                                                 FROM CardBoxSets AS s";
                    return await connection.QueryAsync<ReturnModels.CardBoxSetModel>(sql);
                });
        }
        
    }
}
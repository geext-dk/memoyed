using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using GraphQL.DataLoader;
using GraphQL.Types;
using Memoyed.Application.DataModel;
using Memoyed.Application.Dto;
using Microsoft.EntityFrameworkCore;

namespace Memoyed.WebApi.GraphQL.Types
{
    public sealed class CardBoxSetType : ObjectGraphType<ReturnModels.CardBoxSetModel>
    {
        public CardBoxSetType(CardsContext cardsDb, IDataLoaderContextAccessor contextAccessor)
        {
            var connection = cardsDb.Database.GetDbConnection();
            Name = "CardBoxSet";
            Description = "A set of card boxes. Every card box contains cards";

            Field(c => c.Id).Description("The id of the card box set");
            Field(c => c.Name).Description("Name of the card box set");
            Field(c => c.NativeLanguage).Description("The language the user knows");
            Field(c => c.TargetLanguage).Description("The language the user is learning");
            FieldAsync<ListGraphType<CardBoxType>, IEnumerable<ReturnModels.CardBoxModel>>("cardBoxes",
                resolve: async c =>
                {
                    var dataLoader =
                        contextAccessor.Context.GetOrAddCollectionBatchLoader<Guid, ReturnModels.CardBoxModel>(
                            "GetSetCardBoxes", async ids =>
                            {
                                const string sql = @"SELECT c.Id, c.SetId, c.Level, c.RevisionDelay
                                                     FROM CardBoxes AS c
                                                     WHERE c.SetId IN @SetIds";
                                var result = await connection.QueryAsync<ReturnModels.CardBoxModel>(sql, new
                                {
                                    SetIds = ids
                                });

                                return result.ToLookup(r => r.SetId);
                            });

                    return await dataLoader.LoadAsync(c.Source.Id);
                });

            FieldAsync<ListGraphType<CardType>, IEnumerable<ReturnModels.CardModel>>("cards",
                resolve: async c =>
                {
                    var dataLoader =
                        contextAccessor.Context.GetOrAddCollectionBatchLoader<Guid, ReturnModels.CardModel>(
                            "GetSetCards", async ids =>
                            {
                                const string sql = @"SELECT c.Id, c.NativeLanguageWord, c.TargetLanguageWord,
                                                         c.Comment, c.CardBoxId, b.SetId, b.Level
                                                     FROM Cards AS c
                                                     INNER JOIN CardBoxes AS b ON b.Id = c.CardBoxId
                                                     WHERE b.SetId IN @SetIds";

                                var result = await connection.QueryAsync<ReturnModels.CardModel>(sql, new
                                {
                                    SetIds = ids
                                });

                                return result.ToLookup(r => r.SetId);
                            });

                    return await dataLoader.LoadAsync(c.Source.Id);
                });
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using GraphQL.DataLoader;
using GraphQL.Types;
using Memoyed.Application.Dto;
using Microsoft.Extensions.DependencyInjection;

namespace Memoyed.WebApi.GraphQL.Types
{
    public sealed class CardBoxSetType : ObjectGraphType<ReturnModels.CardBoxSetModel>
    {
        public CardBoxSetType(IServiceProvider serviceProvider, IDataLoaderContextAccessor contextAccessor)
        {
            Name = "CardBoxSet";
            Description = "A set of card boxes. Every card box contains cards";

            Field(c => c.Id).Description("The id of the card box set");
            Field(c => c.Name).Description("Name of the card box set");
            Field(c => c.NativeLanguage).Description("The language the user knows");
            Field(c => c.TargetLanguage).Description("The language the user is learning");
            FieldAsync<ListGraphType<CardBoxType>, IEnumerable<ReturnModels.CardBoxModel>>("cardBoxes",
                resolve: async c =>
                {
                    using var scope = serviceProvider.CreateScope();
                    var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
                    var dataLoader =
                        contextAccessor.Context.GetOrAddCollectionBatchLoader<Guid, ReturnModels.CardBoxModel>(
                            "GetSetCardBoxes", async ids =>
                            {
                                const string sql = @"SELECT c.id, c.set_id, c.level, c.revision_delay
                                                     FROM card_boxes AS c
                                                     WHERE c.set_id = ANY(@SetIds)";
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
                    using var scope = serviceProvider.CreateScope();
                    var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
                    var dataLoader =
                        contextAccessor.Context.GetOrAddCollectionBatchLoader<Guid, ReturnModels.CardModel>(
                            "GetSetCards", async ids =>
                            {
                                const string sql = @"SELECT c.id, c.native_language_word, c.target_language_word,
                                                         c.comment, c.card_box_id, b.set_id, b.level
                                                     FROM cards AS c
                                                     INNER JOIN card_boxes AS b ON b.id = c.card_box_id
                                                     WHERE b.set_id = ANY(@SetIds)";

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
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
    public sealed class CardBoxType : ObjectGraphType<ReturnModels.CardBoxModel>
    {
        public CardBoxType(IServiceProvider serviceProvider, IDataLoaderContextAccessor contextAccessor)
        {
            Name = "CardBox";
            Description = "A single box from a card box set that contains cards";

            Field(c => c.Id).Description("The id of the card box");
            Field(c => c.SetId).Description("Id of the set containing the box belongs to");
            Field(c => c.Level).Description("Level of the box. The more the level, the more " +
                                            "days should pass until a revision of a contained card");
            Field(c => c.RevisionDelay).Description("Amount of days should pass until a " +
                                                    "revision of a contained card");
            FieldAsync<ListGraphType<CardType>, IEnumerable<ReturnModels.CardModel>>("cards",
                resolve: async c =>
                {
                    using var scope = serviceProvider.CreateScope();
                    var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
                    var dataLoader =
                        contextAccessor.Context.GetOrAddCollectionBatchLoader<Guid, ReturnModels.CardModel>(
                            "GetBoxCards",
                            async ids =>
                            {
                                const string sql = @"SELECT c.id, c.native_language_word, c.target_language_word,
                                                    c.comment, c.card_box_id, b.set_id,
                                                    (c.card_box_changed_date + b.revision_delay * INTERVAL '1 day')
                                                        AS RevisionAllowedDate
                                                 FROM cards AS c
                                                 INNER JOIN card_boxes AS b ON b.id = c.card_box_id
                                                 WHERE b.id = ANY(@BoxIds)";
                                var result = await connection.QueryAsync<ReturnModels.CardModel>(sql, new
                                {
                                    BoxIds = ids
                                });

                                return result.ToLookup(r => r.CardBoxId.Value);
                            });

                    return await dataLoader.LoadAsync(c.Source.Id);
                });
        }
    }
}
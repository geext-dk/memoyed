using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Dapper;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;

namespace Memoyed.WebApi.GraphQL.ReturnTypes
{
    /// <summary>
    /// A single box from a card box set that contains cards
    /// </summary>
    [GraphQLName("CardBox")]
    public class CardBoxType
    {
        /// <summary>
        /// The id of the card box
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Id of the set containing the box
        /// </summary>
        public Guid SetId { get; set; }
        
        /// <summary>
        /// Level of the box. The more the level, the more days should pass until a revision of a contained card
        /// </summary>
        public int Level { get; set; }
        
        /// <summary>
        /// Amount of days should pass until a revision of a contained card
        /// </summary>
        public int RevisionDelay { get; set; }
        
        /// <summary>
        /// Cards contained in the box
        /// </summary>
        [GetCardsResolver] public IEnumerable<CardType> Cards { get; set; }

        private class GetCardsResolverAttribute : ObjectFieldDescriptorAttribute
        {
            public override void OnConfigure(IDescriptorContext context, IObjectFieldDescriptor descriptor,
                MemberInfo member)
            {
                descriptor.Resolver(async (ctx, ct) =>
                {
                    var connection = ctx.Service<IDbConnection>();
                    var parent = ctx.Parent<CardBoxType>();
                    return await ctx.BatchDataLoader<Guid, IEnumerable<CardType>>("CardBox_GetCards",
                        async (keys, batchCt) =>
                        {
                            const string sql = @"SELECT c.id, c.native_language_word, c.target_language_word,
                                                    c.comment, c.card_box_id, b.set_id,
                                                    (c.card_box_changed_date + b.revision_delay * INTERVAL '1 day')
                                                        AS RevisionAllowedDate
                                                 FROM cards AS c
                                                 INNER JOIN card_boxes AS b ON b.id = c.card_box_id
                                                 WHERE b.id = ANY(@BoxIds)";

                            var result = await connection.QueryAsync<CardType>(new CommandDefinition(sql, new
                            {
                                BoxIds = keys
                            }, cancellationToken: batchCt));

                            return result
                                .GroupBy(r => r.SetId)
                                .ToDictionary(r => r.Key, r => r.AsEnumerable());
                        }).LoadAsync(parent.Id, ct);
                });
            }
        }
    }
}
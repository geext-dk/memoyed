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
    /// A set of card boxes. Every card box contains cards"
    /// </summary>
    [GraphQLName("CardBoxSet")]
    public class CardBoxSetType
    {
        /// <summary>
        /// The id of the card box set
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Name of the card box set
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The language the user knows
        /// </summary>
        public string NativeLanguage { get; set; }
        
        /// <summary>
        /// The language the user is learning
        /// </summary>
        public string TargetLanguage { get; set; }

        /// <summary>
        /// Cards contained in boxes of the set
        /// </summary>
        [GetCardsResolver] public IEnumerable<CardType> Cards { get; set; }
        
        /// <summary>
        /// Card boxes contained in the set
        /// </summary>
        [GetCardBoxesResolver] public IEnumerable<CardBoxType> CardBoxes { get; set; }

        private class GetCardsResolverAttribute : ObjectFieldDescriptorAttribute
        {
            public override void OnConfigure(IDescriptorContext context, IObjectFieldDescriptor descriptor,
                MemberInfo member)
            {
                descriptor.Resolver(async (ctx, ct) =>
                {
                    var connection = ctx.Service<IDbConnection>();
                    var parent = ctx.Parent<CardBoxSetType>();
                    return await ctx.BatchDataLoader<Guid, IEnumerable<CardType>>("CardBoxSet_GetCards",
                        async (keys, batchCt) =>
                        {
                            const string sql = @"SELECT c.id, c.native_language_word, c.target_language_word,
                                                     c.comment, c.card_box_id, b.set_id, b.level,
                                                     (c.card_box_changed_date + b.revision_delay * INTERVAL '1 day')
                                                        AS RevisionAllowedDate
                                                 FROM cards AS c
                                                 INNER JOIN card_boxes AS b ON b.id = c.card_box_id
                                                 WHERE b.set_id = ANY(@SetIds)";

                            var result = await connection.QueryAsync<CardType>(new CommandDefinition(sql, new
                            {
                                SetIds = keys
                            }, cancellationToken: batchCt));

                            return result
                                .GroupBy(r => r.SetId)
                                .ToDictionary(r => r.Key, r => r.AsEnumerable());
                        }).LoadAsync(parent.Id, ct);
                });
            }
        }

        private class GetCardBoxesResolverAttribute : ObjectFieldDescriptorAttribute
        {
            public override void OnConfigure(IDescriptorContext context, IObjectFieldDescriptor descriptor,
                MemberInfo member)
            {
                descriptor.Resolver(async (ctx, ct) =>
                {
                    var connection = ctx.Service<IDbConnection>();
                    var parent = ctx.Parent<CardBoxSetType>();
                    return await ctx.BatchDataLoader<Guid, IEnumerable<CardBoxType>>("CardBoxSet_GetCardBoxes",
                        async (keys, batchCt) =>
                        {
                            const string sql = @"SELECT c.id, c.set_id, c.level, c.revision_delay
                                                 FROM card_boxes AS c
                                                 WHERE c.set_id = ANY(@SetIds)";

                            var result = await connection.QueryAsync<CardBoxType>(new CommandDefinition(sql, new
                            {
                                SetIds = keys
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
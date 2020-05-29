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
using Memoyed.Domain.Cards.RevisionSessions;

namespace Memoyed.WebApi.GraphQL.ReturnTypes
{
    /// <summary>
    /// A revision session of cards within a single card box set
    /// </summary>
    [GraphQLName("RevisionSession")]
    public class RevisionSessionType
    {
        /// <summary>
        /// Id of the revision session
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Id of the card box set id the revision session is based on
        /// </summary>
        public Guid CardBoxSetId { get; set; }
        
        /// <summary>
        /// Status of the revision session
        /// </summary>
        public RevisionSessionStatus Status { get; set; }
        
        /// <summary>
        /// Session cards of the revision session
        /// </summary>
        [GetSessionCardsResolver] public IEnumerable<SessionCardType> SessionCards { get; set; }
        
        private class GetSessionCardsResolverAttribute : ObjectFieldDescriptorAttribute
        {
            public override void OnConfigure(IDescriptorContext context, IObjectFieldDescriptor descriptor,
                MemberInfo member)
            {
                descriptor.Resolver(async (ctx, ct) =>
                {
                    var connection = ctx.Service<IDbConnection>();
                    var parent = ctx.Parent<RevisionSessionType>();
                    return await ctx.BatchDataLoader<Guid, IEnumerable<SessionCardType>>(
                        "RevisionSession_GetSessionCards",
                        async (keys, batchCt) =>
                        {
                            const string sql = @"SELECT sc.session_id, sc.card_id, sc.status, 
                                                     sc.native_language_word, sc.target_language_word 
                                                     FROM session_cards AS sc 
                                                     WHERE sc.session_id = ANY(@SessionIds)";

                            var result = await connection.QueryAsync<SessionCardType>(new CommandDefinition(sql, new
                            {
                                SessionIds = keys
                            }, cancellationToken: batchCt));

                            return result
                                .GroupBy(r => r.SessionId)
                                .ToDictionary(r => r.Key, r => r.AsEnumerable());
                        }).LoadAsync(parent.Id, ct);
                });
            }
        }
    }
}
using System;
using System.Data;
using System.Linq;
using Dapper;
using GraphQL.DataLoader;
using GraphQL.Types;
using Memoyed.Application.Dto;
using Microsoft.Extensions.DependencyInjection;

namespace Memoyed.WebApi.GraphQL.Types
{
    public sealed class RevisionSessionType : ObjectGraphType<ReturnModels.RevisionSessionModel>
    {
        public RevisionSessionType(IServiceProvider serviceProvider, IDataLoaderContextAccessor contextAccessor)
        {
            Name = "RevisionSession";
            Description = "A revision session of cards within a single card box set";

            Field(c => c.Id).Description("Id of the revision session");
            Field(c => c.CardBoxSetId);
            Field<NonNullGraphType<RevisionSessionStatusType>>("status", "Status of the revision session", resolve:
                c => c.Source.Status);

            FieldAsync<NonNullGraphType<ListGraphType<SessionCardType>>>("sessionCards",
                "Session cards the user must answer to complete the revision session",
                resolve: async c =>
                {
                    var scope = serviceProvider.CreateScope();
                    var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
                    var dataLoader =
                        contextAccessor.Context.GetOrAddCollectionBatchLoader<Guid, ReturnModels.SessionCardModel>(
                            "GetSessionCards", async ids =>
                            {
                                const string sql = @"SELECT sc.session_id, sc.card_id, sc.status, 
                                                     sc.native_language_word, sc.target_language_word 
                                                     FROM session_cards AS sc 
                                                     WHERE sc.session_id = ANY(@SessionIds)";
                                var result = await connection.QueryAsync<ReturnModels.SessionCardModel>(sql, new
                                {
                                    SessionIds = ids
                                });

                                return result.ToLookup(r => r.SessionId);
                            });

                    return await dataLoader.LoadAsync(c.Source.Id);
                });
        }
    }
}
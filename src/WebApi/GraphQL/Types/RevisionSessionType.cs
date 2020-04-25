using System;
using System.Linq;
using Dapper;
using GraphQL.DataLoader;
using GraphQL.Types;
using Memoyed.Application.DataModel;
using Memoyed.Application.Dto;
using Microsoft.EntityFrameworkCore;

namespace Memoyed.WebApi.GraphQL.Types
{
    public sealed class RevisionSessionType : ObjectGraphType<ReturnModels.RevisionSessionModel>
    {
        public RevisionSessionType(CardsContext cardsDb, IDataLoaderContextAccessor contextAccessor)
        {
            var connection = cardsDb.Database.GetDbConnection();
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
                    var dataLoader =
                        contextAccessor.Context.GetOrAddCollectionBatchLoader<Guid, ReturnModels.SessionCardModel>(
                            "GetSessionCards", async ids =>
                            {
                                const string sql = "SELECT sc.SessionId, sc.CardId, sc.Status, " +
                                                   "sc.NativeLanguageWord, c.TargetLanguageWord " +
                                                   "FROM SessionCards AS sc " +
                                                   "WHERE sc.SessionId IN @SessionIds";
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
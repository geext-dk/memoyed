using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using HotChocolate;
using Memoyed.Application.Dto;
using Memoyed.WebApi.GraphQL.ReturnTypes;

namespace Memoyed.WebApi.GraphQL
{
    public class CardsQuery
    {
        public async Task<IEnumerable<CardBoxSetType>> CardBoxSets(Guid? id, [Service] IDbConnection connection)
        {
            var sql = @"SELECT s.id, s.name, s.native_language, s.target_language
                                FROM card_box_sets AS s";

            if (id.HasValue) sql += " WHERE @Id = s.id";
            return await connection.QueryAsync<CardBoxSetType>(sql, new
            {
                Id = id
            });
        }

        public async Task<IEnumerable<ReturnModels.RevisionSessionModel>> RevisionSessions(Guid? id,
            [Service] IDbConnection connection)
        {
            var sql = @"SELECT rs.id, rs.status, rs.card_box_set_id 
                                FROM revision_sessions AS rs";
            if (id.HasValue) sql += " WHERE @Id = rs.id";

            return await connection.QueryAsync<ReturnModels.RevisionSessionModel>(sql, new
            {
                Id = id
            });
        }
    }
}
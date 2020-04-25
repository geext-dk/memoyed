﻿using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using GraphQL;
using GraphQL.Types;
using Memoyed.Application.Dto;
using Memoyed.WebApi.GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Memoyed.WebApi.GraphQL
{
    public class CardsQuery : ObjectGraphType
    {
        public CardsQuery(IServiceProvider serviceProvider)
        {
            Name = "CardsQuery";

            FieldAsync<ListGraphType<CardBoxSetType>, IEnumerable<ReturnModels.CardBoxSetModel>>("cardBoxSets",
                arguments: new QueryArguments(
                    new QueryArgument<IdGraphType>
                    {
                        Name = "id",
                        Description = "Id of a set to query"
                    }),
                resolve: async c =>
                {
                    using var scope = serviceProvider.CreateScope();
                    var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
                    var sql = @"SELECT s.id, s.name, s.native_language, s.target_language
                                FROM card_box_sets AS s";

                    var id = c.GetArgument<Guid?>("id");
                    if (id.HasValue) sql += " WHERE @Id = s.id";
                    return await connection.QueryAsync<ReturnModels.CardBoxSetModel>(sql, new
                    {
                        Id = id
                    });
                });

            FieldAsync<ListGraphType<RevisionSessionType>, IEnumerable<ReturnModels.RevisionSessionModel>>(
                "revisionSessions",
                arguments: new QueryArguments(new QueryArgument<IdGraphType>
                {
                    Name = "id",
                    Description = "Id of a revision session to query"
                }),
                resolve: async c =>
                {
                    using var scope = serviceProvider.CreateScope();
                    var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
                    var sql = @"SELECT rs.id, rs.status, rs.card_box_set_id 
                                FROM revision_sessions AS rs";

                    var id = c.GetArgument<Guid?>("id");
                    if (id.HasValue) sql += " WHERE @Id = rs.id";

                    return await connection.QueryAsync<ReturnModels.RevisionSessionModel>(sql, new
                    {
                        Id = id
                    });
                });
        }
    }
}
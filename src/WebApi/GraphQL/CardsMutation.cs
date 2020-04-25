using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using GraphQL;
using GraphQL.Types;
using Memoyed.Application.Dto;
using Memoyed.Application.Services;
using Memoyed.WebApi.GraphQL.InputTypes;
using Memoyed.WebApi.GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Memoyed.WebApi.GraphQL
{
    public class CardsMutation : ObjectGraphType
    {
        private static readonly Guid TestUserGuid = Guid.Parse("deadbeef-dead-beef-dead-beef00000075");
        private readonly CardBoxSetsCommandsHandler _commandsHandler;

        private readonly IServiceProvider _serviceProvider;

        public CardsMutation(IServiceProvider serviceProvider, CardBoxSetsCommandsHandler commandsHandler)
        {
            _serviceProvider = serviceProvider;
            _commandsHandler = commandsHandler;

            Name = "CardsMutation";

            FieldAsync<CardBoxSetType>("createCardBoxSet", "Create a card box set for the user",
                new QueryArguments(new QueryArgument<NonNullGraphType<CardBoxSetInput>>
                {
                    Name = "cardBoxSet",
                    Description = "Card box set to create"
                }),
                async c =>
                {
                    var command = c.GetArgument<Commands.CreateCardBoxSetCommand>("cardBoxSet");

                    await commandsHandler.Handle(command, TestUserGuid);

                    return await GetCardBoxSetModel(command.Name);
                });

            FieldAsync<CardBoxSetType>("createCardBox", "Creates a card box in a card box set",
                new QueryArguments(new QueryArgument<NonNullGraphType<CardBoxInput>>
                {
                    Name = "cardBox",
                    Description = "Card box to create"
                }),
                async c =>
                {
                    var command = c.GetArgument<Commands.CreateCardBoxCommand>("cardBox");

                    await commandsHandler.Handle(command, TestUserGuid);

                    return await GetCardBoxSetModel(command.CardBoxSetId);
                });

            FieldAsync<CardBoxSetType>("createCard", "Creates a card in a card box set",
                new QueryArguments(new QueryArgument<NonNullGraphType<CardInput>>
                {
                    Name = "card",
                    Description = "Card to create"
                }),
                async c =>
                {
                    var command = c.GetArgument<Commands.CreateCardCommand>("card");

                    await commandsHandler.Handle(command, TestUserGuid);

                    return await GetCardBoxSetModel(command.CardBoxSetId);
                });

            FieldAsync<CardBoxSetType>("removeCard", "Removes a card from a card box set",
                new QueryArguments(new QueryArgument<NonNullGraphType<RemoveCardInput>>
                {
                    Name = "card",
                    Description = "Card to remove"
                }),
                async c =>
                {
                    var command = c.GetArgument<Commands.RemoveCardCommand>("card");

                    await commandsHandler.Handle(command, TestUserGuid);

                    return await GetCardBoxSetModel(command.CardBoxSetId);
                });

            FieldAsync<CardBoxSetType>("renameCardBoxSet", "Renames a card box set",
                new QueryArguments(new QueryArgument<NonNullGraphType<RenameCardBoxSetInput>>
                {
                    Name = "nameInput",
                    Description = "Card box set to rename, and its new name"
                }),
                async c =>
                {
                    var command = c.GetArgument<Commands.RenameCardBoxSetCommand>("nameInput");

                    await commandsHandler.Handle(command, TestUserGuid);

                    return await GetCardBoxSetModel(command.CardBoxSetId);
                });

            FieldAsync<RevisionSessionType>("startRevisionSession",
                "Starts a revision session from a card box set",
                new QueryArguments(new QueryArgument<NonNullGraphType<StartRevisionSessionInput>>
                {
                    Name = "startRevisionSessionInput",
                    Description = "Id of a card box set from which to start a revision session"
                }),
                async c =>
                {
                    using var scope = serviceProvider.CreateScope();
                    var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
                    var command = c.GetArgument<Commands.StartRevisionSessionCommand>("startRevisionSessionInput");

                    await commandsHandler.Handle(command, TestUserGuid);

                    // TODO: now there is no check that there is only one active revision at the moment.
                    const string sql = @"SELECT rs.id, rs.card_box_set_id, rs.status
                                         FROM revision_sessions as rs 
                                         WHERE rs.card_box_set_id = @CardBoxSetId AND rs.status = 0";

                    return await connection.QueryFirstAsync<ReturnModels.RevisionSessionModel>(sql, new
                    {
                        command.CardBoxSetId
                    });
                });
        }

        private async Task<ReturnModels.CardBoxSetModel> GetCardBoxSetModel(Guid byId)
        {
            return await GetCardBoxSetModel(byId, null);
        }

        private async Task<ReturnModels.CardBoxSetModel> GetCardBoxSetModel(string byName)
        {
            return await GetCardBoxSetModel(null, byName);
        }

        private async Task<ReturnModels.CardBoxSetModel> GetCardBoxSetModel(Guid? byId, string? byName)
        {
            if (byId == null && byName == null)
                throw new InvalidOperationException("You must specify at least 1 argument");

            using var scope = _serviceProvider.CreateScope();
            var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();

            var sql = @"SELECT s.id, s.name, s.native_language, s.target_language 
                        FROM card_box_sets AS s";

            if (byId != null) sql += " WHERE s.id = @Id";

            if (byName != null) sql += (byId == null ? " WHERE" : " AND") + " s.name = @Name";

            return await connection.QueryFirstAsync<ReturnModels.CardBoxSetModel>(sql, new
            {
                Id = byId,
                Name = byName
            });
        }
    }
}
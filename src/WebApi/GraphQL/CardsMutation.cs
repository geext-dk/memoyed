using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using GraphQL;
using GraphQL.Types;
using Memoyed.Application.DataModel;
using Memoyed.Application.Dto;
using Memoyed.Application.Services;
using Memoyed.WebApi.GraphQL.InputTypes;
using Memoyed.WebApi.GraphQL.Types;
using Microsoft.EntityFrameworkCore;

namespace Memoyed.WebApi.GraphQL
{
    public class CardsMutation : ObjectGraphType
    {
        private static readonly Guid TestUserGuid = Guid.Parse("deadbeef-dead-beef-dead-beef00000075");

        private readonly IDbConnection _connection;
        private readonly CardBoxSetsCommandsHandler _commandsHandler;
        public CardsMutation(CardsContext cardsDb, CardBoxSetsCommandsHandler commandsHandler)
        {
            _connection = cardsDb.Database.GetDbConnection();
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
                    var command = c.GetArgument<Commands.StartRevisionSessionCommand>("startRevisionSessionInput");

                    await commandsHandler.Handle(command, TestUserGuid);

                    // TODO: now there is no check that there is only one active revision at the moment.
                    const string sql = "SELECT rs.Id, rs.CardBoxSetId, rs.Status FROM RevisionSessions " +
                                       "WHERE rs.CardBoxSetId = @CardBoxSetId AND rs.Status = 0";

                    return await _connection.QueryFirstAsync<ReturnModels.RevisionSessionModel>(sql, new
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
            {
                throw new InvalidOperationException("You must specify at least 1 argument");
            }
            var sql = "SELECT s.Id, s.Name, s.NativeLanguage, s.TargetLanguage " +
                      "FROM CardBoxSets AS s";

            if (byId != null)
            {
                sql += " WHERE s.Id = @Id";
            }

            if (byName != null)
            {
                sql += (byId == null ? " WHERE" : " AND") + " s.Name = @Name";
            }

            return await _connection.QueryFirstAsync<ReturnModels.CardBoxSetModel>(sql, new
            {
                Id = byId,
                Name = byName
            });
        }
    }
}
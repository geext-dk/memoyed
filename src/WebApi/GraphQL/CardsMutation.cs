using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using HotChocolate;
using Memoyed.Application.Dto;
using Memoyed.Application.Services;
using Memoyed.WebApi.GraphQL.InputTypes;
using Memoyed.WebApi.GraphQL.ReturnTypes;
using Microsoft.Extensions.DependencyInjection;

namespace Memoyed.WebApi.GraphQL
{
    public class CardsMutation
    {
        private static readonly Guid TestUserGuid = Guid.Parse("deadbeef-dead-beef-dead-beef00000075");

        /// <summary>
        /// Create a card box set for the user
        /// </summary>
        /// <param name="input"></param>
        /// <param name="connection"></param>
        /// <param name="commandsHandler"></param>
        /// <returns></returns>
        public async Task<CardBoxSetType> CreateCardBoxSet(CreateCardBoxSetInput input,
            [Service] IDbConnection connection, [Service]CardBoxSetsCommandsHandler commandsHandler)
        {
            await commandsHandler.Handle(input, TestUserGuid);

            return await GetCardBoxSetModel(connection, input.Name);
        }

        /// <summary>
        /// Creates a card box in a card box set
        /// </summary>
        /// <param name="input">Card box to create</param>
        /// <param name="connection"></param>
        /// <param name="commandsHandler"></param>
        /// <returns></returns>
        public async Task<CardBoxSetType> CreateCardBox(CreateCardBoxInput input,
            [Service] IDbConnection connection, [Service] CardBoxSetsCommandsHandler commandsHandler)
        {
            await commandsHandler.Handle(input, TestUserGuid);

            return await GetCardBoxSetModel(connection, input.CardBoxSetId);
        }

        /// <summary>
        /// Creates a card in a card box set
        /// </summary>
        /// <param name="input">Card to create</param>
        /// <param name="connection"></param>
        /// <param name="commandsHandler"></param>
        /// <returns></returns>
        public async Task<CardBoxSetType> CreateCard(CreateCardInput input,
            [Service] IDbConnection connection, [Service] CardBoxSetsCommandsHandler commandsHandler)
        {
            await commandsHandler.Handle(input, TestUserGuid);

            return await GetCardBoxSetModel(connection, input.CardBoxSetId);
        }

        /// <summary>
        /// Removes a card from a card box set
        /// </summary>
        /// <param name="input">Card to remove</param>
        /// <param name="connection"></param>
        /// <param name="commandsHandler"></param>
        /// <returns></returns>
        public async Task<CardBoxSetType> RemoveCard(RemoveCardInput input,
            [Service] IDbConnection connection, [Service] CardBoxSetsCommandsHandler commandsHandler)
        {
            await commandsHandler.Handle(input, TestUserGuid);

            return await GetCardBoxSetModel(connection, input.CardBoxSetId);
        }

        /// <summary>
        /// Renames a card box set
        /// </summary>
        /// <param name="input">Card box set to rename, and its new name</param>
        /// <param name="connection"></param>
        /// <param name="commandsHandler"></param>
        /// <returns></returns>
        public async Task<CardBoxSetType> RenameCardBoxSet(RenameCardBoxSetInput input,
            [Service] IDbConnection connection, [Service] CardBoxSetsCommandsHandler commandsHandler)
        {
            await commandsHandler.Handle(input, TestUserGuid);

            return await GetCardBoxSetModel(connection, input.CardBoxSetId);
        }
        
        /// <summary>
        /// Starts a revision session from a card box set
        /// </summary>
        /// <param name="input">Id of a card box set from which to start a revision session</param>
        /// <param name="connection"></param>
        /// <param name="commandsHandler"></param>
        /// <returns></returns>
        public async Task<RevisionSessionType> StartRevisionSession(StartRevisionSessionInput input,
            [Service] IDbConnection connection, [Service] RevisionSessionsCommandsHandler commandsHandler)
        {
            await commandsHandler.Handle(input, TestUserGuid);

            // TODO: currently there is no check that there is only one active revision at the moment.
            return await GetRevisionSessionModelBySetId(connection, input.CardBoxSetId);
        }
        
        /// <summary>
        /// Answer a session card with a given answer
        /// </summary>
        /// <param name="input">An input object for answering a card</param>
        /// <param name="connection"></param>
        /// <param name="commandsHandler"></param>
        /// <returns></returns>
        public async Task<RevisionSessionType> AnswerCard(SetCardAnswerInput input,
            [Service] IDbConnection connection, [Service] RevisionSessionsCommandsHandler commandsHandler)
        {
            await commandsHandler.Handle(input, TestUserGuid);

            // TODO: currently there is no check that there is only one active revision at the moment.
            return await GetRevisionSessionModelById(connection, input.RevisionSessionId);
        }
        
        /// <summary>
        /// Completed a revision session with the given id
        /// </summary>
        /// <param name="input">An input object for completing revision sessions</param>
        /// <param name="connection"></param>
        /// <param name="commandsHandler"></param>
        /// <returns></returns>
        public async Task<RevisionSessionType> CompleteRevisionSession(CompleteRevisionSessionInput input,
            [Service] IDbConnection connection, [Service] RevisionSessionsCommandsHandler commandsHandler)
        {
            await commandsHandler.Handle(input, TestUserGuid);

            return await GetRevisionSessionModelById(connection, input.RevisionSessionId);
        }

        private static async Task<CardBoxSetType> GetCardBoxSetModel(IDbConnection connection, Guid byId)
        {
            return await GetCardBoxSetModel(connection, byId, null);
        }

        private static async Task<CardBoxSetType> GetCardBoxSetModel(IDbConnection connection, string byName)
        {
            return await GetCardBoxSetModel(connection, null, byName);
        }

        private static async Task<RevisionSessionType> GetRevisionSessionModelById(IDbConnection connection,
            Guid id)
        {
            return await GetRevisionSessionModel(connection, id, null);
        }

        private static async Task<RevisionSessionType> GetRevisionSessionModelBySetId(IDbConnection connection,
            Guid setId)
        {
            return await GetRevisionSessionModel(connection, null, setId);
        }

        private static async Task<RevisionSessionType> GetRevisionSessionModel(IDbConnection connection,
            Guid? byId, Guid? bySetId)
        {
            if (byId == null && bySetId == null)
                throw new InvalidOperationException("You must specify at least 1 argument");

            var sql = @"SELECT rs.id, rs.status, rs.card_box_set_id 
                        FROM revision_sessions AS rs";

            if (byId != null) sql += " WHERE rs.id = @Id";

            if (bySetId!= null) sql += (byId == null ? " WHERE" : " AND") + " rs.card_box_set_id = @CardBoxSetId";

            return await connection.QueryFirstAsync<RevisionSessionType>(sql, new
            {
                Id = byId,
                CardBoxSetId = bySetId
            });
        }

        private static async Task<CardBoxSetType> GetCardBoxSetModel(IDbConnection connection, Guid? byId,
            string? byName)
        {
            if (byId == null && byName == null)
                throw new InvalidOperationException("You must specify at least 1 argument");

            var sql = @"SELECT s.id, s.name, s.native_language, s.target_language 
                        FROM card_box_sets AS s";

            if (byId != null) sql += " WHERE s.id = @Id";

            if (byName != null) sql += (byId == null ? " WHERE" : " AND") + " s.name = @Name";

            return await connection.QueryFirstAsync<CardBoxSetType>(sql, new
            {
                Id = byId,
                Name = byName
            });
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Memoyed.Cards.ApplicationServices.Dto;
using Memoyed.Cards.ApplicationServices.Services;
using Memoyed.Cards.Domain.CardBoxSets;
using Microsoft.AspNetCore.Mvc;

namespace Memoyed.Cards.Api.Controllers
{
    [ApiController]
    [Route("api/cardBoxSets")]
    public class CardBoxSetsController : ControllerBase
    {
        private readonly CardBoxSetsCommandsHandler _commandsHandler;
        private readonly CardBoxSetsQueriesHandler _queriesHandler;

        public CardBoxSetsController(CardBoxSetsCommandsHandler commandsHandler, CardBoxSetsQueriesHandler queriesHandler)
        {
            _commandsHandler = commandsHandler;
            _queriesHandler = queriesHandler;
        }

        [HttpGet]
        public async Task<IEnumerable<ReturnModels.CardBoxSetModel>> GetCardBoxSets(
            [FromQuery]Queries.GetCardBoxSetsQuery query)
        {
            return await _queriesHandler.GetCardBoxSets(query);
        }

        [Route("{setId:guid}")]
        [HttpGet]
        public async Task<IEnumerable<ReturnModels.CardBoxModel>> GetCardBoxes(Guid setId,
            [FromQuery]Queries.GetCardBoxesQuery query)
        {
            query.CardBoxSetId = setId;
            return await _queriesHandler.GetCardBoxes(query);
        }

        [Route("{setId:guid}/{cardBoxId:guid}")]
        [HttpGet]
        public async Task<IEnumerable<ReturnModels.LearningCardModel>> GetLearningCards(Guid setId, Guid boxId,
            [FromQuery]Queries.GetLearningCardsQuery query)
        {
            query.CardBoxSetId = setId;
            query.CardBoxId = boxId;
            return await _queriesHandler.GetLearningCards(query);
        }

        [HttpPost]
        [Route("createSet")]
        public async Task CreateCardBoxSet(Commands.CreateCardBoxSetCommand command)
        {
            await _commandsHandler.CreateCardBoxSet(command);
        }

        [HttpPost]
        [Route("createBox")]
        public async Task CreateCardBox(Commands.CreateCardBoxCommand command)
        {
            await _commandsHandler.CreateCardBox(command);
        }

        [HttpPost]
        [Route("createCard")]
        public async Task CreateLearning(Commands.AddNewLearningCardCommand command)
        {
            await _commandsHandler.AddNewLearningCard(command);
        }
    }
}
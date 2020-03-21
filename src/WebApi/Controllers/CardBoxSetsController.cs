﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Memoyed.Application.Dto;
using Memoyed.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Memoyed.WebApi.Controllers
{
    [ApiController]
    [Route("api/cardBoxSets")]
    public class CardBoxSetsController : ControllerBase
    {
        private readonly CardBoxSetsCommandsHandler _commandsHandler;
        private readonly CardBoxSetsQueriesHandler _queriesHandler;

        public CardBoxSetsController(CardBoxSetsCommandsHandler commandsHandler,
            CardBoxSetsQueriesHandler queriesHandler)
        {
            _commandsHandler = commandsHandler;
            _queriesHandler = queriesHandler;
        }

        [HttpGet("getCardBoxSets")]
        public async Task<IEnumerable<ReturnModels.CardBoxSetModel>> GetCardBoxSets(
            [FromQuery] Queries.GetCardBoxSetsQuery query)
        {
            return await _queriesHandler.GetCardBoxSets(query);
        }

        [HttpGet("getCardBoxes")]
        public async Task<IEnumerable<ReturnModels.CardBoxModel>> GetCardBoxes(
            [FromQuery] Queries.GetCardBoxesQuery query)
        {
            return await _queriesHandler.GetCardBoxes(query);
        }

        [HttpGet("getCards")]
        public async Task<IEnumerable<ReturnModels.CardModel>> GetCards(
            [FromQuery] Queries.GetCardsQuery query)
        {
            return await _queriesHandler.GetCards(query);
        }

        [HttpPost("createSet")]
        public async Task CreateCardBoxSet(Commands.CreateCardBoxSetCommand command)
        {
            await _commandsHandler.Handle(command, Guid.Empty);
        }

        [HttpPost("createBox")]
        public async Task CreateCardBox(Commands.CreateCardBoxCommand command)
        {
            await _commandsHandler.Handle(command, Guid.Empty);
        }

        [HttpPost("createCard")]
        public async Task CreateCard(Commands.AddNewCardCommand command)
        {
            await _commandsHandler.Handle(command, Guid.Empty);
        }
    }
}
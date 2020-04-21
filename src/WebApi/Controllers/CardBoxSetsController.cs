using System;
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
        private static readonly Guid TestUserGuid = Guid.Parse("deadbeef-dead-beef-dead-beef00000075");

        public CardBoxSetsController(CardBoxSetsCommandsHandler commandsHandler)
        {
            _commandsHandler = commandsHandler;
        }

        [HttpPost("createSet")]
        public async Task CreateCardBoxSet(Commands.CreateCardBoxSetCommand command)
        {
            await _commandsHandler.Handle(command, TestUserGuid);
        }

        [HttpPost("createBox")]
        public async Task CreateCardBox(Commands.CreateCardBoxCommand command)
        {
            await _commandsHandler.Handle(command, TestUserGuid);
        }

        [HttpPost("createCard")]
        public async Task CreateCard(Commands.AddNewCardCommand command)
        {
            await _commandsHandler.Handle(command, TestUserGuid);
        }
    }
}
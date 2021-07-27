using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CustomCommandBot.Shared.Models.Api;
using CustomCommandBot.Shared.Models;

namespace CustomCommandBot.Server.Controllers
{
    [Route("api/guild/{guildId}")]
    [Authorize]
    public class GuildController : ControllerBase
    {
        /*
        [HttpGet]
        public ActionResult<ApiGuild> GetGuild()
        {
            // Get a guild by ID
        }

        [HttpGet("commands")]
        public ActionResult<Command> GetGuildCommands()
        {
            // Get a paginated list of commands
        }

        [HttpGet("commands/{commandId}")]
        public ActionResult<Command> GetGuildCommand()
        {
            // Get a full command
        }

        [HttpPost("commands/{commandId}")]
        public ActionResult SetGuildCommand()
        {
            // Update a command
        }
        */
    }
}

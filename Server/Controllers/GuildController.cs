using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CustomCommandBot.Shared.Models.Api;
using CustomCommandBot.Shared.Models;
using CustomCommandBot.Shared.Models.Discord;
using CustomCommandBot.Server.Bot;
using Discord.WebSocket;
using CustomCommandBot.Server.Models.Pagination;
using System.Collections.Generic;
using LiteDB;
using System.ComponentModel.Design;
using System.Web;
using CustomCommandBot.Client.Pages.Dashboard;

namespace CustomCommandBot.Server.Controllers
{
    [Route("api/guild/{guildId}")]
    [Authorize]
    public class GuildController : ControllerBase
    {
        private DiscordClient _client;

        public GuildController(DiscordClient client)
        {
            _client = client;
        }

        private User _getUser()
            => Shared.Models.Discord.User.FromClaims(User.Claims.ToList(), _client.GetGuildIds());

        private SocketGuild _getClientGuild(string guildId)
            => _client.Client.Guilds.First(g => g.Id == ulong.Parse(guildId));

        private ActionResult<ApiGuild> _getGuild([FromRoute] string guildId)
        {
            var clientGuild = _getClientGuild(guildId);

            if (clientGuild is null)
                return NotFound();

            var user = _getUser();
            var userGuild = user.Guilds.First(g => g.Id == guildId);

            if (userGuild is null)
                return Forbid();

            var guild = ApiGuild.FromSocket(clientGuild, userGuild);

            return guild;
        }

        [HttpGet]
        public ActionResult<ApiGuild> GetGuild([FromRoute] string guildId)
        {
            var guild = _getGuild(guildId);

            if (guild.Value is null)
                return NotFound();

            return new JsonResult(guild.Value);
        }

        [HttpGet("commands/{commandId}")]
        public ActionResult<Command> GetGuildCommand([FromRoute] string guildId, [FromRoute] string commandId)
        {
            var guild = _getGuild(guildId);

            if (guild.Value is null)
                return NotFound();

            using (var database = new LiteDatabase(@"Databases/Commands.db"))
            {
                var collection = database.GetCollection<Command>("g" + guildId);
                collection.EnsureIndex(d => d.TriggerType);

                var command = collection.Find((c) => c.Trigger == commandId);

                if (!command.Any())
                    return NotFound();

                return new JsonResult(command.First());
            }
        }

        [HttpPost("commands/{commandId}")]
        public ActionResult SetGuildCommand([FromRoute] string guildId, [FromRoute] string commandId, [FromBody] Command newCommand)
        {
            var guild = _getGuild(guildId);

            if (guild.Value is null)
                return NotFound();

            using (var database = new LiteDatabase(@"Databases/Commands.db"))
            {
                var collection = database.GetCollection<Command>("g" + guildId);
                collection.EnsureIndex(d => d.TriggerType);

                var command = collection.Find((c) => c.Trigger == commandId).First();

                if (command is null)
                {
                    collection.Insert(newCommand);

                    return Created(HttpContext.Request.PathBase + $"/api/guild/{guildId}/commands/{HttpUtility.UrlEncode(newCommand.Trigger)}", null);
                }

                collection.Update(commandId, newCommand);

                return Ok();
            }
        }
    }
}

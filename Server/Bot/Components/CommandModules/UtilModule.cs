using CustomCommandBot.Server.Bot.Components.CommandHandler;
using CustomCommandBot.Server.Extentions;
using CustomCommandBot.Shared.Models;
using CustomCommandBot.Shared.Models.CommandActions;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomCommandBot.Server.Bot.Components
{
    public class UtilModule : ModuleBase<SocketCommandContext>
    {
        [Command("guild-count")]
        [Summary("Get guild count for debugging.")]
        public async Task<RuntimeResult> GuildCount()
        {
            return CommandResult.FromSuccess(Context.Client.Guilds.Count.ToString());
        }
    }
}

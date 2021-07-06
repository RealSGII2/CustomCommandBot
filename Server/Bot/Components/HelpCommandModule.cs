using CustomCommandBot.Server.Bot.Components.CommandHandler;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomCommandBot.Server.Bot.Components
{
    public class HelpCommandModule : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        [Summary("Get information about the bot and a server.")]
        [Alias("commands", "cmds")]
        public async Task<RuntimeResult> TestCommand()
        {
            return CommandResult.FromError("Unimplemented.");
        }
    }
}

using CustomCommandBot.Server.Bot.Components.CommandHandler;
using CustomCommandBot.Server.Extentions;
using CustomCommandBot.Shared.Models;
using CustomCommandBot.Shared.Models.CommandActions;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomCommandBot.Server.Bot.Components.CommandModules
{
    public class HelpCommandModule : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        [Summary("Get information about the bot and a server.")]
        [Alias("commands", "cmds")]
        public async Task<RuntimeResult> TestCommand()
        {
            return CommandExceptionResult.FromError(new("Command not implemented", Environment.StackTrace));
        }

        [Command("add-test-command")]
        [Summary("Add a command to test the bot")]
        [RequireOwner]
        public async Task<RuntimeResult> AddTestCommand()
        {
            Context.Guild.AddCommand(new()
            {
                Trigger = "!test",
                TriggerType = CommandTriggerType.BeginsWith,
                Actions = new()
                {
                    new ReplyAction()
                    {
                        Content = "Test succeeded."
                    }
                }
            });

            return CommandResult.FromSuccess("`!test` command added.");
        }
    }
}

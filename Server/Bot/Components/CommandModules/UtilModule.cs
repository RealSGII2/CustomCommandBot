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

        [Command("add-test-command")]
        [Summary("Add a command to test the bot")]
        [RequireOwner]
        public async Task<RuntimeResult> AddTestCommand()
        {
            Context.Guild.AddCommand(new()
            {
                Trigger = "!test",
                TriggerType = CommandTriggerType.BeginsWith,
                Description = "Test command for use in testing custom commands",
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

        [Command("add-test-2-command")]
        [Summary("Add a command to test the bot")]
        [RequireOwner]
        public async Task<RuntimeResult> AddTest2Command()
        {
            Context.Guild.AddCommand(new()
            {
                Trigger = "!test=2",
                TriggerType = CommandTriggerType.BeginsWith,
                Description = "Test command for use in testing custom commands",
                Actions = new()
                {
                    new ReplyAction()
                    {
                        Content = "Test didn't fail."
                    }
                }
            });

            return CommandResult.FromSuccess("`!test-2` command added.");
        }
    }
}

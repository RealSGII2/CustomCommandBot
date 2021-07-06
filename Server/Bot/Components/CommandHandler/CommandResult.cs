using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomCommandBot.Server.Bot.Components.CommandHandler
{
    public class CommandResult : RuntimeResult
    {
        public CommandResult(CommandError? error, string reason) : base(error, reason)
        {
        }

        public static CommandResult FromError(string reason) =>
            new(CommandError.Unsuccessful, reason);

        public static CommandResult FromSuccess(string reason = null) =>
            new(null, reason);
    }
}

using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomCommandBot.Server.Bot.Components.CommandHandler
{
    public class CommandExceptionResult : RuntimeResult
    {
        public CommandExceptionResult(CommandError? error, string reason) : base(error, reason)
        {
        }

        public static CommandExceptionResult FromError(string reason) =>
            new(CommandError.Unsuccessful, reason);
    }
}

using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomCommandBot.Server.Bot.Components.CommandHandler
{
    public class CommandExceptionResult : RuntimeResult
    {
        public readonly CommandFailureException Exception;

        public CommandExceptionResult(CommandFailureException exception) : base(CommandError.Unsuccessful, exception.Message)
        {
            Exception = exception;
        }

        public static CommandExceptionResult FromError(CommandFailureException exception) =>
            new(exception);
    }
}

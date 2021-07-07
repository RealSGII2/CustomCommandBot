using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomCommandBot.Server.Bot.Components.Logging
{
    public class ExceptionLog : Log
    {
        private readonly Exception Exception;
        private string ErrorCode;

        public ExceptionLog(Exception exception, DiscordSocketClient client) : base(client)
        {
            Exception = exception;
        }

        public ExceptionLog(Exception exception, SocketCommandContext context) : base(context)
        {
            Exception = exception;
        }

        public void AddErrorCode(string errorCode)
        {
            ErrorCode = errorCode;
        }

        public override async Task Send()
        {
            var embed = GetBaseEmbed("Exception Log");
            embed.Color = new(207, 102, 121);

            if (Exception is CommandFailureException)
            {
                embed.AddField(new EmbedFieldBuilder()
                {
                    Name = "Exception Type",
                    Value = $"`{(Exception as CommandFailureException).Type}`",
                    IsInline = true
                });
            }

            if (ErrorCode is not null)
            {
                embed.AddField(new EmbedFieldBuilder()
                {
                    Name = "Error Code",
                    Value = $"`{ErrorCode}`",
                    IsInline = true
                });
            }

            if (Context is not null)
            {
                embed.AddField(new EmbedFieldBuilder()
                {
                    Name = "Command Used Like",
                    Value = $"```{Context.Message.Content}```"
                });
            }

            embed.AddField(new EmbedFieldBuilder()
            {
                Name = "Message",
                Value = $"```{Exception.Message ?? "N/A"}```"
            });

            embed.AddField(new EmbedFieldBuilder()
            {
                Name = "Stack Trace",
                Value = $"```{Exception.StackTrace?.Truncate(1000) ?? "N/A"}```"
            });

            await SendToChannelWithID(861886092264865802, embed);
        }
    }
}

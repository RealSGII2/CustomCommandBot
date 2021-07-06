using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomCommandBot.Server.Bot.Components.Logging
{
    public class ExceptionLog : Log
    {
        private readonly Exception Exception;

        public ExceptionLog(Exception exception, DiscordSocketClient client) : base(client)
        {
            Exception = exception;
        }

        public ExceptionLog(Exception exception, SocketCommandContext context) : base(context)
        {
            Exception = exception;
        }

        public override async Task Send()
        {
            var embed = GetBaseEmbed("Exception Log");
            embed.Color = new(207, 102, 121);
            embed.Description = $"**Exception Type:** {Exception.GetType()}";

            embed.AddField(new EmbedFieldBuilder()
            {
                Name = "Message",
                Value = $"```{Exception.Message ?? "N/A"}```"
            });

            embed.AddField(new EmbedFieldBuilder()
            {
                Name = "Stack Trace",
                Value = $"```{Exception.StackTrace ?? "N/A"}```"
            });

            await SendToChannelWithID(861886092264865802, embed);
        }
    }
}

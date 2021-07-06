using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomCommandBot.Server.Bot.Components.Logging
{
    public abstract class Log
    {
        protected readonly DiscordSocketClient Client;
        protected readonly SocketCommandContext Context;

        public Log(SocketCommandContext _context)
        {
            Client = _context.Client;
            Context = _context;
        }

        public Log(DiscordSocketClient _client)
        {
            Client = _client;
        }

        public abstract Task Send();

        protected async Task SendToChannelWithID(ulong id, EmbedBuilder embed)
        {
            var channel = await Client.GetChannelAsync(id) as IMessageChannel;

            if (channel is not null)
            {
                if (Context is not null)
                {
                    embed.Author = new()
                    {
                        IconUrl = Context.Guild.IconUrl,
                        Name = $"{Context.Guild.Name}"
                    };
                }

                embed.Footer = new()
                {
                    IconUrl = Client.CurrentUser.GetAvatarUrl(),
                    Text = $"{Client.CurrentUser.Username}#{Client.CurrentUser.Discriminator}"
                };

                await channel.SendMessageAsync(embed: embed.Build());
            }
            else
            {
                Console.Error.WriteLine("HIGH PRIORITY ERROR: Unable to find Channel to log to");
            }
        }

        protected EmbedBuilder GetBaseEmbed(string type, string message = null)
        {
            EmbedBuilder embed = new()
            {
                Title = $"New {type}",
                Timestamp = DateTime.Now
            };

            if (Context is not null)
            {
                embed.AddField(new EmbedFieldBuilder()
                {
                    Name = "Executor ID",
                    Value = Context.Message.Author.Id,
                    IsInline = true
                });

                embed.AddField(new EmbedFieldBuilder()
                {
                    Name = "Channel ID",
                    Value = Context.Channel.Id,
                    IsInline = true
                });

                embed.AddField(new EmbedFieldBuilder()
                {
                    Name = "Guild ID",
                    Value = Context.Guild.Id,
                    IsInline = true
                });

                embed.AddField(new EmbedFieldBuilder()
                {
                    Name = "Command Used Like",
                    Value = $"```{Context.Message.Content}```"
                });
            }

            if (message is not null)
                embed.AddField(new EmbedFieldBuilder()
                {
                    Name = "Message",
                    Value = message
                });

            return embed;
        }
    }
}

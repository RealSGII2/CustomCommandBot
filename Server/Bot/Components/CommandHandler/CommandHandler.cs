using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CustomCommandBot.Server.Bot.Components.CommandHandler
{
    public class CommandHandler
    {
        private DiscordSocketClient Client;
        private CommandService CommandService;
        private IServiceProvider Services;

        public CommandHandler(IServiceProvider services, CommandService commandService, DiscordSocketClient client)
        {
            Client = client;
            CommandService = commandService;
            Services = services;

            CommandService.Log += Log;
        }

        private Task Log(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }

        public async Task Setup()
        {
            Client.MessageReceived += OnMessage;
            CommandService.CommandExecuted += OnCommandExecuted;

            await CommandService.AddModuleAsync<HelpCommandModule>(Services);
        }

        private async Task OnMessage(SocketMessage _message)
        {
            var message = _message as SocketUserMessage;

            if (message is null)
                return;

            int argPosition = 0;

            if (!message.HasCharPrefix('!', ref argPosition) || message.Author.IsBot)
                return;

            SocketCommandContext context = new(Client, message);

            var result = await CommandService.ExecuteAsync(
                context: context,
                argPos: argPosition,
                services: null
            );

           /* if (!result.IsSuccess && result.Error is not CommandError.UnknownCommand)
            {
                EmbedBuilder errorEmbed = new()
                {
                    Author = new()
                    {
                        Name = "Command Failed"
                    },
                    Description = $"**Reason:** {result.ErrorReason}",
                    Color = new(207, 102, 121)
                };

                await message.Channel.SendMessageAsync(embed: errorEmbed.Build(), component: null);
            }*/
        }

        private async Task OnCommandExecuted(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (result is CommandResult && result.IsSuccess)
            {
                if (!string.IsNullOrEmpty(result.ErrorReason))
                    await context.Channel.SendMessageAsync(result.ErrorReason);

                return;
            }

            if (result.IsSuccess || result.Error == CommandError.UnknownCommand)
                return;

            EmbedBuilder errorEmbed = new()
            {
                Title = "Command Failed",
                Description = $"**Reason:** {result.ErrorReason}",
                Color = new(207, 102, 121)
            };

            await context.Message.Channel.SendMessageAsync(embed: errorEmbed.Build(), component: null);
        }
    }
}

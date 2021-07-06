using CustomCommandBot.Server.Extentions;
using CustomCommandBot.Shared.Models;
using CustomCommandBot.Shared.Models.CommandActions;
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

            if (message is null || message.Author.IsBot)
                return;

            SocketCommandContext context = new(Client, message);

            // Check if custom command
            var results = context.Guild.GetCommands().Where(command =>
                (command.TriggerType == CommandTriggerType.BeginsWith && message.Content.StartsWith(command.Trigger)) ||
                (command.TriggerType == CommandTriggerType.EndsWith && message.Content.EndsWith(command.Trigger)) ||
                (command.TriggerType == CommandTriggerType.Contains && message.Content.Contains(command.Trigger))
            ).ToList();

            if (results.Count == 0)
            {
                // Nope, check if bot command
                int argPosition = 0;

                // Check if it starts with `!`
                if (!message.HasCharPrefix('!', ref argPosition))
                    return;

                var result = await CommandService.ExecuteAsync(
                    context: context,
                    argPos: argPosition,
                    services: null
                );
            }
            else
            {
                // Yes, it is a custom command
                var command = results[0];

                foreach (ICommandAction action in command.Actions)
                {
                    await action.OnExecute(context);
                }
            }
        }

        private async Task OnCommandExecuted(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {

            switch (result)
            {
                case CommandExceptionResult:

                    EmbedBuilder errorEmbed = new()
                    {
                        Title = "Command Failed",
                        Description = "Oops! That wasn't supposed to happen. Please make a [new issue](https://github.com/RealSGII2/CustomCommandBot/issues/new) on our GitHub following the provided format.",
                        Color = new(207, 102, 121)
                    };

                    errorEmbed.AddField("Executor ID", context.User.Id, true);
                    errorEmbed.AddField("Guild ID", context.Guild.Id, true);
                    errorEmbed.AddField("Error", $"```{result.ErrorReason}```");

                    await context.Message.Channel.SendMessageAsync(embed: errorEmbed.Build(), component: null);

                    break;
                case CommandResult:

                    if (result.IsSuccess)
                    {
                        if (!string.IsNullOrEmpty(result.ErrorReason))
                            await context.Channel.SendMessageAsync(result.ErrorReason);
                        return;
                    }

                    if (result.IsSuccess || result.Error == CommandError.UnknownCommand)
                        return;

                    break;
            }
        }
    }
}

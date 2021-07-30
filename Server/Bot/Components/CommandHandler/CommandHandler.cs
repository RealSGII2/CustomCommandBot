using CustomCommandBot.Server.Bot.Components.Logging;
using CustomCommandBot.Server.Bot.Components.CommandModules;
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

        private CustomCodeRunner _customCode;

        public CommandHandler(IServiceProvider services, CommandService commandService, DiscordSocketClient client)
        {
            Client = client;
            CommandService = commandService;
            Services = services;

            _customCode = new();

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
            await CommandService.AddModuleAsync<UtilModule>(Services);
        }

        private async Task OnMessage(SocketMessage _message)
        {
            var message = _message as SocketUserMessage;

            if (message is null || message.Author.IsBot)
                return;

            SocketCommandContext context = new(Client, message);

            var chunks = message.Content.Split(' ');

            // Check if custom command
            var results = context.Guild.GetCommands().Where(command =>
                (command.TriggerType == CommandTriggerType.BeginsWith && chunks[0] == command.Trigger) ||
                (command.TriggerType == CommandTriggerType.EndsWith && chunks.Last() == command.Trigger) ||
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

                if (command.CustomCode is not null)
                    _customCode.Run(command.CustomCode, context);

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

                    var exception = (result as CommandExceptionResult).Exception;
                    var errorCode = exception.GetErrorCode(context.Guild.Id);

                    var log = new ExceptionLog(exception, context as SocketCommandContext);
                    log.AddErrorCode(errorCode);
                    await log.Send();

                    string issueURL = "https://github.com/RealSGII2/CustomCommandBot/issues/new?assignees=&labels=bug&template=bug-report.md&title=bug%3A+";

                    EmbedBuilder errorEmbed = new()
                    {
                        Title = "Command Failed",
                        Description = $"Oops! That wasn't supposed to happen. Please make a [new issue]({issueURL}) on our GitHub, providing the error code below.",
                        Color = new(207, 102, 121)
                    };

                    errorEmbed.AddField("Error Code", $"```{errorCode}```");
                    errorEmbed.AddField("Message (do not include in bug report)", $"```{exception.Message}```");


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

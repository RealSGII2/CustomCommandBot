using CustomCommandBot.Server.Bot.Components.CommandHandler;
using CustomCommandBot.Server.Components;
using CustomCommandBot.Server.Config.Models;
using CustomCommandBot.Shared.Models.Discord;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomCommandBot.Server.Bot
{
    public class DiscordClient
    {
        public DiscordSocketClient Client;
        private CommandService CommandService;

        public DiscordClient()
        {
            Client = new();
            Client.Log += Log;

            Start();
        }

        private async Task Start()
        {
            string token = ConfigReader.ReadConfigFile<SecretsConfig>(@"Config/secrets.yaml").BotToken;

            await Client.LoginAsync(TokenType.Bot, token);
            await Client.StartAsync();

            CommandService = new(new CommandServiceConfig()
            {
                CaseSensitiveCommands = false
            });

            var provider = ConfigureServices();
            await provider.GetRequiredService<CommandHandler>().Setup();
        }

        public List<ulong> GetGuildIds()
            => Client.Guilds.Select(g => g.Id).ToList();

        private IServiceProvider ConfigureServices()
         => new ServiceCollection()
                .AddSingleton(Client)
                .AddSingleton(CommandService)
                .AddSingleton<CommandHandler>()
                .BuildServiceProvider();

        private Task Log(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }
    }
}

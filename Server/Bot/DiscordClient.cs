using CustomCommandBot.Server.Bot.Components.CommandHandler;
using CustomCommandBot.Server.Components;
using CustomCommandBot.Server.Config.Models;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace CustomCommandBot.Server.Bot
{
    public class DiscordClient
    {
        private DiscordSocketClient Client;
        private CommandService CommandService;

        public static void Start()
            => new DiscordClient().Create().GetAwaiter().GetResult();

        public async Task Create()
        {
            Client = new();

            Client.Log += Log;

            string token = ConfigReader.ReadConfigFile<SecretsConfig>(@"Config/secrets.yaml").BotToken;

            await Client.LoginAsync(TokenType.Bot, token);
            await Client.StartAsync();

            CommandService = new(new CommandServiceConfig()
            {
                CaseSensitiveCommands = false
            });

            var provider = ConfigureServices();
            await provider.GetRequiredService<CommandHandler>().Setup();
            

            // await Task.Delay(-1);
        }

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

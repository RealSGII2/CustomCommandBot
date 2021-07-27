using CustomCommandBot.Client.Pages.Dashboard;
using CustomCommandBot.Server.Bot;
using CustomCommandBot.Server.Bot.Components.CommandHandler;
using CustomCommandBot.Server.Extentions;
using CustomCommandBot.Shared.Models;
using CustomCommandBot.Shared.Models.CommandActions;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomCommandBot.Server.Bot.Components.CommandModules
{
    public class HelpCommandModule : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _commandService;

        public HelpCommandModule(CommandService commandService)
        {
            _commandService = commandService;
        }

        [Command("help")]
        [Summary("Get information about the bot and a server.")]
        [Alias("commands", "cmds")]
        public async Task<RuntimeResult> GuildHelpCommand()
        {
            var helpEmbed = new EmbedBuilder()
            {
                Author = new EmbedAuthorBuilder()
                {
                    Name = $"{Context.Guild.Name}'s Commands",
                    IconUrl = $"{Context.Guild.IconUrl}"
                },
                Color = Colors.Blue,
                Description = "Use `!help [command]` to see a detailed description and arguments."
            };

            var commands = _commandService.Commands.ToList();
            commands.Sort((x, y) => x.Name.CompareTo(y.Name));

            var baseCommands = "";
            foreach (var command in commands)
            {
                if (!(await command.CheckPreconditionsAsync(Context)).IsSuccess)
                    continue;

                baseCommands += $"- `!{command.Name}` - {command.Summary ?? "*No description.*"}\n";
            }

            var guildCommands = "";
            using (var database = new LiteDatabase(@"Databases/Commands.db"))
            {
                var collection = database.GetCollection<Command>("g" + Context.Guild.Id.ToString());
                collection.EnsureIndex(d => d.TriggerType);

                foreach (var command in collection.FindAll())
                {
                    guildCommands += $"- `{command.Trigger}` - {command.Description ?? "*No description.*"}\n";
                }
            }

            if (string.IsNullOrEmpty(guildCommands))
                guildCommands = $"{Context.Guild.Name} has no commands set.";

            helpEmbed.AddField("Global Commands", baseCommands);
            helpEmbed.AddField("Server Commands", guildCommands);

            await Context.Channel.SendMessageAsync(embed: helpEmbed.Build());

            return CommandResult.FromSuccess($"{Icons.Message} I've DM'd you a list of commands. (haha no doing it here because funny)");
        }

        [Command("help")]
        [Summary("Get information about the bot and a server.")]
        [Alias("commands", "cmds")]
        public async Task<RuntimeResult> GuildDetailedHelpCommand(string commandName)
        {
            var search = _commandService.Commands.Where(c => c.Name.Contains(commandName));

            using (var database = new LiteDatabase(@"Databases/Commands.db"))
            {
                var collection = database.GetCollection<Command>("g" + Context.Guild.Id.ToString());
                collection.EnsureIndex(d => d.TriggerType);

                var customSearch = collection.FindAll();

                if (search.Any() || customSearch.Any())
                {
                    var helpEmbed = new EmbedBuilder()
                    {
                        Author = new EmbedAuthorBuilder()
                        {
                            Name = $"{Context.Guild.Name}'s Commands (Detailed)",
                            IconUrl = $"{Context.Guild.IconUrl}"
                        },
                        Color = Colors.Blue,
                        Description = "`{}` means an argument is optional, while `[]` means an argument is mandatory."
                    };

                    string embedName = "Best Match";
                    var command = search.First();
                    var customCommand = customSearch.First();

                    if (command is not null)
                    {
                        IEnumerable<string> commandParameters = command.Parameters.Select(p =>
                        {
                            var result = $"{(p.IsRemainder ? "..." : string.Empty)}{p.Name}";

                            return p.IsOptional ? $"{{{result}}}" : $"[{result}]";
                        });

                        string commandParametersString = string.Join(' ', commandParameters);

                        if (!string.IsNullOrEmpty(commandParametersString))
                            commandParametersString = ' ' + commandParametersString;

                        helpEmbed.Title = $"!{command.Name}{commandParametersString}";
                        helpEmbed.Description = command.Summary ?? "*No description.*";

                        if (command.Parameters.Any())
                        {
                            string detailedParameters = "";

                            foreach (var parameter in command.Parameters)
                            {
                                detailedParameters += $"- `{parameter.Name}`: `{(parameter.IsRemainder ? "..." : string.Empty)}{parameter.Type.Name}` {(!string.IsNullOrEmpty(parameter.Summary) ? $"- {parameter.Summary}" : string.Empty)}\n";
                            }

                            helpEmbed.AddField("Parameters", detailedParameters);
                        }
                        else
                            helpEmbed.AddField("Parameters", "This command does not take any parameters.");
                    }
                    else if (customCommand is not null)
                    {
                        helpEmbed.Title = customCommand.Trigger;
                        helpEmbed.Description = customCommand.Description ?? "*No description.*";
                    }

                    if (search.Count() > 1)
                    {
                        string otherResults = "";

                        foreach (var otherCommand in search.Skip(1))
                        {
                            if (!(await otherCommand.CheckPreconditionsAsync(Context)).IsSuccess)
                                continue;

                            otherResults += $"- `!{otherCommand.Name}` - {otherCommand.Summary}\n";
                        }

                        helpEmbed.AddField("Other results, global commands", otherResults);
                    }

                    if (search.Any() ? customSearch.Any() : (customSearch.Count() > 1))
                    {
                        string otherResults = "";
                        var toSearchFrom = search.Count() > 0 ? customSearch : customSearch.Skip(1);

                        foreach (var otherCommand in toSearchFrom)
                        {
                            otherResults += $"- `{otherCommand.Trigger}` - {otherCommand.Description}\n";
                        }

                        helpEmbed.AddField("Other results, custom commands", otherResults);
                    }

                    await Context.Channel.SendMessageAsync(embed: helpEmbed.Build());
                }
                else
                {
                    return CommandResult.FromError($"There are no commands that match `{commandName}`.");
                }
            }

            return CommandResult.FromSuccess($"{Icons.Message} I've DM'd you a list of commands. (haha no doing it here because funny)");
        }

        [Command("invite")]
        [Summary("Get my invite.")]
        [Alias("inv")]
        public async Task<RuntimeResult> InviteCommand()
        {
            var message = $"{Icons.Invite} You can invite me from my dashboard at https://localhost:5001.\n*Psst! I'm open source at https://github.com/RealSGII2/CustomCommandBot.*";

            return CommandResult.FromSuccess(message);
        }

        [Command("inv-test")]
        [Summary("Testing command for args checking.")]
        [Alias("invt")]
        [RequireOwner]
        public async Task<RuntimeResult> TestCommand([Summary("Example description")] string arg1, bool arg2, [Remainder] string arg3)
        {
            var message = $"{Icons.Invite} You can invite me from my dashboard at https://localhost:5001.\n*Psst! I'm open source at https://github.com/RealSGII2/CustomCommandBot.*";

            return CommandResult.FromSuccess(message);
        }

        [Command("dashboard")]
        [Summary("Get the link to configure this server.")]
        [Alias("dash", "configure", "config")]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        [RequireContext(ContextType.Guild)]
        public async Task<RuntimeResult> DashboardCommand()
        {
            var message = $"{Icons.Settings} You can configure **{Context.Guild.Name}** at https://localhost:5001/dashboard/{Context.Guild.Id}.";

            return CommandResult.FromSuccess(message);
        }
    }
}

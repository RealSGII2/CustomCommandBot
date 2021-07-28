using CustomCommandBot.Shared.Models.Discord;
using Discord;
using Discord.WebSocket;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCommandBot.Shared.Models.Api
{
    public class ApiGuild
    {
        public string Name { get; init; }
        public string Id { get; init; }
        public string Permissions { get; init; }

        public string IconUrl { get; init; }

        public IReadOnlyCollection<ApiRole> Roles { get; init; }
        public IReadOnlyCollection<ApiChannel> TextChannels { get; init; }

        public IReadOnlyCollection<ApiPartialCommand> Commands { get; init; }

        // Exists for Newtonsoft to parse
        public ApiGuild() { }

        public static ApiGuild FromSocket(SocketGuild socketGuild, UserGuild userGuild)
        {
            using (var database = new LiteDatabase(@"Databases/Commands.db"))
            {
                var collection = database.GetCollection<Command>("g" + userGuild.Id);
                collection.EnsureIndex(d => d.TriggerType);

                var commands = collection.FindAll().Select(c => ApiPartialCommand.FromSocket(c)).ToList();

                return new()
                {
                    Name = socketGuild.Name,
                    Id = socketGuild.Id.ToString(),
                    IconUrl = socketGuild.IconUrl,
                    Permissions = userGuild.Permissions.ToString(),
                    Roles = socketGuild.Roles.Select(r => ApiRole.FromSocket(r)).ToList(),
                    TextChannels = socketGuild.Channels.Where(g => g is IMessageChannel).Select(c => ApiChannel.FromSocket(c)).ToList(),
                    Commands = commands
                };
            }
        }
    }
}


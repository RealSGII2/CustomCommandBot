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
        public string Name { get; private init; }
        public ulong Id { get; private init; }
        public Int32 Permissions { get; private init; }

        public string IconUrl { get; private init; }

        public IReadOnlyCollection<ApiRole> Roles { get; private init; }
        public IReadOnlyCollection<ApiChannel> Channels { get; private init; }

        public IReadOnlyCollection<ApiPartialCommand> Commands { get; private init; }

        public ApiGuild(SocketGuild socketGuild, UserGuild userGuild)
        {
            Name = socketGuild.Name;
            Id = socketGuild.Id;
            IconUrl = socketGuild.IconUrl;

            Permissions = userGuild.Permissions;

            Roles = socketGuild.Roles.Select(r => new ApiRole(r)).ToList();
            Channels = socketGuild.Channels.Select(c => new ApiChannel(c)).ToList();

            using (var database = new LiteDatabase(@"Databases/Commands.db"))
            {
                var collection = database.GetCollection<Command>("g" + Id);
                collection.EnsureIndex(d => d.TriggerType);

                Commands = collection.FindAll().Select(c => new ApiPartialCommand(c)).ToList();
            }
        }
    }
}


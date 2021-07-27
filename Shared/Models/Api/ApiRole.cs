using CustomCommandBot.Shared.Models.Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCommandBot.Shared.Models.Api
{
    public class ApiRole
    {
        public string Name { get; private init; }
        public ulong Id { get; private init; }

        public string ColorHex { get; private init; }

        public ApiRole(SocketRole socketRole)
        {
            Name = socketRole.Name;
            Id = socketRole.Id;
            ColorHex = socketRole.Color.ToString();
        }
    }
}

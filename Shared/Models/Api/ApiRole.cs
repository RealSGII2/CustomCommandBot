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
        public string Name { get; init; }
        public ulong Id { get; init; }

        public string ColorHex { get; init; }

        // Exists for Newtonsoft to parse
        public ApiRole() { }

        public static ApiRole FromSocket(SocketRole socketRole)
        {
            return new()
            {
                Name = socketRole.Name,
                Id = socketRole.Id,
                ColorHex = socketRole.Color.ToString()
            };
        }
    }
}

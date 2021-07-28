using CustomCommandBot.Shared.Models.Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCommandBot.Shared.Models.Api
{
    public class ApiChannel
    {
        public string Name { get; init; }
        public ulong Id { get; init; }

        // Exists for Newtonsoft to parse
        public ApiChannel() { }

        public static ApiChannel FromSocket(SocketGuildChannel socketChannel)
        {
            return new()
            {
                Name = socketChannel.Name,
                Id = socketChannel.Id
            };
        }
    }
}

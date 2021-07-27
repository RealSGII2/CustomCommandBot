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
        public string Name { get; private init; }
        public ulong Id { get; private init; }

        public ApiChannel(SocketGuildChannel socketChannel)
        {
            Name = socketChannel.Name;
            Id = socketChannel.Id;
        }
    }
}

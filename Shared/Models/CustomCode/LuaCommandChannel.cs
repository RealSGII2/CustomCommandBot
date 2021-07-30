using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace CustomCommandBot.Shared.Models.CustomCode
{
    public class LuaCommandChannel
    {
        /// <summary>
        /// The ID of this channel.
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// The name of this channel.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// The clickable mention of this channel.
        /// </summary>
        public string Mention => $"<#{Id}>";

        public LuaCommandChannel(SocketGuildChannel channel)
        {
            Id = channel.Id.ToString();
            Name = channel.Name;
        }
    }
}

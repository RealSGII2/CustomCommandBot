using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace CustomCommandBot.Shared.Models.CustomCode
{
    public class LuaCommandCategoryChannel : LuaCommandChannel
    {
        /// <summary>
        /// The child channels of this category.
        /// </summary>
        public IReadOnlyCollection<LuaCommandChannel> Channels { get; init; }

        public LuaCommandCategoryChannel(SocketCategoryChannel channel) : base(channel)
        {
            Channels = channel.Channels.ToList().Select(c => {
                switch (c)
                {
                    case SocketTextChannel channel:
                        return new LuaCommandTextChannel(channel);
                    case SocketVoiceChannel channel:
                        return new LuaCommandVoiceChannel(channel);
                    default:
                        return new LuaCommandChannel(c);
                }
            }).ToList();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace CustomCommandBot.Shared.Models.CustomCode
{
    public class LuaCommandTextChannel : LuaCommandChannel
    {
        /// <summary>
        /// The topic of this channel.
        /// </summary>
        public string Topic { get; init; }

        /// <summary>
        /// The slowmode of this channel, counted in seconds.
        /// </summary>
        public int Slowmode { get; init; }

        /// <summary>
        /// Indicates if this channel is NSFW.
        /// </summary>
        public bool IsNsfw { get; init; }

        public LuaCommandTextChannel(SocketTextChannel channel) : base(channel)
        {
            Topic = channel.Topic;
            Slowmode = channel.SlowModeInterval;
            IsNsfw = channel.IsNsfw;
        }
    }
}

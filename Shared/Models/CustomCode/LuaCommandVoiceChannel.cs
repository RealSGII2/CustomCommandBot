using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace CustomCommandBot.Shared.Models.CustomCode
{
    public class LuaCommandVoiceChannel : LuaCommandChannel
    {
        /// <summary>
        /// The bitrate of this channel.
        /// </summary>
        public int BitRate { get; init; }

        public LuaCommandVoiceChannel(SocketVoiceChannel channel) : base(channel)
        {
            BitRate = channel.Bitrate;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace CustomCommandBot.Shared.Models.CustomCode
{
    public class LuaCommandMessage
    {
        private SocketUserMessage _message;

        /// <summary>
        /// The ID of this message.
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// The <see cref="LuaCommandUser"/> who sent this message.
        /// </summary>
        public LuaCommandUser Author { get; init; }

        /// <summary>
        /// The <see cref="LuaCommandChannel"/> this message was sent in.
        /// </summary>
        public LuaCommandChannel Channel { get; init; }

        /// <summary>
        /// The content this message had.
        /// </summary>
        public string Content { get; init; }

        /// <summary>
        /// Indicates if this message is deleted.
        /// </summary>
        public bool IsDeleted { get; private set; }

        /// <summary>
        /// Delete this message.
        /// </summary>
        /// <returns>A task completed once the message has been deleted.</returns>
        public async Task Delete()
        {
            await _message.DeleteAsync();
        }

        public LuaCommandMessage(SocketUserMessage message)
        {
            _message = message;

            Id = message.Id.ToString();
            Author = new(message.Author as SocketGuildUser);
            Channel = new(message.Channel as SocketTextChannel);
            Content = message.Content;
        }
    }
}

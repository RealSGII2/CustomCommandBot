using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace CustomCommandBot.Shared.Models.CustomCode
{
    public class LuaCommandContext
    {
        /// <summary>
        /// The <see cref="LuaCommandMessage"/> that invocated this command.
        /// </summary>
        public LuaCommandMessage Message { get; init; }

        /// <summary>
        /// The <see cref="LuaCommandGuild"/> this command was ran in.
        /// </summary>
        public LuaCommandGuild Guild { get; init; }

        /// <summary>
        /// The <see cref="LuaCommandUser"/> who invocated this command.
        /// </summary>
        public LuaCommandUser Executor => Message.Author;

        /// <summary>
        /// The <see cref="LuaCommandChannel"/> this command was ran in.
        /// </summary>
        public LuaCommandChannel Channel => Message.Channel;

        /// <summary>
        /// Cancels the command and prevents any actions from running. Note that any remaining code WILL run.
        /// </summary>
        public void Cancel()
        {
            IsCancelled = true;
        }

        /// <summary>
        /// If cancelled, this command will not run any actions. Cancel with <see cref="Cancel"/>.
        /// </summary>
        public bool IsCancelled { get; private set; } = false;

        /// <summary>
        /// The arguments this command was ran with.
        /// </summary>
        public IReadOnlyCollection<string> Arguments { get; init; }

        public LuaCommandContext(SocketCommandContext context)
        {
            Message = new(context.Message);
            Guild = new(context.Guild);
        }
    }
}

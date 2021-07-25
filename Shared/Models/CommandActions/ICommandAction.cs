using System.Threading.Tasks;
using Discord.Commands;
using LiteDB;

namespace CustomCommandBot.Shared.Models.CommandActions
{
    public interface ICommandAction
    {
        /// <summary>
        /// Whether this CommandAction is able to be executed
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// The group ID of this CommandAction. Used to give an entire set
        /// of CommandActions options. Can also be used to randomise the
        /// outcome.
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// A function to run when this action is being executed.
        /// </summary>
        /// <param name="ctx">The CommandContext of the command running this action</param>
        /// <returns>A result indicating an error or success.</returns>
        [BsonIgnore]
        public Task<CommandActionResult> OnExecute(SocketCommandContext ctx);
    }
}

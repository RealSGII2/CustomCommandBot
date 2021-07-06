using Discord;
using Discord.Commands;
using LiteDB;
using System.Threading.Tasks;

namespace CustomCommandBot.Shared.Models.CommandActions
{
    public class ReplyAction : ICommandAction
    {
        public bool IsEnabled { get; init; }
        public int GroupId { get; init; }

        /// <summary>
        /// Text content that should be replied with, containing markdown and links.
        /// The user should include image links to embed images.
        /// </summary>
        public string Content { get; init; }

        /// <summary>
        /// A CommandEmbed compatible with the database representing an embed that should be sent.
        /// </summary>
        public EmbedBuilder Embed { get; init; }

        [BsonIgnore]
        public virtual async Task<CommandActionResult> OnExecute(SocketCommandContext context)
        {
            await context.Channel.SendMessageAsync(text: Content, embed: Embed?.Build());
            return CommandActionResult.FromSuccess();
        }
    }
}

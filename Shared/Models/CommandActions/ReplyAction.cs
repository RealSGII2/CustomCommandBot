using Discord;
using Discord.Commands;
using LiteDB;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CustomCommandBot.Shared.Models.CommandActions
{
    public class ReplyAction : ICommandAction
    {
        public bool IsEnabled { get; set; }

        public int GroupId { get; set; }

        /// <summary>
        /// Text content that should be replied with, containing markdown and links.
        /// The user should include image links to embed images.
        /// </summary>
        [Required]
        [StringLength(2000, ErrorMessage = "Message contents should be less than or equal to 2000 characters.")]
        public string Content { get; set; }

        /// <summary>
        /// A CommandEmbed compatible with the database representing an embed that should be sent.
        /// </summary>
        public EmbedBuilder Embed { get; set; }

        [BsonIgnore]
        public virtual async Task<CommandActionResult> OnExecute(SocketCommandContext context)
        {
            await context.Channel.SendMessageAsync(text: Content, embed: Embed?.Build());
            return CommandActionResult.FromSuccess();
        }
    }
}

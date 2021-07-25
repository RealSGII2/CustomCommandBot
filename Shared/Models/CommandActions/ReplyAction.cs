using CustomCommandBot.Shared.Attributes.Command;
using Discord;
using Discord.Commands;
using LiteDB;
using System.ComponentModel;
using System.Threading.Tasks;

namespace CustomCommandBot.Shared.Models.CommandActions
{
    [Description("Reply in channel")]
    public class ReplyAction : ICommandAction
    {
        public bool IsEnabled { get; set; }
        [Input(InputType.Textfield, "Yes")]
        public int GroupId { get; set; }

        /// <summary>
        /// Text content that should be replied with, containing markdown and links.
        /// The user should include image links to embed images.
        /// </summary>
        [Input(InputType.Textarea, "Message", Limit = 2000)]
        public string Content { get; set; }

        /// <summary>
        /// A CommandEmbed compatible with the database representing an embed that should be sent.
        /// </summary>
        [Input(InputType.Embed, "Embed")]
        public EmbedBuilder Embed { get; set; }

        [BsonIgnore]
        public virtual async Task<CommandActionResult> OnExecute(SocketCommandContext context)
        {
            await context.Channel.SendMessageAsync(text: Content, embed: Embed?.Build());
            return CommandActionResult.FromSuccess();
        }
    }
}

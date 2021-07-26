using CustomCommandBot.Shared.Attributes.Command;
using Discord;
using Discord.Commands;
using LiteDB;
using System.ComponentModel;
using System.Threading.Tasks;

namespace CustomCommandBot.Shared.Models.CommandActions
{
    [Description("Send to channel")]
    public class SendToChannelAction : ReplyAction
    {
        /// <summary>
        /// The channel to send the message to
        /// </summary>
        [Input(InputType.Channel, "Channel")]
        public ulong ChannelID { get; set; }

        [BsonIgnore]
        public override async Task<CommandActionResult> OnExecute(SocketCommandContext context)
        {
            var channel = await context.Client.GetChannelAsync(ChannelID) as ITextChannel;
            await channel.SendMessageAsync(text: Content, embed: Embed?.Build());

            return CommandActionResult.FromSuccess();
        }
    }
}

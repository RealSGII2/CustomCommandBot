using Discord;
using Discord.Commands;
using LiteDB;
using System.Threading.Tasks;

namespace CustomCommandBot.Shared.Models.CommandActions
{
    public class SendToChannelAction : ReplyAction
    {
        public ulong ChannelID { get; init; }

        [BsonIgnore]
        public override async Task<CommandActionResult> OnExecute(CommandContext context)
        {
            var channel = await context.Client.GetChannelAsync(ChannelID) as ITextChannel;
            await channel.SendMessageAsync(text: Content, embed: Embed.Build());

            return CommandActionResult.FromSuccess();
        }
    }
}

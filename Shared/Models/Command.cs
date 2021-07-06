using CustomCommandBot.Shared.Models.CommandActions;
using LiteDB;

namespace CustomCommandBot.Shared.Models
{
    public class Command
    {
        [BsonId]
        public string Trigger { get; init; }
        public CommandTriggerType TriggerType { get; init; }

        public ICommandAction[] Actions { get; init; }
    }

    public enum CommandTriggerType
    {
        BeginsWith,
        EndsWith,
        Contains,
        Regex
    }
}
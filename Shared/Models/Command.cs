using CustomCommandBot.Shared.Models.CommandActions;
using LiteDB;
using System.Collections.Generic;
using System.ComponentModel;

namespace CustomCommandBot.Shared.Models
{
    public class Command
    {
        [BsonId]
        public string Trigger { get; set; }
        public CommandTriggerType TriggerType { get; set; }

        public List<ICommandAction> Actions { get; set; }
    }

    public enum CommandTriggerType
    {
        [Description("Begins with")]
        BeginsWith,
        [Description("Ends with")]
        EndsWith,
        [Description("Contains")]
        Contains,
        //[Description("Regex")]
        //Regex
    }
}
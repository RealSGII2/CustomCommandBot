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

        public string Description { get; set; }

        public List<ICommandAction> Actions { get; set; }
    }

    public enum CommandTriggerType
    {
        BeginsWith,
        EndsWith,
        Contains,
        //Regex
    }
}
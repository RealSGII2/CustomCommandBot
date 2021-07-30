using CustomCommandBot.Shared.Models.CommandActions;
using LiteDB;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CustomCommandBot.Shared.Models
{
    public class Command
    {
        [BsonId]
        [Required]
        [StringLength(48, ErrorMessage = "Command triggers should be less than or equal to 48 characters.")]
        public string Trigger { get; set; }
        public CommandTriggerType TriggerType { get; set; }

        [Required]
        public string Description { get; set; }

        [ValidateComplexType]
        public List<ICommandAction> Actions { get; set; }

        public string CustomCode { get; set; }
    }

    public enum CommandTriggerType
    {
        [Description("Begins with")]
        BeginsWith,
        [Description("Ends with")]
        EndsWith,
        [Description("Contains")]
        Contains,
        //Regex
    }
}
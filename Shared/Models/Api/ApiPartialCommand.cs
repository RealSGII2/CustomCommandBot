using CustomCommandBot.Shared.Models.Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCommandBot.Shared.Models.Api
{
    public class ApiPartialCommand
    {
        public string Trigger { get; private init; }
        public string Description { get; private init; }

        public ApiPartialCommand(Command command)
        {
            Trigger = command.Trigger;
            Description = command.Description;
        }
    }
}

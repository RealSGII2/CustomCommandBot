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
        public string Trigger { get; init; }
        public string Description { get; init; }

        // Exists for Newtonsoft to parse
        public ApiPartialCommand() { }

        public static ApiPartialCommand FromSocket(Command command)
        {
            return new()
            {
                Trigger = command.Trigger,
                Description = command.Description
            };
        }
    }
}

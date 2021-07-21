using CustomCommandBot.Shared.Models.Discord;
using System.Collections.Generic;

namespace CustomCommandBot.Shared.Models
{
    public class DatabaseGuild : UserGuild
    {
        public List<Command> Commands { get; set; } = new();
    }
}

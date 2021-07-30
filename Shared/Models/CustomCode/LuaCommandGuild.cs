using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace CustomCommandBot.Shared.Models.CustomCode
{
    public class LuaCommandGuild
    {
        /// <summary>
        /// The ID of this guild.
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// The name of this guild.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// The description of this guild.
        /// </summary>
        public string? Description { get; init; }

        /// <summary>
        /// The owner of this guild
        /// </summary>
        public LuaCommandUser Owner { get; init; }

        /// <summary>
        /// The CDN URL of this guild's icon.
        /// </summary>
        public string IconUrl { get; init; }

        /// <summary>
        /// The <see cref="LuaCommandTextChannel"/> where Discord sends system messages to.
        /// </summary>
        public LuaCommandTextChannel? SystemChannel { get; init; }

        /// <summary>
        /// The <see cref="LuaCommandTextChannel"/> where Discord sends system messages to.
        /// </summary>
        public LuaCommandTextChannel? RulesChannel { get; init; }

        /// <summary>
        /// The <see cref="LuaCommandVoiceChannel"/> that users are sent to when idle.
        /// </summary>
        public LuaCommandVoiceChannel? AfkChannel { get; init; }

        public LuaCommandGuild(SocketGuild guild)
        {
            Id = guild.Id.ToString();
            Name = guild.Name;
            IconUrl = guild.IconUrl;
            Owner = new(guild.Owner);

            if (guild.Description is not null)
                Description = guild.Description;

            if (guild.SystemChannel is not null)
                SystemChannel = new(guild.SystemChannel);

            if (guild.AFKChannel is not null)
                AfkChannel = new(guild.AFKChannel);

            if (guild.RulesChannel is not null)
                RulesChannel = new(guild.RulesChannel);
        }
    }
}

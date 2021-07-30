using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace CustomCommandBot.Shared.Models.CustomCode
{
    public class LuaCommandUser
    {
        /// <summary>
        /// The ID of this user.
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// The username of this user.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// The discriminator of this user, i.e. 0001.
        /// </summary>
        public string Discriminator { get; init; }

        /// <summary>
        /// The URL of this user's avatar.
        /// </summary>
        public string AvatarUrl { get; init; }

        /// <summary>
        /// The nickname of this user.
        /// </summary>
        public string Nickname { get; init; }

        /// <summary>
        /// The clickable mention of this user.
        /// </summary>
        public string Mention { get; init; }

        /// <summary>
        /// Indicates if this user is streaming.
        /// </summary>
        public bool IsStreaming { get; init; }

        /// <summary>
        /// Indicates if this user is guild muted.
        /// </summary>
        public bool IsMuted { get; init; }

        /// <summary>
        /// Indicates if this user is guild deafened.
        /// </summary>
        public bool IsDeafened { get; init; }

        /// <summary>
        /// Indicates if this user has muted themself.
        /// </summary>
        public bool IsSelfMuted { get; init; }

        /// <summary>
        /// Indicates if this user has deafened themself.
        /// </summary>
        public bool IsSelfDeafened { get; init; }

        /// <summary>
        /// Get this user's highest role's position.
        /// </summary>
        public int Position { get; init; } 

        /// <summary>
        /// Get the roles of this user.
        /// </summary>
        public IReadOnlyCollection<LuaCommandRole> Roles { get; set; }

        /// <summary>
        /// Get the full name of the user, including discriminator, i.e. User#0001
        /// </summary>
        public string Tag => $"{Name}#{Discriminator}";

        public LuaCommandUser(SocketGuildUser user)
        {
            Id = user.Id.ToString();
            Name = user.Username;
            Discriminator = user.Discriminator;
            AvatarUrl = user.GetAvatarUrl();
            Nickname = user.Nickname;
            IsStreaming = user.IsStreaming;
            IsMuted = user.IsMuted;
            IsDeafened = user.IsDeafened;
            IsSelfMuted = user.IsSelfMuted;
            IsSelfDeafened = user.IsSelfDeafened;
            Position = user.Hierarchy;
            Roles = user.Roles.Select(r => new LuaCommandRole(r)).ToList();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace CustomCommandBot.Shared.Models.CustomCode
{
    public class LuaCommandRole
    {
        /// <summary>
        /// The ID of this role.
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// The name of this role.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// The color of this role in Hexadecimal form.
        /// </summary>
        public string HexColor { get; init; }

        /// <summary>
        /// Indicates whether this role can be mentioned by anyone.
        /// </summary>
        public bool IsMentionable { get; init; }

        /// <summary>
        /// Indicates whether this role is seperated from online members.
        /// </summary>
        public bool IsHoisted { get; init; }

        /// <summary>
        /// The position of this role.
        /// </summary>
        public int Position { get; init; }

        /// <summary>
        /// The permissions of this role.
        /// </summary>
        public ulong Permissions { get; init; }

        public LuaCommandRole(SocketRole role)
        {
            Id = role.Id.ToString();
            Name = role.Name;
            HexColor = role.Color.ToString();
            IsMentionable = role.IsMentionable;
            IsHoisted = role.IsHoisted;
            Position = role.Position;
            Permissions = role.Permissions.RawValue;
        }
    }
}

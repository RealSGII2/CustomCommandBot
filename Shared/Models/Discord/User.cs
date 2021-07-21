using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CustomCommandBot.Shared.Models.Discord
{
    public class User
    {
        public string Username { get; init; }
        public string Id { get; init; }
        public int Discriminator { get; init; }
        public string AvatarUrl { get; init; }
        public List<UserGuild> Guilds { get; init; }

        public static User FromClaims(List<Claim> claims, List<ulong> inGuildIds)
        {
            return new()
            {
                Username = claims.FirstOrDefault(e => e.Type == "name" || e.Type == ClaimTypes.Name).Value,
                Id = claims.FirstOrDefault(e => e.Type == "sub" || e.Type == ClaimTypes.NameIdentifier).Value,
                Discriminator = int.Parse(claims.FirstOrDefault(e => e.Type == "urn:discord:user:discriminator").Value),
                AvatarUrl = claims.FirstOrDefault(e => e.Type == "urn:discord:avatar:url").Value,
                Guilds = UserGuild.ListFromJson(claims.FirstOrDefault(e => e.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/guilds").Value, inGuildIds).ToList()
            };
        }
    }

    public static class UserExtensions
    {
        public static User GetDiscordUser(this ClaimsPrincipal userClaims, List<ulong> guildIds)
        {
            return User.FromClaims(userClaims.Claims.ToList(), guildIds);
        }
    }
}

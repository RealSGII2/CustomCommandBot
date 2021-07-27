using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomCommandBot.Shared.Models.Discord
{
    public class UserGuild
    {
        public string Name { get; init; }
        public string Id { get; init; }
        public Int32 Permissions { get; init; }
        public string Icon { get; init; }
        public bool IsClientInGuild { get; set; }

        public string IconUrl
        {
            get => Icon is null ? null : $"https://cdn.discordapp.com/icons/{Id}/{Icon}.webp?size=128";
        }

        public bool CanManage
        {
            get => (Permissions & 0x32) == 0x32;
        }


        public static UserGuild FromJson(string json)
        {
            return JsonConvert.DeserializeObject<UserGuild>(json);
        }

        public static IEnumerable<UserGuild> ListFromJson(string json, List<ulong> inGuildIds)
        {
            var guildIds = inGuildIds.Select(i => i.ToString()).ToList();
            var result = JsonConvert.DeserializeObject<List<UserGuild>>(json);

            for (int i = 0; i < result.Count; i++)
                result[i].IsClientInGuild = guildIds.Contains(result[i].Id);

            return result;
        }
    }

    public static class UserGuildExtensions
    {
        public static IEnumerable<UserGuild> GetInvitedGuilds(this IEnumerable<UserGuild> userGuilds)
            => userGuilds.Where(x => x.CanManage && x.IsClientInGuild);

        public static IEnumerable<UserGuild> GetUninvitedGuilds(this IEnumerable<UserGuild> userGuilds)
            => userGuilds.Where(x => x.CanManage && !x.IsClientInGuild);
    }
}

using CustomCommandBot.Shared.Models;
using Discord;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomCommandBot.Server.Extentions
{
    public static class CommandsExtentions
    {
        /// <summary>
        /// Gets a List of all Commands this guild has
        /// </summary>
        /// <param name="guild">The guild to get commands from</param>
        /// <returns>A List of Commands</returns>
        public static List<Command> GetCommands(this IGuild guild)
        {
            using (var database = new LiteDatabase(@"Databases/Commands.db"))
            {
                var collection = database.GetCollection<Command>("g" + guild.Id.ToString());
                collection.EnsureIndex(d => d.TriggerType);

                return collection.FindAll().ToList();
            }
        }

        public static void AddCommand(this IGuild guild, Command command)
        {
            using (var database = new LiteDatabase(@"Databases/Commands.db"))
            {
                var collection = database.GetCollection<Command>("g" + guild.Id.ToString());
                collection.EnsureIndex(d => d.TriggerType);

                collection.Insert(command);
            }
        }
    }
}

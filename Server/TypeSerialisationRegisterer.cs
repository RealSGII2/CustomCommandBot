using Discord;
using LiteDB;

namespace CustomCommandBot.Server
{
    public static class TypeSerialisationRegisterer
    {
        /// <summary>
        /// Register types for LiteDB serialisation
        /// </summary>
        public static void SerialiseDatabaseTypes()
        {
            // Discord's Color
            BsonMapper.Global.RegisterType(
                serialize: (Color color) => (double) color.RawValue,
                deserialize: (BsonValue bson) => new Color((uint) bson.AsDouble)
            );
        }
    }
}

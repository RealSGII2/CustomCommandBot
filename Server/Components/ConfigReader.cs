using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace CustomCommandBot.Server.Components
{
    public class ConfigReader
    {
        public static T ReadConfigFile<T>(string path)
        {
            string rawText = File.ReadAllText(path);

            var deserialiser = new DeserializerBuilder()
                .WithNamingConvention(UnderscoredNamingConvention.Instance)
                .Build();

            return deserialiser.Deserialize<T>(rawText);
        }
    }
}

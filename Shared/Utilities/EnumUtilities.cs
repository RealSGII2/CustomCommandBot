using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CustomCommandBot.Shared.Utilities
{
    public static class EnumUtilities
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var descriptions = field.GetCustomAttributes(typeof(DescriptionAttribute), true) as DescriptionAttribute[];

            if (descriptions is not null && descriptions.Any())
                return descriptions.First().Description;

            return value.ToString();
        }
    }
}

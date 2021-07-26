using CustomCommandBot.Shared.Attributes.Command;
using CustomCommandBot.Shared.Models.CommandActions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CustomCommandBot.Shared.Utilities
{
    public static class CommandActionUtilities
    {
        public static Dictionary<string, InputAttribute> GetInputFields(this Type type)
        {
            var propsWithInput = type.GetProperties().Where(p => p.IsDefined(typeof(InputAttribute), false));

            Dictionary<string, InputAttribute> inputFields = new();

            foreach (var prop in propsWithInput)
            {
                var inputs = (InputAttribute[])prop.GetCustomAttributes(typeof(InputAttribute), false);

                if (inputs.Length != 0)
                    inputFields.Add(prop.Name, inputs[0]);
            }

            return inputFields;
        }

        public static string GetHumanName(this Type type)
        {
            var descriptions = (DescriptionAttribute[])
            type.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (descriptions.Length == 0)
            {
                return null;
            }
            return descriptions[0].Description.ToString();
        }
    }
}

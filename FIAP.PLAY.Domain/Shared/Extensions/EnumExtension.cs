using System.ComponentModel;

namespace FIAP.PLAY.Domain.Shared.Extensions
{
    public static class EnumExtension
    {
        public static string GetDescription(this Enum value)
        {
            var attributes = (DescriptionAttribute[])value
                .GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        public static string GetDescription(this Enum value, params string[] parameters)
        {
            var attributes = (DescriptionAttribute[])value
                .GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? string.Format(attributes[0].Description, parameters) : value.ToString();
        }

        public static string GetDescription(this Enum value, params Enum[] parameters)
        {
            var attributes = (DescriptionAttribute[])value
                .GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? string.Format(attributes[0].Description, parameters.Select(e => e.GetDescription()).ToArray()) : value.ToString();
        }
    }
}

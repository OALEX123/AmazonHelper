namespace AmazonHelper.Common
{
    using System;

    public static class EnumExtensions
    {
        public static string GetDisplayName(this System.Enum value)
        {
            var type = value.GetType();
            if (!type.IsEnum) throw new ArgumentException($"Type '{type}' is not Enum");

            var members = type.GetMember(value.ToString());
            if (members.Length == 0) throw new ArgumentException($"Member '{value}' not found in type '{type.Name}'");

            var member = members[0];
            var attributes = member.GetCustomAttributes(typeof(EnumDisplayNameAttribute), false);
            if (attributes.Length == 0) throw new ArgumentException(
                $"'{type.Name}.{value}' doesn't have DisplayAttribute");

            var attribute = (EnumDisplayNameAttribute)attributes[0];
            return attribute.Name;
        }
    }
}

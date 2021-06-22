using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace System
{
    public static class EnumExtensions
    {
        public static T GetAttribute<T>(this Enum enumValue) where T : Attribute
        {
            var type = enumValue.GetType();
            var memberInfo = type.GetMember(enumValue.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
            return attributes.Length > 0 ? (T)attributes[0] : null;
        }

        public static string GetDescription(this Enum enumValue)
        {
            var descriptionAttr = enumValue.GetAttribute<DescriptionAttribute>();
            return descriptionAttr?.Description;
        }

        public static T ToEnumValue<T>(this string description)
            where T : Enum
        {
            var values = Enum.GetValues(typeof(T));
            foreach (T value in values)
            {
                if (description.Equals(value.GetDescription()))
                {
                    return value;
                }
            }

            throw new BizException("invalid enum value");
        }
    }
}

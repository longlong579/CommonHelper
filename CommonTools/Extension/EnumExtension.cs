using System;
using System.ComponentModel;
using System.Reflection;

namespace HZZG.Common.Tolls
{
    /// <summary>
    /// 枚举的转换可以参考CastTo<>
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// 获取枚举描述
        /// </summary>
        /// <param name="value"></param>
        /// <param name="nameInstend"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value, bool nameInstend = true)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name == null)
            {
                return null;
            }
            FieldInfo field = type.GetField(name);
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (attribute == null && nameInstend == true)
            {
                return name;
            }
            return attribute == null ? null : attribute.Description;
        }

        /// <summary>
        /// 获取枚举名称，枚举名称可以由NameAttribute指定
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetName(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            NameAttribute attribute = Attribute.GetCustomAttribute(field, typeof(NameAttribute)) as NameAttribute;
            return attribute == null ? value.ToString() : attribute.Name;
        }
    }
}

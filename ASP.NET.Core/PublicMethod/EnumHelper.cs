using System;
using System.ComponentModel;

namespace PublicMethod
{
    public static class EnumHelper
    {
        /// <summary>
        /// 获取枚举的描述
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDescription(this Enum value)
        {
            if (value == null)
                return "";

            System.Reflection.FieldInfo fieldInfo = value.GetType().GetField(value.ToString());

            object[] attribArray = fieldInfo.GetCustomAttributes(false);
            if (attribArray.Length == 0)
                return value.ToString();
            else
                return (attribArray[0] as DescriptionAttribute).Description;
        }
    }
}

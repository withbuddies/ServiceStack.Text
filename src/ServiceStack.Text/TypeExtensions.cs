using System;
using System.Linq;

namespace ServiceStack.Text
{
    public static class TypeExtensions
    {
        public static bool ShouldWriteFlags(this Type enumType)
        {
            enumType = Nullable.GetUnderlyingType(enumType) ?? enumType;
            return enumType.HasAttribute(typeof(FlagsAttribute), false)
                || Attribute.GetCustomAttributes(enumType).Any(a => a.GetType().Name == "JsonNumeric" || a.GetType().Name == "JsonNumericAttribute");
        }
    }
}

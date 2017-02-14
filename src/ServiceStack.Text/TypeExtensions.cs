using System;
using System.Linq;
using System.Reflection;

namespace ServiceStack.Text
{
    public static class TypeExtensions
    {
        public static bool ShouldWriteFlags(this Type enumType)
        {
            enumType = Nullable.GetUnderlyingType(enumType) ?? enumType;
            return enumType.HasAttribute(typeof(FlagsAttribute), false)
                || enumType.GetTypeInfo().GetCustomAttributes(false).Any(a => a.GetType().Name == "JsonNumeric" || a.GetType().Name == "JsonNumericAttribute");
        }
    }
}

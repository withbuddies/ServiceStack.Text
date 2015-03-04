using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace ServiceStack.Text
{
    internal static class EnumUtils
    {
        public static T Parse<T>(string value, bool ignoreCase)
        {
            if (!EnumInfo<T>.IsEnum) throw new InvalidOperationException("This method is only allowed to operate on Enum's");
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("value may not be null or empty", "value");
            var source = ignoreCase
                ? EnumInfo<T>.CaseInsensitiveValuesByName
                : EnumInfo<T>.ValuesByName;

            if (char.IsDigit(value[0]) || value[0] == '-' || value[0] == '+')
            {
                return ParseUnderlyingType<T>(value);
            }
            
            T result;
            if (source.TryGetValue(value, out result)) return result;

            var message = string.Format("Unexpected value encountered '{0}' for enum of type '{1}'", value, typeof(T).Name);
            throw new ArgumentException(message, "value");
        }

        private static T ParseUnderlyingType<T>(string value)
        {
            var underlyingType = EnumInfo<T>.UnderlyingType;
            var value2 = Convert.ChangeType(value, underlyingType, CultureInfo.InvariantCulture);
            return (T)Enum.ToObject(EnumInfo<T>.EnumType, value2);
        }

        private static class EnumInfo<T>
        {
            public static readonly bool IsEnum;
            public static readonly Type EnumType;
            public static readonly Type UnderlyingType;
            public static readonly Dictionary<string, T> ValuesByName;
            public static readonly Dictionary<string, T> CaseInsensitiveValuesByName; 

            static EnumInfo()
            {
                var type = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
                EnumType = type;
                IsEnum = type.IsEnum;
                if (!IsEnum) return;
                UnderlyingType = Enum.GetUnderlyingType(type);
                var q = from field in type.GetFields(BindingFlags.Public | BindingFlags.Static)
                        let value = (T)field.GetValue(null)
                        let name = value.ToString()
                        select new { name, value };
                var pairs = q.ToList();
                ValuesByName = pairs.ToDictionary(v => v.name, v => v.value);
                CaseInsensitiveValuesByName = pairs.ToDictionary(v => v.name, v => v.value, StringComparer.OrdinalIgnoreCase);
            }
        }
    }
}

//
// http://code.google.com/p/servicestack/wiki/TypeSerializer
// ServiceStack.Text: .NET C# POCO Type Text Serializer.
//
// Authors:
//   Demis Bellot (demis.bellot@gmail.com)
//
// Copyright 2011 Liquidbit Ltd.
//
// Licensed under the same terms of ServiceStack: new BSD license.
//

using System;
using System.IO;
using System.Reflection;
using System.Text;
using ServiceStack.Text.Common;
using ServiceStack.Text.Json;

namespace ServiceStack.Text
{
	public class JsonSerializer<T> : ITypeSerializer<T>
	{
		public bool CanCreateFromString(Type type)
		{
			return JsonReader.GetParseFn(type) != null;
		}

		/// <summary>
		/// Parses the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public T DeserializeFromString(string value)
		{
			if (string.IsNullOrEmpty(value)) return default(T);
			return (T)JsonReader<T>.Parse(value);
		}

		public T DeserializeFromReader(TextReader reader)
		{
			return DeserializeFromString(reader.ReadToEnd());
		}

		public string SerializeToString(T value)
		{
			if (value == null) return null;
			if (typeof(T) == typeof(string)) return value as string;
            var info = typeof(T).GetTypeInfo();
            if (typeof(T) == typeof(object) || info.IsAbstract || info.IsInterface)
            {
                if (info.IsAbstract || info.IsInterface) JsState.IsWritingDynamic = true;
                try
                {
                    return JsonSerializer.SerializeToString(value, value.GetType());
                }
                finally
                {
                    if (info.IsAbstract || info.IsInterface) JsState.IsWritingDynamic = false;
                }
            }

			var sb = new StringBuilder();
			using (var writer = new StringWriter(sb))
			{
				JsonWriter<T>.WriteObject(writer, value);
			}
			return sb.ToString();
		}

		public void SerializeToWriter(T value, TextWriter writer)
		{
			if (value == null) return;
			if (typeof(T) == typeof(string))
			{
				writer.Write(value);
				return;
			}
            var info = typeof(T).GetTypeInfo();
            if (typeof(T) == typeof(object) || info.IsAbstract || info.IsInterface)
            {
                if (info.IsAbstract || info.IsInterface) JsState.IsWritingDynamic = true;
                try
                {
                    JsonSerializer.SerializeToWriter(value, value.GetType(), writer);
                }
                finally
                {
                    if (info.IsAbstract || info.IsInterface) JsState.IsWritingDynamic = false;
                }
                return;
            }
           
            JsonWriter<T>.WriteObject(writer, value);
		}
	}
}
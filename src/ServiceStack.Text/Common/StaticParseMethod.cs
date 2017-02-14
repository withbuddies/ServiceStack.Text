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
using System.Linq;
using System.Reflection;
using ServiceStack.Text.Jsv;

namespace ServiceStack.Text.Common
{
	internal delegate object ParseDelegate(string value);

	public static class StaticParseMethod<T>
	{
		const string ParseMethod = "Parse";

		private static readonly ParseStringDelegate CacheFn;

		public static ParseStringDelegate Parse
		{
			get { return CacheFn; }
		}

		static StaticParseMethod()
		{
			CacheFn = GetParseFn();
		}

		public static ParseStringDelegate GetParseFn()
		{
			// Get the static Parse(string) method on the type supplied
            var parseMethodInfo = typeof(T).GetTypeInfo()
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(m => m.Name == ParseMethod)
                .Where(m =>
                {
                    var p = m.GetParameters();
                    return p.Length == 1 && p[0].ParameterType == typeof(string);
                })
                .FirstOrDefault();

			if (parseMethodInfo == null) return null;

			ParseDelegate parseDelegate;
			try
			{
				parseDelegate = (ParseDelegate)parseMethodInfo.CreateDelegate(typeof(ParseDelegate));
			}
			catch (ArgumentException)
			{
				//Try wrapping strongly-typed return with wrapper fn.
				var typedParseDelegate = (Func<string, T>)parseMethodInfo.CreateDelegate(typeof(Func<string, T>));
				parseDelegate = x => typedParseDelegate(x);
			}
			if (parseDelegate != null)
				return value => parseDelegate(value.FromCsvField());

			return null;
		}
	}

	internal static class StaticParseRefTypeMethod<TSerializer, T>
		where TSerializer : ITypeSerializer
	{
		static string ParseMethod = typeof(TSerializer) == typeof(JsvTypeSerializer)
			? "ParseJsv"
			: "ParseJson";

		private static readonly ParseStringDelegate CacheFn;

		public static ParseStringDelegate Parse
		{
			get { return CacheFn; }
		}

		static StaticParseRefTypeMethod()
		{			
			CacheFn = GetParseFn();
		}

		public static ParseStringDelegate GetParseFn()
		{
			// Get the static Parse(string) method on the type supplied
            var parseMethodInfo = typeof(T).GetTypeInfo()
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(m => m.Name == ParseMethod)
                .Where(m =>
                {
                    var p = m.GetParameters();
                    return p.Length == 1 && p[0].ParameterType == typeof(string);
                })
                .FirstOrDefault();

			if (parseMethodInfo == null) return null;

			ParseDelegate parseDelegate;
			try
			{
				parseDelegate = (ParseDelegate)parseMethodInfo.CreateDelegate(typeof(ParseDelegate));
			}
			catch (ArgumentException)
			{
				//Try wrapping strongly-typed return with wrapper fn.
				var typedParseDelegate = (Func<string, T>)parseMethodInfo.CreateDelegate(typeof(Func<string, T>));
				parseDelegate = x => typedParseDelegate(x);
			}
			if (parseDelegate != null)
				return value => parseDelegate(value);

			return null;
		}
	}

}
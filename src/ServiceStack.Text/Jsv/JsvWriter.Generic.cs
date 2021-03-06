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
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using ServiceStack.Text.Common;

namespace ServiceStack.Text.Jsv
{
	internal static class JsvWriter
	{
		public static readonly JsWriter<JsvTypeSerializer> Instance = new JsWriter<JsvTypeSerializer>();

        private static Dictionary<Type, WriteObjectDelegate> WriteFnCache = new Dictionary<Type, WriteObjectDelegate>();

		public static WriteObjectDelegate GetWriteFn(Type type)
		{
			try
			{
                WriteObjectDelegate writeFn;
                if (WriteFnCache.TryGetValue(type, out writeFn)) return writeFn;

                var genericType = typeof(JsvWriter<>).MakeGenericType(type);
                var mi = genericType.GetMethod("WriteFn", BindingFlags.Public | BindingFlags.Static);
                var writeFactoryFn = (Func<WriteObjectDelegate>)mi.CreateDelegate(typeof(Func<WriteObjectDelegate>));
                writeFn = writeFactoryFn();

                Dictionary<Type, WriteObjectDelegate> snapshot, newCache;
                do
                {
                    snapshot = WriteFnCache;
                    newCache = new Dictionary<Type, WriteObjectDelegate>(WriteFnCache);
                    newCache[type] = writeFn;

                } while (!ReferenceEquals(
                    Interlocked.CompareExchange(ref WriteFnCache, newCache, snapshot), snapshot));

                return writeFn;
			}
			catch (Exception ex)
			{
				Tracer.Instance.WriteError(ex);
				throw;
			}
		}

		public static void WriteLateBoundObject(TextWriter writer, object value)
		{
			if (value == null) return;
			var writeFn = GetWriteFn(value.GetType());

			var prevState = JsState.IsWritingDynamic;
			JsState.IsWritingDynamic = true;
            try
            {
                writeFn(writer, value);
            }
            finally
            {
                JsState.IsWritingDynamic = prevState;
            }
		}

		public static WriteObjectDelegate GetValueTypeToStringMethod(Type type)
		{
			return Instance.GetValueTypeToStringMethod(type);
		}
	}

	/// <summary>
	/// Implement the serializer using a more static approach
	/// </summary>
	/// <typeparam name="T"></typeparam>
	internal static class JsvWriter<T>
	{
		private static readonly WriteObjectDelegate CacheFn;

		public static WriteObjectDelegate WriteFn()
		{
			return CacheFn ?? WriteObject;
		}

		static JsvWriter()
		{
		    CacheFn = typeof(T) == typeof(object) 
                ? JsvWriter.WriteLateBoundObject 
                : JsvWriter.Instance.GetWriteFn<T>();
		}

	    public static void WriteObject(TextWriter writer, object value)
		{
            using (JsState.OpenWriteObjectScope<T>())
            {
                CacheFn(writer, value);
            }
		}

	}
}
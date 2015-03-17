using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServiceStack.Text.Common
{
	internal static class JsState
	{
		//Exposing field for perf
		[ThreadStatic] internal static int WritingKeyCount = 0;

		[ThreadStatic] internal static bool IsWritingValue = false;

		[ThreadStatic] internal static bool IsWritingDynamic = false;

        public static int Depth { get { return _depth; } }

        [ThreadStatic] static Type _rootType;
        [ThreadStatic] static int _depth;

        public static IDisposable OpenWriteObjectScope<T>()
        {
            if (_depth >= JsConfig.MaxDepth)
            {
                var type = _rootType ?? typeof(T);
                var message = string.Format("Depth exceeded on type {0}. JsConfig.MaxDepth is {1}", type, JsConfig.MaxDepth);
                throw new SerializationException(message);
            }
            if (_depth++ == 0)
            {
                _rootType = typeof(T);
            }
            return WriteScopeDisposable.Instance;
        }

        private class WriteScopeDisposable : IDisposable
        {
            public static readonly IDisposable Instance = new WriteScopeDisposable();

            private WriteScopeDisposable()
            {
            }

            public void Dispose()
            {
                var depth = --_depth;
                if (depth == 0)
                {
                    _rootType = null;
                }
                Debug.Assert(depth >= 0);
            }
        }
	}
}
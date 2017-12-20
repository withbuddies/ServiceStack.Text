using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ServiceStack.Text
{
    public class JsonSerializationException : Exception
    {
        public JsonSerializationException() : base() { }
        public JsonSerializationException(string message) : base(message) { }
#if !CORE_CLR
        protected JsonSerializationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}

using System;
using System.Collections.Generic;
using System.Dynamic;

namespace ServiceStack.Text.Tests.NetCore
{
    internal class MockBaseDocument : DynamicObject
    {
        public Dictionary<string, MockDocumentAttribute> Attributes { get; set; }
    }
}

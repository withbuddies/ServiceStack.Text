using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceStack.Text.Tests.NetCore
{
    internal class MockDocumentAttribute
    {
        public MockDocumentAttribute(string value) { }

        public string T { get; set; }

        public string V { get; set; }
    }
}

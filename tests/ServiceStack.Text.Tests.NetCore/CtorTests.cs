using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceStack.Text.Tests.NetCore
{
    [TestFixture]
    public class CtorTests
    {
        [Test]
        public void Private_ctor_can_deserialize()
        {
            var result = JsonSerializer.DeserializeFromString<PrivateCtor>("{}");
            Assert.IsNotNull(result);
        }

        private class PrivateCtor
        {
            private PrivateCtor() { }
        }
    }
}

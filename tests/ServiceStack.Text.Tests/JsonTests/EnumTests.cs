using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace ServiceStack.Text.Tests.JsonTests
{
    [TestFixture]
    public class EnumTests
    {
        [Test]
        public void CanSerializeIntFlag()
        {
            var val = JsonSerializer.SerializeToString(FlagEnum.A);

            Assert.AreEqual("0", val);
        }

        [Test]
        public void CanSerializeSbyteFlag()
        {
            var val = JsonSerializer.SerializeToString(SbyteFlagEnum.A);

            Assert.AreEqual("0", val);
        }
        
        [Flags]
        public enum FlagEnum
        {
            A, B
        }

        [Flags]
        public enum SbyteFlagEnum : sbyte
        {
            A, B
        }
    }

    
}

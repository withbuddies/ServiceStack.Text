using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceStack.Text.Tests.NetCore
{
    [TestFixture]
    public class Base64Tests
    {
        class Mock { public byte[] Value { get; set; } }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(3)]
        [TestCase(10)]
        [TestCase(30)]
        [TestCase(4096)]
        [TestCase(4097)]
        [TestCase(4098)]
        [TestCase(4099)]
        [TestCase(4100)]
        public void CanDeserializeJsonWithBase64ByteArray(int size)
        {
            var bytes = new byte[size];
            new Random(1).NextBytes(bytes);
            var base64 = Convert.ToBase64String(bytes);
            var json1 = $"{{\"Value\":\"{base64}\"}}";
            var mock1 = JsonSerializer.DeserializeFromString<Mock>(json1);
            CollectionAssert.AreEqual(bytes, mock1.Value);
            var csvBytes = string.Join(",", bytes.Select(b => b.ToString()));
            var json2 = $"{{\"Value\":[{csvBytes}]}}";
            var mock2 = JsonSerializer.DeserializeFromString<Mock>(json2);
            CollectionAssert.AreEqual(bytes, mock2.Value);
        }
    }
}

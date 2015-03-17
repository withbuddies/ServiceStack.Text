using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using NUnit.Framework;
using ServiceStack.Text.Common;

namespace ServiceStack.Text.Tests
{
    [TestFixture]
    public class MadDepthTests
    {
        [SetUp]
        public void SetUp()
        {
            JsConfig.Reset();
        }

        [TearDown]
        public void TearDown()
        {
            JsConfig.Reset();
        }

        [Test]
        public void Serializing_recursive_type_to_csv_throws_serialization_exception()
        {
            var ex = Assert.Throws(typeof(SerializationException), () =>
            {
                JsConfig.MaxDepth = 20;
                var target = new MockRecursive();
                CsvSerializer.SerializeToString(target);
            });
            Trace.WriteLine(ex.Message);
            Assert.That(ex.Message.Contains(typeof(MockRecursive).Name));
            Assert.That(ex.Message.Contains("MaxDepth"));
            Assert.That(ex.Message.Contains("20"));
            Assert.AreEqual(0, JsState.Depth);
        }

        [Test]
        public void Serializing_recursive_type_to_jsv_throws_serialization_exception()
        {
            var ex = Assert.Throws(typeof(SerializationException), () =>
            {
                JsConfig.MaxDepth = 20;
                var target = new MockRecursive();
                TypeSerializer.SerializeToString(target);
            });
            Trace.WriteLine(ex.Message);
            Assert.That(ex.Message.Contains(typeof(MockRecursive).Name));
            Assert.That(ex.Message.Contains("MaxDepth"));
            Assert.That(ex.Message.Contains("20"));
            Assert.AreEqual(0, JsState.Depth);
        }

        [Test]
        public void Serializing_recursive_type_to_json_throws_serialization_exception()
        {
            var ex = Assert.Throws(typeof(SerializationException), () =>
            {
                JsConfig.MaxDepth = 20;
                var target = new MockRecursive();
                JsonSerializer.SerializeToString(target);
            });
            Trace.WriteLine(ex.Message);
            Assert.That(ex.Message.Contains(typeof(MockRecursive).Name));
            Assert.That(ex.Message.Contains("MaxDepth"));
            Assert.That(ex.Message.Contains("20"));
            Assert.AreEqual(0, JsState.Depth);
        }

        private class MockRecursive
        {
            public string Foo { get; set; }

            public MockRecursive Child
            {
                get { return this; }
                set
                {
                }
            }
        }
    }
}

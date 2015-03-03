using System;
using System.Linq;
using NUnit.Framework;

namespace ServiceStack.Text.Tests
{
    [TestFixture]
    public class EnumUtilsTests
    {
        [Test]
        public void Can_parse_enum_by_name()
        {
            Assert.That(EnumUtils.Parse<Foo>("Value1", false), Is.EqualTo(Foo.Value1));
            Assert.That(EnumUtils.Parse<Foo>("Value2", false), Is.EqualTo(Foo.Value2));
        }

        [Test]
        public void Can_parse_case_insensitive()
        {
            Assert.That(EnumUtils.Parse<Foo>("vAlue1", true), Is.EqualTo(Foo.Value1));
            Assert.That(EnumUtils.Parse<Foo>("value2", true), Is.EqualTo(Foo.Value2));
        }

        [Test]
        public void Throws_when_invalid_value_is_encountered()
        {
            Assert.Throws(typeof(ArgumentException), () => EnumUtils.Parse<Foo>("value", false));
        }

        [Test]
        public void Can_parse_numeric_values()
        {
            Assert.That(EnumUtils.Parse<Foo>("-234", false), Is.EqualTo(Foo.Value1));
            Assert.That(EnumUtils.Parse<Foo>("1", false), Is.EqualTo(Foo.Value2));
        }

        private enum Foo : short
        {
            Value1 = -234,
            Value2 = 1,
        }
    }
}

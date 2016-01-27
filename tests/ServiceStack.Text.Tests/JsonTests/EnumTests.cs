using System;
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

        [Test]
        public void CanSerializeNullableFlag()
        {
            FlagEnum? flag = null;

            var val = JsonSerializer.SerializeToString(flag);
            Assert.AreEqual(null, val);

            flag = FlagEnum.A;
            val = JsonSerializer.SerializeToString(flag);
            Assert.AreEqual("0", val);

        }

        [Test]
        public void CanDeserializeNullableEnum()
        {
            var enumJson = JsonSerializer.SerializeToString(FlagEnum.A);
            Console.WriteLine("before: " + enumJson);
            var result = JsonSerializer.DeserializeFromString<FlagEnum?>(enumJson);
            Assert.AreEqual(FlagEnum.A, result);

            enumJson = JsonSerializer.SerializeToString((FlagEnum?)null);
            Console.WriteLine("before: " + enumJson);
            result = JsonSerializer.DeserializeFromString<FlagEnum?>(enumJson);
            Assert.AreEqual(null, result);
        }

        [Test]
        public void CanDeserializeMockWithEnums()
        {
            var json = @"{""Value1"": ""A"", ""Value2"": ""B""}";
            var mock = JsonSerializer.DeserializeFromString<MockWithEnums>(json);
            Assert.AreEqual(FlagEnum.A, mock.Value1);
            Assert.AreEqual(FlagEnum.B, mock.Value2);
            json = @"{""Value1"": ""A"", ""Value2"": null}";
            mock = JsonSerializer.DeserializeFromString<MockWithEnums>(json);
            Assert.AreEqual(FlagEnum.A, mock.Value1);
            Assert.AreEqual(null, mock.Value2);
        }

        [Test]
        public void JsonNumericSerializesWithUnderlyingType()
        {
            var json = JsonSerializer.SerializeToString(new { Value = JsonNumericEnum.A });
            Assert.AreEqual(@"{""Value"":0}", json);
        }

        [JsonNumeric]
        public enum JsonNumericEnum
        {
            A, B
        }

        private class MockWithEnums
        {
            public FlagEnum Value1 { get; set; }
            public FlagEnum? Value2 { get; set; }
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

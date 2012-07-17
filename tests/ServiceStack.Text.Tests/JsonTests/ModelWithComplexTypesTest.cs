using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using NUnit.Framework;
using ServiceStack.Text.Tests.DynamicModels;

namespace ServiceStack.Text.Tests.JsonTests
{
    

    [TestFixture]
    public class ModelWithComplexTypesTest
    {
        [Flags]
        public enum FlagsEnum
        {
            A=1, B=2, C=3
        }

        public class HasDictOfFlagsToInt
        {
            public Dictionary<FlagsEnum, int> Dict { get; set; }
        }

        private class HasDate
        {
            public DateTime Date { get; set; }
        }
        
        [Test]
        public void CanSerializeTypeWithDictOfFlags()
        {
            var instance = new HasDictOfFlagsToInt
                               {
                                   Dict = new Dictionary<FlagsEnum, int> {{FlagsEnum.A, 123}}
                               };

            var json = JsonSerializer.SerializeToString(instance);

            var fromJson = new JavaScriptSerializer().Deserialize<HasDate>(json);

            StringAssert.Contains("\"1\":123", json);
            Assert.IsNotNull(fromJson );
        }

        [Test]
        public void CanSerializeOddDate()
        {
            var dt = new DateTime(2012, 3, 25, 1, 30, 0, 0, DateTimeKind.Unspecified);

            TimeZoneInfo.ConvertTimeToUtc(dt);

            var entity = new HasDate {Date = dt};

            var json = JsonSerializer.SerializeToString(entity);

            var fromJson = new JavaScriptSerializer().Deserialize<HasDate>(json);

            Assert.AreEqual(fromJson.Date, entity.Date);
        }

        [Test]
        public void Can_Serialize()
        {
            var m1 = ModelWithComplexTypes.Create(1);
            var s = JsonSerializer.SerializeToString(m1);
            Console.WriteLine(s);

            var m2 = new JavaScriptSerializer().Deserialize<ModelWithComplexTypes>(s);

            Assert.AreEqual(m1.ListValue[0], m2.ListValue[0]);
            Assert.AreEqual(m1.DictionaryValue["a"], m2.DictionaryValue["a"]);
            Assert.AreEqual(m1.ByteArrayValue[0], m2.ByteArrayValue[0]);
        }

        [Test]
        public void Can_Serialize_WhenNull()
        {
            var m1 = new ModelWithComplexTypes();

            JsConfig.IncludeNullValues = false;
            var s = JsonSerializer.SerializeToString(m1);
            Console.WriteLine(s);

            var m2 = new JavaScriptSerializer().Deserialize<ModelWithComplexTypes>(s);
            JsConfig.Reset();

            Assert.IsNull(m2.DictionaryValue);
            Assert.IsNull(m2.ListValue);
            Assert.IsNull(m2.ConcreteListValue);
            Assert.IsNull(m2.ArrayValue);
            Assert.IsNull(m2.NestedTypeValue);
            Assert.IsNull(m2.ByteArrayValue);
        }

        [Test]
        public void Can_Serialize_NullsWhenNull()
        {
            var m1 = new ModelWithComplexTypes();

            JsConfig.IncludeNullValues = true;
            var s = JsonSerializer.SerializeToString(m1);
            Console.WriteLine(s);

            var m2 = new JavaScriptSerializer().Deserialize<ModelWithComplexTypes>(s);
            JsConfig.Reset();

            Assert.IsNull(m2.DictionaryValue);
            Assert.IsNull(m2.ListValue);
            Assert.IsNull(m2.ArrayValue);
            Assert.IsNull(m2.NestedTypeValue);
            Assert.IsNull(m2.ByteArrayValue);
            Assert.IsNull(m2.DateTimeValue);
        }


    }
}
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ServiceStack.Text.Tests.NetCore
{
    [TestFixture]
    class JsConfigTests
    {
        public class Mock
        {
            public string Value { get; set; }
        }

        public static IEnumerable OpenIncludeNullVaulesCases = new[]
        {
            new TestCaseData(true, new Mock { Value = "foo" }, "{\"Value\":\"foo\"}"),
            new TestCaseData(false, new Mock { Value = "foo" }, "{\"Value\":\"foo\"}"),
            new TestCaseData(true, new Mock(), "{\"Value\":null}"),
            new TestCaseData(false, new Mock(), "{}"),
        };

        [TestCaseSource(nameof(OpenIncludeNullVaulesCases))]
        public void OpenIncludeNullValuesScope_should_serialize_to_expected_output(bool includeNullValues, Mock before, string expectedJson)
        {
            using (JsConfig.OpenIncludeNullValuesScope(includeNullValues))
            {
                var json = JsonSerializer.SerializeToString(before);
                Assert.AreEqual(expectedJson, json);
            }
        }


        [TestCaseSource(nameof(OpenIncludeNullVaulesCases))]
        public void OpenIncludeNullValuesScope_should_serialize_to_expected_output_with_recursion(bool includeNullValues, Mock before, string expectedJson)
        {
            using (JsConfig.OpenIncludeNullValuesScope(!includeNullValues))
            using (JsConfig.OpenIncludeNullValuesScope(includeNullValues))
            {
                var json = JsonSerializer.SerializeToString(before);
                Assert.AreEqual(expectedJson, json);
            }
        }
    }
}

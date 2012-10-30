using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using NUnit.Framework;
using ServiceStack.Text.Json;

namespace ServiceStack.Text.Tests.JsonTests
{
    [TestFixture]
    public class StringParsingTests
    {
        [Test]
        public void CanUnescapeRegular()
        {
            Assert.AreEqual("test", StringEscaper.Unescape("test"));
            Assert.AreEqual("", StringEscaper.Unescape(""));
            Assert.AreEqual("asdf asdf asdf ", StringEscaper.Unescape("asdf asdf asdf "));
        }

        [Test]
        public void CanUnesacapeOne()
        {
            Assert.AreEqual("test\ttest", StringEscaper.Unescape("test\\ttest"));
            Assert.AreEqual("\ttesttest", StringEscaper.Unescape("\\ttesttest"));
            Assert.AreEqual("testtest\t", StringEscaper.Unescape("testtest\\t"));
        }

        [Test]
        public void CanUnesacapeMultiple()
        {
            Assert.AreEqual("test\t\ttest", StringEscaper.Unescape("test\\t\\ttest"));
            Assert.AreEqual("\t\ttesttest", StringEscaper.Unescape("\\t\\ttesttest"));
            Assert.AreEqual("testtest\t\t", StringEscaper.Unescape("testtest\\t\\t"));

            Assert.AreEqual("test\tt\test", StringEscaper.Unescape("test\\tt\\test"));
            Assert.AreEqual("\ttest\ttest", StringEscaper.Unescape("\\ttest\\ttest"));
            Assert.AreEqual("test\ttest\t", StringEscaper.Unescape("test\\ttest\\t"));
        }

        [Test]
        public void CanUnescapeUnicodeEntity()
        {
            Assert.AreEqual("the word is \u1ab9", StringEscaper.Unescape("the word is \\u1ab9"));
        }

        [Test]
        public void Performance()
        {
            string[] tests = new[]
                                 {
                                     "test\\tt\\test",
                                     "test\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\test"
                                     ,
                                     "test\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\test"
                                     ,
                                     "test\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\testtest\\tt\\test"
                                 };


            const int itterations = 100;

            Stopwatch ours = Stopwatch.StartNew();
            for(int i = 0; i < itterations; i++)
            {
                StringEscaper.Unescape(tests[0]);
                StringEscaper.Unescape(tests[1]);
                StringEscaper.Unescape(tests[2]);
                StringEscaper.Unescape(tests[3]);
            }
            ours.Stop();
            
            Console.WriteLine("Ours executed in " + ours.ElapsedMilliseconds + "ms");

            Stopwatch reges = Stopwatch.StartNew();
            for (int i = 0; i < itterations; i++)
            {
                Regex.Unescape(tests[0]);
                Regex.Unescape(tests[1]);
                Regex.Unescape(tests[2]);
                Regex.Unescape(tests[3]);
            }
            reges.Stop();

            Console.WriteLine("REgex executed in " + reges.ElapsedMilliseconds + "ms");

        }
    }
}
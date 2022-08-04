using NUnit.Framework;
using System;
using System.IO;

namespace HelloWorld
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                DemoTest.Program.Main();

                var result = sw.ToString().Trim();
                Assert.AreEqual("Hello World!", result);
            }
        }
    }
}
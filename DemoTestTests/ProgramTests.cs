using Microsoft.VisualStudio.TestTools.UnitTesting;
using DemoTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DemoTest.Tests
{
    [TestClass()]
    public class ProgramTests
    {
        private const string Expected = "Hello World!";
        [TestMethod()]
        public void MainTest()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                DemoTest.Program.Main();

                var result = sw.ToString().Trim();
                Assert.AreEqual(Expected, result);
            }
        }
    }
}

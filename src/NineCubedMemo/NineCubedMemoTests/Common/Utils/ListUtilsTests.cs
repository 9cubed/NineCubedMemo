using Microsoft.VisualStudio.TestTools.UnitTesting;
using NineCubed.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Common.Utils.Tests
{
    [TestClass()]
    public class ListUtilsTests
    {
        [TestMethod()]
        public void RemoveAllEmptyTest()
        {
            Test(null, "");
            Test("", "");
            Test("    ", "");
            Test("1", "1");
            Test("1 2", "1 2");
            Test("1 2 3", "1 2 3");
            Test("    1    2    3    ", "1 2 3");

            void Test(string param, string result)
            {
                var list = param?.Split(' ').ToList();
                ListUtils.RemoveAllEmpty(list);
                if (list != null) Assert.AreEqual(string.Join(" ", list), result);
            }
        }

        [TestMethod()]
        public void SplitTest()
        {
            Test(null, "");
            Test("", "");
            Test("    ", "");
            Test("1", "1");
            Test("1 2", "1 2");
            Test("1 2 3", "1 2 3");
            Test("    1    2    3    ", "1 2 3");

            void Test(string param, string result)
            {
                var list = ListUtils.Split(" ", param);
                Assert.AreEqual(string.Join(" ", list), result);
            }
        }
    }
}
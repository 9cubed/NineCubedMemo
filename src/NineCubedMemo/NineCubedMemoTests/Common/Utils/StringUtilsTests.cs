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
    public class StringUtilsTests
    {
        [TestMethod()]
        public void CountCharTest()
        {
            Assert.AreEqual(StringUtils.CountChar("1111111111", '1'), 10);
            Assert.AreEqual(StringUtils.CountChar("1234567890", 'A'), 0);
            Assert.AreEqual(StringUtils.CountChar("", '1'), 0);
            Assert.AreEqual(StringUtils.CountChar(null, 'A'), 0);
        }

        [TestMethod()]
        public void ExistsCharTest()
        {
            Assert.IsTrue(StringUtils.ExistsChar("0123456789", '0'));
            Assert.IsTrue(StringUtils.ExistsChar("0123456789", '5'));
            Assert.IsTrue(StringUtils.ExistsChar("0123456789", '9'));
            Assert.IsFalse(StringUtils.ExistsChar("0123456789", 'A'));
            Assert.IsFalse(StringUtils.ExistsChar("", '9'));
            Assert.IsFalse(StringUtils.ExistsChar(null, '9'));
        }
    }
}
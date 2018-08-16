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
    public class BinaryUtilsTests
    {
        [TestMethod()]
        public void HexToByteArrayTest()
        {
            {
                string hex = "00 01 fe ff";
                byte[] byteArray = BinaryUtils.HexToByteArray(hex);
                Assert.AreEqual(byteArray[0], 0);
                Assert.AreEqual(byteArray[1], 1);
                Assert.AreEqual(byteArray[2], 0xfe);
                Assert.AreEqual(byteArray[3], 0xff);

                Assert.AreEqual(BinaryUtils.ByteArrayToHex(byteArray), hex);
            }

            {
                //\t \n スペースは無視する
                byte[] byteArray = BinaryUtils.HexToByteArray("00\t\n01\n  \tfe  ff");
                Assert.AreEqual(byteArray[0], 0);
                Assert.AreEqual(byteArray[1], 1);
                Assert.AreEqual(byteArray[2], 0xfe);
                Assert.AreEqual(byteArray[3], 0xff);

                Assert.AreEqual(BinaryUtils.ByteArrayToHex(byteArray), "00 01 fe ff");
            }

            {
                byte[] byteArray = BinaryUtils.HexToByteArray("  \r \t \n ");
                Assert.AreEqual(byteArray.Length, 0);
            }

            {
                //例外発生パターン
                try
                {
                    byte[] byteArray = BinaryUtils.HexToByteArray("w");
                    Assert.Fail();
                }
                catch (Exception) { }
            }

            {
                //例外発生パターン
                try
                {
                    byte[] byteArray = BinaryUtils.HexToByteArray("111");
                    Assert.Fail();
                }
                catch (Exception) { }
            }

        } //HexToByteArrayTest()

        [TestMethod()]
        public void ByteArrayToHexTest()
        {
            //基本的なテストは HexToByteArrayTest() で行っています。

            {
                byte[] byteArray = new byte[0];
                string hex = BinaryUtils.ByteArrayToHex(byteArray);
                Assert.AreEqual(hex.Length, 0);
            }

            {
                byte[] byteArray = null;
                string hex = BinaryUtils.ByteArrayToHex(byteArray);
                Assert.AreEqual(hex.Length, 0);
            }

        } //ByteArrayToHexTest()


    } //class
}
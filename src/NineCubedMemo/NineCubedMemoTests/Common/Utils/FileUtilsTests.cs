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
    public class FileUtilsTests
    {
        [TestMethod()]
        [DeploymentItem(@"TestData/Common/Files/Utils/FileUtils/empty.txt", "TestData/Common/Files/Utils/FileUtils")]
        public void SaveFileFromByteArrayTest()
        {
            string s = "あいうえお\nかきくけこ\nさしすせそ\nたちつてと\nなにぬねの\nはひふへほ\nまみむめも\nやゆよ\nわをん\n";
            byte[] byteArray = Encoding.GetEncoding(932).GetBytes(s);
            string path = "TestData/Common/Files/Utils/FileUtils/test.txt";
            FileUtils.SaveFileFromByteArray("TestData/Common/Files/Utils/FileUtils/test.txt", byteArray);

            {
                byte[] byteArray2 = FileUtils.LoadFileToByteArray(path);
                string s2 = Encoding.GetEncoding(932).GetString(byteArray2);
                Assert.AreEqual(s, s2);
            }

            {
                //サイズ指定読み込みテスト
                byte[] byteArray2 = FileUtils.LoadFileToByteArray(path, 10);
                Assert.AreEqual(byteArray2.Length, 10);
            }

            {
                //サイズ指定読み込みテスト
                byte[] byteArray2 = FileUtils.LoadFileToByteArray(path, int.MaxValue);
                Assert.AreEqual(byteArray.Length, byteArray2.Length);
            }

        }

        [TestMethod()]
        public void ContainsDirTest()
        {
            Assert.IsTrue(FileUtils.ContainsDir("c:/", "c:/test1"));
            Assert.IsTrue(FileUtils.ContainsDir("c:/", "c:\\test1"));
            Assert.IsTrue(FileUtils.ContainsDir("c:\\", "c:/test1"));

            Assert.IsTrue(FileUtils.ContainsDir("c:/test"  , "c:/test"));
            Assert.IsTrue(FileUtils.ContainsDir("c:/test"  , "c:/test/"));
            Assert.IsTrue(FileUtils.ContainsDir("c:/test"  , "c:/test\\"));

            Assert.IsTrue(FileUtils.ContainsDir("c:/test/"  , "c:/test"));
            Assert.IsTrue(FileUtils.ContainsDir("c:/test/"  , "c:/test/"));
            Assert.IsTrue(FileUtils.ContainsDir("c:/test/"  , "c:/test\\"));

            Assert.IsTrue(FileUtils.ContainsDir("c:/test\\"  , "c:/test"));
            Assert.IsTrue(FileUtils.ContainsDir("c:/test\\"  , "c:/test/"));
            Assert.IsTrue(FileUtils.ContainsDir("c:/test\\"  , "c:/test\\"));

            Assert.IsTrue(FileUtils.ContainsDir("c:/test"  , "c:/test/test1"));
            Assert.IsTrue(FileUtils.ContainsDir("c:/test/" , "c:/test/test1"));
            Assert.IsTrue(FileUtils.ContainsDir("c:/test\\", "c:/test/test1"));

            Assert.IsTrue(FileUtils.ContainsDir("c:/test"  , "c:/test/test1/test2"));
            Assert.IsTrue(FileUtils.ContainsDir("c:/test"  , "c:/test/test1/test2/test3"));

            //False
            Assert.IsFalse(FileUtils.ContainsDir("c:/test", "d:/test/test1/"));
            Assert.IsFalse(FileUtils.ContainsDir("c:/test/test1", "c:/test/"));
            Assert.IsFalse(FileUtils.ContainsDir("c:/test1/abc", "c:/test2/abc"));
        }


    } //class
}
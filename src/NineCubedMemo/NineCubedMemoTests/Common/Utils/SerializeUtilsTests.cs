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
    public class SerializeUtilsTests
    {

        [ClassInitialize]
        public static void TestStart(TestContext context) { }

        [ClassCleanup]
        public static void TestEnd() { }

        //ダミーのテストメソッド
        //[DeploymentItem]が付いたメソッドがない場合、カレントパスがTestResults配下に変わらないため
        [DeploymentItem("dummy")]
        [TestMethod()]
        public void Dummy() {
            //System.Diagnostics.Debug.WriteLine(System.IO.Directory.GetCurrentDirectory());
        }


        [TestMethod()]
        public void SerializeToFileTest()
        {
            string path = "SerializeToFileTest.xml";

            {
                var list1 = new List<string>();
                list1.Add("abc");
                list1.Add("あいうえお");
                list1.Add("");
                list1.Add("<\n>");
                list1.Add(null);

                //Obj -> XmlFile
                SerializeUtils.SerializeToFile(list1, path);

                //XmlFile -> Obj
                var list2 = (List<string>)SerializeUtils.DeserializeFromFile(typeof(List<string>), path);

                //Check
                Assert.AreEqual(list1.Count, list2.Count);
                for (int i = 0; i < list1.Count; i++) {
                    Assert.AreEqual(list1[i], list2[i]);
                }
            }
        }

        [TestMethod()]
        public void DeserializeFromFileTest()
        {
            //SerializeToFileTest();
        }

        [TestMethod()]
        public void SerializeToStringTest()
        {
            var list1 = new List<string>();
            list1.Add("abc");
            list1.Add("あいうえお");
            list1.Add("");
            list1.Add("<\n>");
            list1.Add(null);

            //Obj -> Xml形式の文字列
            var strXml = SerializeUtils.SerializeToString(list1);

            //Xml形式の文字列 -> Obj
            var list2 = (List<string>)SerializeUtils.DeserializeFromString(typeof(List<string>), strXml);

            //Check
            Assert.AreEqual(list1.Count, list2.Count);
            for (int i = 0; i < list1.Count; i++) {
                Assert.AreEqual(list1[i], list2[i]);
            }
        }

        [TestMethod()]
        public void DeserializeFromStringTest()
        {
            //SerializeToStringTest();
        }

    } //class
}
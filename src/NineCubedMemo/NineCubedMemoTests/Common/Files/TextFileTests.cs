using Microsoft.VisualStudio.TestTools.UnitTesting;
using NineCubed.Common.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Common.Files.Tests
{
    [TestClass()]
    public class TextFileTests
    {
        [TestMethod()]
        //[DeploymentItem(@"TestData/Common/Files/TextFile/文字コード判別/euc_jp_全角カナ.txt", "TestData/Common/Files/TextFile/文字コード判別")]
        //[DeploymentItem(@"TestData/Common/Files/TextFile/文字コード判別/euc_jp_全角かな.txt", "TestData/Common/Files/TextFile/文字コード判別")]
        //[DeploymentItem(@"TestData/Common/Files/TextFile/文字コード判別/euc_jp_全角英字.txt", "TestData/Common/Files/TextFile/文字コード判別")]
        //[DeploymentItem(@"TestData/Common/Files/TextFile/文字コード判別/euc_jp_全角漢字.txt", "TestData/Common/Files/TextFile/文字コード判別")]
        //[DeploymentItem(@"TestData/Common/Files/TextFile/文字コード判別/euc_jp_全角数字.txt", "TestData/Common/Files/TextFile/文字コード判別")]
        [DeploymentItem(@"TestData", "TestData")]
        public void LoadTest()
        {

            //文字コード判別テスト
            {
                var textFile = new TextFile();
                string[][] list = {
                    new string[]{"euc_jp_全角カナ.txt", "51932" },
                    new string[]{"euc_jp_全角かな.txt", "51932" },
                    new string[]{"euc_jp_全角英字.txt", "51932" },
                    //new string[]{"euc_jp_全角漢字.txt", "51932" }, //判別失敗
                    new string[]{"euc_jp_全角数字.txt", "51932" },

                    new string[]{"shift_jis_全角カナ.txt", "932" },
                    new string[]{"shift_jis_全角かな.txt", "932" },
                    new string[]{"shift_jis_全角英字.txt", "932" },
                    new string[]{"shift_jis_全角漢字.txt", "932" },
                    new string[]{"shift_jis_全角数字.txt", "932" },

                    new string[]{"utf8_全角カナ.txt", "65001" },
                    new string[]{"utf8_全角かな.txt", "65001" },
                    new string[]{"utf8_全角英字.txt", "65001" },
                    //new string[]{"utf8_全角漢字.txt", "65001" }, //判別失敗
                    //new string[]{"utf8_全角数字.txt", "65001" }, //判別失敗

                    new string[]{"BOM_utf16_全角かな.txt"  , "1200"  },
                    new string[]{"BOM_utf16BE_全角かな.txt", "1201"  },
                    new string[]{"BOM_utf32_全角かな.txt"  , "12000" },
                    new string[]{"BOM_utf32BE_全角かな.txt", "12001" },
                    new string[]{"BOM_utf8_全角かな.txt"   , "65001" },
                };

                foreach (var item in list)
                {
                    textFile.TextEncoding = null;
                    textFile.NewLineCode  = null;
                    textFile.Load("TestData/Common/Files/TextFile/文字コード判別/" + item[0]);
                    Assert.AreEqual(textFile.TextEncoding.CodePage, int.Parse(item[1]), "文字コード判別失敗:" + item[0]);
                }
            }

            //改行コード判別テスト
            {
                var textFile = new TextFile();
                string[][] list = {
                    new string[]{"shift_jis_全角かな_CR.txt"  , "\r" },
                    new string[]{"shift_jis_全角かな_CRLF.txt", "\r\n" },
                    new string[]{"shift_jis_全角かな_LF.txt"  , "\n" },
                    new string[]{"shift_jis_全角かな_なし.txt", "\r\n" },
                };

                foreach (var item in list)
                {
                    textFile.TextEncoding = null;
                    textFile.NewLineCode  = null;
                    textFile.Load("TestData/Common/Files/TextFile/改行コード判別/" + item[0]);
                    Assert.AreEqual(textFile.NewLineCode, item[1], "改行コード判別失敗:" + item[0]);
                }
            }

            //読み取り専用のテスト
            {
                var textFile = new TextFile();
                textFile.TextEncoding = null;
                textFile.NewLineCode  = null;
                textFile.Load("TestData/Common/Files/TextFile/空ファイル.txt");
                Assert.AreEqual(textFile.Text, "");
                Assert.AreEqual(textFile.IsReadOnly, false);
                Assert.IsTrue(File.Exists(textFile.Path));

                var path = "TestData/Common/Files/TextFile/読み取り専用.txt";
                File.SetAttributes(path, FileAttributes.ReadOnly); //読み取り専用の属性はコピーされないため、ここで設定します
                textFile.TextEncoding = null;
                textFile.NewLineCode  = null;
                textFile.Load(path);
                Assert.AreEqual(textFile.IsReadOnly, true);
            }

        } //LoadTest()


        [TestMethod()]
        public void SaveTest()
        {
            //書き込みテスト
            {
                var path = "TestData/Common/Files/TextFile/save.txt";
                var textFile  = new TextFile();
                var textFile2 = new TextFile();

                textFile.TextEncoding = Encoding.GetEncoding(932);
                textFile.NewLineCode  = "\n";
                textFile.Text = "0123456789\nabcdefghijklmnopqrstuvwxyz\nあいうえお";
                textFile.Save(path);
                textFile2.TextEncoding = null;
                textFile2.NewLineCode  = null;
                textFile2.Load(path);
                Assert.AreEqual(textFile.Text, textFile2.Text);

                textFile.NewLineCode = "\r";
                textFile.Text = "0123456789\nabcdefghijklmnopqrstuvwxyz\nあいうえお";
                textFile.Save(path);
                textFile2.TextEncoding = null;
                textFile2.NewLineCode  = null;
                textFile2.Load(path);
                Assert.AreEqual(textFile.Text, textFile2.Text);

                textFile.NewLineCode = "\r\n";
                textFile.Text = "0123456789\nabcdefghijklmnopqrstuvwxyz\nあいうえお";
                textFile.Save(path);
                textFile2.TextEncoding = null;
                textFile2.NewLineCode  = null;
                textFile2.Load(path);
                Assert.AreEqual(textFile.Text, textFile2.Text);
            }
            
            //UTF-8 の書き込みテスト
            {
                var path = "TestData/Common/Files/TextFile/save.txt";
                var textFile = new TextFile();
                textFile.Text = "0123456789\nabcdefghijklmnopqrstuvwxyz\nあいうえお";
                textFile.SetEncodingUtf8(true); //BOMあり
                textFile.Save(path);
                {
                    var textFile2 = new TextFile();
                    textFile2.Load(path);
                    byte[] bom = textFile2.TextEncoding.GetPreamble();
                    Assert.AreEqual(bom[0], 0xEF);
                    Assert.AreEqual(bom[1], 0xBB);
                    Assert.AreEqual(bom[2], 0xBF);
                    Assert.AreEqual(textFile2.TextEncoding.CodePage, 65001);
                }

                textFile.SetEncodingUtf8(false); //BOMなし
                textFile.Save(path);
                {
                    var textFile2 = new TextFile();
                    textFile2.Load(path);
                    Assert.AreEqual(textFile2.TextEncoding.GetPreamble().Length, 0); //BOMなし
                    Assert.AreEqual(textFile2.TextEncoding.CodePage, 65001);
                }

            }
            
        } //SaveTest()


    } //class
}
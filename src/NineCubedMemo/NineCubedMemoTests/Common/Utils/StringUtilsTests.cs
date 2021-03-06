﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        [TestMethod()]
        public void HankakuToZenkakuTest()
        {
            {
                //半角 -> 全角
                string hankaku = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~｡｢｣､･ｦｧｨｩｪｫｬｭｮｯｰｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃﾄﾅﾆﾇﾈﾉﾊﾋﾌﾍﾎﾏﾐﾑﾒﾓﾔﾕﾖﾗﾘﾙﾚﾛﾜﾝﾞﾟ";
                string zenkaku = StringUtils.HankakuToZenkaku(hankaku);
                Assert.AreEqual(zenkaku, "　！”＃＄％＆’（）＊＋，－．／０１２３４５６７８９：；＜＝＞？＠ＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＹＺ［￥］＾＿｀ａｂｃｄｅｆｇｈｉｊｋｌｍｎｏｐｑｒｓｔｕｖｗｘｙｚ｛｜｝￣。「」、・ヲァィゥェォャュョッーアイウエオカキクケコサシスセソタチツテトナニヌネノハヒフヘホマミムメモヤユヨラリルレロワン゛゜");

                //全角 -> 半角
                string hankaku2 = StringUtils.ZenkakuToHankaku(zenkaku);
                Assert.AreEqual(hankaku, hankaku2);
            }

            {
                //半角 -> 全角
                string hankaku = "ﾞｦﾞｧﾞｨﾞｩﾞｪﾞｫﾞｬﾞｭﾞｮﾞｯﾞｰﾞｱﾞｲﾞｳﾞｴﾞｵﾞｶﾞｷﾞｸﾞｹﾞｺﾞｻﾞｼﾞｽﾞｾﾞｿﾞﾀﾞﾁﾞﾂﾞﾃﾞﾄﾞﾅﾞﾆﾞﾇﾞﾈﾞﾉﾞﾊﾞﾋﾞﾌﾞﾍﾞﾎﾞﾏﾞﾐﾞﾑﾞﾒﾞﾓﾞﾔﾞﾕﾞﾖﾞﾗﾞﾘﾞﾙﾞﾚﾞﾛﾞﾜﾞﾝﾞﾞ";
                string zenkaku = StringUtils.HankakuToZenkaku(hankaku);
                Assert.AreEqual(zenkaku, "゛ヲ゛ァ゛ィ゛ゥ゛ェ゛ォ゛ャ゛ュ゛ョ゛ッ゛ー゛ア゛イ゛ヴエ゛オ゛ガギグゲゴザジズゼゾダヂヅデドナ゛ニ゛ヌ゛ネ゛ノ゛バビブベボマ゛ミ゛ム゛メ゛モ゛ヤ゛ユ゛ヨ゛ラ゛リ゛ル゛レ゛ロ゛ワ゛ン゛゛");

                //全角 -> 半角
                string hankaku2 = StringUtils.ZenkakuToHankaku(zenkaku);
                Assert.AreEqual(hankaku, hankaku2);
            }

            {
                //半角 -> 全角
                string hankaku = "ﾟｦﾟｧﾟｨﾟｩﾟｪﾟｫﾟｬﾟｭﾟｮﾟｯﾟｰﾟｱﾟｲﾟｳﾟｴﾟｵﾟｶﾟｷﾟｸﾟｹﾟｺﾟｻﾟｼﾟｽﾟｾﾟｿﾟﾀﾟﾁﾟﾂﾟﾃﾟﾄﾟﾅﾟﾆﾟﾇﾟﾈﾟﾉﾟﾊﾟﾋﾟﾌﾟﾍﾟﾎﾟﾏﾟﾐﾟﾑﾟﾒﾟﾓﾟﾔﾟﾕﾟﾖﾟﾗﾟﾘﾟﾙﾟﾚﾟﾛﾟﾜﾟﾝﾟﾟ";
                string zenkaku = StringUtils.HankakuToZenkaku(hankaku);
                Assert.AreEqual(zenkaku, "゜ヲ゜ァ゜ィ゜ゥ゜ェ゜ォ゜ャ゜ュ゜ョ゜ッ゜ー゜ア゜イ゜ウ゜エ゜オ゜カ゜キ゜ク゜ケ゜コ゜サ゜シ゜ス゜セ゜ソ゜タ゜チ゜ツ゜テ゜ト゜ナ゜ニ゜ヌ゜ネ゜ノ゜パピプペポマ゜ミ゜ム゜メ゜モ゜ヤ゜ユ゜ヨ゜ラ゜リ゜ル゜レ゜ロ゜ワ゜ン゜゜");

                //全角 -> 半角
                string hankaku2 = StringUtils.ZenkakuToHankaku(zenkaku);
                Assert.AreEqual(hankaku, hankaku2);
            }

            {
                //
                //半角 -> 全角 -> 半角
                char c = (char)0;
                for (int i = 0; i <= char.MaxValue; i++)
                {
                    c = (char)i;
                    string hankaku = c.ToString();
                    string zenkaku = StringUtils.HankakuToZenkaku(hankaku);
                    if (hankaku.Equals(zenkaku))
                    {
                    }
                    else
                    {
                        string hankaku2 = StringUtils.ZenkakuToHankaku(zenkaku);
                        Assert.AreEqual(hankaku, hankaku2);
                    }
                }
            }
        }

        [TestMethod()]
        public void RepeatCharTest()
        {
            Assert.AreEqual(StringUtils.RepeatChar('a', 1), "a");
            Assert.AreEqual(StringUtils.RepeatChar('a', 5), "aaaaa");
            Assert.AreEqual(StringUtils.RepeatChar('a', 0), "");
            Assert.AreEqual(StringUtils.RepeatChar('a', -1), "");
        }

        [TestMethod()]
        public void IsNotEmptyTest()
        {
            Assert.AreEqual(StringUtils.IsNotEmpty(null), false);
            Assert.AreEqual(StringUtils.IsNotEmpty(""), false);
            Assert.AreEqual(StringUtils.IsNotEmpty(" "), true);
            Assert.AreEqual(StringUtils.IsNotEmpty("0"), true);
            Assert.AreEqual(StringUtils.IsNotEmpty("a"), true);
        }

        [TestMethod()]
        public void ToBoolTest()
        {
            Assert.AreEqual(StringUtils.ToBool("true" , false), true);
            Assert.AreEqual(StringUtils.ToBool("TRUE" , false), true);
            Assert.AreEqual(StringUtils.ToBool("TRue" , false), true);
            Assert.AreEqual(StringUtils.ToBool("false", false), false);
            Assert.AreEqual(StringUtils.ToBool("FALSE", false), false);
            Assert.AreEqual(StringUtils.ToBool("FAlse", false), false);
            Assert.AreEqual(StringUtils.ToBool(""  , false), false);
            Assert.AreEqual(StringUtils.ToBool("0" , false), false);
            Assert.AreEqual(StringUtils.ToBool("1" , false), false);
            Assert.AreEqual(StringUtils.ToBool("-1", false), false);
        }
    } //class
}
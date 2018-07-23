using Microsoft.VisualStudio.TestTools.UnitTesting;
using NineCubed.Common.Calculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Common.Calculation.Tests
{
    [TestClass()]
    public class CalculatorTests
    {
        [TestMethod()]
        public void CalcTest()
        {
            Assert.AreEqual(Calculator.Calc("0"), "0");
            Assert.AreEqual(Calculator.Calc("1"), "1");
            Assert.AreEqual(Calculator.Calc("+1"), "1");
            Assert.AreEqual(Calculator.Calc("-1"), "-1");
            Assert.AreEqual(Calculator.Calc("+10"), "10");
            Assert.AreEqual(Calculator.Calc("-10"), "-10");
            Assert.AreEqual(Calculator.Calc("1+1"), "2");
            Assert.AreEqual(Calculator.Calc("+1+1"), "2");
            Assert.AreEqual(Calculator.Calc("1-1"), "0");
            Assert.AreEqual(Calculator.Calc("+1-1"), "0");
            Assert.AreEqual(Calculator.Calc("-1-1"), "-2");
            Assert.AreEqual(Calculator.Calc("--1-1"), "0");
            Assert.AreEqual(Calculator.Calc("---1-1"), "-2");
            Assert.AreEqual(Calculator.Calc("+--+-1-1"), "-2");
            Assert.AreEqual(Calculator.Calc("12.3+65.4"), "77.7");
            Assert.AreEqual(Calculator.Calc("12.3-1.2"), "11.1");
            Assert.AreEqual(Calculator.Calc("-1.2+12.3"), "11.1");
            Assert.AreEqual(Calculator.Calc("-.2+.3"), "0.1");
            Assert.AreEqual(Calculator.Calc("-0.01+0.02"), "0.01");
            Assert.AreEqual(Calculator.Calc("10*5"), "50");
            Assert.AreEqual(Calculator.Calc("-10*5"), "-50");
            Assert.AreEqual(Calculator.Calc("10*-5"), "-50");
            Assert.AreEqual(Calculator.Calc("-10*-5"), "50");
            Assert.AreEqual(Calculator.Calc("10+10*5"), "60");
            Assert.AreEqual(Calculator.Calc("5*10+10"), "60");
            Assert.AreEqual(Calculator.Calc("10+10/5"), "12");
            Assert.AreEqual(Calculator.Calc("5/10+10"), "10.5");
            Assert.AreEqual(Calculator.Calc("1*2*3*4"), "24");
            Assert.AreEqual(Calculator.Calc("24/4/3/2/1"), "1");
            Assert.AreEqual(Calculator.Calc("10/10*10/5*5"), "10");
            Assert.AreEqual(Calculator.Calc("1+2*3+4"), "11");
            Assert.AreEqual(Calculator.Calc("1*2+3*4"), "14");
            Assert.AreEqual(Calculator.Calc("-1-2*-3-4"), "1");
            Assert.AreEqual(Calculator.Calc("-1*-2-3*-4"), "14");
            Assert.AreEqual(Calculator.Calc("-1*10+2*20-30/3-10"), "10");
            Assert.AreEqual(Calculator.Calc("10/3"), "3.33333333333333");
            Assert.AreEqual(Calculator.Calc("-10/-3"), "3.33333333333333");
            Assert.AreEqual(Calculator.Calc("-10/3"), "-3.33333333333333");
            Assert.AreEqual(Calculator.Calc("10/-3"), "-3.33333333333333");
            Assert.AreEqual(Calculator.Calc("10/3*3"), "9.99999999999999");
            Assert.AreEqual(Calculator.Calc("10/-3*3"), "-9.99999999999999");
            Assert.AreEqual(Calculator.Calc("10/-3*3-0.00000000000001"), "-10");

            Assert.AreEqual(Calculator.Calc("-"), null);
            Assert.AreEqual(Calculator.Calc("."), null);
            Assert.AreEqual(Calculator.Calc("="), null);
            Assert.AreEqual(Calculator.Calc("a"), null);
            Assert.AreEqual(Calculator.Calc("\t\r\n1\t\r\n"), "1");
            Assert.AreEqual(Calculator.Calc("*1+2"), null);
            Assert.AreEqual(Calculator.Calc("/1+2"), null);
            Assert.AreEqual(Calculator.Calc("1/0"), null);
            Assert.AreEqual(Calculator.Calc("-1/0"), null);
            Assert.AreEqual(Calculator.Calc("0/0"), null);
            Assert.AreEqual(Calculator.Calc("1..1+5"), null);
            Assert.AreEqual(Calculator.Calc("1.5ee-2"), null);
            Assert.AreEqual(Calculator.Calc("1.5EE-2"), null);
            Assert.AreEqual(Calculator.Calc("1.5ee2"), null);
            Assert.AreEqual(Calculator.Calc("1.5EE2"), null);

            Assert.AreEqual(Calculator.Calc("8^3") , (8*8*8).ToString());
            Assert.AreEqual(Calculator.Calc("8^2") , (8*8).ToString());
            Assert.AreEqual(Calculator.Calc("8^1") , (8).ToString());
            Assert.AreEqual(Calculator.Calc("8^0") , (1).ToString());
            Assert.AreEqual(Calculator.Calc("8^-1"), ((decimal)1/8).ToString());
            Assert.AreEqual(Calculator.Calc("8^-2"), ((decimal)1/(8*8)).ToString());
            Assert.AreEqual(Calculator.Calc("8^-3"), ((decimal)1/(8*8*8)).ToString());
            Assert.AreEqual(Calculator.Calc("-2.5^3"), "-15.625");
            Assert.AreEqual(Calculator.Calc("2.5^3"), "15.625");
            Assert.AreEqual(Calculator.Calc("2.5^-3"), "0.064");
            Assert.AreEqual(Calculator.Calc("100^-0.5"), "0.1");
            Assert.AreEqual(Calculator.Calc("1+2^-3*-4"), "0.5");

            Assert.AreEqual(Calculator.Calc("5!"),  (1*2*3*4*5).ToString());
            Assert.AreEqual(Calculator.Calc("3!!"), (1*2*3*4*5*6).ToString());
            Assert.AreEqual(Calculator.Calc("4!!"), null);
            Assert.AreEqual(Calculator.Calc("-3!"), null);
            Assert.AreEqual(Calculator.Calc("1E0"), "1");
            Assert.AreEqual(Calculator.Calc("1E1"), "10");
            Assert.AreEqual(Calculator.Calc("1E2"), "100");
            Assert.AreEqual(Calculator.Calc("1E+2"), "100");
            Assert.AreEqual(Calculator.Calc("1E-1"), "0.1");
            Assert.AreEqual(Calculator.Calc("1E-2"), "0.01");
            Assert.AreEqual(Calculator.Calc("3E0!"), "6");
            Assert.AreEqual(Calculator.Calc("1.5e2+1"), "151");
            Assert.AreEqual(Calculator.Calc("1.5e-2+1"), "1.015");
            Assert.AreEqual(Calculator.Calc("1.5e-2+1.5e-2"), "0.03");
            
            Assert.AreEqual(Calculator.Calc("sin(0)"), "0");
            Assert.AreEqual(Calculator.Calc("sin(π)"), "0");
            Assert.AreEqual(Calculator.Calc("sin(π/2)"), "1");
            Assert.AreEqual(Calculator.Calc("sin(π/3)"), Math.Round((Math.Sqrt(3)/2), 14).ToString());
            Assert.AreEqual(Calculator.Calc("-5*sin(π/2)"), "-5");
            Assert.AreEqual(Calculator.Calc("-5*sin(+1E1-1E+1+π/2-1E1+1E+1)"), "-5");
            Assert.AreEqual(Calculator.Calc("-5*sin(+1E1-1E+1+π/2-1E1+1E+1)"), "-5");
            Assert.AreEqual(Calculator.Calc("cos(0)"), "1");
            Assert.AreEqual(Calculator.Calc("cos(π)"), "-1");
            Assert.AreEqual(Calculator.Calc("cos(π/2)"), "0");
            Assert.AreEqual(Calculator.Calc("cos(π/3)"), "0.5");
            Assert.AreEqual(Calculator.Calc("-5*cos(0)*-5*cos(π)"), "-25");
            Assert.AreEqual(Calculator.Calc("cos(π/3)*cos(π/3)+sin(π/3)*sin(π/3)"), "1");

        }
    }
}
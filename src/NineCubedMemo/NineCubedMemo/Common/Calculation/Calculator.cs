using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NineCubed.Common.Calculation
{
    public class Calculator
    {

        public static string Calc(string formula) {
            try {
                string orgFormula = formula; //最後のログ出力用。計算には関係ない。

                //両端に括弧をつけます
                formula = "(" + formula + ")";

                //括弧がなくなるのまで計算します
                int indexEnd = 0;
                while ((indexEnd = formula.IndexOf(')')) >= 0) {
                    //一番深いところにある括弧の式を取得します
                    int indexStart = formula.Substring(0, indexEnd).LastIndexOf('(');
                    var subFormula = formula.Substring(indexStart + 1, indexEnd - indexStart - 1);

                    //式を計算します
                    var result = CalcSub(subFormula);

                    //括弧を計算結果に置換して、式を再設定します
                    formula =
                        formula.Substring(0, indexStart) +
                        result +
                        formula.Substring(indexEnd + 1);
                }

                //結果を数値に変換します
                double calcResult = double.Parse(formula);

                #region //ログ出力 合計
                #if DEBUG
                    Console.WriteLine("■合計 {0} = {1}", orgFormula, calcResult);
                #endif
                #endregion

                if (double.IsInfinity(calcResult)) throw new Exception("Infinity");
                if (double.IsNaN(calcResult))      throw new Exception("NaN");

                return calcResult.ToString();
            } catch (Exception e) {
                #region //ログ出力 エラー
                #if DEBUG
                    Console.WriteLine(e.Message);
                #endif
                #endregion
                return null;
            }
        }

        //数式を解析して要素リストに変換します
        private static IList<CalcElement> GetElementList(string formula) {
            var elementList = new List<CalcElement>();
            CalcElement element = null;
           
            //式から1文字ずつ取得してループします
            foreach (var c in formula) {
                if (('9' >= c && c >= '0') || c == '.') {
                    //数値の場合

                    //現在の要素が演算子の場合は、現在の要素をリストに追加して、次の要素の準備をします
                    if (element != null && element is IOperator) {
                        elementList.Add(element);
                        element = null;
                    }

                    //要素に文字を追加します
                    if (element == null) element = new NumericElement();
                    ((NumericElement)element).ElementString += c;
                    
                } else {
                    //演算子の場合

                    //現在の要素がある場合は、現在の要素をリストに追加します
                    if (element != null) elementList.Add(element);

                    element = null; //次の要素
                    if (c == '*')  element = new MulElement         { Priority = 2 };
                    if (c == '/')  element = new DivElement         { Priority = 2 };
                    if (c == '^')  element = new PowElement         { Priority = 3 };
                    if (c == '!')  element = new FactorialElement   { Priority = 4 };
                    if (c == '√') element = new SqrtElement        { Priority = 5 };
                    if (c == 'Ｓ') element = new SinElement         { Priority = 5 };
                    if (c == 'Ｃ') element = new CosElement         { Priority = 5 };
                    if (c == 'E')  element = new ExponentialElement { Priority = 6 };
                    if (c == '+')  element = new AddElement         { Priority = 7 };
                    if (c == '-')  element = new SubElement         { Priority = 7 };
                    element.ElementString = c.ToString();
                }
            }

            //現在の要素をリストに追加する
            elementList.Add(element);

            return elementList;

        } //GetElementList()

        //リストから要素を取得します。不正なインデックスを指定した場合は null を返します
        private static CalcElement GetElement(IList<CalcElement> elementList, int index) {
            if (index < 0)                    return null;
            if (index >= elementList.Count()) return null;
            return elementList[index];
        }

        //括弧がない式を計算します
        private static double CalcSub(string formula) {
            #region //ログ出力 式
            #if DEBUG
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("式 = " + formula);
            #endif
            #endregion

            //置換処理
            formula = formula.Replace(" " , ""); //スペースを全て削除
            formula = formula.Replace("\r", ""); //改行を全て削除
            formula = formula.Replace("\n", ""); //改行を全て削除
            formula = formula.Replace("\t", ""); //タブを全て削除
            formula = formula.Replace("π", Math.PI.ToString()); //πを Math.PI に置換します

            // = がある場合は、左辺のみを有効にする
            int eqIndex = formula.IndexOf("=");
            if (eqIndex >= 0) formula = formula.Substring(0, eqIndex);

            //大文字で統一します
            formula = formula.ToUpper();

            //関数を1文字の記号に置換します（演算子と同様に1文字にした方が解析が簡単なため）
            formula = formula.Replace("SIN", "Ｓ");
            formula = formula.Replace("COS", "Ｃ");

            #region //ログ出力 式 -> 要素リスト
            #if DEBUG
                Console.WriteLine("式 -> 要素リスト");
            #endif
            #endregion

            //数式を解析して要素リストに変換します
            var elementList = GetElementList(formula);

            #region //ログ出力 : 要素リスト
            #if DEBUG
                for (int i = 0; i < elementList.Count(); i++) Console.WriteLine("[{0}] = {1}", i, elementList[i].ElementString);
                Console.WriteLine("----------------------------------------");
            #endif
            #endregion

            int index = -1;

            //最優先の演算子を取得して、演算子がなくなるまでループします
            while ((index = GetTopPriorityElementIndex(elementList)) >= 0) {

                //各要素を取得します
                var valueElement1   = GetElement(elementList, index - 1);
                var operatorElement = GetElement(elementList, index);
                var valueElement2   = GetElement(elementList, index + 1);

                //計算します
                CalcElement resultElement = null;
                if (operatorElement is IOperatorElement)      resultElement = ((IOperatorElement     )operatorElement).Calc(valueElement1, valueElement2);
                if (operatorElement is IOperatorLeftElement)  resultElement = ((IOperatorLeftElement )operatorElement).Calc(valueElement1);
                if (operatorElement is IOperatorRightElement) resultElement = ((IOperatorRightElement)operatorElement).Calc(valueElement2);

                //計算結果をリストに反映します
                elementList[index] = resultElement;

                //リストから要素を削除します
                if (operatorElement is IOperatorElement || operatorElement is IOperatorRightElement) elementList.RemoveAt(index + 1);
                if (operatorElement is IOperatorElement || operatorElement is IOperatorLeftElement ) elementList.RemoveAt(index - 1);
            }

            //余った数値の要素を全て加算します
            var result = elementList.Sum(_ => _.Value());

            #region //ログ出力
            #if DEBUG
                Console.WriteLine("□小計 {0} = {1}", formula, result);
            #endif
            #endregion

            return result;

        } //CalcSub()

        //式で使用されている最優先の要素のインデックスを返します
        private static int GetTopPriorityElementIndex(IList<CalcElement> elementList) {
            //優先順位の最大値を取得します
            int topPriority = elementList.Max(_ => _.Priority); 

            //優先順位が 1以下(数値)の場合は要素なしとする
            if (topPriority <= 1) return -1; 

            //最優先の要素を検索してインデックスを返します
            for (int i = 0; i < elementList.Count(); i++) {
                if (elementList[i].Priority == topPriority) return i;
            }
            return -1;
        }

    } //class

    //要素
    public class CalcElement {
        public int    Priority = 0;       //優先順位。大きい順に処理します
        public string ElementString = ""; //要素の文字列
        public int    Sign = 1;           //符号(1 or -1)。数値だけでなく、演算子の時も使用します。

        //要素の値を数値として返します
        public double Value() {
            return Math.Round(double.Parse(this.ElementString) * this.Sign, 14);
        }
    }

    //数値要素
    public class NumericElement : CalcElement {
        public NumericElement() {
            this.Priority = 1;
        }
        public NumericElement(double value) {
            this.Priority = 1;
            this.ElementString = value.ToString();
        }
    }

    //演算子要素用インターフェース
    public interface IOperator { } //演算子かどうかの判定用
    public interface IOperatorElement      : IOperator { CalcElement Calc(CalcElement element1, CalcElement element2); }
    public interface IOperatorLeftElement  : IOperator { CalcElement Calc(CalcElement element); } //左の数値を使う演算子
    public interface IOperatorRightElement : IOperator { CalcElement Calc(CalcElement element); } //右の数値を使う演算子

    //加算(符号の反映のみで計算はしない)
    public class AddElement : CalcElement, IOperatorRightElement {
        public AddElement() { this.Sign = 1; }
        public CalcElement Calc(CalcElement element) {
            element.Sign = element.Sign * this.Sign;
            return element;
        }
    }

    //減算(符号の反映のみで計算はしない)
    public class SubElement : CalcElement, IOperatorRightElement {
        public SubElement() { this.Sign = -1; }
        public CalcElement Calc(CalcElement element) {
            element.Sign = element.Sign * this.Sign;
            return element;
        }
    }

    //乗算
    public class MulElement : CalcElement, IOperatorElement {
        public CalcElement Calc(CalcElement element1, CalcElement element2) {
            checked {
                return new NumericElement(element1.Value() * element2.Value());
            }
        }
    }

    //除算
    public class DivElement : CalcElement, IOperatorElement {
        public CalcElement Calc(CalcElement element1, CalcElement element2) {
            checked {
                return new NumericElement(element1.Value() / element2.Value());
            }
        }
    }

    //べき乗
    public class PowElement  : CalcElement, IOperatorElement {
        public CalcElement Calc(CalcElement element1, CalcElement element2) {
            checked {
                return new NumericElement(Math.Pow(element1.Value(), element2.Value()));
            }
        }
    }

    //平方根
    public class SqrtElement : CalcElement, IOperatorRightElement {
        public CalcElement Calc(CalcElement element) {
            return new NumericElement(Math.Sqrt(element.Value()) * this.Sign);
        }
    }

    //sin()
    public class SinElement : CalcElement, IOperatorRightElement {
        public CalcElement Calc(CalcElement element) {
            return new NumericElement(Math.Sin(element.Value()) * this.Sign);
        }
    }

    //cos()
    public class CosElement : CalcElement, IOperatorRightElement {
        public CalcElement Calc(CalcElement element) {
            return new NumericElement(Math.Cos(element.Value()) * this.Sign);
        }
    }

    //指数表記対応
    public class ExponentialElement : CalcElement, IOperatorElement {
        public CalcElement Calc(CalcElement element1, CalcElement element2) {
            checked {
                return new NumericElement(element1.Value() * Math.Pow(10, element2.Value()));
            }
        }
    }

    //階乗
    public class FactorialElement : CalcElement, IOperatorLeftElement {
        public CalcElement Calc(CalcElement element) {
            if (element.Value() < 0) throw new ArgumentException();
            return new NumericElement(Factorial((int)element.Value()));
        }

        private int Factorial(int value) {
            checked { //オーバーフロー検知
                if (value == 0) return 1;
                return value * Factorial(value - 1);
            }
        }
    }

}

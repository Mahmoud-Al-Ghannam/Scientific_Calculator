using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Numerical_Systems
{
    public partial class CalculatorForm : Form
    {
        private string messageError = "Math Error";
        private bool shift = false;
        private bool isRad = true;
        private string ans = "0";
        private int numberOfDecimalPlaces = 9;
        public CalculatorForm()
        {
            InitializeComponent();
            lblShift.Text = "";
        }

        private long fact(int n)
        {
            long res = 1;
            for (int i = 1; i <= n; i++) res *= i;
            return res;
        }

        private long gcd(int a, int b)
        {
            if (b == 0) return a;
            return gcd(b, a % b);
        }

        private long lcm(int a, int b)
        {
            return (a * b) / gcd(a, b);
        }

        private long nCr(int n, int r)
        {
            if (n < r) return 0;
            if (n < 0 || r < 0) return 0;
            if (r == 0) return 1;
            return nCr(n - 1, r - 1) + nCr(n - 1, r);
        }

        private long nPr(int n, int r)
        {
            if (n < r) return 0;
            if (n < 0 || r < 0) return 0;
            if (r == 0) return 1;
            return n * nPr(n - 1, r - 1);
        }

        private double loga(double baseOfLog, double value)
        {
            return Math.Log(value) / Math.Log(baseOfLog);
        }

        private double cot(double x)
        {
            return 1 / Math.Tan(x);
        }
        private double acot(double x)
        {
            return (x < 0 ? -Math.PI / 2 : Math.PI / 2) - Math.Atan(x);
        }

        private double radToDeg(double rad)
        {
            return rad * (180 / Math.PI);
        }

        private double degToRad(double deg)
        {
            return deg * (Math.PI / 180);
        }
        private bool isDigit(char c)
        {
            return (c <= '9' && c >= '0');
        }

        private bool isOperation(char c)
        {
            return (c == '-' || c == '+' || c == '*' || c == '/');
        }
        private bool isNumber(string number)
        {
            try
            {
                Convert.ToDouble(number);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        private bool isValidPairOperations(char op1, char op2)
        {
            if (op1 == '*' && op2 == '/') return false;
            if (op1 == '/' && op2 == '*') return false;
            if (op1 == '+' && (op2 == '/' || op2 == '*')) return false;
            if (op1 == '-' && (op2 == '/' || op2 == '*')) return false;
            return true;
        }

        private bool splitSimpleEquationToNumbersAndOperations(string equation, ref List<double> numbers, ref List<char> operations)
        {
            if (equation == "") return false;
            numbers.Clear();
            operations.Clear();
            int n = equation.Length;
            string number = "";
            if (equation[0] == '-' || equation[0] == '+') number += equation[0];
            for (int i = (equation[0] == '-' || equation[0] == '+' ? 1 : 0); i < n; i++)
            {
                if (isOperation(equation[i]))
                {
                    if (i == n - 1) return false;
                    if (!isNumber(number)) return false;
                    if (isOperation(equation[i + 1]) && !isValidPairOperations(equation[i], equation[i + 1])) return false;

                    numbers.Add(Convert.ToDouble(number));
                    operations.Add(equation[i]);
                    number = "";
                    if (equation[i + 1] == '-' || equation[i + 1] == '+')
                    {
                        i++;
                        number += equation[i];
                    }
                }
                else number += equation[i];
            }
            if (!isNumber(number)) return false;
            numbers.Add(Convert.ToDouble(number));
            return true;
        }


        // function like: sin,cos,pow,ln,exp, ... etc
        // p1 is the first parameter 
        // p2 is the second parameter
        private string calculateFunction(string function, string p1, string p2 = "")
        {
            /* 
               Functions:
              "sin","cos","tan","cot",
              "asin","acos","atan","acot",
              "pow","sqrt","fact","abs",
              "ln","log","loga","exp",
              "max","min","floor","ceil",
              "gcd","lcm","nCr","nPr"
            */

            string[] functions1 = {"","sin","cos","tan","cot",
                                  "asin","acos","atan","acot",
                                  "sqrt","fact","abs","ln","log","exp",
                                  "floor","ceil"};

            string[] functions2 = { "pow", "loga", "max", "min",
                                    "gcd","lcm","nCr","nPr"};

            for (int i = 0; i < functions1.Length; i++)
            {
                if (function == functions1[i] && p2 != "")
                    return messageError;
            }

            for (int i = 0; i < functions2.Length; i++)
            {
                if (function == functions2[i] && p2 == "")
                    return messageError;
            }

            double q1 = Convert.ToDouble(p1);
            double q2 = 0;
            double ans = 0;
            if (p2 != "") q2 = Convert.ToDouble(p2);


            if (function == "") ans = q1;
            else if (function == "sin")
            {
                if (!isRad) q1 = degToRad(q1);
                ans = Math.Sin(q1);
            }
            else if (function == "cos")
            {
                if (!isRad) q1 = degToRad(q1);
                ans = Math.Cos(q1);
            }
            else if (function == "tan")
            {
                if (!isRad) q1 = degToRad(q1);
                ans = Math.Tan(q1);
            }
            else if (function == "cot")
            {
                if (!isRad) q1 = degToRad(q1);
                ans = cot(q1);
            }
            else if (function == "asin")
            {
                double res = Math.Asin(q1);
                if (!isRad) res = radToDeg(res);
                ans = res;
            }
            else if (function == "acos")
            {
                double res = Math.Acos(q1);
                if (!isRad) res = radToDeg(res);
                ans = res;
            }
            else if (function == "atan")
            {
                double res = Math.Atan(q1);
                if (!isRad) res = radToDeg(res);
                ans = res;
            }
            else if (function == "acot")
            {
                double res = acot(q1);
                if (!isRad) res = radToDeg(res);
                ans = res;
            }
            else if (function == "pow") ans = Math.Pow(q1, q2);
            else if (function == "sqrt") ans = Math.Sqrt(q1);
            else if (function == "fact") ans = fact((int)q1);
            else if (function == "abs") ans = Math.Abs(q1);
            else if (function == "ln") ans = Math.Log(q1);
            else if (function == "lon") ans = Math.Log10(q1);
            else if (function == "loga") ans = loga(q1, q2);
            else if (function == "exp") ans = Math.Exp(q1);
            else if (function == "max") ans = Math.Max(q1, q2);
            else if (function == "min") ans = Math.Min(q1, q2);
            else if (function == "floor") ans = Math.Floor(q1);
            else if (function == "ceil") ans = Math.Ceiling(q1);
            else if (function == "gcd") ans = gcd((int)q1, (int)q2);
            else if (function == "lcm") ans = lcm((int)q1, (int)q2);
            else if (function == "nCr") ans = nCr((int)q1, (int)q2);
            else if (function == "nPr") ans = nPr((int)q1, (int)q2);
            else return messageError;

            return Math.Round(ans, numberOfDecimalPlaces).ToString();
        }



        // equation with one operation like: 6+4 , 5*9 , ... etc
        private double calculateEquationWithOneOperation(double num1, double num2, char op)
        {
            switch (op)
            {
                case '+': return num1 + num2;
                case '*': return num1 * num2;
                case '/': return num1 / num2;
                case '-': return num1 - num2;
            }
            return 0;
        }

        // Simple Equation: 5*2+5/6
        // It's equation contains only the basic operations / - + *
        private string calculateSimpleEquation(string equation)
        {
            // MessageBox.Show("sim eq: " + equation);
            List<double> numbers = new List<double>();
            List<char> operations = new List<char>();
            bool flag = splitSimpleEquationToNumbersAndOperations(equation, ref numbers, ref operations);
            if (!flag) return messageError;

            string[] priority = { "*/", "-+" };

            for (int i = 0; i < priority.Length; i++)
            {
                for (int j = 0; j < operations.Count; j++)
                {
                    if (priority[i].Contains(operations[j]))
                    {
                        double res = calculateEquationWithOneOperation(numbers[j], numbers[j + 1], operations[j]);
                        operations.RemoveAt(j);
                        numbers.RemoveAt(j + 1);
                        numbers[j] = res;
                        j--;
                    }
                }
            }

            // MessageBox.Show("sol: " + numbers[0].ToString());
            return Math.Round(numbers[0], numberOfDecimalPlaces).ToString();
        }


        // Equation: sin(exp(3)*(3+2)/10) + (3-1)/2
        // It's equation contains everything
        private string calculateEquation(string equation)
        {
            equation = equation.Replace("PI", Math.PI.ToString());
            int n = equation.Length;
            string temp = "";
            Stack<int> indexOfOpenBraket = new Stack<int>();
            for (int i = 0; i < n; i++)
            {
                if (equation[i] == ')' && indexOfOpenBraket.Count == 0) return messageError;
                if (equation[i] == '(')
                {
                    indexOfOpenBraket.Push(temp.Length);
                    temp += '(';
                }
                else if (equation[i] == ')')
                {
                    // here match the brakets ()
                    temp += ')';
                    int idx1 = indexOfOpenBraket.Pop();
                    int idx2 = temp.Length - 1;
                    string[] simpleEquations = temp.Substring(idx1 + 1, idx2 - idx1 - 1).Split(',');
                    if (simpleEquations.Length > 2) return messageError;
                    string solutionOfFirstSimpleEquation = calculateSimpleEquation(simpleEquations[0]);
                    string solutionOfSeonndSimpleEquation = "";
                    if (simpleEquations.Length > 1) solutionOfSeonndSimpleEquation = calculateSimpleEquation(simpleEquations[1]);
                    if (solutionOfFirstSimpleEquation == messageError) return messageError;
                    if (solutionOfSeonndSimpleEquation == messageError) return messageError;
                    int idxOfStartFunction = idx1;
                    for (int j = idx1 - 1; j >= 0; j--)
                    {
                        if (temp[j] == ',' || temp[j] == '(' || isOperation(temp[j]) || isDigit(temp[j]))
                        {
                            break;
                        }
                        idxOfStartFunction = j;
                    }
                    string function = temp.Substring(idxOfStartFunction, idx1 - idxOfStartFunction);
                    string valueOfFunction = calculateFunction(function, solutionOfFirstSimpleEquation, solutionOfSeonndSimpleEquation);
                    if (valueOfFunction == messageError) return messageError;
                    temp = temp.Substring(0, idxOfStartFunction) + valueOfFunction;
                }
                else temp += equation[i];
                // MessageBox.Show(temp);
            }

            if (indexOfOpenBraket.Count > 0) return messageError;
            return calculateSimpleEquation(temp);
        }
        private void ClickOnButton(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string[] tags = btn.Tag.ToString().Split('#');
            string s = "";
            if (tags.Length == 1) shift = false;
            if (shift) s = tags[1];
            else s = tags[0];
            string output = tbOutputScreen.Text;
            int positionOfCaret = tbOutputScreen.SelectionStart;

            if (output == messageError)
            {
                output = "";
                positionOfCaret = 0;
            }

            if (s == "del")
            {
                if (output.Length > 0)
                {
                    int prevPos = positionOfCaret;
                    int pos = moveCaretToLeft(output, prevPos);
                    output = output.Substring(0, pos) + output.Substring(prevPos);
                    positionOfCaret -= (prevPos - pos);
                }
            }
            else if (s == "ac")
            {
                if (output == "") lblAns.Text = "";
                positionOfCaret = 0;
                output = "";
            }
            else if (s == "=")
            {
                output = calculateEquation(output);
                lblAns.Text = output;
                if (output != messageError) ans = output;
            }
            else if (s == "ans")
            {
                output = output.Insert(positionOfCaret, ans);
                positionOfCaret += ans.Length;
            }
            else if (s == "RD")
            {
                isRad = !isRad;
                btnRadDeg.Text = (isRad ? "DEG" : "RAD");
                lblRadDeg.Text = (isRad ? "RAD" : "DEG");
            }
            else
            {
                output = output.Insert(positionOfCaret, s);
                positionOfCaret += s.Length;
            }

            if(s != "=") tbOutputScreen.Text = output;
            tbOutputScreen.Select(positionOfCaret, 0);
            tbOutputScreen.Focus();

            shift = false;
            lblShift.Text = "";
        }

        private int moveCaretToRight(string equation, int idx)
        {
            if (idx == equation.Length) return idx;
            if (isDigit(equation[idx]) || isOperation(equation[idx])) return idx + 1;
            for (int i = idx; i < equation.Length; i++)
                if (equation[i] == '(') return i + 1;
            return equation.Length;
        }

        private int moveCaretToLeft(string equation, int idx)
        {
            if (idx == 0) return idx;
            if (equation[idx - 1] != '(') return idx - 1;
            for (int i = idx - 2; i >= 0; i--)
                if (equation[i] == '(' || equation[i] == ',' || isDigit(equation[i]) || isOperation(equation[i])) return i + 1;
            return 0;

        }
        private void btnArrow_Click(object sender, EventArgs e)
        {
            tbOutputScreen.Focus();
            tbOutputScreen.Select(tbOutputScreen.SelectionStart, 0);
            Button button = (Button)sender;
            int idx = 0;
            if (button.Tag == "1") // right arrow
            {
                idx = moveCaretToRight(tbOutputScreen.Text, tbOutputScreen.SelectionStart);
            }
            else // left arrow
            {
                idx = moveCaretToLeft(tbOutputScreen.Text, tbOutputScreen.SelectionStart);
            }
            tbOutputScreen.Select(idx, 0);
            tbOutputScreen.ScrollToCaret();
        }

        private void btnShift_Click(object sender, EventArgs e)
        {
            shift = !shift;
            lblShift.Text = (shift ? "S" : "");
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You are smart you don't need any help :)", "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}

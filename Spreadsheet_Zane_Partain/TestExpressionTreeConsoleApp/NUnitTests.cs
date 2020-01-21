/*
 * Zane Partain
 * 3/4/2019
 * 11488182
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using CptS321;

namespace TestExpressionTreeConsoleApp
{
    [TestFixture]
    class NUnitTests
    {
        ExpressionTree Etree;

        /// <summary>
        /// TestConvertInfixToPostfix:
        ///     This class will test whether or not an entered infix expression, will be 
        ///     correctly converted to a postfix expression.
        ///     note: { ... } = variable names
        ///           : ... : = double constants
        /// </summary>
        [Test]
        public void TestConvertInfixToPostfix()
        {
            //test converting infix to postfix notation
            ExpressionTree Etree = new ExpressionTree("a+b*(c^d-e)^(f+g*h)-i");
            Assert.AreEqual(Etree.PostfixExpression, "{a}{b}{c}{d}^{e}-{f}{g}{h}*+^*+{i}-");

            ExpressionTree Etree2 = new ExpressionTree("A+B");
            Assert.AreEqual(Etree2.PostfixExpression, "{A}{B}+");

            ExpressionTree Etree3 = new ExpressionTree("(Apple-Ben)/72");
            Assert.AreEqual(Etree3.PostfixExpression, "{Apple}{Ben}-:72:/");

            ExpressionTree Etree4 = new ExpressionTree("3+((5+93246)+apple-caramel*2)");
            Assert.AreEqual(Etree4.PostfixExpression, ":3::5::93246:+{apple}+{caramel}:2:*-+");
        }


        /// <summary>
        /// TestExpressionTreeEvaluation:
        ///     This class will test some basic arithmetic evaluation of the expression tree.
        ///     It covers: +,-,*,/,^,() (PEMDAS is correctly applied to the evaluation)
        /// </summary>
        [Test]
        public void TestExpressionTreeEvaluation()
        {
            /**test expression tree evaluation after created*/
            /********************************************/
            Etree = new ExpressionTree("(30-5)/5");
            Assert.AreEqual(5, Etree.Evaluate());
            

            /**test power evaluation*/
            /********************************************/
            Etree = new ExpressionTree("apple^2");
            Etree.SetVariable("apple", 10);
            Assert.AreEqual(100, Etree.Evaluate());

            Etree = new ExpressionTree("apple^2");
            Etree.SetVariable("apple", 10);
            Assert.AreEqual(100, Etree.Evaluate());
            

            /**test variable evalutation*/
            /********************************************/
            Etree = new ExpressionTree("E1*(A9+B7+C1)");
            Etree.SetVariable("A9", 1);
            Etree.SetVariable("B7", 1);
            Etree.SetVariable("C1", 1);
            Etree.SetVariable("E1", 32);
            Assert.AreEqual(96, Etree.Evaluate());
            

        }

        /// <summary>
        /// TestExpressionTreeExceptions:
        ///     This class will test divide by zero, and arithmetic overflow exceptions.
        /// </summary>
        [Test]
        public void TestExpressionTreeExceptions()
        {
            Etree = new ExpressionTree("2*3/0");
            double result;

            /**DivideByZeroException*/
            try
            {
                result = Etree.Evaluate();
            }
            catch (DivideByZeroException e)
            {
                DivideByZeroException div = new DivideByZeroException();
                Assert.AreEqual(e,div);
            }
            

            /**OverflowException*/
            string maxDouble = double.MaxValue.ToString();
            maxDouble += "+1";
            Etree = new ExpressionTree(maxDouble);
            try
            {
                result = Etree.Evaluate();
            }
            catch (OverflowException e)
            {
                OverflowException div = new OverflowException();
                Assert.AreEqual(e, div);
            }
            
        }

    }
}

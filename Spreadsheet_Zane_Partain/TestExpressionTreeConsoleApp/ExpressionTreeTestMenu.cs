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
using CptS321;

namespace TestExpressionTreeConsoleApp
{
    class ExpressionTreeTestMenu
    {
        private ExpressionTree expressionTree; //expression tree that will contain the entered equation
        private string input = string.Empty;   //stores user input 
        private string option;                 //stores user picked menu option

        public ExpressionTreeTestMenu()
        {
            
        }

        /// <summary>
        /// DeisplayMenu:
        ///     this will display the test menu in an infinite loop until "4" is entered
        ///     by the user.
        /// </summary>
        public void DisplayMenu()
        {
            string infixExpression = string.Empty;

            do
            {
                Console.WriteLine("(1) Enter a new expression ");
                Console.WriteLine("(2) Set a variable value ");
                Console.WriteLine("(3) Evaluate tree ");
                Console.WriteLine("(4) Quit" );
                Console.WriteLine();
                if(infixExpression != string.Empty)
                {
                    Console.WriteLine("Expression: " + infixExpression);
                }

                option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        Console.Write("Enter new expression: ");
                        infixExpression = Console.ReadLine();
                        expressionTree = new ExpressionTree(infixExpression);
                        break;

                    case "2":
                        Console.Write("Enter variable name: "); //get variable name
                        input = Console.ReadLine();

                        bool setValue = false;            //setValue flag 
                        do
                        {
                            Console.Write("Set value: "); //get vriable value
                            string value = Console.ReadLine();
                            double res = 0;

                            if (double.TryParse(value, out res))
                            {
                                expressionTree.SetVariable(input, res);
                                setValue = true;
                            }
                            Console.WriteLine();
    
                        } while (!setValue);
                        break;

                    case "3":
                        double result = expressionTree.Evaluate();
                        if(result.CompareTo(double.NaN) == 0)
                        {
                            Console.WriteLine("_err: can't evaluate, not all variables are declared.");
                            Console.WriteLine();
                        }
                        else if(result == double.PositiveInfinity)
                        {
                            Console.WriteLine("_err: can't evaluate, DivideByZeroException.");
                            Console.WriteLine();
                        }
                        else
                        {
                            Console.WriteLine("result = " + result);
                        }
                        Console.ReadLine();

                        break;

                    default:
                        Console.WriteLine("Goodbye");
                        break;
                }
                
                Console.Clear();

            } while (option != "4");
        }

    }
}

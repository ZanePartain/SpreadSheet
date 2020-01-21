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
using CptS321;  //include the SpreadsheetEngine project

namespace TestExpressionTreeConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ExpressionTreeTestMenu menu = new ExpressionTreeTestMenu();
            menu.DisplayMenu();
        }
    }
}

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

namespace CptS321
{
    /// <summary>
    /// ExpressionTree:
    ///     container for an expression tree. This class will also contain
    ///     methods to evaluate the expression tree.
    /// </summary>
    internal class ExpressionTree
    {
        ExpressionTreeBaseNode root; //root of expression tree
        ExpressionTreeNodeFactory nodeFactory = new ExpressionTreeNodeFactory(); //factory to construct all expression tree nodes
        Dictionary<string, double> variableDictionary = new Dictionary<string, double>(); //dictionary to hold (variable,value) pairs
        Dictionary<char, int> operatorsPrecidence = new Dictionary<char, int>() { { '+', 1 }, { '-', 1 }, { '*', 2 }, { '/', 2 }, { '^', 3 }};
       
        //field & property of postfixExpression
        string postfixExpression = string.Empty;
        public string PostfixExpression { get { return postfixExpression; } }                        // post fix expression
        public List<string> Variables { get { return new List<string>(variableDictionary.Keys); } }  // variable ref dictionairy

        //constructor
        public ExpressionTree(string expression)
        {
            postfixExpression = ConvertInfixToPostfix(expression);
            root = CreateExpressionTree(postfixExpression);
        }

        public void SetVariable(string variableName, double variableValue)
        {
            //set dictionary variable name and value
            variableDictionary[variableName] = variableValue;
        }

        

        private ExpressionTreeBaseNode CreateExpressionTree(string expression)
        {
            Stack<ExpressionTreeBaseNode> operandStack = new Stack<ExpressionTreeBaseNode>();
            int precidence = -1; 
            /**********************************************************************/
            // we will parse the expression string into the expression tree here! //
            /**********************************************************************/
            int expressionIndex = 0;
            for (; expressionIndex < expression.Length; expressionIndex++)
            {
                char currentChar = expression[expressionIndex];
                //not an operator
                if(!CheckOperatorPrecidence(currentChar, ref precidence))
                {
                    switch (currentChar)
                    {
                        //Constant (double)
                        case ':':
                            string constantValue = "";
                            expressionIndex++;

                            //traverse expression to grab the entire constant (double), and store in constantValue
                            while(expressionIndex < expression.Length && expression[expressionIndex] != ':')
                            {
                                constantValue += expression[expressionIndex];
                                expressionIndex++;
                            }
                            
                            //create new ConstantNode and pass in the corresponding Value prop.
                            ExpressionTreeBaseNode constantNode = nodeFactory.GetNode("C", constantValue);
                            
                            //convert type to constant node to print the value
                            /*
                            ConstantNode node = (ConstantNode)constantNode; 
                            Console.WriteLine("constantNode.Value = " + node.Value);
                            */

                            operandStack.Push(constantNode);  //push to operand stack

                            break;

                        //Variable (string)
                        case '{':
                            string variableName = "";
                            expressionIndex++;

                            //traverse expression to grab the entire constant (double), and store in constantValue
                            while (expressionIndex < expression.Length && expression[expressionIndex] != '}')
                            {
                                variableName += expression[expressionIndex];
                                expressionIndex++;
                            }

                            //create new VariableNode and pass in the corresponding Name prop.
                            ExpressionTreeBaseNode variableNode = nodeFactory.GetNode("V", variableName);

                            //convert type to constant node to set the default value
                            VariableNode tempNode = (VariableNode)variableNode;
                            SetVariable(tempNode.Name, 0.0);

                            operandStack.Push(variableNode); //push to operand stack

                            break;
                    }
                }
                //currentChar is an operator
                else
                {
                    //create new OperatorNode and pass in the corresponding operator prop.
                    ExpressionTreeBaseNode operatorNode = nodeFactory.GetNode("O", currentChar.ToString());

                    //convert type to constant node to print the value
                    OperatorNode tempNode = (OperatorNode)operatorNode;
                   
                    //pop two operands off of the stack
                    ExpressionTreeBaseNode rightNode = operandStack.Pop();
                    ExpressionTreeBaseNode leftNode = operandStack.Pop();

                    //set operatornode left and right nodes.
                    tempNode.Left = leftNode;
                    tempNode.Right = rightNode;

                    operandStack.Push(tempNode); //push operator node onto stack

                }

            }

            ExpressionTreeBaseNode expressionTreeRoot = operandStack.Peek(); //expression tree root is top of stack
            operandStack.Pop(); //pop top of stack (root)

            return expressionTreeRoot;
        }

        /// <summary>
        /// ConvertInfixToPostfix:
        ///     Takes an infix expression, and converts it into post fix. Then returns the postfix expression.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private string ConvertInfixToPostfix(string expression)
        {
            StringBuilder postfixStringBuilder = new StringBuilder();
            Stack<char> operatorStack = new Stack<char>();
            int expressionIndex = 0;

            //parse infix expression string and store postfix representation into postfixStringBuilder
            for (; expressionIndex < expression.Length; expressionIndex++)
            {
                char currentChar = expression[expressionIndex];
                int precidence = 0;

                //CHECK IF PARENTHESIS
                if (currentChar == '(')
                {
                    operatorStack.Push(currentChar);
                }
                else if( currentChar == ')')
                {
                    //pop all operators in operatorStack until the top operator is '('
                    while(operatorStack.Count > 0)
                    {
                        postfixStringBuilder.Append(operatorStack.Pop());

                        if(operatorStack.Peek() == '(')
                        {
                            break;
                        }
                    }
                    //pop off the starting '('
                    if (operatorStack.Count > 0)
                    {
                        operatorStack.Pop();
                    }
                  
                }
                //CHECK IF OPERATOR
                else if (CheckOperatorPrecidence(currentChar, ref precidence))
                {
                    int tempPrecidence = 0;
                    
                    //pop the stack and append to postfixStringBuilder
                    while(operatorStack.Count > 0)
                    {
                        CheckOperatorPrecidence(operatorStack.Peek(), ref tempPrecidence);
                        if (precidence <= tempPrecidence)
                        {
                            postfixStringBuilder.Append(operatorStack.Pop());
                        }
                        else break;
                    }
                    //stack is empty
                    operatorStack.Push(currentChar); 
                }
                //CHECK IF VARIABLE OR CONSTANT
                else
                {
                    if(currentChar != ' ')
                    {
                        double result;

                        //currentChar is a constant or variable
                        if (double.TryParse(currentChar.ToString(), out result)) 
                        {
                            //parse the entire double number
                            HandleConstantNumParsing(postfixStringBuilder, expression, ref expressionIndex);
                        }
                        else
                        {
                            //parse the entire variable name
                            HandleVariableNameParsing(postfixStringBuilder, expression, ref expressionIndex);
                        }

                    }
                }
            }

            //append the rest of the operators in the operator stack to postfixStringBuilder
            while(operatorStack.Count > 0)
            {
                postfixStringBuilder.Append(operatorStack.Pop());
            }

            return postfixStringBuilder.ToString();
        }

        /// <summary>
        /// CheckOperatorPrecidence:
        ///     checks if op (param) is infact a supported operator. If op (param) is supported then
        ///     it will store the precidence ranking of that operator in the precidence (param), and 
        ///     return true.
        /// </summary>
        /// <param name="op"></param>
        /// <param name="precidence"></param>
        /// <returns>true or false</returns>
        private bool CheckOperatorPrecidence(char op, ref int precidence)
        {
            if (operatorsPrecidence.ContainsKey(op))
            {
                //set precedence = to operator precidence
                precidence = operatorsPrecidence[op]; 
                return true;
            }
            precidence = -1;
            return false;
        }

        private void HandleVariableNameParsing(StringBuilder postfixStringBuilder,string expression, ref int expressionIndex)
        {
            int precidence = 0;
            char currentChar = expression[expressionIndex];

            //append {variableName} to postfixStringBuilder
            postfixStringBuilder.Append('{');
            postfixStringBuilder.Append(currentChar);

            //grab the entire variable name before appending ending '}'
            while (expressionIndex + 1 < expression.Length
                    && CheckOperatorPrecidence(expression[expressionIndex + 1], ref precidence) == false)
            {
                if (expression[expressionIndex + 1] == '(' || expression[expressionIndex + 1] == ')')
                {
                    break; //if its a perenthesis break the loop
                }
                //append current character of variable name
                postfixStringBuilder.Append(expression[expressionIndex + 1]);
                expressionIndex++;
            }
            //append ending '}'
            postfixStringBuilder.Append('}');
        }

        private void HandleConstantNumParsing(StringBuilder postfixStringBuilder, string expression, ref int expressionIndex)
        {
            double result = 0;
            char currentChar = expression[expressionIndex];

            //append current digit to postfixStringBuilder
            postfixStringBuilder.Append(':');
            postfixStringBuilder.Append(currentChar);

            //grab the entire double number before appending ending ':'
            while (expressionIndex + 1 < expression.Length
                    && double.TryParse(expression[expressionIndex + 1].ToString(), out result))
            {
                if (expression[expressionIndex + 1] == '(' || expression[expressionIndex + 1] == ')')
                {
                    break; 
                }

                //append current character of variable name
                postfixStringBuilder.Append(expression[expressionIndex + 1]);
                expressionIndex++;
            }
            //append ending ':'
            postfixStringBuilder.Append(':');

        }

        /// <summary>
        /// Evaluate:
        ///     This function is used to evaluate the expression tree. This logic is given to 
        ///     us in our Etree.c lecture notes. It takes in an ExpressionTreeBaseNode param.
        ///     then tries to cast it as a ConstantNode, VariableNode, or OperatorNode. Based
        ///     on what the node type is (see ExpressionTreeNodeFactory) it will return a value
        ///     to be evaluated. (all root nodes should be operators)
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private double Evaluate(ExpressionTreeBaseNode node)
        {
           
            ConstantNode constantNode = node as ConstantNode;
            if (constantNode != null)
            {
                return constantNode.Value;
            }

            // as a variable
            VariableNode variableNode = node as VariableNode;
            if (variableNode != null)
            {
                return variableDictionary[variableNode.Name];
            }

            // it is an operator node if we came here
            OperatorNode operatorNode = node as OperatorNode;
            if (operatorNode != null)
            {
                // but which one?
                switch (operatorNode.Operator)
                {
                    case '+':
                        return Evaluate(operatorNode.Left) + Evaluate(operatorNode.Right);
                    case '-':
                        return Evaluate(operatorNode.Left) - Evaluate(operatorNode.Right);
                    case '*':
                        return Evaluate(operatorNode.Left) * Evaluate(operatorNode.Right);
                    case '/':

                        try
                        {
                            return Evaluate(operatorNode.Left) / Evaluate(operatorNode.Right);
                        }
                        catch (DivideByZeroException e)
                        {
                            Console.Write(e.Message);
                            Console.ReadLine();
                        }
                        break;
                    case '^':
                        return Math.Pow(Evaluate(operatorNode.Left), Evaluate(operatorNode.Right));
                    default: // if it is not any of the operators that we support, throw an exception:
                        throw new NotSupportedException(
                            "Operator " + operatorNode.Operator.ToString() + " not supported.");
                }
            }

            throw new NotSupportedException();
        }

        //Call private evaluate
        public double Evaluate()
        {
            if (ConfirmExpressionVariables())
            {
                return Evaluate(root);   
            }
            return double.NaN;
        }

        /// <summary>
        /// ConfirmExpressionVariables:
        ///     Confirms if all variables in the expression have a suported value.
        ///     Returns true if all variables have a value, false otherwise.
        /// </summary>
        /// <returns></returns>
        private bool ConfirmExpressionVariables()
        {
            if(variableDictionary.Count > 0)
            {
                foreach(KeyValuePair<string,double> variable in variableDictionary)
                {
                    if(variable.Value.CompareTo(double.NaN) == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

    }
}

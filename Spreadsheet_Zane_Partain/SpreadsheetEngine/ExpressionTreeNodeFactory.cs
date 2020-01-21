using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    class ExpressionTreeNodeFactory
    {
        public ExpressionTreeNodeFactory()
        {
        }

        /// <summary>
        /// GetNode:
        ///   returns a ExpressionTreeNode such based off of client specified parameters. Only supports
        ///   Constant, Variable, and Operator Nodes. All of which inherit from ExpressionTreeBaseNode.
        /// </summary>
        /// <param name="nodeType">
        ///   nodeType string will specify which kind of ExpressionTree Node the client wishes to instantiate
        /// </param>
        /// <param name="operation">
        ///   operation char specifies the operation the client wishes to pass into OperationNode constructor.
        ///   (Optional) If passed in with "C" or "V" it will not be used.
        /// </param>
        /// <returns> ExpressionTreeBaseNode </returns>
        public ExpressionTreeBaseNode GetNode(string nodeType, string operation = "\0")
        {
            ExpressionTreeBaseNode node;
            switch (nodeType)
            {
                
                case "C":  /*constant*/
                    ConstantNode constantNode = new ConstantNode();
                    double result;
                  

                    if (double.TryParse(operation, out result))
                    {
                        constantNode.Value = result;
                    }
                
                    node = constantNode;

                    return node;

                case "V":  /*variable*/
                    VariableNode variableNode = new VariableNode();

                    variableNode.Name = operation;
                    node = variableNode;

                    return node;

                case "O":  /*operator*/
                    char op = operation[0];
                    node = new OperatorNode(op);
                    return node;

                default: return null;
            }
        }

    }
}

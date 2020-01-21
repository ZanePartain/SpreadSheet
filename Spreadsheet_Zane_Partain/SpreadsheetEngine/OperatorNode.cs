using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    /// <summary>
    /// OperatorNode:
    ///     this class will contain the the operator node(s)
    ///     in an expression. The Left property will contain the expression to the 
    ///     left of the operator. The Right property will do the same, but for 
    ///     the expression to the right of the operator.
    /// </summary> 
    internal class OperatorNode : ExpressionTreeBaseNode
    {
        public OperatorNode(char c)
        {
            Operator = c;
            Left = Right = null;
        }

        public char Operator { get; set; }

        public ExpressionTreeBaseNode Left { get; set; }
        public ExpressionTreeBaseNode Right { get; set; }
    }
}

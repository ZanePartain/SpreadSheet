using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    /// <summary>
    /// ConstantNode:
    ///     this class contains the Value of a constant (double) that 
    ///     will be used in an expression tree.
    /// </summary>
    class ConstantNode : ExpressionTreeBaseNode
    {
        public double Value { get; set; }
    }

}

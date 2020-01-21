using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    /// <summary>
    /// VariableNode:
    ///     this class contains the Name of the variable (cell) that 
    ///     it will be referencing.
    /// </summary>
    class VariableNode : ExpressionTreeBaseNode
    {
        public string Name { get; set; }
    }


}

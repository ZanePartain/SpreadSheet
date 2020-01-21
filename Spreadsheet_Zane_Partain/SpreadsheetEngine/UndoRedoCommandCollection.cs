/*
 * Zane Partain
 *     11488182
 *      2/25/19
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CptS321;

namespace SpreadsheetEngine
{
    /// <summary>
    /// This class encapsulates the collection of Undo/Redo commands 
    /// that each cell will be able to perform. This class is going to be used for my
    /// simple Command design pattern when handling all Undo/Redo commands.
    /// </summary>
    public class UndoRedoCommandCollection
    {
        private string title;

        /// <summary>
        /// Each one of the stacks below should be popped and pushed in parallel.
        /// </summary>
        protected Stack<Cell> prevCellStack = new Stack<Cell>();         //previous cell stack
        protected Stack<Cell> curCellStack = new Stack<Cell>();          //current cell stack
        protected Stack<string> prevCellTextStack = new Stack<string>(); //previous text stack
        protected Stack<string> curCellTextStack = new Stack<string>();  //current text stack
        protected Stack<uint> prevBGColorStack = new Stack<uint>();      //previous BGColor stack
        protected Stack<uint> curBGColorStack = new Stack<uint>();       //current BGColor stack

        /// <summary>
        /// Both Undo and Redo classes will need to be overidden by any child class
        /// </summary>
        public virtual void Undo() { } 
        public virtual void Redo() { }


        public string Title
        {
            get { return title; }
            set
            {
                if(value == title) { return; }

                title = value;
            }
        }
    }

}

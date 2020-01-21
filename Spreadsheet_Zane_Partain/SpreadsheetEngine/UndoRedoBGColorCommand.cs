using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CptS321;

namespace SpreadsheetEngine
{
    /// <summary>
    /// Class for handling the Undo, and Redo of a BGColor change in the spreadsheet
    /// </summary>
    public class UndoRedoBGColorCommand : UndoRedoCommandCollection
    {
        //constructor
        public UndoRedoBGColorCommand(string title) { this.Title = title; }

        public void SetCollections(Stack<uint> prevBGColors, Stack<uint> curBGColors, Stack<Cell> prevCells, Stack<Cell> curCells)
        {
            this.prevBGColorStack = prevBGColors;  //set cmd collection previous BGColor stack
            this.curBGColorStack = curBGColors;    //set cmd collection current BGColor stack
            this.prevCellStack = prevCells;        //set cmd collection previous Cell stack
            this.curCellStack = curCells;          //set cmd collection current Cell stack
        }

        public override void Undo()
        {
            Stack<Cell> prevCellsCopy = new Stack<Cell>(prevCellStack);
            Stack<uint> prevBGColorsCopy = new Stack<uint>(prevBGColorStack);
            int cellCount = prevCellStack.Count;

            while(cellCount != 0)
            {
                prevCellStack.Pop().BGColor = prevBGColorStack.Pop();  //perform undo; set prev cell BGColor to its previous 
                cellCount--;
            }

            prevCellStack = prevCellsCopy;        //persist orig. state of prevCellStack 
            prevBGColorStack = prevBGColorsCopy;  //persist orig. state of prevBGColorStack
        }

        public override void Redo()
        {
            Stack<Cell> curCellsCopy = new Stack<Cell>(curCellStack);
            Stack<uint> curBGColorsCopy = new Stack<uint>(curBGColorStack);
            int cellCount = curCellStack.Count;

            while(cellCount != 0)
            {
                curCellStack.Pop().BGColor = curBGColorStack.Pop();  //perform undo; set prev cell BGColor to its previous 
                cellCount--;
            }

            curCellStack = curCellsCopy;        //persist orig. state of curCellStack 
            curBGColorStack = curBGColorsCopy;   //persist orig. state of curBGColorStack
        }
    }
}

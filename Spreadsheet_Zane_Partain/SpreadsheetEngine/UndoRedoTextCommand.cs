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
    /// Class for handling the Undo, and Redo of a Text change in the spreadsheet
    /// </summary>
    public class UndoRedoTextCommand : UndoRedoCommandCollection
    {
        public UndoRedoTextCommand(string title) { this.Title = title; }

        public void SetCollections(Stack<string> oldText, Stack<string> newText, Stack<Cell> oldCells, Stack<Cell> newCells)
        {
            this.prevCellTextStack = oldText;    //set cmd collection previous BGColor stack
            this.curCellTextStack = newText;     //set cmd collection current BGColor stack
            this.prevCellStack = oldCells;       //set cmd collection previous Cell stack
            this.curCellStack = newCells;        //set cmd collection current Cell stack
        }

        public override void Undo()
        {
            Stack<Cell> oldCellsCopy = new Stack<Cell>(prevCellStack);
            Stack<string> oldTextsCopy = new Stack<string>(prevCellTextStack);
            int cellCount = oldCellsCopy.Count;

            while (cellCount != 0)
            {
                prevCellStack.Pop().Text = oldTextsCopy.Pop();  //perform undo; set prev cell BGColor to its previous 
                cellCount--;
            }

            prevCellStack = oldCellsCopy;        //persist orig. state of prevCellStack 
            prevCellTextStack = oldTextsCopy;  //persist orig. state of prevBGColorStack
        }


        public override void Redo()
        {
            Stack<Cell> newCellsCopy = new Stack<Cell>(curCellStack);
            Stack<string> newTextsCopy = new Stack<string>(prevCellTextStack);
            int cellCount = newCellsCopy.Count;

            while (cellCount != 0)
            {
                prevCellStack.Pop().Text = newTextsCopy.Pop();  //perform undo; set prev cell BGColor to its previous 
                cellCount--;
            }

            curCellStack = newCellsCopy;        //persist orig. state of prevCellStack 
            curCellTextStack = newTextsCopy;   //persist orig. state of prevBGColorStack
        }

    }
}

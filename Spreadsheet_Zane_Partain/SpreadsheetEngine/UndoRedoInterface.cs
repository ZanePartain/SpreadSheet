using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine
{
    /// <summary>
    /// This class will be the interface where the client can actually use public methods to 
    /// interact with the UndoRedoCommandCollection Class.
    /// </summary>
    public class UndoRedoInterface
    {
        public UndoRedoInterface() { } //constructor

        private Stack<UndoRedoCommandCollection> redoStack = new Stack<UndoRedoCommandCollection>();  //contains all redo's of the entire program
        private Stack<UndoRedoCommandCollection> undoStack = new Stack<UndoRedoCommandCollection>();  //contains all undo's of the entire program

        public int UndoCount { get { return undoStack.Count; } }  //count of commands in undo stack
        public int RedoCount { get { return redoStack.Count; } }  //count of commands in redo stack
  
        public string UndoTitle { get { return undoStack.Peek().Title; } }  //title of the top cmd on undo stack
        public string RedoTitle { get { return redoStack.Peek().Title; } }  //title of the top cmd on redo stack


        public void PushUndo(UndoRedoCommandCollection command)
        {
            undoStack.Push(command);      //push command onto undo stack
        }


        public void DoUndo()
        {
            UndoRedoCommandCollection undoCommad = undoStack.Pop();

            redoStack.Push(undoCommad);   //push command onto redo stack
            undoCommad.Undo();            //perform undoCommand's overidden Undo command
        }
         

        public void DoRedo()
        {
            UndoRedoCommandCollection redoCommad = redoStack.Pop();

            undoStack.Push(redoCommad);   //push command onto undo stack
            redoCommad.Redo();            //perform redoCommand's overidden Undo command
        }


    }
}

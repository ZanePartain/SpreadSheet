/*
 * Zane Partain
 *     11488182
 *      2/25/19
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetEngine;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace CptS321
{
    public class Spreadsheet
    {
        Cell[,] sheet;  // 2D array to to hold entire spreadsheet (of any size)
        UndoRedoInterface undoRedo = new UndoRedoInterface(); //undo_redo interface instance
        Dictionary<SubCell, HashSet<SubCell>> dependencies = new Dictionary<SubCell, HashSet<SubCell>>(); //holds all the dependencies for each cell
        int rowCount, colCount;

        public int RowCount
        {
            get { return rowCount; }
            set { rowCount = value;}
        }
        public int ColCount
        {
            get { return colCount; }
            set { colCount = value; }
        }
        public int UndoCount
        {
            get { return undoRedo.UndoCount; }
        }
        public int RedoCount
        {
            get { return undoRedo.RedoCount; }
        }

        /***************************SubCell class*****************************/
        /// <summary>
        /// SubCell is a class that inherits from abstract class Cell.
        ///  This class is mainly used for Spreadsheet to construct a new Cell object.
        /// </summary>
        public class SubCell : Cell
        {
            public SubCell(int row, int col) : base(row, col) { }
           
        }


        public event PropertyChangedEventHandler CellPropertyChanged;

        //constructor
        /// <summary>
        /// Spreadsheet constructor will take in the specified number of rows, and columns.
        ///    Then create a matrix of Cells with respect to the number of cells and columns.
        ///    It will then subscribe to every Cell in the entire matrix. This way it can 
        ///    keep track of INotifyPropertyChanged for each cell.
        /// </summary>
        /// <param name="numRows"></param>
        /// <param name="numCols"></param>
        public Spreadsheet(int numRows, int numCols)
        {
            //init sheet to hold Cell's of param spec size
            sheet = new Cell[numRows, numCols];
            this.rowCount = numRows;
            this.colCount = numCols;

            //set each Cell's RowIndex and ColumnIndex in sheet 
            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {
                    //instantiate each cell in spreadsheet
                    sheet[row, col] = new SubCell(row, col);   
                    sheet[row, col].SetName(ConvertLocationToName(row+1, col));                              //set each cells name Representation (e.g. "A1")
                    sheet[row, col].PropertyChanged += new PropertyChangedEventHandler(CellChangeHandler); //subscribe to all cells INotifyPropertyChanged
                }
            }
        }


        //return the cellnin sheet if it exists, otherwise return null
        public SubCell GetCell(int row, int col)
        {
            if((row >= 0 && row < rowCount) 
                &&(col >= 0 && col < colCount))
            {
                return (SubCell)sheet[row,col];
            }
            return null;
        }


        //Notify all subscribers that the property has changed (e.g. form1 --> dataGridview1)
        private void NotifyPropertyChanged(object sender, string propertyName)
        {
           CellPropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// CellChangeHandler will carry out tasks that need to be done upon changing the text of a cell.
        ///    If the cell's text starts with "=" this event handler will then carry out the necessary computation
        ///    and return the result/replace the text of the cell with the returned result.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CellChangeHandler(object sender, PropertyChangedEventArgs e)
        {
            SubCell target = sender as SubCell;
            Console.WriteLine("Property =  " + target.Text);   //for debug purposes
    
            /*EVALTUATE EXPRESSION*/
            if (e.PropertyName == "Text")
            {
                ClearPreviousDependencies(target);     //clear all previous dependencies of target cell 
                HandleCellExpressionChange(target);    //determine what further action should be taken
                NotifyPropertyChanged(target, "Value"); //Notify subscribers

            }
            else if(e.PropertyName == "Value")
            {
                //update all dependent cells
                UpdateDependentCells(target);

                NotifyPropertyChanged(target, "Value");  //Notify subscribers

            }
            else if(e.PropertyName == "BGColor")
            {
                NotifyPropertyChanged(target, "BGColor"); //Notify subscribers
            }

        }


        private bool CheckIfExpression(string expression)
        {
            HashSet<char> operators = new HashSet<char>() { '+', '-', '/', '*', '^' };
            
            for(int i = 0; i < expression.Length; i++)
            {
                if (operators.Contains(expression[i]))
                {
                    return true;
                }
            }
           
            return false;
        }


        /// <summary>
        /// Clears the target cell of any previous dependencies that it might have.
        ///   This will allow the expression to be changed, and then reapply its new dependencies.
        /// </summary>
        /// <param name="targetCell"></param>
        private void ClearPreviousDependencies(SubCell targetCell)
        {
            //targetCell has dependencies so clear them
            if (dependencies.ContainsKey(targetCell))
            {
                dependencies[targetCell].Clear();
            }

            dependencies.Remove(targetCell);
        }


        /// <summary>
        ///  This method will decide what course of action will need to be taken to
        ///  re-evaluate the odified expression.
        /// </summary>
        /// <param name="targetCell"></param>
        private void HandleCellExpressionChange(SubCell targetCell)
        {
            string text = targetCell.Text;
            /*EVALTUATE EXPRESSION*/
            if (text.Length == 0)
            {
                targetCell.SetValue("");
            }
            else if(text[0] != '=')
            {
                //just set text as normal
                targetCell.SetValue(targetCell.Text);
            }
            else  //text.StartsWith("=") therefore, has cell ref
            {
                
                bool result;
                try
                {
                    EvaluateExpression(targetCell);       //try to evaluate expression
                }
                catch
                {
                    targetCell.SetValue("#REF!");         //set the value to empty string
                }

                result = VerifyNoCircleRef(targetCell);
                if (result == true)
                {
                    targetCell.SetValue("#CIRC REF!");         //set the value to empty string
                }

            }
        }


        /*****BUG: ********************/
        /* when 1 cell references 2 cells and then evaluates them to its own unique end value.
         * It's value can't be referenced by another separate cell.
         */
        /// <summary>
        /// update all cells values that are dependent on other cells
        /// </summary>
        /// <param name="cell"></param>
        private void UpdateDependentCells(SubCell cell)
        { 
            //loop over all cells that have dependencies,
            //check if any cells have a current dependency in cellCoord
            foreach (SubCell key in dependencies.Keys)
            {
                if (dependencies[key].Contains(cell))
                {
                    HandleCellExpressionChange(key);
                }
            }
        }


        /// <summary>
        /// evaluate the the expression of the target cell. Find and set the value of each variable in the expression,
        /// then set the target cells dependencies. Finally, set the value of the target cell to the result of the 
        /// evaluated expression tree.
        /// </summary>
        /// <param name="targetCell"></param>
        private void EvaluateExpression(SubCell targetCell)
        {
            string targetExpression = targetCell.Text.Substring(1); //grab entire expression except "="
            ExpressionTree expressionTree = new ExpressionTree(targetExpression); //create expression tree

            try
            {
                if (expressionTree.Variables.Count == 1)
                {
                    string variable = expressionTree.Variables.First();
                    int[] index = ConvertNameToLocation(variable);
                    SubCell dependentCell = GetCell(index[0], index[1]); //get cell at name location


                    if (dependencies.ContainsKey(targetCell) == false)
                    {
                        dependencies.Add(targetCell, new HashSet<SubCell>()); //create dependency Cell entry
                    }
                    dependencies[targetCell].Add(dependentCell);  //add dependent cell hash value at key entry

                     if (dependentCell.Value == dependentCell.Text)
                    {
                        targetCell.SetValue(dependentCell.Text);
                    }
                    if (VerifyNoCircleRef(targetCell))
                    {
                        return;
                    }
                }
                else
                {
                    foreach (string variable in expressionTree.Variables)
                    {
                        int[] index = ConvertNameToLocation(variable);
                        SubCell dependentCell = GetCell(index[0], index[1]); //get cell at name location


                        if(dependencies.ContainsKey(targetCell) == false)
                        {
                            dependencies.Add(targetCell, new HashSet<SubCell>()); //create dependency Cell entry
                        }
                        dependencies[targetCell].Add(dependentCell);  //add dependent cell hash value at key entry
                
                        //if the dependent cell value is a constant (double)
                        double result;
                        if(double.TryParse(dependentCell.Value, out result))
                        {
                            expressionTree.SetVariable(variable, result);
                        }
                    }
                    //evaluate the expression tree, and set target cell value
                    targetCell.SetValue(expressionTree.Evaluate().ToString());
                }
            }
            catch
            {
                throw; //throw the exception to HandleCellExpressionChange()
            }
        }


        /// <summary>
        /// converts the string name param. (e.g. "A2") into an actual integer coordinate of that 
        /// named cell's location in the spreadsheet
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private int[] ConvertNameToLocation(string name)
        {
            string columnName = new string(name.Where(char.IsLetter).ToArray());  //column name
            string rowNum = new string(name.Where(char.IsDigit).ToArray());    //column number
            int[] index = { 0, 0 };

            for(int i = 0; i < columnName.Length; i++)
            {
                index[1] += columnName[i] % 65; //column number
            }

            int index1;
            if(int.TryParse(rowNum, out index1))
            {
                index[0] = index1 - 1;  //row number
            }

            return index;
        }


        /// <summary>
        /// converts the int row/column param. (e.g. 0,1) into an actual string representation of that 
        /// cells location in the spreadsheet
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string ConvertLocationToName(int row, int column)
        {
            string name = "";

            if((row < 50 && row >= 0) && (column >= 0 && column < 26))
            {
                name += Convert.ToChar(65 + column).ToString();  //convert column num to letter representation
                name += row.ToString();                          //append row to name
            }  

            return name;
        }


        /// <summary>
        /// Adds and undo command onto the Undo Stack
        /// </summary>
        /// <param name="command"></param>
        public void AddUndo(UndoRedoCommandCollection command)
        {
            undoRedo.PushUndo(command);
        }

        // Pop an item off of the Undo stack, and push it on Redo stack
        public void Undo()
        {
            undoRedo.DoUndo(); 
        }

        // Pop an item off of the Redo stack, and push it on Undo stack
        public void Redo()
        {
            undoRedo.DoRedo();
        }

        //Return the title of the top Undo command
        public string GetUndoTitle()
        {
            return undoRedo.UndoTitle;
        }

        //Return the title of the top Redo command
        public string GetRedoTitle()
        {
            return undoRedo.RedoTitle;
        }


        /// <summary>
        /// save a spreadsheet by converting a Stream contents into and XML Document
        /// </summary>
        /// <param name="saveFile"></param>
        public void SaveSpreadsheet(Stream saveFile)
        {
            //create xml writer to write to saveFile destination
            XmlWriter xmlWriter = XmlWriter.Create(saveFile);

            xmlWriter.WriteStartDocument();              //write XML decloration
            xmlWriter.WriteStartElement("spreadsheet");  //write XML start tag <Spreadsheet>

            //loop over every cell in the entire 2D sheet[50,26] array of Cells 
            foreach (Cell cell in sheet)
            {
                //if the text or color is not default,
                //then write cell specs to xmlWriter
                if (cell.Text != null || cell.BGColor != 0xFFFFFFFF)
                {
                    xmlWriter.WriteStartElement("cell");       //<Cell name="cell.Name">
                    xmlWriter.WriteAttributeString("name","SpreadsheetEngine",cell.Name);

                    xmlWriter.WriteElementString("bgcolor", cell.BGColor.ToString()); //<bgcolor> some color </bgcolor>
                    xmlWriter.WriteElementString("text", cell.Text);                  //<text> some text </text>

                    xmlWriter.WriteEndElement(); //end the <cell> element
                }
            }

            //finish the xml file tag, and close it
            xmlWriter.WriteEndElement();
            xmlWriter.Close();
        }

        public void LoadSpreadsheet(Stream loadStream)
        {
            //create document for XML
            XDocument loadFileXML = XDocument.Load(loadStream);

            //for every cell tag in the XML Document
            foreach(XElement cellTag in loadFileXML.Root.Elements("cell"))
            {
                string name = cellTag.FirstAttribute.Value.ToString();  //name of current cell

                int[] index = ConvertNameToLocation(name);  //map name to index

                //set cell at index location: text, BGColor
                Cell newCell = sheet[index[0], index[1]];
                newCell.BGColor = uint.Parse(cellTag.Element("bgcolor").Value.ToString());
                newCell.Text = cellTag.Element("text").Value.ToString();
            }
        }


        /// <summary>
        /// verfiy whether or not an expression tree has a circular reference.
        /// this includes self references as well
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private bool VerifyNoCircleRef(SubCell targetCell)
        {
            if (dependencies.ContainsKey(targetCell))
            {
                if (dependencies[targetCell].Contains(targetCell))
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Demo method will randomly select 50 cells to write "random" in, It
        ///  will then write in ever Row in Column B the column name and number (e.g. B#).
        ///  After it will set every Row in COlumn A to reference Column B.
        /// </summary>
        public void Demo()
        {
            int randomCol = 0;
            int randomRow = 0;
            Random randomNum = new Random();
            
            //write "random test" to 50 psuedo-randomly generated cell locations
            for (int i = 0; i < 50; i++)
            {
                randomCol = randomNum.Next(26);
                randomRow = randomNum.Next(50);
                sheet[randomRow, randomCol].Text = "random test";
            }

            //write to every cell in column B, along with their row number
            for (int i = 0; i < 50; i++) 
            {
                sheet[i, 1].Text = "This is Cell B" + (i + 1).ToString();
            }
            
            //set a reference of every cell in column A to match the cells in column B
            for (int i = 0; i < 50; i++) 
            {
                sheet[i,0].Text = "=B" + (i + 1).ToString();
            }
            
        }
    }



}

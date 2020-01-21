/*
 * Zane Partain
 *     11488182
 *      2/25/19
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using CptS321;

namespace Spreadsheet_Zane_Partain
{
    public partial class Form1 : Form
    {
        //init spreadsheet
        private Spreadsheet spreadSheet;

        public Form1()
        {
            InitializeComponent();
            spreadSheet = new Spreadsheet(50, 26);   //instance of spreadhsheet
            button1.BringToFront();

            RedoEditStripMenuItem1.Enabled = false;  //set both undo and redo to false upon construction
            UndoEditStripMenuItem2.Enabled = false;
        }


        /// <summary>
        /// Form1_Load will programmatically  create the columns A to Z
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            HandleSubscribers();        //handle all Event listeners (subscriptions)
            InitializeDataGridView();   //init dataGridView1 to row: 1-50, col: A-Z
       
            //perform a Demo on the spreadsheet
            spreadSheet.Demo(); 
        }

       private void InitializeDataGridView()
        {
            dataGridView1.Columns.Clear();  //clear all columns
            dataGridView1.ColumnCount = 26; //for A-Z columns
            dataGridView1.RowCount = 50;    //for all 50 rows
            int asciiChar = 65;             //ascii value of A

            //populate the rows with A-Z
            while (asciiChar <= Convert.ToInt32('Z'))
            {
                char letter = Convert.ToChar(asciiChar);
                dataGridView1.Columns[asciiChar % 65].Name = Convert.ToString(letter);
                asciiChar++;
            }

            //populate row headers with 1-50
            for (int i = 1; i <= 50; i++)
            {
                dataGridView1.Rows[i - 1].HeaderCell.Value = i.ToString();
            }

            dataGridView1.RowHeadersWidth = 50;  //scale row headers to fit 1-50
        }



        private void HandleSubscribers()
        {
            // subscribe/delegate handlers
            spreadSheet.CellPropertyChanged += Spreadsheet_CellPropertyChanged;
            dataGridView1.CellBeginEdit += new DataGridViewCellCancelEventHandler(dataGridView1_CellBeginEdit);
            dataGridView1.CellEndEdit += new DataGridViewCellEventHandler(dataGridView1_CellEndEdit);
        }

        /// <summary>
        /// Handle cell property changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Spreadsheet_CellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Cell targetCell = sender as Cell;
            
            //update UI at specific Cell to match the value of the targetCell value
            dataGridView1.Rows[targetCell.RowIndex].Cells[targetCell.ColumnIndex].Value = targetCell.Value;
            
            //handle setting the BGColor of the target cell on the dataGridView1
            if(e.PropertyName == "BGColor")
            {
                dataGridView1.Rows[targetCell.RowIndex].Cells[targetCell.ColumnIndex].Style.BackColor = Color.FromArgb((int)targetCell.BGColor);
            }
        }


        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            string msg = String.Format("Editing Cell at ({0}, {1})", e.ColumnIndex, e.RowIndex);
            Cell targetCell = spreadSheet.GetCell(e.RowIndex, e.ColumnIndex);

            dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = spreadSheet.GetCell(e.RowIndex, e.ColumnIndex).Text;
        }


        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                Cell targetCell = spreadSheet.GetCell(e.RowIndex, e.ColumnIndex);   //get focused cell
                string oldText = targetCell.Text;

                if (oldText == null)  //if old text was empty 
                {
                    oldText = "";
                }

                targetCell.Text = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

                //This method is invoked any time a single dataGridView1 cell is being edited (by text)
                HandleUndoRedoText(targetCell, e.RowIndex, e.ColumnIndex, oldText);

                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = targetCell.Value; //update dataGridView1.Value 
            }
        }


        /// <summary>
        /// Used to push the old text and updated text to the respective stacks in the UndoRedoCommandCollections
        /// -- BUG: when the value of a cell is changed by undoing text, it can not seem to redo that same text again,
        ///         after it had been deleted by Undo
        /// </summary>
        /// <param name="targetCell"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="prevText"></param>
        private void HandleUndoRedoText(Cell targetCell,int row, int col, string prevText)
        {
            Stack<Cell> newCells = new Stack<Cell>();       //updated Cell state 
            Stack<Cell> oldCells = new Stack<Cell>();       //dataGridView1 cell state
            Stack<string> newText = new Stack<string>();    //updated Cell.Text property
            Stack<string> oldText = new Stack<string>();    //dataGridView1 cell text

            //create BGColor command for undo/redo
            SpreadsheetEngine.UndoRedoTextCommand commandText = new SpreadsheetEngine.UndoRedoTextCommand("Text");
            oldText.Push(prevText);
            oldCells.Push(targetCell);

            Cell newCell = spreadSheet.GetCell(row, col); //get updated cell text

            newCells.Push(newCell);
            newText.Push(newCell.Text);

            commandText.SetCollections(oldText, newText, oldCells, newCells);
            spreadSheet.AddUndo(commandText);
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }


        //Cell button will show the OptionContextMenuStrip
        private void button2_Click(object sender, EventArgs e)
        {
            /*Display the Option Context Menu Strip*/
            Button btnSender = (Button)sender;
            Point ptLowerLeft = new Point(0, btnSender.Height);
            ptLowerLeft = btnSender.PointToScreen(ptLowerLeft);
            OptioncontextMenuStrip.Show(ptLowerLeft);
        }


        /// <summary>
        /// BGColor tool strip menu item will have a color dialog pop-up when the user clicks this item.
        /// The then selected color will change the color of all currently selected cells on the dataGridView1.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BGColorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();

            Stack<Cell> newCells = new Stack<Cell>();      //updated Cell state 
            Stack<Cell> oldCells = new Stack<Cell>();      //dataGridView1 cell state
            Stack<uint> newBGColor = new Stack<uint>();    //updated Cell.BGColor property
            Stack<uint> oldBGColor = new Stack<uint>();    //dataGridView1 cell text stack

            //create BGColor command for undo/redo
            SpreadsheetEngine.UndoRedoBGColorCommand commandBGColor = new SpreadsheetEngine.UndoRedoBGColorCommand("BGColor");


            colorDialog.ShowDialog();  //show color dialog
            

            foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
            {
                Cell targetCell = spreadSheet.GetCell(cell.RowIndex, cell.ColumnIndex);
                
                oldCells.Push(targetCell);             //push the old target cell state and color
                oldBGColor.Push(targetCell.BGColor);
                newCells.Push(targetCell);             //push current cell state to newCell stack

                //push updated BGColor of target cell
                targetCell.BGColor = (uint)colorDialog.Color.ToArgb();
                newBGColor.Push(targetCell.BGColor);  
            }

            //Set collection of all old and new stacks, then clear dataGridView selection
            commandBGColor.SetCollections(oldBGColor, newBGColor, oldCells, newCells);
            spreadSheet.AddUndo(commandBGColor);

            dataGridView1.ClearSelection(); 
        }


        /// <summary>
        /// Open Edit Context Menu Strip for user to call and use menu options/settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Button btnSender = (Button)sender;
            Point ptLowerLeft = new Point(0, btnSender.Height);
            ptLowerLeft = btnSender.PointToScreen(ptLowerLeft);
            UndoRedoContextMenuStrip.Show(ptLowerLeft);

            UndoEditStripMenuItem2.Text = "Undo";
            RedoEditStripMenuItem1.Text = "Redo";


            // enable Undo menu item iff there is a command to undo 
            if (spreadSheet.UndoCount > 0)
            {
                UndoEditStripMenuItem2.Enabled = true;
                UndoEditStripMenuItem2.Text += " " + spreadSheet.GetUndoTitle();
            }
            else UndoEditStripMenuItem2.Enabled = false;

            // enable Redo menu item iff there is a command to redo 
            if (spreadSheet.RedoCount > 0)
            {
                RedoEditStripMenuItem1.Enabled = true;
                RedoEditStripMenuItem1.Text += " " + spreadSheet.GetRedoTitle();
            }
            else RedoEditStripMenuItem1.Enabled = false;
        }


        private void RedoEditStripMenuItem1_Click(object sender, EventArgs e)
        {
            //if you have made it here, then the Redo stack must contain atleast 1 item
            spreadSheet.Redo();
        }


        private void UndoEditStripMenuItem2_Click(object sender, EventArgs e)
        {
            //if you have made it here, then the Undo stack must contain atleast 1 item
            spreadSheet.Undo();
        }


        /// <summary>
        /// File Button, click to present user with options of save and load
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            Button btnSender = (Button)sender;
            Point ptLowerLeft = new Point(0, btnSender.Height);
            ptLowerLeft = btnSender.PointToScreen(ptLowerLeft);
            FileContextMenuStrip.Show(ptLowerLeft);
        }


        /// <summary>
        /// Saves the current spreadsheet for the user to load up at a later time. Uses SaveFileDailog,
        /// so the user can pick the location that they wish to save their spreadsheet. Then Invokes
        /// the SpreadsheetEngine Save method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfDialog = new SaveFileDialog();  //dialog for saving files

            //if the dialog opens successfully, and no 'cancel'
            if(sfDialog.ShowDialog() == DialogResult.OK)   
            {
                string name = sfDialog.FileName;
                FileStream fileToSave = new FileStream(name, FileMode.Create, FileAccess.Write,FileShare.ReadWrite);

                spreadSheet.SaveSpreadsheet(fileToSave); //convert to XML Doc. and save

                //handle properly closing save file stream
                fileToSave.Close();    
                fileToSave.Dispose(); 
            }
        }


        /// <summary>
        /// Load a file (spreadsheet) that the user chooses to upload.
        /// The new spreadsheet that will be loaded in will have to take
        /// place of the current running spreadsheet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //menu for loading a file
            OpenFileDialog loadFileDialog = new OpenFileDialog();

            //if the dialog opens successfully, and no 'cancel'
            if (loadFileDialog.ShowDialog() == DialogResult.OK)
            {
                //we will initialize a brand new spreadhsheet
                //in place of the current loaded one
                spreadSheet = new Spreadsheet(50, 26);
                InitializeDataGridView();    
                HandleSubscribers();

                string name = loadFileDialog.FileName;
                FileStream fileToLoad = new FileStream(name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                spreadSheet.LoadSpreadsheet(fileToLoad);  //spreadsheet Load() file

                //handle properly closing load file stream
                fileToLoad.Close();
                fileToLoad.Dispose();
            }
        }
    }

}

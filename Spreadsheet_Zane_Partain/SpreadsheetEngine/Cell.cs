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
using System.ComponentModel;

namespace CptS321
{
    abstract public class Cell : INotifyPropertyChanged
    {
        //fields
        private int rowIndex;
        private int colIndex;
        protected string cellText;
        protected string cellValue;
        private uint backGroundColor;
        private string name;
        
        //constructor
        protected Cell(int row, int col) {
            rowIndex = row;
            colIndex = col;
            BGColor = 0xFFFFFFFF;
        }


        //implement interface  INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        // OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        //Properties of a Cell
        public string Name { get { return name; } }
        public int RowIndex { get { return rowIndex; } }
        public int ColumnIndex { get { return colIndex; } }
        public string Value { get { return cellValue; } }
        public uint BGColor
        {
            get
            {
                return backGroundColor;
            }
            set
            {
                if (value == backGroundColor) { return; }
                backGroundColor = value;
                OnPropertyChanged("BGColor");
            }
        } 
        public string Text
        {
            get { return cellText; }
            set
            {
                if(value == Text) return; 
                cellText = value;
                OnPropertyChanged("Text");
            }
        }


        internal void SetValue(string value)
        {
            this.cellValue = value;
            OnPropertyChanged("Value");
        }

    
        internal void SetName(string value)
        {
            this.name = value;
        }
    }
}

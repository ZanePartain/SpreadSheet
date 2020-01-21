namespace Spreadsheet_Zane_Partain
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.A = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.B = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.C = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.D = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OptioncontextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.BGColorToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.UndoRedoContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.RedoEditStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.UndoEditStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.FileContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.LoadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.OptioncontextMenuStrip.SuspendLayout();
            this.UndoRedoContextMenuStrip.SuspendLayout();
            this.FileContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.A,
            this.B,
            this.C,
            this.D});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView1.Location = new System.Drawing.Point(0, 29);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(528, 352);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // A
            // 
            this.A.HeaderText = "A";
            this.A.Name = "A";
            // 
            // B
            // 
            this.B.HeaderText = "B";
            this.B.Name = "B";
            // 
            // C
            // 
            this.C.HeaderText = "C";
            this.C.Name = "C";
            // 
            // D
            // 
            this.D.HeaderText = "D";
            this.D.Name = "D";
            // 
            // OptioncontextMenuStrip
            // 
            this.OptioncontextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BGColorToolStripMenuItem1});
            this.OptioncontextMenuStrip.Name = "OptioncontextMenuStrip";
            this.OptioncontextMenuStrip.Size = new System.Drawing.Size(119, 26);
            this.OptioncontextMenuStrip.Text = "Edit";
            // 
            // BGColorToolStripMenuItem1
            // 
            this.BGColorToolStripMenuItem1.Name = "BGColorToolStripMenuItem1";
            this.BGColorToolStripMenuItem1.Size = new System.Drawing.Size(118, 22);
            this.BGColorToolStripMenuItem1.Text = "BGColor";
            this.BGColorToolStripMenuItem1.Click += new System.EventHandler(this.BGColorToolStripMenuItem1_Click);
            // 
            // button1
            // 
            this.button1.ContextMenuStrip = this.UndoRedoContextMenuStrip;
            this.button1.Location = new System.Drawing.Point(73, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(91, 31);
            this.button1.TabIndex = 2;
            this.button1.Text = "Edit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // UndoRedoContextMenuStrip
            // 
            this.UndoRedoContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RedoEditStripMenuItem1,
            this.UndoEditStripMenuItem2});
            this.UndoRedoContextMenuStrip.Name = "UndoRedoContextMenuStrip";
            this.UndoRedoContextMenuStrip.Size = new System.Drawing.Size(104, 48);
            // 
            // RedoEditStripMenuItem1
            // 
            this.RedoEditStripMenuItem1.Name = "RedoEditStripMenuItem1";
            this.RedoEditStripMenuItem1.Size = new System.Drawing.Size(103, 22);
            this.RedoEditStripMenuItem1.Text = "Redo";
            this.RedoEditStripMenuItem1.Click += new System.EventHandler(this.RedoEditStripMenuItem1_Click);
            // 
            // UndoEditStripMenuItem2
            // 
            this.UndoEditStripMenuItem2.Name = "UndoEditStripMenuItem2";
            this.UndoEditStripMenuItem2.Size = new System.Drawing.Size(103, 22);
            this.UndoEditStripMenuItem2.Text = "Undo";
            this.UndoEditStripMenuItem2.Click += new System.EventHandler(this.UndoEditStripMenuItem2_Click);
            // 
            // textBox1
            // 
            this.textBox1.ContextMenuStrip = this.OptioncontextMenuStrip;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox1.ImeMode = System.Windows.Forms.ImeMode.On;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(528, 31);
            this.textBox1.TabIndex = 3;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Transparent;
            this.button2.ContextMenuStrip = this.OptioncontextMenuStrip;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button2.Location = new System.Drawing.Point(162, 0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(84, 31);
            this.button2.TabIndex = 4;
            this.button2.Text = "Cell";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.ContextMenuStrip = this.FileContextMenuStrip;
            this.button3.Location = new System.Drawing.Point(0, 0);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 31);
            this.button3.TabIndex = 5;
            this.button3.Text = "File";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // FileContextMenuStrip
            // 
            this.FileContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LoadToolStripMenuItem,
            this.SaveToolStripMenuItem});
            this.FileContextMenuStrip.Name = "FileContextMenuStrip";
            this.FileContextMenuStrip.Size = new System.Drawing.Size(101, 48);
            // 
            // LoadToolStripMenuItem
            // 
            this.LoadToolStripMenuItem.Name = "LoadToolStripMenuItem";
            this.LoadToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.LoadToolStripMenuItem.Text = "Load";
            this.LoadToolStripMenuItem.Click += new System.EventHandler(this.LoadToolStripMenuItem_Click);
            // 
            // SaveToolStripMenuItem
            // 
            this.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem";
            this.SaveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.SaveToolStripMenuItem.Text = "Save";
            this.SaveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 381);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Form1";
            this.Text = "Zane Partain SpreadSheet";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.OptioncontextMenuStrip.ResumeLayout(false);
            this.UndoRedoContextMenuStrip.ResumeLayout(false);
            this.FileContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn A;
        private System.Windows.Forms.DataGridViewTextBoxColumn B;
        private System.Windows.Forms.DataGridViewTextBoxColumn C;
        private System.Windows.Forms.DataGridViewTextBoxColumn D;
        private System.Windows.Forms.ContextMenuStrip OptioncontextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem BGColorToolStripMenuItem1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ContextMenuStrip UndoRedoContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem RedoEditStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem UndoEditStripMenuItem2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ContextMenuStrip FileContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem LoadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveToolStripMenuItem;
    }
}


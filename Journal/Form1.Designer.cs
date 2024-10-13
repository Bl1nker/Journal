namespace Journal
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            button_selectFiles = new Button();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            Table1 = new DataGridView();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            contextForTable1 = new ContextMenuStrip(components);
            tabPage2 = new TabPage();
            Table2 = new DataGridView();
            Column3 = new DataGridViewTextBoxColumn();
            Column4 = new DataGridViewTextBoxColumn();
            Column5 = new DataGridViewTextBoxColumn();
            Column6 = new DataGridViewTextBoxColumn();
            Column7 = new DataGridViewTextBoxColumn();
            Column8 = new DataGridViewTextBoxColumn();
            button_findCabels = new Button();
            button_ToExcel = new Button();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Table1).BeginInit();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Table2).BeginInit();
            SuspendLayout();
            // 
            // button_selectFiles
            // 
            button_selectFiles.Location = new Point(20, 12);
            button_selectFiles.Name = "button_selectFiles";
            button_selectFiles.Size = new Size(150, 28);
            button_selectFiles.TabIndex = 0;
            button_selectFiles.Text = "Выбор файлов";
            button_selectFiles.UseVisualStyleBackColor = true;
            button_selectFiles.Click += But_selectFiles_Click;
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.ItemSize = new Size(80, 20);
            tabControl1.Location = new Point(6, 57);
            tabControl1.Margin = new Padding(5);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1275, 400);
            tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(Table1);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1267, 372);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Список файлов";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // Table1
            // 
            Table1.AllowUserToAddRows = false;
            Table1.AllowUserToResizeRows = false;
            Table1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new Padding(4, 0, 0, 0);
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            Table1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            Table1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            Table1.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2 });
            Table1.ContextMenuStrip = contextForTable1;
            Table1.Location = new Point(0, 0);
            Table1.MultiSelect = false;
            Table1.Name = "Table1";
            Table1.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Control;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            Table1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            Table1.RowHeadersVisible = false;
            Table1.RowTemplate.Height = 25;
            Table1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            Table1.Size = new Size(1267, 372);
            Table1.TabIndex = 0;
            // 
            // Column1
            // 
            Column1.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.Padding = new Padding(4, 0, 0, 0);
            Column1.DefaultCellStyle = dataGridViewCellStyle2;
            Column1.HeaderText = "№";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Width = 49;
            // 
            // Column2
            // 
            Column2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Column2.HeaderText = "Файл";
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            // 
            // contextForTable1
            // 
            contextForTable1.Name = "contextMenuStrip1";
            contextForTable1.Size = new Size(61, 4);            
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(Table2);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1267, 372);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Список кабелей";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // Table2
            // 
            Table2.AllowUserToAddRows = false;
            Table2.AllowUserToResizeRows = false;
            Table2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = SystemColors.Control;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle4.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            Table2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            Table2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            Table2.Columns.AddRange(new DataGridViewColumn[] { Column3, Column4, Column5, Column6, Column7, Column8 });
            Table2.Location = new Point(0, 0);
            Table2.Name = "Table2";
            Table2.RowHeadersVisible = false;
            Table2.RowTemplate.Height = 25;
            Table2.Size = new Size(1267, 372);
            Table2.TabIndex = 0;
            // 
            // Column3
            // 
            Column3.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Column3.DefaultCellStyle = dataGridViewCellStyle5;
            Column3.HeaderText = "№";
            Column3.MinimumWidth = 10;
            Column3.Name = "Column3";
            Column3.Resizable = DataGridViewTriState.False;
            Column3.Width = 30;
            // 
            // Column4
            // 
            Column4.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Column4.DefaultCellStyle = dataGridViewCellStyle6;
            Column4.HeaderText = "Маркировка кабеля";
            Column4.MinimumWidth = 90;
            Column4.Name = "Column4";
            Column4.Width = 90;
            // 
            // Column5
            // 
            Column5.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Column5.DefaultCellStyle = dataGridViewCellStyle7;
            Column5.HeaderText = "Заводская маркировка кабеля";
            Column5.MinimumWidth = 140;
            Column5.Name = "Column5";
            Column5.Width = 140;
            // 
            // Column6
            // 
            Column6.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Column6.DefaultCellStyle = dataGridViewCellStyle8;
            Column6.HeaderText = "Число жил и сечение (мм2)";
            Column6.MinimumWidth = 120;
            Column6.Name = "Column6";
            Column6.Width = 120;
            // 
            // Column7
            // 
            Column7.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Column7.FillWeight = 50F;
            Column7.HeaderText = "Направление (откуда)";
            Column7.Name = "Column7";
            // 
            // Column8
            // 
            Column8.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Column8.FillWeight = 50F;
            Column8.HeaderText = "Направление (куда)";
            Column8.Name = "Column8";
            // 
            // button_findCabels
            // 
            button_findCabels.Location = new Point(200, 12);
            button_findCabels.Name = "button_findCabels";
            button_findCabels.Size = new Size(150, 28);
            button_findCabels.TabIndex = 2;
            button_findCabels.Text = "Найти кабели";
            button_findCabels.UseVisualStyleBackColor = true;
            button_findCabels.Click += But_findCabels;
            // 
            // button_ToExcel
            // 
            button_ToExcel.Enabled = false;
            button_ToExcel.Location = new Point(380, 12);
            button_ToExcel.Name = "button_ToExcel";
            button_ToExcel.Size = new Size(150, 28);
            button_ToExcel.TabIndex = 3;
            button_ToExcel.Text = "Запись в Excel";
            button_ToExcel.UseVisualStyleBackColor = true;
            button_ToExcel.Click += But_ToExcel_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1284, 461);
            Controls.Add(button_ToExcel);
            Controls.Add(button_findCabels);
            Controls.Add(tabControl1);
            Controls.Add(button_selectFiles);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "Journal 1.0";
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Table1).EndInit();
            tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Table2).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button button_selectFiles;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private DataGridView Table1;
        private DataGridView Table2;
        private ContextMenuStrip contextForTable1;
        private Button button_findCabels;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column4;
        private DataGridViewTextBoxColumn Column5;
        private DataGridViewTextBoxColumn Column6;
        private DataGridViewTextBoxColumn Column7;
        private DataGridViewTextBoxColumn Column8;
        private Button button_ToExcel;
    }
}
namespace Employee_Display_and_Sorting
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.UI_DisplayUnsort_Btn = new System.Windows.Forms.Button();
            this.UI_ClearUnsorted_Btn = new System.Windows.Forms.Button();
            this.UI_EmployeeIdSort_Btn = new System.Windows.Forms.Button();
            this.UI_LastNameSort_Btn = new System.Windows.Forms.Button();
            this.UI_ClearSorted_Btn = new System.Windows.Forms.Button();
            this.UI_DataSource_Gbx = new System.Windows.Forms.GroupBox();
            this.UI_FileData_Rbtn = new System.Windows.Forms.RadioButton();
            this.UI_Provided_Rbtn = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.UI_Sorted_Lbl = new System.Windows.Forms.Label();
            this.UI_Unsorted_Dgv = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UI_Sorted_Dgv = new System.Windows.Forms.DataGridView();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label3 = new System.Windows.Forms.Label();
            this.UI_Tick_Tbx = new System.Windows.Forms.TextBox();
            this.UI_OFD_Btn = new System.Windows.Forms.Button();
            this.UI_Data_OFD = new System.Windows.Forms.OpenFileDialog();
            this.UI_DataSource_Gbx.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UI_Unsorted_Dgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UI_Sorted_Dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // UI_DisplayUnsort_Btn
            // 
            this.UI_DisplayUnsort_Btn.Location = new System.Drawing.Point(356, 66);
            this.UI_DisplayUnsort_Btn.Name = "UI_DisplayUnsort_Btn";
            this.UI_DisplayUnsort_Btn.Size = new System.Drawing.Size(173, 40);
            this.UI_DisplayUnsort_Btn.TabIndex = 0;
            this.UI_DisplayUnsort_Btn.Text = "Display Unsorted List";
            this.UI_DisplayUnsort_Btn.UseVisualStyleBackColor = true;
            this.UI_DisplayUnsort_Btn.Click += new System.EventHandler(this.Unsorted_Btn_Click);
            // 
            // UI_ClearUnsorted_Btn
            // 
            this.UI_ClearUnsorted_Btn.Location = new System.Drawing.Point(356, 112);
            this.UI_ClearUnsorted_Btn.Name = "UI_ClearUnsorted_Btn";
            this.UI_ClearUnsorted_Btn.Size = new System.Drawing.Size(173, 40);
            this.UI_ClearUnsorted_Btn.TabIndex = 1;
            this.UI_ClearUnsorted_Btn.Text = "Clear Unsorted Data Grid";
            this.UI_ClearUnsorted_Btn.UseVisualStyleBackColor = true;
            this.UI_ClearUnsorted_Btn.Click += new System.EventHandler(this.Unsorted_Btn_Click);
            // 
            // UI_EmployeeIdSort_Btn
            // 
            this.UI_EmployeeIdSort_Btn.Location = new System.Drawing.Point(356, 238);
            this.UI_EmployeeIdSort_Btn.Name = "UI_EmployeeIdSort_Btn";
            this.UI_EmployeeIdSort_Btn.Size = new System.Drawing.Size(173, 40);
            this.UI_EmployeeIdSort_Btn.TabIndex = 2;
            this.UI_EmployeeIdSort_Btn.Text = "Sort By EmployeeID";
            this.UI_EmployeeIdSort_Btn.UseVisualStyleBackColor = true;
            this.UI_EmployeeIdSort_Btn.Click += new System.EventHandler(this.Sorted_Btn_Click);
            // 
            // UI_LastNameSort_Btn
            // 
            this.UI_LastNameSort_Btn.Location = new System.Drawing.Point(356, 284);
            this.UI_LastNameSort_Btn.Name = "UI_LastNameSort_Btn";
            this.UI_LastNameSort_Btn.Size = new System.Drawing.Size(173, 40);
            this.UI_LastNameSort_Btn.TabIndex = 3;
            this.UI_LastNameSort_Btn.Text = "Sort By Last Names";
            this.UI_LastNameSort_Btn.UseVisualStyleBackColor = true;
            this.UI_LastNameSort_Btn.Click += new System.EventHandler(this.Sorted_Btn_Click);
            // 
            // UI_ClearSorted_Btn
            // 
            this.UI_ClearSorted_Btn.Location = new System.Drawing.Point(356, 330);
            this.UI_ClearSorted_Btn.Name = "UI_ClearSorted_Btn";
            this.UI_ClearSorted_Btn.Size = new System.Drawing.Size(173, 40);
            this.UI_ClearSorted_Btn.TabIndex = 4;
            this.UI_ClearSorted_Btn.Text = "Clear Sorted Data Grid";
            this.UI_ClearSorted_Btn.UseVisualStyleBackColor = true;
            this.UI_ClearSorted_Btn.Click += new System.EventHandler(this.Sorted_Btn_Click);
            // 
            // UI_DataSource_Gbx
            // 
            this.UI_DataSource_Gbx.Controls.Add(this.UI_FileData_Rbtn);
            this.UI_DataSource_Gbx.Controls.Add(this.UI_Provided_Rbtn);
            this.UI_DataSource_Gbx.Location = new System.Drawing.Point(356, 158);
            this.UI_DataSource_Gbx.Name = "UI_DataSource_Gbx";
            this.UI_DataSource_Gbx.Size = new System.Drawing.Size(173, 74);
            this.UI_DataSource_Gbx.TabIndex = 6;
            this.UI_DataSource_Gbx.TabStop = false;
            this.UI_DataSource_Gbx.Text = "Data Source";
            // 
            // UI_FileData_Rbtn
            // 
            this.UI_FileData_Rbtn.AutoSize = true;
            this.UI_FileData_Rbtn.Location = new System.Drawing.Point(6, 41);
            this.UI_FileData_Rbtn.Name = "UI_FileData_Rbtn";
            this.UI_FileData_Rbtn.Size = new System.Drawing.Size(67, 17);
            this.UI_FileData_Rbtn.TabIndex = 8;
            this.UI_FileData_Rbtn.Text = "File Data";
            this.UI_FileData_Rbtn.UseVisualStyleBackColor = true;
            this.UI_FileData_Rbtn.CheckedChanged += new System.EventHandler(this.DataSource_CheckChanged);
            // 
            // UI_Provided_Rbtn
            // 
            this.UI_Provided_Rbtn.AutoSize = true;
            this.UI_Provided_Rbtn.Checked = true;
            this.UI_Provided_Rbtn.Location = new System.Drawing.Point(6, 19);
            this.UI_Provided_Rbtn.Name = "UI_Provided_Rbtn";
            this.UI_Provided_Rbtn.Size = new System.Drawing.Size(86, 17);
            this.UI_Provided_Rbtn.TabIndex = 7;
            this.UI_Provided_Rbtn.TabStop = true;
            this.UI_Provided_Rbtn.Text = "Provided List";
            this.UI_Provided_Rbtn.UseVisualStyleBackColor = true;
            this.UI_Provided_Rbtn.CheckedChanged += new System.EventHandler(this.DataSource_CheckChanged);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(335, 23);
            this.label1.TabIndex = 7;
            this.label1.Text = "Unsorted Data";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UI_Sorted_Lbl
            // 
            this.UI_Sorted_Lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UI_Sorted_Lbl.Location = new System.Drawing.Point(535, 40);
            this.UI_Sorted_Lbl.Name = "UI_Sorted_Lbl";
            this.UI_Sorted_Lbl.Size = new System.Drawing.Size(337, 23);
            this.UI_Sorted_Lbl.TabIndex = 8;
            this.UI_Sorted_Lbl.Text = "Sorted Data";
            this.UI_Sorted_Lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UI_Unsorted_Dgv
            // 
            this.UI_Unsorted_Dgv.AllowUserToResizeColumns = false;
            this.UI_Unsorted_Dgv.AllowUserToResizeRows = false;
            this.UI_Unsorted_Dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.UI_Unsorted_Dgv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.UI_Unsorted_Dgv.ColumnHeadersHeight = 21;
            this.UI_Unsorted_Dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.UI_Unsorted_Dgv.Location = new System.Drawing.Point(15, 66);
            this.UI_Unsorted_Dgv.Name = "UI_Unsorted_Dgv";
            this.UI_Unsorted_Dgv.ReadOnly = true;
            this.UI_Unsorted_Dgv.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.UI_Unsorted_Dgv.RowHeadersVisible = false;
            this.UI_Unsorted_Dgv.Size = new System.Drawing.Size(335, 350);
            this.UI_Unsorted_Dgv.TabIndex = 9;
            this.UI_Unsorted_Dgv.DragDrop += new System.Windows.Forms.DragEventHandler(this.DragDrop);
            this.UI_Unsorted_Dgv.DragEnter += new System.Windows.Forms.DragEventHandler(this.DragEnter);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Employee Id";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "First Name";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Last Name";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // UI_Sorted_Dgv
            // 
            this.UI_Sorted_Dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.UI_Sorted_Dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.UI_Sorted_Dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column4,
            this.Column5,
            this.Column6});
            this.UI_Sorted_Dgv.Location = new System.Drawing.Point(535, 66);
            this.UI_Sorted_Dgv.Name = "UI_Sorted_Dgv";
            this.UI_Sorted_Dgv.RowHeadersVisible = false;
            this.UI_Sorted_Dgv.Size = new System.Drawing.Size(337, 350);
            this.UI_Sorted_Dgv.TabIndex = 10;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Employee ID";
            this.Column4.Name = "Column4";
            // 
            // Column5
            // 
            this.Column5.HeaderText = "First Name";
            this.Column5.Name = "Column5";
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Last Name";
            this.Column6.Name = "Column6";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(558, 440);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(201, 23);
            this.label3.TabIndex = 11;
            this.label3.Text = "Time Take (Elapsed Ticks): ";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UI_Tick_Tbx
            // 
            this.UI_Tick_Tbx.Location = new System.Drawing.Point(768, 442);
            this.UI_Tick_Tbx.Name = "UI_Tick_Tbx";
            this.UI_Tick_Tbx.ReadOnly = true;
            this.UI_Tick_Tbx.Size = new System.Drawing.Size(81, 20);
            this.UI_Tick_Tbx.TabIndex = 12;
            // 
            // UI_OFD_Btn
            // 
            this.UI_OFD_Btn.AllowDrop = true;
            this.UI_OFD_Btn.Enabled = false;
            this.UI_OFD_Btn.Location = new System.Drawing.Point(356, 376);
            this.UI_OFD_Btn.Name = "UI_OFD_Btn";
            this.UI_OFD_Btn.Size = new System.Drawing.Size(173, 40);
            this.UI_OFD_Btn.TabIndex = 5;
            this.UI_OFD_Btn.Text = "Open File Through OFD";
            this.UI_OFD_Btn.UseVisualStyleBackColor = true;
            this.UI_OFD_Btn.Click += new System.EventHandler(this.UI_OFD_Btn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(889, 486);
            this.Controls.Add(this.UI_Tick_Tbx);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.UI_Sorted_Dgv);
            this.Controls.Add(this.UI_Unsorted_Dgv);
            this.Controls.Add(this.UI_Sorted_Lbl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.UI_DataSource_Gbx);
            this.Controls.Add(this.UI_OFD_Btn);
            this.Controls.Add(this.UI_ClearSorted_Btn);
            this.Controls.Add(this.UI_LastNameSort_Btn);
            this.Controls.Add(this.UI_EmployeeIdSort_Btn);
            this.Controls.Add(this.UI_ClearUnsorted_Btn);
            this.Controls.Add(this.UI_DisplayUnsort_Btn);
            this.Name = "Form1";
            this.Text = "Form1";
            this.UI_DataSource_Gbx.ResumeLayout(false);
            this.UI_DataSource_Gbx.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UI_Unsorted_Dgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UI_Sorted_Dgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button UI_DisplayUnsort_Btn;
        private System.Windows.Forms.Button UI_ClearUnsorted_Btn;
        private System.Windows.Forms.Button UI_EmployeeIdSort_Btn;
        private System.Windows.Forms.Button UI_LastNameSort_Btn;
        private System.Windows.Forms.Button UI_ClearSorted_Btn;
        private System.Windows.Forms.GroupBox UI_DataSource_Gbx;
        private System.Windows.Forms.RadioButton UI_FileData_Rbtn;
        private System.Windows.Forms.RadioButton UI_Provided_Rbtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label UI_Sorted_Lbl;
        private System.Windows.Forms.DataGridView UI_Unsorted_Dgv;
        private System.Windows.Forms.DataGridView UI_Sorted_Dgv;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox UI_Tick_Tbx;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.Button UI_OFD_Btn;
        private System.Windows.Forms.OpenFileDialog UI_Data_OFD;
    }
}


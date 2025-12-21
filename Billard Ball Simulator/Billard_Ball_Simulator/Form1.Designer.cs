namespace Billard_Ball_Simulator
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
            UI_AddTable_Btn = new Button();
            UI_Friction_Lbl = new Label();
            UI_Sort_Gbx = new GroupBox();
            UI_TotalHits_Rbtn = new RadioButton();
            UI_Hits_Rbtn = new RadioButton();
            UI_Radius_Rbtn = new RadioButton();
            UI_Table_Dgv = new DataGridView();
            UI_Timer1 = new System.Windows.Forms.Timer(components);
            UI_FrictionInput_Lbl = new Label();
            UI_Sort_Gbx.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)UI_Table_Dgv).BeginInit();
            SuspendLayout();
            // 
            // UI_AddTable_Btn
            // 
            UI_AddTable_Btn.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            UI_AddTable_Btn.Location = new Point(12, 12);
            UI_AddTable_Btn.Name = "UI_AddTable_Btn";
            UI_AddTable_Btn.Size = new Size(129, 23);
            UI_AddTable_Btn.TabIndex = 0;
            UI_AddTable_Btn.Text = "New Table[0]";
            UI_AddTable_Btn.UseVisualStyleBackColor = true;
            // 
            // UI_Friction_Lbl
            // 
            UI_Friction_Lbl.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            UI_Friction_Lbl.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            UI_Friction_Lbl.Location = new Point(185, 12);
            UI_Friction_Lbl.Name = "UI_Friction_Lbl";
            UI_Friction_Lbl.Size = new Size(65, 23);
            UI_Friction_Lbl.TabIndex = 1;
            UI_Friction_Lbl.Text = "Friction:";
            UI_Friction_Lbl.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // UI_Sort_Gbx
            // 
            UI_Sort_Gbx.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            UI_Sort_Gbx.Controls.Add(UI_TotalHits_Rbtn);
            UI_Sort_Gbx.Controls.Add(UI_Hits_Rbtn);
            UI_Sort_Gbx.Controls.Add(UI_Radius_Rbtn);
            UI_Sort_Gbx.Location = new Point(12, 41);
            UI_Sort_Gbx.Name = "UI_Sort_Gbx";
            UI_Sort_Gbx.Size = new Size(309, 59);
            UI_Sort_Gbx.TabIndex = 2;
            UI_Sort_Gbx.TabStop = false;
            UI_Sort_Gbx.Text = "Sort Mode:";
            // 
            // UI_TotalHits_Rbtn
            // 
            UI_TotalHits_Rbtn.AutoSize = true;
            UI_TotalHits_Rbtn.Location = new Point(229, 22);
            UI_TotalHits_Rbtn.Name = "UI_TotalHits_Rbtn";
            UI_TotalHits_Rbtn.Size = new Size(75, 19);
            UI_TotalHits_Rbtn.TabIndex = 2;
            UI_TotalHits_Rbtn.Text = "Total Hits";
            UI_TotalHits_Rbtn.UseVisualStyleBackColor = true;
            // 
            // UI_Hits_Rbtn
            // 
            UI_Hits_Rbtn.AutoSize = true;
            UI_Hits_Rbtn.Location = new Point(130, 22);
            UI_Hits_Rbtn.Name = "UI_Hits_Rbtn";
            UI_Hits_Rbtn.Size = new Size(46, 19);
            UI_Hits_Rbtn.TabIndex = 1;
            UI_Hits_Rbtn.Text = "Hits";
            UI_Hits_Rbtn.UseVisualStyleBackColor = true;
            // 
            // UI_Radius_Rbtn
            // 
            UI_Radius_Rbtn.AutoSize = true;
            UI_Radius_Rbtn.Checked = true;
            UI_Radius_Rbtn.Location = new Point(6, 22);
            UI_Radius_Rbtn.Name = "UI_Radius_Rbtn";
            UI_Radius_Rbtn.Size = new Size(60, 19);
            UI_Radius_Rbtn.TabIndex = 0;
            UI_Radius_Rbtn.TabStop = true;
            UI_Radius_Rbtn.Text = "Radius";
            UI_Radius_Rbtn.UseVisualStyleBackColor = true;
            // 
            // UI_Table_Dgv
            // 
            UI_Table_Dgv.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            UI_Table_Dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            UI_Table_Dgv.Location = new Point(12, 106);
            UI_Table_Dgv.Name = "UI_Table_Dgv";
            UI_Table_Dgv.Size = new Size(309, 332);
            UI_Table_Dgv.TabIndex = 1;
            // 
            // UI_Timer1
            // 
            UI_Timer1.Enabled = true;
            UI_Timer1.Interval = 35;
            // 
            // UI_FrictionInput_Lbl
            // 
            UI_FrictionInput_Lbl.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            UI_FrictionInput_Lbl.BackColor = SystemColors.ActiveCaption;
            UI_FrictionInput_Lbl.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            UI_FrictionInput_Lbl.Location = new Point(256, 12);
            UI_FrictionInput_Lbl.Name = "UI_FrictionInput_Lbl";
            UI_FrictionInput_Lbl.Size = new Size(65, 23);
            UI_FrictionInput_Lbl.TabIndex = 3;
            UI_FrictionInput_Lbl.Text = "0.991";
            UI_FrictionInput_Lbl.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(333, 450);
            Controls.Add(UI_FrictionInput_Lbl);
            Controls.Add(UI_Table_Dgv);
            Controls.Add(UI_Sort_Gbx);
            Controls.Add(UI_Friction_Lbl);
            Controls.Add(UI_AddTable_Btn);
            Name = "Form1";
            Text = "Form1";
            UI_Sort_Gbx.ResumeLayout(false);
            UI_Sort_Gbx.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)UI_Table_Dgv).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button UI_AddTable_Btn;
        private Label UI_Friction_Lbl;
        private GroupBox UI_Sort_Gbx;
        private RadioButton UI_TotalHits_Rbtn;
        private RadioButton UI_Hits_Rbtn;
        private RadioButton UI_Radius_Rbtn;
        private DataGridView UI_Table_Dgv;
        private System.Windows.Forms.Timer UI_Timer1;
        private Label UI_FrictionInput_Lbl;
    }
}

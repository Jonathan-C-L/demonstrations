namespace Drawer
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
            this.UI_InfoDisplay_Lbl = new System.Windows.Forms.Label();
            this.UI_UndoLine_Btn = new System.Windows.Forms.Button();
            this.UI_Thickness_Tbar = new System.Windows.Forms.TrackBar();
            this.UI_Thickness_Lbl = new System.Windows.Forms.Label();
            this.UI_Alpha_Lbl = new System.Windows.Forms.Label();
            this.UI_UndoSeg_Btn = new System.Windows.Forms.Button();
            this.UI_Colour_Btn = new System.Windows.Forms.Button();
            this.UI_Timer = new System.Windows.Forms.Timer(this.components);
            this.UI_ColorDiag = new System.Windows.Forms.ColorDialog();
            ((System.ComponentModel.ISupportInitialize)(this.UI_Thickness_Tbar)).BeginInit();
            this.SuspendLayout();
            // 
            // UI_InfoDisplay_Lbl
            // 
            this.UI_InfoDisplay_Lbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.UI_InfoDisplay_Lbl.Location = new System.Drawing.Point(12, 22);
            this.UI_InfoDisplay_Lbl.Name = "UI_InfoDisplay_Lbl";
            this.UI_InfoDisplay_Lbl.Size = new System.Drawing.Size(358, 31);
            this.UI_InfoDisplay_Lbl.TabIndex = 0;
            this.UI_InfoDisplay_Lbl.Text = "0 lines, 0 total segments";
            this.UI_InfoDisplay_Lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UI_UndoLine_Btn
            // 
            this.UI_UndoLine_Btn.Location = new System.Drawing.Point(12, 56);
            this.UI_UndoLine_Btn.Name = "UI_UndoLine_Btn";
            this.UI_UndoLine_Btn.Size = new System.Drawing.Size(358, 31);
            this.UI_UndoLine_Btn.TabIndex = 1;
            this.UI_UndoLine_Btn.Text = "Undo Line";
            this.UI_UndoLine_Btn.UseVisualStyleBackColor = true;
            this.UI_UndoLine_Btn.Click += new System.EventHandler(this.UI_UndoLine_Btn_Click);
            // 
            // UI_Thickness_Tbar
            // 
            this.UI_Thickness_Tbar.Location = new System.Drawing.Point(28, 167);
            this.UI_Thickness_Tbar.Maximum = 50;
            this.UI_Thickness_Tbar.Minimum = 1;
            this.UI_Thickness_Tbar.Name = "UI_Thickness_Tbar";
            this.UI_Thickness_Tbar.Size = new System.Drawing.Size(329, 45);
            this.UI_Thickness_Tbar.TabIndex = 2;
            this.UI_Thickness_Tbar.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.UI_Thickness_Tbar.Value = 1;
            this.UI_Thickness_Tbar.Scroll += new System.EventHandler(this.UI_Thickness_Scroll);
            // 
            // UI_Thickness_Lbl
            // 
            this.UI_Thickness_Lbl.AutoSize = true;
            this.UI_Thickness_Lbl.Location = new System.Drawing.Point(9, 199);
            this.UI_Thickness_Lbl.Name = "UI_Thickness_Lbl";
            this.UI_Thickness_Lbl.Size = new System.Drawing.Size(74, 13);
            this.UI_Thickness_Lbl.TabIndex = 3;
            this.UI_Thickness_Lbl.Text = "Thickeness: 1";
            // 
            // UI_Alpha_Lbl
            // 
            this.UI_Alpha_Lbl.AutoSize = true;
            this.UI_Alpha_Lbl.Location = new System.Drawing.Point(312, 199);
            this.UI_Alpha_Lbl.Name = "UI_Alpha_Lbl";
            this.UI_Alpha_Lbl.Size = new System.Drawing.Size(58, 13);
            this.UI_Alpha_Lbl.TabIndex = 4;
            this.UI_Alpha_Lbl.Text = "Alpha: 255";
            // 
            // UI_UndoSeg_Btn
            // 
            this.UI_UndoSeg_Btn.Location = new System.Drawing.Point(12, 93);
            this.UI_UndoSeg_Btn.Name = "UI_UndoSeg_Btn";
            this.UI_UndoSeg_Btn.Size = new System.Drawing.Size(358, 31);
            this.UI_UndoSeg_Btn.TabIndex = 5;
            this.UI_UndoSeg_Btn.Text = "Undo Segment";
            this.UI_UndoSeg_Btn.UseVisualStyleBackColor = true;
            this.UI_UndoSeg_Btn.Click += new System.EventHandler(this.UI_UndoSeg_Btn_Click);
            // 
            // UI_Colour_Btn
            // 
            this.UI_Colour_Btn.Location = new System.Drawing.Point(12, 130);
            this.UI_Colour_Btn.Name = "UI_Colour_Btn";
            this.UI_Colour_Btn.Size = new System.Drawing.Size(358, 31);
            this.UI_Colour_Btn.TabIndex = 7;
            this.UI_Colour_Btn.Text = "Colour";
            this.UI_Colour_Btn.UseVisualStyleBackColor = true;
            this.UI_Colour_Btn.Click += new System.EventHandler(this.UI_Colour_Btn_Click);
            // 
            // UI_Timer
            // 
            this.UI_Timer.Enabled = true;
            this.UI_Timer.Interval = 10;
            this.UI_Timer.Tick += new System.EventHandler(this.UI_Timer_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 226);
            this.Controls.Add(this.UI_Colour_Btn);
            this.Controls.Add(this.UI_UndoSeg_Btn);
            this.Controls.Add(this.UI_Alpha_Lbl);
            this.Controls.Add(this.UI_Thickness_Lbl);
            this.Controls.Add(this.UI_Thickness_Tbar);
            this.Controls.Add(this.UI_UndoLine_Btn);
            this.Controls.Add(this.UI_InfoDisplay_Lbl);
            this.Name = "Form1";
            this.Text = "StackyListDraw";
            ((System.ComponentModel.ISupportInitialize)(this.UI_Thickness_Tbar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label UI_InfoDisplay_Lbl;
        private System.Windows.Forms.Button UI_UndoLine_Btn;
        private System.Windows.Forms.TrackBar UI_Thickness_Tbar;
        private System.Windows.Forms.Label UI_Thickness_Lbl;
        private System.Windows.Forms.Label UI_Alpha_Lbl;
        private System.Windows.Forms.Button UI_UndoSeg_Btn;
        private System.Windows.Forms.Button UI_Colour_Btn;
        private System.Windows.Forms.Timer UI_Timer;
        private System.Windows.Forms.ColorDialog UI_ColorDiag;
    }
}


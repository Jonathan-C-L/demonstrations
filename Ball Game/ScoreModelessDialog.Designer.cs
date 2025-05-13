namespace Ball_Game
{
    partial class ScoreModelessDialog
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
            this.UI_Score_Lbl = new System.Windows.Forms.Label();
            this.UI_ScoreCount_Lbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // UI_Score_Lbl
            // 
            this.UI_Score_Lbl.Font = new System.Drawing.Font("OCR A Extended", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UI_Score_Lbl.Location = new System.Drawing.Point(12, 34);
            this.UI_Score_Lbl.Name = "UI_Score_Lbl";
            this.UI_Score_Lbl.Size = new System.Drawing.Size(224, 72);
            this.UI_Score_Lbl.TabIndex = 0;
            this.UI_Score_Lbl.Text = "Score:";
            this.UI_Score_Lbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // UI_ScoreCount_Lbl
            // 
            this.UI_ScoreCount_Lbl.Font = new System.Drawing.Font("OCR A Extended", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UI_ScoreCount_Lbl.Location = new System.Drawing.Point(242, 34);
            this.UI_ScoreCount_Lbl.Name = "UI_ScoreCount_Lbl";
            this.UI_ScoreCount_Lbl.Size = new System.Drawing.Size(242, 72);
            this.UI_ScoreCount_Lbl.TabIndex = 1;
            this.UI_ScoreCount_Lbl.Text = "0";
            this.UI_ScoreCount_Lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ScoreModelessDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 146);
            this.Controls.Add(this.UI_ScoreCount_Lbl);
            this.Controls.Add(this.UI_Score_Lbl);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ScoreModelessDialog";
            this.Text = "Score";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ScoreModelessDialog_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label UI_Score_Lbl;
        private System.Windows.Forms.Label UI_ScoreCount_Lbl;
    }
}
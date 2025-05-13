namespace Ball_Game
{
    partial class AnimationModeless_Dialog
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
            this.UI_AnimateSpeed_Tbar = new System.Windows.Forms.TrackBar();
            this.UI_10ms_Lbl = new System.Windows.Forms.Label();
            this.UI_200ms_Lbl = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.UI_AnimateSpeed_Tbar)).BeginInit();
            this.SuspendLayout();
            // 
            // UI_AnimateSpeed_Tbar
            // 
            this.UI_AnimateSpeed_Tbar.LargeChange = 2;
            this.UI_AnimateSpeed_Tbar.Location = new System.Drawing.Point(12, 12);
            this.UI_AnimateSpeed_Tbar.Maximum = 20;
            this.UI_AnimateSpeed_Tbar.Minimum = 1;
            this.UI_AnimateSpeed_Tbar.Name = "UI_AnimateSpeed_Tbar";
            this.UI_AnimateSpeed_Tbar.Size = new System.Drawing.Size(369, 45);
            this.UI_AnimateSpeed_Tbar.TabIndex = 0;
            this.UI_AnimateSpeed_Tbar.Value = 1;
            // 
            // UI_10ms_Lbl
            // 
            this.UI_10ms_Lbl.AutoSize = true;
            this.UI_10ms_Lbl.Font = new System.Drawing.Font("OCR A Extended", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UI_10ms_Lbl.Location = new System.Drawing.Point(9, 44);
            this.UI_10ms_Lbl.Name = "UI_10ms_Lbl";
            this.UI_10ms_Lbl.Size = new System.Drawing.Size(40, 12);
            this.UI_10ms_Lbl.TabIndex = 1;
            this.UI_10ms_Lbl.Text = "10 ms";
            // 
            // UI_200ms_Lbl
            // 
            this.UI_200ms_Lbl.Font = new System.Drawing.Font("OCR A Extended", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UI_200ms_Lbl.Location = new System.Drawing.Point(334, 45);
            this.UI_200ms_Lbl.Name = "UI_200ms_Lbl";
            this.UI_200ms_Lbl.Size = new System.Drawing.Size(57, 19);
            this.UI_200ms_Lbl.TabIndex = 2;
            this.UI_200ms_Lbl.Text = "200 ms";
            this.UI_200ms_Lbl.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // AnimationModeless_Dialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(393, 73);
            this.Controls.Add(this.UI_200ms_Lbl);
            this.Controls.Add(this.UI_10ms_Lbl);
            this.Controls.Add(this.UI_AnimateSpeed_Tbar);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AnimationModeless_Dialog";
            this.Text = "Animation Speed";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AnimationModeless_Dialog_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.UI_AnimateSpeed_Tbar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar UI_AnimateSpeed_Tbar;
        private System.Windows.Forms.Label UI_10ms_Lbl;
        private System.Windows.Forms.Label UI_200ms_Lbl;
    }
}
namespace Ball_Game
{
    partial class DifficultyModalDialog
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
            this.UI_Difficulty_Gbx = new System.Windows.Forms.GroupBox();
            this.UI_Hard_Rbtn = new System.Windows.Forms.RadioButton();
            this.UI_Medium_Rbtn = new System.Windows.Forms.RadioButton();
            this.UI_Easy_Rbtn = new System.Windows.Forms.RadioButton();
            this.UI_OK_Btn = new System.Windows.Forms.Button();
            this.UI_Cancel_Btn = new System.Windows.Forms.Button();
            this.UI_Difficulty_Gbx.SuspendLayout();
            this.SuspendLayout();
            // 
            // UI_Difficulty_Gbx
            // 
            this.UI_Difficulty_Gbx.Controls.Add(this.UI_Hard_Rbtn);
            this.UI_Difficulty_Gbx.Controls.Add(this.UI_Medium_Rbtn);
            this.UI_Difficulty_Gbx.Controls.Add(this.UI_Easy_Rbtn);
            this.UI_Difficulty_Gbx.Font = new System.Drawing.Font("OCR A Extended", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UI_Difficulty_Gbx.Location = new System.Drawing.Point(18, 12);
            this.UI_Difficulty_Gbx.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.UI_Difficulty_Gbx.Name = "UI_Difficulty_Gbx";
            this.UI_Difficulty_Gbx.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.UI_Difficulty_Gbx.Size = new System.Drawing.Size(234, 95);
            this.UI_Difficulty_Gbx.TabIndex = 0;
            this.UI_Difficulty_Gbx.TabStop = false;
            this.UI_Difficulty_Gbx.Text = "Difficulty";
            // 
            // UI_Hard_Rbtn
            // 
            this.UI_Hard_Rbtn.AutoSize = true;
            this.UI_Hard_Rbtn.Font = new System.Drawing.Font("OCR A Extended", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UI_Hard_Rbtn.Location = new System.Drawing.Point(9, 65);
            this.UI_Hard_Rbtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.UI_Hard_Rbtn.Name = "UI_Hard_Rbtn";
            this.UI_Hard_Rbtn.Size = new System.Drawing.Size(51, 16);
            this.UI_Hard_Rbtn.TabIndex = 2;
            this.UI_Hard_Rbtn.Text = "Hard";
            this.UI_Hard_Rbtn.UseVisualStyleBackColor = true;
            // 
            // UI_Medium_Rbtn
            // 
            this.UI_Medium_Rbtn.AutoSize = true;
            this.UI_Medium_Rbtn.Font = new System.Drawing.Font("OCR A Extended", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UI_Medium_Rbtn.Location = new System.Drawing.Point(9, 42);
            this.UI_Medium_Rbtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.UI_Medium_Rbtn.Name = "UI_Medium_Rbtn";
            this.UI_Medium_Rbtn.Size = new System.Drawing.Size(65, 16);
            this.UI_Medium_Rbtn.TabIndex = 1;
            this.UI_Medium_Rbtn.Text = "Medium";
            this.UI_Medium_Rbtn.UseVisualStyleBackColor = true;
            // 
            // UI_Easy_Rbtn
            // 
            this.UI_Easy_Rbtn.AutoSize = true;
            this.UI_Easy_Rbtn.Checked = true;
            this.UI_Easy_Rbtn.Font = new System.Drawing.Font("OCR A Extended", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UI_Easy_Rbtn.Location = new System.Drawing.Point(9, 19);
            this.UI_Easy_Rbtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.UI_Easy_Rbtn.Name = "UI_Easy_Rbtn";
            this.UI_Easy_Rbtn.Size = new System.Drawing.Size(51, 16);
            this.UI_Easy_Rbtn.TabIndex = 0;
            this.UI_Easy_Rbtn.TabStop = true;
            this.UI_Easy_Rbtn.Text = "Easy";
            this.UI_Easy_Rbtn.UseVisualStyleBackColor = true;
            // 
            // UI_OK_Btn
            // 
            this.UI_OK_Btn.Font = new System.Drawing.Font("OCR A Extended", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UI_OK_Btn.Location = new System.Drawing.Point(18, 113);
            this.UI_OK_Btn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.UI_OK_Btn.Name = "UI_OK_Btn";
            this.UI_OK_Btn.Size = new System.Drawing.Size(112, 23);
            this.UI_OK_Btn.TabIndex = 1;
            this.UI_OK_Btn.Text = "OK";
            this.UI_OK_Btn.UseVisualStyleBackColor = true;
            this.UI_OK_Btn.Click += new System.EventHandler(this.UI_OK_Btn_Click);
            // 
            // UI_Cancel_Btn
            // 
            this.UI_Cancel_Btn.Font = new System.Drawing.Font("OCR A Extended", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UI_Cancel_Btn.Location = new System.Drawing.Point(140, 113);
            this.UI_Cancel_Btn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.UI_Cancel_Btn.Name = "UI_Cancel_Btn";
            this.UI_Cancel_Btn.Size = new System.Drawing.Size(112, 23);
            this.UI_Cancel_Btn.TabIndex = 2;
            this.UI_Cancel_Btn.Text = "Cancel";
            this.UI_Cancel_Btn.UseVisualStyleBackColor = true;
            this.UI_Cancel_Btn.Click += new System.EventHandler(this.UI_Cancel_Btn_Click);
            // 
            // DifficultyModalDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(273, 148);
            this.ControlBox = false;
            this.Controls.Add(this.UI_Cancel_Btn);
            this.Controls.Add(this.UI_OK_Btn);
            this.Controls.Add(this.UI_Difficulty_Gbx);
            this.Font = new System.Drawing.Font("ROG Fonts", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "DifficultyModalDialog";
            this.Text = "Select Difficulty";
            this.UI_Difficulty_Gbx.ResumeLayout(false);
            this.UI_Difficulty_Gbx.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox UI_Difficulty_Gbx;
        private System.Windows.Forms.RadioButton UI_Hard_Rbtn;
        private System.Windows.Forms.RadioButton UI_Medium_Rbtn;
        private System.Windows.Forms.RadioButton UI_Easy_Rbtn;
        private System.Windows.Forms.Button UI_OK_Btn;
        private System.Windows.Forms.Button UI_Cancel_Btn;
    }
}
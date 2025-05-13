using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ball_Game
{
    public partial class DifficultyModalDialog : Form
    {
        public DifficultyModalDialog()
        {
            InitializeComponent();
        }

        private void UI_OK_Btn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void UI_Cancel_Btn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// GetDifficulty() sends difficulty info from modal dialog to main form
        /// </summary>
        /// <returns></returns>
        public int GetDifficulty()
        {
            if (UI_Easy_Rbtn.Checked)
                return 3;
            else if (UI_Medium_Rbtn.Checked)
                return 4;
            else
                return 5;

        }
    }
}

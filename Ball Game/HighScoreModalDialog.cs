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
    public partial class HighScoreModalDialog : Form
    {
        public HighScoreModalDialog()
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
        /// GetName() gets the name in the textbox after a game has been completed, which is then 
        /// stored in player's information struct
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            if (UI_Name_Tbx.Text.Length <= 0)
                MessageBox.Show("Please input a name!");
            string name = UI_Name_Tbx.Text;
            UI_Name_Tbx.Clear();
            return name;
        }


    }
}

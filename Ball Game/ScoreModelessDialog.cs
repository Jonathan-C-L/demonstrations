using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ball_Game
{
    public delegate void delScoreClosing();

    public partial class ScoreModelessDialog : Form
    {
        /***************************************Delegates**********************************************/
        public delScoreClosing delScoreClosing = null;

        public ScoreModelessDialog()
        {
            InitializeComponent();
        }

        private void ScoreModelessDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            //verify if user is closing form
            if(e.CloseReason == CloseReason.UserClosing)
            {
                //invoke delegate when form is closing
                if (delScoreClosing != null)
                    delScoreClosing();

                //prevent closing
                e.Cancel = true;

                //hide modeless dialog instead
                Hide();
            }
        }

        /// <summary>
        /// DisplayScore() takes the points scored from the game and adds it to the current score value
        /// </summary>
        /// <param name="score"></param>
        public void DisplayScore(int score)
        {
            int.TryParse(UI_ScoreCount_Lbl.Text, out int currentScore);
            UI_ScoreCount_Lbl.Text = (currentScore + score).ToString();
        }
        /// <summary>
        /// GetFinalScore() sends the final score to the main form for it to be saved in a player's total points
        /// </summary>
        /// <returns></returns>
        public int GetFinalScore()
        {
            int.TryParse(UI_ScoreCount_Lbl.Text, out int scoreTotal);
            UI_ScoreCount_Lbl.Text = "0";
            return scoreTotal;
        }
    }
}

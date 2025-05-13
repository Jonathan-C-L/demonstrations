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
    public delegate void delAnimationClosing();

    public partial class AnimationModeless_Dialog : Form
    {
        public delAnimationClosing delAnimationClosing = null; 

        public AnimationModeless_Dialog()
        {
            InitializeComponent();
        }

        private void AnimationModeless_Dialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            //verify if user is closing form
            if (e.CloseReason == CloseReason.UserClosing)
            {
                //invoke delegate when form is closing
                if (delAnimationClosing != null)
                    delAnimationClosing();

                //prevent closing
                e.Cancel = true;

                //hide modeless dialog instead
                Hide();
            }
        }
        /// <summary>
        /// GetSpeed() sends the main form the value of the animation speed trackbar
        /// </summary>
        /// <returns></returns>
        public int GetSpeed()
        {
            return UI_AnimateSpeed_Tbar.Value;
        }
    }
}

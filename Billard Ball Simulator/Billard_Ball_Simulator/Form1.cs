/* CMPE 2300 - Object Oriented Programming
 * 
 * Author: Jonathan Le
 * 
 * Date: Nov. 17, 2025
 * 
 * Purpose: this application will be a billard ball simulator, that uses
 * basics in classes, properties, methods, and composition
 */
using GDIDrawer;
using System.Text;

namespace Billard_Ball_Simulator
{
    public partial class Form1 : Form
    {
        Table table = null;
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        bool runningFlag = false;
        int numBalls = 1;

        public Form1()
        {
            InitializeComponent();
 
            UI_AddTable_Btn.Click += UI_AddTable_Btn_Click;
            UI_Timer1.Tick += UI_Timer1_Tick;
            UI_Radius_Rbtn.CheckedChanged += UI_Rbtn_CheckedChanged;
            UI_Hits_Rbtn.CheckedChanged += UI_Rbtn_CheckedChanged;
            UI_TotalHits_Rbtn.CheckedChanged += UI_Rbtn_CheckedChanged;
            UI_AddTable_Btn.MouseWheel += UI_AddTable_Btn_MouseWheel;
            UI_FrictionInput_Lbl.MouseWheel += UI_FrictionInput_Lbl_MouseWheel;
            UI_Radius_Rbtn.Checked = true;
            UI_Timer1.Interval = 35; // 35ms interval
            UI_Timer1.Enabled = true; // turning on timer
            UI_Table_Dgv.DataSource = null;
            UI_AddTable_Btn.Text = $"New Table [{numBalls}]";
            UI_FrictionInput_Lbl.Text = $"{Ball.Friction}";
        }

        private void UI_FrictionInput_Lbl_MouseWheel(object? sender, MouseEventArgs e)
        {
            //positive delta - scroll up
            if (e.Delta > 0)
            {
                if (Ball.Friction < 0.999)
                {
                    //increment
                    Ball.Friction += 0.001f;
                }
            }
            //negative delta - scroll down
            else if (e.Delta < 0.001)
            {
                if (Ball.Friction > 0.001f)
                {
                    //decrement
                    Ball.Friction -= 0.001f;
                }
            }
            UI_FrictionInput_Lbl.Text = Ball.Friction.ToString("f3");
        }

        private void UI_AddTable_Btn_MouseWheel(object? sender, MouseEventArgs e)
        {
            //positive delta - scroll up
            if (e.Delta > 0)
            {
                if (numBalls < 15)
                {
                    //increment
                    numBalls += 1;
                }
            }
            //negative delta - scroll down
            else if (e.Delta < 0)
            {
                if (numBalls > 1)
                {
                    //decrement
                    numBalls -= 1;
                }
            }
            UI_AddTable_Btn.Text = $"New Table [{numBalls}]";
        }

        private void UI_Rbtn_CheckedChanged(object? sender, EventArgs e)
        {
            if (table is null) // escape if there is no play table
                return;
            UpdateGridView();
        }

        private void UI_Timer1_Tick(object? sender, EventArgs e)
        {
            if (table is null) // escape if there is no play table
                return;
            table.ShowTable(); // render the table

            if (!table.Running) // last update when balls stop moving
                UpdateGridView();
        }
        /// <summary>
        /// UpdateGridView() resets the binding of the DataGridView and displays the Radius, Hits, and TotalHits information
        /// </summary>
        private void UpdateGridView()
        {
            UI_Table_Dgv.DataSource = null;
            List<Ball> ballsCopy = table.Balls;
            if (UI_Radius_Rbtn.Checked) // default compare is by radius
                ballsCopy.Sort();
            if (UI_Hits_Rbtn.Checked)
                ballsCopy.Sort(Ball.CompareByHits); // compare by hits
            if (UI_TotalHits_Rbtn.Checked)
                ballsCopy.Sort(Ball.CompareByTotalHits); // compare by total hits
            UI_Table_Dgv.DataSource = ballsCopy;
            UI_Table_Dgv.ResetBindings();

            UI_Table_Dgv.RowHeadersVisible = false; // row headers off
            // turn off BallColor, Center, and Velocity from view
            UI_Table_Dgv.Columns[0].Visible = false;
            UI_Table_Dgv.Columns[1].Visible = false;
            UI_Table_Dgv.Columns[5].Visible = false;

            // assigning back color of row in radius column
            foreach (DataGridViewRow row in UI_Table_Dgv.Rows)
            {
                if (row.Cells["BallColor"].Value is Color color)
                    row.Cells["Radius"].Style.BackColor = color;
            }

            UI_Table_Dgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
        }

        private void UI_AddTable_Btn_Click(object? sender, EventArgs e)
        {
            if (table is not null)
                table.Pool.Close();
            table = new Table();
            table.MakeTable(600, 800, numBalls);
            table.Pool.Position = new Point(this.Location.X + this.Width, this.Location.Y);
            UpdateGridView();
        }
    }
}

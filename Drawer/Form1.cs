/* Drawing Application
 * 
 * Author: Jonathan Le
 * 
 * Purpose: drawer application that can undo lines, segments, change opacity, 
 * color, and display drawing information
 * 
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GDIDrawer;
using System.Diagnostics;
using System.Drawing.Text;
using System.Threading;

namespace Drawer
{
    public partial class Form1 : Form
    {
        /*******************************************Class Level items*****************************************************/
        struct LineSeg
        {
            public Point start, end;
            public byte thickness;
            public float alpha;
            public Color color;
        };
        Queue<LineSeg> lineQueue;
        Stack<Queue<LineSeg>> lineStack = new Stack<Queue<LineSeg>>();
        CDrawer canvas = new CDrawer(1024, 768);

        /*******************************************Class Level Variables*****************************************************/
        bool drawFlag = false;
        Point lastP = new Point();

        public Form1()
        {
            InitializeComponent();
            MouseWheel += UI_MouseWheel_Scroll;
            UI_ColorDiag.Color = Color.White;
        }

        /*******************************************Event Handlers*****************************************************/
        private void UI_Timer_Tick(object sender, EventArgs e)
        {
            //getting alpha value from alpha label
            float.TryParse(UI_Alpha_Lbl.Text.Substring(7), out float a);

            //start drawing
            if (canvas.GetLastMouseLeftClick(out Point leftP) && !drawFlag)
            {
                Console.WriteLine("Start drawing");
                lineQueue = new Queue<LineSeg>();
                lastP = leftP;
                //turn on drawing
                drawFlag = true;
            }
            //continue drawing
            if (canvas.GetLastMousePosition(out Point p) && drawFlag)
            {
                Console.WriteLine("Drawing");
                //new line segment being created
                LineSeg seg = new LineSeg
                {
                    start = lastP,
                    end = p,
                    thickness = (byte)UI_Thickness_Tbar.Value,
                    alpha = a,
                    color = UI_ColorDiag.Color
                };
                //drawing the line
                canvas.AddLine(seg.start.X, seg.start.Y, seg.end.X, seg.end.Y, Color.FromArgb((int)seg.alpha, seg.color), seg.thickness);
                //add segment into the queue
                lineQueue.Enqueue(seg);
                lastP = p;
            }
            //stop drawing
            if (canvas.GetLastMouseRightClick(out Point rightP) && drawFlag)
            {
                Console.WriteLine("Drawing stopped");
                lastP = rightP;
                //add line to the stack
                lineStack.Push(lineQueue);
                //turn off drawing
                drawFlag = false;
            }
            UpdateData();
        }
        private void UI_MouseWheel_Scroll(object sender, MouseEventArgs e)
        {
            //getting alpha from the label
            float.TryParse(UI_Alpha_Lbl.Text.Substring(7), out float a);

            //positive delta
            if (e.Delta > 0)
            {
                if(a < 255)
                {
                    //increment
                    a += 5;
                    UI_Alpha_Lbl.Text = $"Alpha: {a}";
                }
            }
            //negative delta
            else if (e.Delta < 0)
            {
                if (a > 0)
                {
                    //decrement
                    a -= 5;
                    UI_Alpha_Lbl.Text = $"Alpha: {a}";
                }
            }

        }
        private void UI_UndoLine_Btn_Click(object sender, EventArgs e)
        {
            //remove the last line drawn
            lineStack.Pop();
            RenderAll();

            //re-render all the lines without the last line
            drawFlag = false;
            UpdateData();

        }

        private void UI_UndoSeg_Btn_Click(object sender, EventArgs e)
        {
            //remove the last seg
            Queue<LineSeg> transQ = lineStack.Pop();
            Queue<LineSeg> tempQ = new Queue<LineSeg>();
            while ( transQ.Count - 1 > 0)
            {
                tempQ.Enqueue(transQ.Dequeue());
            }
            lineStack.Push(tempQ);
            UpdateData();

            RenderAll();
        }

        private void UI_Colour_Btn_Click(object sender, EventArgs e)
        {
            //show color to change line color
            UI_ColorDiag.ShowDialog();
        }
        private void UI_Thickness_Scroll(object sender, EventArgs e)
        {
            UpdateThickness();
        }
        /************************************************Methods********************************************************/
        /// <summary>
        /// UpdateData() gets a count of the number of segs from the stack items
        /// </summary>
        private void UpdateData()
        {
            int segCount = 0;
            foreach (Queue<LineSeg> line in lineStack)
            {
                foreach (LineSeg seg in line)
                {
                    segCount++;
                }
            }
            UI_InfoDisplay_Lbl.Text = $"{lineStack.Count} lines, {segCount} total segments";
        }
        /// <summary>
        /// UpdateThickness() uses the Tbar control to update the text
        /// </summary>
        private void UpdateThickness()
        {
            UI_Thickness_Lbl.Text = $"Thickness: {UI_Thickness_Tbar.Value}";
        }
        /// <summary>
        /// RenderAll() iterates through segs in the stack and redraws it
        /// </summary>
        private void RenderAll()
        {
            Console.WriteLine("RenderAll");
            //clear canvas
            canvas.Clear();
            //re-draw all the lines
            foreach (Queue<LineSeg> line in lineStack)
            {
                foreach (LineSeg seg in line)
                {
                    canvas.AddLine(seg.start.X, seg.start.Y, seg.end.X, seg.end.Y, Color.FromArgb((int)seg.alpha, seg.color), seg.thickness);

                }
            }
        }


    }
}

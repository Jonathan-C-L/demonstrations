/* CMPE 2300 - Object Oriented Programming
 * 
 * Author: Jonathan Le
 * 
 * Date: Nov. 17, 2025
 * 
 * Purpose: Table class that will store all the information regarding an instance of a pool game.
 *          Information includes Ball objects, mouse location, and the graphic display for the table
 */
using GDIDrawer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Billard_Ball_Simulator
{
    internal class Table
    {
        // data members
        public CDrawer? Pool { get; private set; } = null; // initialized to null
        private List<Ball> _balls = new List<Ball>();
        private Vector2 _mouseLocation;
        private Ball _cueBall = null;

        // properties
        public List<Ball> Balls { get { return new List<Ball>(_balls); } }
        public bool Running 
        {
            get
            {
                foreach (Ball b in _balls)
                {
                    if(b.Velocity != Vector2.Zero) // if any ball is still moving, animation is still running
                        return true;
                }
                return false;
            }
        }
        // constructor does nothing
        public Table() 
        {
            
        }
        /// <summary>
        /// MakeTable() creates a new CDrawer window at a specified width, height, and number of balls
        /// </summary>
        /// <param name="width">int width of the window</param>
        /// <param name="height">int height of the window</param>
        /// <param name="numBalls">int number of balls in the window</param>
        public void MakeTable(int width, int height, int numBalls)
        {
            Pool = new CDrawer(width, height, false, true);
            Pool.MouseMoveScaled += Pool_MouseMoveScaled;
            Pool.MouseLeftClickScaled += Pool_MouseLeftClickScaled;
            this.MakeBalls(numBalls);
            this.ShowTable();
        }
        /// <summary>
        /// MakeBall() clears the current list of balls and creates a new list of balls with the number of balls specified by the user
        /// </summary>
        /// <param name="balls">int number of balls to be added into the balls list</param>
        public void MakeBalls(int balls)
        {
            _balls.Clear(); // clear all current balls
            for (int count = 0; count < balls; count++)
            {
                Ball newBall = new Ball(Pool, RandColor.GetColor());
                // add if it doesn't overlap
                if (!_balls.Contains(newBall))
                    _balls.Add(newBall);
            }
            _cueBall = null; // reset cue ball
            while (_cueBall is null)
            {
                Ball newCueBall = new Ball(Pool);
                // if the cue ball doesn't overlap with any other ball, add it 
                if (!_balls.Contains(newCueBall))
                {
                    _balls.Add(newCueBall); 
                    _cueBall = newCueBall;
                }
            }
        }
        /// <summary>
        /// ShowTable() animates the table by clearing and rendering each ball, and guide line if it all balls have stopped
        /// </summary>
        public void ShowTable()
        {
            if (Pool is null)
                return;
            Pool.Clear();
            foreach (Ball b in Balls) 
            {
                b.Move(Pool, Balls);
                b.Show(Pool);
            }
            if (!Running)
                Pool.AddLine((int)_cueBall.Center.X, (int)_cueBall.Center.Y, (int)_mouseLocation.X, (int)_mouseLocation.Y, Color.Yellow);
            Pool.Render();
        }
        private void Pool_MouseLeftClickScaled(Point pos, CDrawer dr)
        {
            // reset the hits on each ball
            _balls.ForEach(b => b.ResetHits());
            
            // the velocity and direction for the "shot" with a speed of the yellow line distance in the opposite direction
            Vector2 normalizedVector = Vector2.Normalize(_mouseLocation - _cueBall.Center) * -(Vector2.Distance(_mouseLocation, _cueBall.Center) /15);
            // setting the velocity for the cue ball
            _cueBall.SetVelocity(normalizedVector);
        }
        private void Pool_MouseMoveScaled(Point pos, CDrawer dr)
        {
            _mouseLocation = new Vector2(pos.X, pos.Y); // assign mouse mouse position to current mouse location
            if(!Running)
                ShowTable();
        }
    }
}

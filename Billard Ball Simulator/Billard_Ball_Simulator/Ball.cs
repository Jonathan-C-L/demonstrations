/* CMPE 2300 - Object Oriented Programming
 * 
 * Author: Jonathan Le
 * 
 * Date: Nov. 17, 2025
 * 
 * Purpose: Ball class that will store all the information regarding a specific Ball within a game
 *          of pool. Information stored will be the ball position, velocity, radius, hits, total hits, and friction
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
    internal class Ball : IComparable<Ball>
    {
        // members
        private Vector2 _center;
        private Vector2 _velocity;
        static Random rnd = new Random();

        // properties
        public Vector2 Center { get => _center; }
        public Vector2 Velocity { get => _velocity; }
        public int Radius { get; private set; }
        public int Hits { get; private set; }
        public int TotalHits { get; private set; }
        public Color BallColor { get; private set; }
        public static float Friction { get; set; } = 0.991f; // default value for friction

        public Ball(CDrawer drawer, Color color)
        {
            BallColor = color;
            Radius = rnd.Next(20, 51);
            // random center value
            int randX = rnd.Next(Radius, (drawer.ScaledWidth - Radius) + 1);
            int randY = rnd.Next(Radius, (drawer.ScaledWidth - Radius) + 1);
            _center = new Vector2(randX, randY);
        }
        public Ball(CDrawer drawer)
        {
            BallColor = Color.White;
            Radius = 30;
            // random center value
            int randX = rnd.Next(Radius, (drawer.ScaledWidth - Radius) + 1);
            int randY = rnd.Next(Radius, (drawer.ScaledWidth - Radius) + 1);
            _center = new Vector2(randX, randY);
        }
        /// <summary>
        /// ResetHits() sets Hits back to 0
        /// </summary>
        public void ResetHits() 
        {
            Hits = 0;
        }
        /// <summary>
        /// SetVelocity() sets the velocity of the Ball object
        /// </summary>
        /// <param name="vel"></param>
        public void SetVelocity(Vector2 vel)
        {
            _velocity = vel;
        }
        /// <summary>
        /// Equals() override checks if the current Ball object overlaps with another Ball object
        /// </summary>
        /// <param name="obj">Object of type Ball</param>
        /// <returns>True if the Ball objects overlap, false if not</returns>
        public override bool Equals(object? obj)
        {
            // false if it's not type Ball
            if(obj is not Ball ball)
                return false;
            // hypotenuse is less than the to radii means there's an intersection
            int x = (int)Math.Abs(Center.X - ball.Center.X);
            int y = (int)Math.Abs(Center.Y - ball.Center.Y);
            int hyp = (int)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            if (hyp >= (Radius + ball.Radius))
                return false;
            return true;
        }
        /// <summary>
        /// GetHashCode() override returns 1 as a placeholder for the Equals() override to work
        /// </summary>
        /// <returns>1</returns>
        public override int GetHashCode()
        {
            return 1;
        }
        /// <summary>
        /// ToString() returns a string combining radius and hits
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return $"{Radius}:{Hits} ";
        }
        /// <summary>
        /// CompareTo() for the IComparable interface; compares radii in descending order 
        /// </summary>
        /// <param name="ball">Ball object to compare</param>
        /// <returns>1 (ref > comparison); 0 (ref = comparison); -1 (ref < comparison)</returns>
        public int CompareTo(Ball? ball)
        {
            return ball.Radius.CompareTo(Radius);
        }
        /// <summary>
        /// CompareTo() compares the Hits between two Ball objects in descending order
        /// </summary>
        /// <param name="ball1">Ball object 1</param>
        /// <param name="ball2">Ball object 2</param>
        /// <returns>1 (ref > comparison); 0 (ref = comparison); -1 (ref < comparison)</returns>
        /// <exception cref="ArgumentNullException">Null exception</exception>
        public static int CompareByHits(Ball? ball1, Ball? ball2)
        {
            if (ball1 is null || ball2 is null)
                throw new ArgumentNullException($"Ball:CompareTo: arg1 = {nameof(ball1)} | arg2 = {nameof(ball2)} - is null");
            return ball2.Hits.CompareTo(ball1.Hits);
        }
        /// <summary>
        /// CompareTo() compares the TotalHits between two Ball objects in descending order
        /// </summary>
        /// <param name="ball1">Ball object 1</param>
        /// <param name="ball2">Ball object 2</param>
        /// <returns>1 (ref > comparison); 0 (ref = comparison); -1 (ref < comparison)</returns>
        /// <exception cref="ArgumentNullException">Null exception</exception>
        public static int CompareByTotalHits(Ball? ball1, Ball? ball2)
        {
            if (ball1 is null || ball2 is null)
                throw new ArgumentNullException($"Ball:CompareTo: arg1 = {nameof(ball1)} | arg2 = {nameof(ball2)} - is null");
            return ball2.TotalHits.CompareTo(ball1.TotalHits);
        }
        /// <summary>
        /// Show() renders the ball, differing between the que ball and a regular ball, with the text on radii and hits for that ball
        /// </summary>
        /// <param name="drawer">Provided CDrawer object</param>
        public void Show(CDrawer drawer)
        {
            // cue ball is white -> yellow border
            if (BallColor == Color.White)
                drawer.AddCenteredEllipse((int)Center.X, (int)Center.Y, Radius * 2, Radius * 2, BallColor, 2, Color.Yellow);
            else // otherwise, just a regular ball
                drawer.AddCenteredEllipse((int)Center.X, (int)Center.Y, Radius * 2, Radius * 2, BallColor);

            // render text for all balls
            drawer.AddText(this.ToString(), 8, (int)Center.X - Radius/2, (int)Center.Y - Radius/2, Radius, Radius, Color.Black);
        }
        public void Move(CDrawer drawer, List<Ball> balls)
        {
            _velocity *= Friction; // deccelerate the ball for the next iteration (reduce the distance the next ball is rendered at)
            // if the ball velocity is less than 0.1f, stop the ball in it's current location (velocity = 0)
            if (_velocity.LengthSquared() < 0.1f)
            {
                _velocity = Vector2.Zero;
                return;
            }

            // wall bounces
            if ((Center.X - Radius) < 0 || (Center.X + Radius) > drawer.ScaledWidth)
                _velocity.X *= -1; // inverse x velocity on left and right walls 
            if ((Center.Y - Radius) < 0 || (Center.Y + Radius) > drawer.ScaledHeight)
                _velocity.Y *= -1; // inverse x velocity on top and bottom walls 

            // setting the new center
            _center += _velocity;

            // check for collisions
            foreach (Ball b in balls) 
            {
                if(b == this) // if it is the same ball
                    continue;
                if(b.Equals(this))
                    ProcessCollision(b); // add collisions if target ball intersects with current ball
            }
        }
        private void ProcessCollision(Ball tar)
        {
            Vector2 dist = tar._center - _center; // Get Collision Vector
            Vector2 myNorm = Vector2.Normalize(tar._center - _center); // Normalize for invoking ball
            Vector2 targetNorm = Vector2.Normalize(_center - tar._center); // Normalize for target ball

            // Calculate Radius weighted velocity vector
            //Vector2 CMVelocity = Vector2.Add(Vector2.Multiply((float)_iRadius / (_iRadius + tar._iRadius), _v), Vector2.Multiply((float)tar._iRadius / (_iRadius + tar._iRadius), tar._v));
            Vector2 CMVelocity = (_velocity * ((float)Radius / (Radius + tar.Radius)) + (tar._velocity * ((float)tar.Radius / (Radius + tar.Radius))));

            // Process invoking ball
            _velocity -= CMVelocity;// Vector2.Subtract(_v, CMVelocity);
            _velocity = Vector2.Reflect(_velocity, myNorm); // perform "bounce"
            _velocity += CMVelocity;// Vector2.Add(_v, CMVelocity);
            Hits++;
            TotalHits++;

            // Process target ball
            tar._velocity -= CMVelocity;
            tar._velocity = Vector2.Reflect(tar._velocity, targetNorm); // perform bounce
            tar._velocity += CMVelocity;// Vector2.Add(tar._v, CMVelocity);
            tar.Hits++;
            tar.TotalHits++;

            // "Fix" collision if balls overlapped - could apply weighted adjustment shift between both balls
            //       but here we just move the target ball over on collision vector so it doesn't overlap
            //tar._center = Vector2.Add(tar._center, Vector2.Multiply((float)((_iRadius + tar._iRadius - dist.Length()) / (_iRadius + tar._iRadius)), dist));
            tar._center += dist * (float)((Radius + tar.Radius - dist.Length()) / (Radius + tar.Radius));
        }
    }
}

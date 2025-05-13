/* Pong
 * 
 * Author: Jonathan Le
 * 
 * Date: November 1, 2024
 * 
 * Purpose: to play the 'Pong game', where the user controls a paddle on one 
 * edge of the window, with the ball bouncing against the remaining walls. The
 * user interacts with their paddle using the mouse to control the vertical position of the paddle.
 * The user will be prompted to click an option to play again or quit the game.
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDIDrawer;
using System.Drawing;
using System.Xml;
using System.Threading;
using System.Drawing.Imaging;

namespace Pong
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.CursorLeft = Console.WindowWidth / 2 - 12;
            Console.WriteLine("Lab 2 - Jonathan Le");

            //condition for do while loop
            bool play = true;
            //creating a GDIDrawer window with continuous window off 
            CDrawer Canvas = new CDrawer(800, 600, false);
            Canvas.Scale = 5;

            do
            {
                //randomized variables for starting ball placement and initial values
                Random randomPosition = new Random();
                Point userClick = new Point();
                int ballX = randomPosition.Next(2, 40);
                int ballY = randomPosition.Next(2, 118);
                int ballXVelocity = 1, ballYVelocity = 1;
                bool loop = false, start = true;
                int userScore = 0;
                int ballSpeed = 20;

                //back buffer top and bottom borders
                for (int borderX = 0; borderX < 160; ++borderX)
                {
                    Canvas.Render();
                    Thread.Sleep(0);
                    Canvas.SetBBScaledPixel(borderX, 0, Color.Aquamarine);
                    Canvas.SetBBScaledPixel(borderX, 119, Color.Aquamarine);
                }
                //back buffer left border
                for (int borderY = 0; borderY < 120; ++borderY)
                {
                    Canvas.Render();
                    Thread.Sleep(0);
                    Canvas.SetBBScaledPixel(159, borderY, Color.Aquamarine);
                }

                //waiting for user click to start pong
                while (start)
                {
                    Canvas.Render();
                    Thread.Sleep(10);
                    Canvas.AddText("Click to start!", 50, Color.Gray);
                    if (Canvas.GetLastMouseLeftClickScaled(out userClick))
                    {
                        loop = true;
                        start = false;
                    }
                }

                //animation loop
                while (loop == true)
                {
                    //variables for x and y positions of the ball
                    ballX += ballXVelocity;
                    ballY += ballYVelocity;
                    Canvas.Render();

                    //ball coordinates and animation
                    Canvas.AddCenteredEllipse(ballX, ballY, 2, 2, Color.LightPink);
                    Thread.Sleep(ballSpeed);
                    Canvas.Clear();

                    Canvas.AddText($"{userScore}", 35, Color.Gray);

                    //paddle coordinates and animation
                    Point paddleMove = new Point();
                    Canvas.GetLastMousePositionScaled(out paddleMove);
                    //ensures paddle stays in the screen and can't move into wall
                    paddleMove.Y = (paddleMove.Y < 7) ? 7 : paddleMove.Y;
                    paddleMove.Y = (paddleMove.Y > 113) ? 113 : paddleMove.Y;
                    Canvas.AddLine(2, paddleMove.Y - 5, 2, paddleMove.Y + 5, Color.MediumPurple,10);
                    

                    //boundary walls - ball bounces at 90 degrees when contacting
                    switch (ballX)
                    {
                        case (158):
                        //ball hitting right boundary
                            ballXVelocity *= -1;
                            break;
                        //ball exiting = ending the game
                        case (-5):
                            loop = false;
                            Canvas.Clear();
                            break;
                    }
                    switch (ballY)
                    {
                        //ball hitting lower boundary
                        case (118):
                            ballYVelocity *= -1;
                            break;
                        //ball hitting upper boundary
                        case (2):
                            ballYVelocity *= -1;
                            break;
                    }
                    //ball bounces of paddle
                    if (ballX == 3 && ballY >= paddleMove.Y - 5 && ballY <= paddleMove.Y + 5)
                    {
                        ballXVelocity *= -1;
                        userScore++;
                        //increases speed(difficulty) after each paddle bounce
                        --ballSpeed;
                        ballSpeed = (ballSpeed < 1) ? 1 : ballSpeed;
                    }

                }

                //final score display
                Canvas.Render();
                Canvas.AddText($"Final Score: {userScore}", 30, 45, 25, 70, 50, Color.Yellow);
                Thread.Sleep(2750);

                //start vs quit options display
                Canvas.Render();
                Canvas.AddLine(55, 60, 79, 60, Color.Green, 2);
                Canvas.AddLine(55, 71, 79, 71, Color.Green, 2);
                Canvas.AddLine(55, 60, 55, 71, Color.Green, 2);
                Canvas.AddLine(79, 60, 79, 71, Color.Green, 2);
                Canvas.AddText("Play Again", 15, 47, 45, 40, 40, Color.Green);
                Canvas.AddLine(83, 60, 105, 60, Color.Red, 2);
                Canvas.AddLine(83, 71, 105, 71, Color.Red, 2);
                Canvas.AddLine(83, 60, 83, 71, Color.Red, 2);
                Canvas.AddLine(105, 60, 105, 71, Color.Red, 2);
                Canvas.AddText("Quit", 15, 74, 45, 40, 40, Color.Red);

                //loop to check for user click on outlined boxes
                while (loop == false)
                {
                    Point userChoice = new Point();
                    Thread.Sleep(0);
                    //"Play Again" creates new window and allows user to play again
                    if (Canvas.GetLastMouseLeftClickScaled(out userChoice) && userChoice.X >= 55 && userChoice.X <= 79 && userChoice.Y >= 60 && userChoice.Y <= 71)
                    {
                        Canvas.Clear();
                        play = true;
                        loop = true;
                    }
                    Thread.Sleep(0);
                    //"Quit" closes the program
                    if (Canvas.GetLastMouseLeftClickScaled(out userChoice) && userChoice.X >= 83 && userChoice.X <= 105 && userChoice.Y >= 60 && userChoice.Y <= 71)
                    {
                        Canvas.Close();
                        play = false;
                        loop = true;
                    }
                }
            }
            while (play);

            Console.Write("Press any key to exit... ");
            Console.ReadKey();
        }
    }
}

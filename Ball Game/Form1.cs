/* Ball Clearing Game
 * 
 * Author: Jonathan Le
 * 
 * Purpose: program is a ball clicking game that kills grouped same colored balls.
 * The ball falling speed can be changed while playing and the highest player score
 * of a playthrough will be stored in a txt file.
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
using System.Threading;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Ball_Game
{
    public delegate int delGetSpeed();

    public partial class Form1 : Form
    {
        /*********************************Class Level Objects/Variables*******************************/
        const int screenWidth = 800;
        const int screenHeight = 600;
        const int ballSize = 50;
        const int rowCount = screenHeight / ballSize;
        const int colCount = screenWidth / ballSize;

        private enum BallState
        {
            Alive = 0,
            Dead = 1
        }

        private struct Ball
        {
            public Color color;
            public BallState state;
        }
        private struct Player
        {
            public string name;
            public int scoreTotal;
        }

        CDrawer canvas = new CDrawer(screenWidth, screenHeight);
        Ball[,] balls = new Ball[colCount, rowCount];

        //storing player data per playthrough and saving the highest one in a txt file
        List<Player> players = new List<Player>();  


        ScoreModelessDialog score = null;
        AnimationModeless_Dialog animation = null;

        /***************************************Event Handlers************************************/
        public Form1()
        {
            InitializeComponent();

            //create modeless dialogs in the constructor for score and animations to work without having to check their corresponding checkboxes
            score = new ScoreModelessDialog();
            //for unchecking on close of score dialog
            score.delScoreClosing = CallBackScoreClosing;

            animation = new AnimationModeless_Dialog();
            //for unchecking animation speed on close of animation dialog
            animation.delAnimationClosing = CallBackAnimationClosing;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            int highest = 0;
            int highestIndex = 0;

            //check for highest score and saves the name and score for a specific session
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].scoreTotal > highest)
                {
                    //store highest score and index
                    highest = players[i].scoreTotal;
                    highestIndex = i;
                }
            }
            //save only if a player has fully played through
            if(players.Count > 0)
            {
                File.WriteAllText("../../highscore.txt", $"Current highest score is by: {players[highestIndex].name} with {players[highestIndex].scoreTotal} points!");
            }
        }

        private void UI_Timer_Tick(object sender, EventArgs e)
        {

            //store the score to be incremented
            int clickScore = Pick();
            Console.WriteLine(score);
            if (clickScore > 0)
            {
                //display in score modeless dialog and will be stored there
                score.DisplayScore(clickScore);

                //end game once balls alive = 0
                if (BallsAlive() <= 0)
                {
                    //display that game is over and stop the timer and enable play again
                    canvas.AddText("Game Over", 50, Color.LightGray);

                    //allow user to input their name and verify if the score was a new highscore
                    HighScoreModalDialog highscore = new HighScoreModalDialog();
                    if(highscore.ShowDialog() == DialogResult.OK)
                    {
                        Player player = new Player();
                        player.scoreTotal = score.GetFinalScore();
                        player.name = highscore.GetName();
                        players.Add(player);
                    }
                    UI_Timer.Stop();
                    UI_Play_Btn.Enabled = true;
                }
            }
        }

        private void UI_Play_Btn_Click(object sender, EventArgs e)
        {
            DifficultyModalDialog difficultyDialog = new DifficultyModalDialog();
            //player has to select a difficulty to start the game
            if (difficultyDialog.ShowDialog() == DialogResult.OK)
            {
                Randomize(difficultyDialog.GetDifficulty());
                Display();
                UI_Play_Btn.Enabled = false;
                UI_Timer.Start();
            }

        }
        private void UI_Options_CheckChanged(object sender, EventArgs e)
        {
            //showing score
            if (UI_ShowScore_Cbx.Checked)
            {
                UI_ShowScore_Cbx.Enabled = false;
                score.Show();
            }
            //showing animation speed
            if (UI_ShowAnimationSpeed_Cbx.Checked)
            {
                UI_ShowAnimationSpeed_Cbx.Enabled = false;
                animation.Show();
            }
        }
        /**************************************Methods******************************************/
        /// <summary>
        /// Randomize() takes the difficulty information from the difficulty modal dialog and creates ball structs accordingly
        /// and assigning to corresponding 2D array positions
        /// </summary>
        /// <param name="difficulty"></param>
        private void Randomize(int difficulty)
        {
            Random rand = new Random();
            Color[] ballColors = { Color.LightBlue, Color.LightYellow, Color.LightGreen, Color.MediumPurple, Color.PaleVioletRed};

            //iterating through 2D array and assigning random colors and setting to 'Alive' state
            for (int x = 0; x < colCount; x++)
            {
                for (int y = 0; y < rowCount; y++)
                {
                    Ball ball = new Ball();
                    ball.color = ballColors[rand.Next(0, difficulty)];
                    ball.state = BallState.Alive;
                    balls[x, y] = ball;

                }
            }
        }

        /// <summary>
        /// Display() uses the info in the 2D array to display the balls in the GDIDrawer window
        /// </summary>
        private void Display()
        {
            canvas.Clear();
            for (int x = 0; x < colCount; x++)
            {
                for (int y = 0; y < rowCount; y++)
                {
                    if (balls[x, y].state == BallState.Alive)
                    {
                        //adding ball to the GDIDrawer window
                        canvas.AddEllipse((x*ballSize), (y*ballSize), ballSize, ballSize, balls[x, y].color);
                    }
                }
            }
        }

        /// <summary>
        /// BallsAlive() iterates through the ball array and checks for balls in the alive state
        /// </summary>
        /// <returns>count of balls still alive</returns>
        private int BallsAlive()
        {
            int aliveCount = 0;
            //iterate through 2D array to check for remaining alive balls
            for (int x = 0; x < colCount; x++)
            {
                for (int y = 0; y < rowCount; y++)
                {
                    if (balls[x, y].state == BallState.Alive)
                    {
                        aliveCount++;
                    }
                }
            }
            //number of "alive" elements
            return aliveCount;
        }

        /// <summary>
        /// CheckBalls() checks adjacent balls if they are the same color - if yes, turn ball to dead state
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <param name="compare"></param>
        /// <returns>count of balls killed</returns>
        private int CheckBalls(int col, int row, Color compare)
        {
            int ballsKilled = 0;

            //exit conditions
            if (row < 0 || row >= rowCount || col < 0 || col >= colCount)
                return 0;
            if (balls[col, row].state == BallState.Dead)
                return 0;
            if (balls[col, row].color != compare)
                return 0;

            Console.WriteLine("Recursion");
            //recursive call to count balls killed and to kill adjacent balls that are the same color
            balls[col, row].state = BallState.Dead;
            ballsKilled = 1;
            int ballsKilled1 = CheckBalls(col + 1, row, compare);
            int ballsKilled2 = CheckBalls(col - 1, row, compare);
            int ballsKilled3 = CheckBalls(col, row + 1, compare);
            int ballsKilled4 = CheckBalls(col, row - 1, compare);

            return ballsKilled + ballsKilled1 + ballsKilled2 + ballsKilled3 + ballsKilled4;//sum of all balls killed
        }

        /// <summary>
        /// StepDown() checks balls in the dead state and checks the positions on top of that ball if their are alive.
        /// information from higher position alive balls will be transferred to lower balls
        /// </summary>
        /// <returns>count of balls that where switched to the dead state</returns>
        private int StepDown()
        {

            int ballsDropped = 0;
            int speedVal = 0;

            //iterate through the entire 2D array
            for (int x = 0; x < colCount; x++)
            {
                for (int y = 0; y < rowCount; y++)
                {
                    //if a ball is dead and there are alive balls above, then bring the alive one down
                    if (balls[x, y].state == BallState.Dead && balls[x, (y - 1) > 0 ? y - 1 : 0].state == BallState.Alive)
                    {
                        //alive balls move down
                        balls[x, y].state = BallState.Alive;
                        balls[x, y].color = balls[x, (y - 1) > 0 ? y - 1 : 0].color;
                        //above balls marked as dead
                        balls[x, (y - 1) > 0 ? y - 1 : 0].state = BallState.Dead;
                        ballsDropped++;
                        //speed of animation based on the tbar setting of the animation modeless dialog
                        //delegate required to grab speed information to avoid cross threading error
                        delGetSpeed speed = animation.GetSpeed;//delegate allows the thread to grab information from a different space
                        //invoke in a try catch
                        try
                        {
                            speedVal = (int)Invoke(speed);
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show($"The following exception has occurred: {e}");
                        }                        
                        Thread.Sleep(speedVal * 10);
                    }
                }
            }
            //display result at the end of the iteration
            Display();
            Console.WriteLine($"StepDown call - balls dropped {ballsDropped}");
            return ballsDropped;//number of balls that dropped in function
        }

        /// <summary>
        /// FallDown() loops until the number of balls dropped from StepDown() = 0
        /// </summary>
        private void FallDown()
        {
            //get a count of balls dropped first
            int stepCalls = 0;

            //interate until all balls have dropped
            while(StepDown() > 0)
            {
                StepDown();
                stepCalls++;
                Console.WriteLine($"balls dropped: {StepDown()} step calls: {stepCalls}");
            }
            Console.WriteLine("FallDown call");
        }

        /// <summary>
        /// Pick() gets the last left click on the GDIDrawer window and calls CheckBalls() to kill relevant balls and FallDown() to animate the balls falling
        /// </summary>
        /// <returns>score for the number of balls killed in one click</returns>
        private int Pick()
        {
            Thread th = new Thread(FallDown);
            th.IsBackground = true;

            //if no mouse click - return
            if (!canvas.GetLastMouseLeftClick(out Point p))
                return 0;

            Console.WriteLine($"Mouse check - point {p}");

            //converting pixel to row and col
            int col = p.X / ballSize;
            int row = p.Y / ballSize;
            int ballsKilled = 0, score = 0;

            //beep to indicate the ball clicked is dead
            if (balls[col, row].state == BallState.Dead)
                Console.Beep();
            //if alive, remove ball and calculate score
            else
            {
                ballsKilled = CheckBalls(col, row, balls[col, row].color);
                score = 50 * ballsKilled + (((ballsKilled>1)?10:0)*ballsKilled);
            }
            th.Start();
            //FallDown();
            return score;
        }

        /// <summary>
        /// CallBackScoreClosing() re-enables the ShowScore checkbox values on closing of the score dialog
        /// </summary>
        private void CallBackScoreClosing()
        {
            UI_ShowScore_Cbx.Enabled = true;
            UI_ShowScore_Cbx.Checked = false;
        }

        /// <summary>
        /// CallBackAnimationClosing() re-enables the Animation checkbox values on closing of the animation dialog
        /// </summary>
        private void CallBackAnimationClosing()
        {
            UI_ShowAnimationSpeed_Cbx.Enabled = true;
            UI_ShowAnimationSpeed_Cbx.Checked = false;
        }


    }
}

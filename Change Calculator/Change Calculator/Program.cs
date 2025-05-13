/* Change Calculator
 * 
 * Author: Jonathan Le
 * 
 * Date: Jan. 12, 2025
 * 
 * Purpose: program takes an input from a user and will format it by removing any '$' and any leading/trailing whitespaces, then 
 * normalizes the double value parsed out (providing the few amount of change for the currency). The calculated values will
 * finally be displayed in GDIdrawer with all the information and their corresponding graphics.
 * 
 */ 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDIDrawer;
using System.Drawing;
using System.Data.SqlTypes;

namespace Change_Calculator
{
    internal class Program
    {
        private struct Currency
        {
            public double _denominationUnit;
            public int _denominationCount;
        }
        private struct Shape
        {
            public int _width;
            public int _height;
            public Color _color;
        }
        public enum EDenomination
        {
            Fifty = 0,
            Twenty = 1,
            Ten = 2,
            Five = 3,
            Toonie = 4,
            Loonie = 5,
            Quarter = 6,
            Dime = 7,
            Nickel = 8,
        }
        static CDrawer canvas = new CDrawer();

        static void Main(string[] args)
        {
            bool again;
            do
            {
                Console.CursorLeft = Console.WindowWidth / 2 - 10;
                Console.WriteLine("Lab 1 - Jonathan Le");
                Console.WriteLine();

                canvas.Scale = 5;

                Currency[] currencyArray = new Currency[9];
                Shape[] shapeArray = new Shape[9];

                //assigns values from above arrays to the Currency Struct array
                ValueAssignment(currencyArray, shapeArray);

                //getting input from user on the currency they want to convert
                double currency = GetValue("How much money do you wish to convert? ");

                //gets the currency amount from user and the currency counts at each denomination at each denomination
                double roundedCurrency = Normalization(currencyArray, currency);

                //currency display in GDIdrawer window
                canvas.AddText($"{roundedCurrency:C2}", 25, (canvas.ScaledWidth / 2 - 15), 5, 35, 10, Color.Yellow);

                //initial values for x and y coordinates
                int y = 8, x = canvas.ScaledWidth / 4;

                //array to hold denomination units that have counts >0
                for (int i = 0; i < currencyArray.Length; i++)
                {
                    //increment for the y axis
                    int yInc = canvas.ScaledHeight / 6;

                    //only denomination units that have a count will be rendered
                    if (currencyArray[i]._denominationCount > 0)
                    {
                        y += yInc;
                        //RenderCurrency() method to draw currency and display in GDIDrawer window
                        RenderCurrency(currencyArray, shapeArray, x, y, i);

                        //ensures currency displays will be fully displayed in the GDIdrawer window
                        if (y + 18 > canvas.ScaledHeight)
                        {
                            x *= 3;
                            y = 8;
                        }
                    }
                }
                again = false;
                //RunAgain() method to run program again
                again = RunAgain("Would you like to run the program again? (y/n): ");
                Console.Clear();
                canvas.Clear();
            } while (again);

            Console.Write("Press any key to exit... ");
            Console.ReadKey();
        }

        /// <summary>
        /// GetValue() prompts a user to input a double value from the user and returns that value. 
        /// Input validation against non-numeric and negative values.
        /// </summary>
        /// <param name="prompt">string type to prompt for user input</param>
        /// <returns>double value</returns>
        static double GetValue(string prompt)
        {
            string input;
            double value;
            bool trap;
            int index = 0;

            do
            {
                trap = false;

                Console.Write(prompt);
                input = Console.ReadLine();
                input = input.Replace('$', ' ');
                input = input.Trim();
                //substring starts at position 0, identities "." and only returns the values after 3 positions from "."
                index = input.IndexOf(".");
                input = (index > 0)?input.Substring(0, index+3):input;

                bool valid = double.TryParse(input, out value);

                if (!valid)
                {
                    Console.WriteLine("Input was not valid, please try again.");
                    trap = true;
                }
                else if (value < 0)
                {
                    Console.WriteLine("Input cannot be negative, please try again.");
                    trap = true;
                }
                else
                {
                    trap = false;
                }

            }while (trap);

            return value;
        }

        /// <summary>
        /// Normalization() takes a struct array and double amount to calculate the count of each denomination unit
        /// from the currency amount. The values are passed into the struct at the array position
        /// </summary>
        /// <param name="array"></param>
        /// <param name="currencyAmount"></param>
        /// <returns>double value</returns>
        static double Normalization(Currency[] array, double currencyAmount)
        {
            //storing unrounded amount for display
            double unroundedCurrency = currencyAmount;
            //rounding inputted amount
            double roundedCurrency = Math.Round((unroundedCurrency/0.05))*0.05;
            //storing rounded amount to be operated on
            double remainingCurrency = roundedCurrency;
            Console.WriteLine($"User entry of {currencyAmount:C2} and rounded to {remainingCurrency:C2}");

            //loop to determine denomination counts and to display information in console
            for (int i = 0; i < array.Length; i++)
            {
                array[i]._denominationCount = (int)(remainingCurrency/array[i]._denominationUnit);
                remainingCurrency -= (array[i]._denominationUnit * array[i]._denominationCount);
                Console.WriteLine($"{(EDenomination)i} x {array[i]._denominationCount}");
            }
            return roundedCurrency;
        }

        /// <summary>
        /// ValueAssignment() passes information stored in denomination_unit and color arrays into their respective struct arrays
        /// for the currency values
        /// </summary>
        /// <param name="currencyArray"></param>
        /// <param name="shapeArray"></param>
        static void ValueAssignment(Currency[] currencyArray, Shape[] shapeArray)
        {
            //creating arrays to store values to be assigned into the Currency struct for rendering in GDIdrawer
            double[] denominationUnits = { 50, 20, 10, 5, 2, 1, 0.25, 0.10, 0.05 };
            Color[] colorArray = 
                {Color.PaleVioletRed, Color.LawnGreen, Color.MediumPurple, Color.CornflowerBlue, 
                Color.LightGoldenrodYellow, Color.Yellow, Color.LightGray, Color.LightGray, Color.LightGray};
            
            //loop to assign information to specific index positions
            for (int i = 0; i < currencyArray.Length; i++)
            {
                currencyArray[i]._denominationUnit = denominationUnits[i];
                shapeArray[i]._color = colorArray[i];

                if (currencyArray[i]._denominationUnit >= 5)
                {
                    shapeArray[i]._width = 40;
                    shapeArray[i]._height = 18;
                }
                else if (currencyArray[i]._denominationUnit < 5)
                {
                    shapeArray[i]._width = 18;
                    shapeArray[i]._height = 18;
                }
            }
        }
        /// <summary>
        /// RenderCurrency() takes information from struct arrays and int values to draw the currency according to their
        /// denomination unit, counts, and shape. The resulting drawing is displayed in GDIDrawer
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="currencyArray"></param>
        /// <param name="shapeArray"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="i"></param>
        static void RenderCurrency(Currency[] currencyArray, Shape[] shapeArray, int x, int y, int i)
        {
            if (currencyArray[i]._denominationUnit >= 5)
            {

                canvas.AddCenteredRectangle(x, y, shapeArray[i]._width, shapeArray[i]._height, shapeArray[i]._color, 3, Color.DarkGray);
                canvas.AddText($"${currencyArray[i]._denominationUnit} x {currencyArray[i]._denominationCount}", 15, x - 9, y - 4, 20, 8, Color.Black);
            }
            else if (currencyArray[i]._denominationUnit < 5)
            {
                canvas.AddCenteredEllipse(x, y, shapeArray[i]._width, shapeArray[i]._height, shapeArray[i]._color, 3, Color.DarkGray);
                canvas.AddText($"${currencyArray[i]._denominationUnit} x {currencyArray[i]._denominationCount}", 15, x - 10, y - 4, 20, 8, Color.Black);
            }

        }

        /// <summary>
        /// RunAgain() method takes a string prompt from the caller and only accepts a "y" or "n" response.
        /// If the inputs are not the previously mentioned, the user will be put into a loop until a valid 
        /// response is input. A bool is returned.
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns></returns>
        static bool RunAgain(string prompt)
        {
            bool again, trap;
            do
            {
                again = false;
                trap = false;
                Console.Write(prompt);
                string response = Console.ReadLine();
                response = response.ToLower();
                if (response == "y")
                {
                    again = true;
                    trap = false;
                }
                else if (response == "n")
                {
                    again = false;
                    trap = false;
                }
                else
                {
                    Console.WriteLine("Choice was invalid. Please try again.");
                    trap = true;
                }
            } while (trap);
            
            return again;

        }
    }
}

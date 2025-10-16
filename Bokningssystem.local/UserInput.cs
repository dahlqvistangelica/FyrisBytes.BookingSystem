using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classer
{
    static class UserInputManager
    {
        internal static int UserInputToInt(string prompt)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                string input = Console.ReadLine();
                if (Int32.TryParse(input, out int inputInInt))
                {
                    return inputInInt;
                }
                else if (input == null)
                {
                    Console.WriteLine("Input var null");
                }
            }
        }
        internal static int UserInputToIntMinus1(string prompt)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                string input = Console.ReadLine();
                if (Int32.TryParse(input, out int inputInInt))
                {
                    return --inputInInt;
                }
                else if (input == null)
                {
                    Console.WriteLine("Input var null");
                }
            }
        }
        /// <summary>
        /// Säker konvertering från användardata till int med specifikt spann
        /// </summary>
        /// <param name="prompt"></param>
        /// <param name="maxValue"></param>
        /// <param name="minValue"></param>
        /// <returns></returns>
        internal static int UserInputToIntWithLimitations(string prompt, int maxValue, int minValue)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                string input = Console.ReadLine();
                if (Int32.TryParse(input, out int inputInInt))
                {
                    if (inputInInt <= maxValue && inputInInt >= minValue)
                        return inputInInt;
                    else if (inputInInt > maxValue)
                        Console.WriteLine($"Input måste vara mindre än {maxValue}");
                    else if (inputInInt < minValue)
                        Console.WriteLine($"Input får inte vara mindre än {minValue}");
                }
                else if (input == null)
                {
                    Console.WriteLine("Input får inte vara null!");
                }
            }
        }
        internal static double UserInputToDouble(string prompt, string errorMessage)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                string input = Console.ReadLine();
                if (input != null && Double.TryParse(input.Replace(".", ","), out double inputInDouble))
                {
                    return inputInDouble;
                }
                else if (input == null)
                {
                    Console.WriteLine("Input var null");
                }
            }
        }
        internal static string UserInputSafeString(string prompt, string errorMessage)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                string userInput = Console.ReadLine();
                if (userInput != null)
                {
                    return userInput;
                }
                else Console.WriteLine(errorMessage);
            }
        }
        internal static bool UserInputYesNo(string prompt, string errorMessage)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                string input = Console.ReadLine();
                if (input != null)
                {
                    input = input.ToLower();
                    if (input == "yes" || input == "y" || input == "ja" || input == "j")
                    {
                        return true;
                    }
                    else if (input == "no" || input == "n" || input == "nej")
                    {
                        return false;
                    }
                    else Console.WriteLine("Dina val måste vara [ja] eller [nej]");
                }
                else Console.WriteLine(errorMessage);
            }
        }
        internal static DateTime UserCreateDate()
        {
            while (true)
            {
                int year = UserInputToIntWithLimitations("Ange ett årtal [yyyy]", 9999, 0);
                int month = UserInputToIntWithLimitations("Ange en månad [mm]", 12, 0);
                int maxvalueDate = DateTime.DaysInMonth(year, month);
                int day = UserInputToIntWithLimitations("Ange ett datum[dd]", maxvalueDate, 0);
                DateTime date = new DateTime(year, month, day);
                return date;
            }
        }
        /// <summary>
        /// En Metod som låter anvöndaren skapa ett DateTime
        /// </summary>
        /// <returns></returns>
        internal static DateTime UserCreateDateTime()
        {
            while (true)
            {
                int year = UserInputToIntWithLimitations("Ange ett årtal [yyyy]", 9999, 0);
                int month = UserInputToIntWithLimitations("Ange en månad [mm]", 12, 0);
                int maxvalueDate = DateTime.DaysInMonth(year, month);
                int day = UserInputToIntWithLimitations("Ange ett datum[dd]", maxvalueDate, 0);
                int hours = UserInputToIntWithLimitations("Ange timme", 23, 0);
                int minutes = UserInputToIntWithLimitations("Ange minut", 59, 0);
                DateTime dateTime = new DateTime(year, month, day, hours, minutes, 0 );
                return dateTime;
            }
        }
    }
}

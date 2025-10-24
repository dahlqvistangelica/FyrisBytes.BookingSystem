using System;
using static System.Net.Mime.MediaTypeNames;
/// <summary>
/// En class för säker handering och validering av användarinmatad data. //Filip
/// </summary>
static class UserInputManager
{
    /// <summary>
    /// En metod för att säkert ta in och konvertera användarinput till int.
    /// </summary>
    /// <param name="prompt"></param>
    /// <returns></returns>
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
            else if (!Int32.TryParse(input, out inputInInt))
            {
                Console.WriteLine("Felaktigt värde");
            }
        }
    }
    /// <summary>
    /// metod för att konvertera användarinput till int -1. för använding till indexval i listor osv.
    /// </summary>
    /// <param name="prompt"></param>
    /// <returns></returns>
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
            else if (!Int32.TryParse(input, out inputInInt))
            {
                Console.WriteLine("Felaktigt värde");
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
            else if (!Int32.TryParse(input, out inputInInt))
            {
                Console.WriteLine("Felaktigt värde");
            }
            else if (input == null)
            {
                Console.WriteLine("Input får inte vara null!");
            }
        }
    }
    /// <summary>
    /// Säker hantering av Användarinput och konvertering till double
    /// </summary>
    /// <param name="prompt"></param>
    /// <param name="errorMessage"></param>
    /// <returns></returns>
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
            else if (!(input != null && Double.TryParse(input.Replace(".", ","), out inputInDouble)))
            {
                Console.WriteLine("Felaktigt värde");
            }
            else if (input == null)
            {
                Console.WriteLine("Input var null");
            }
        }
    }
    /// <summary>
    /// En metod för säker hantering av användarstringar
    /// </summary>
    /// <param name="prompt"></param>
    /// <param name="errorMessage"></param>
    /// <returns></returns>
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
    /// <summary>
    /// En metod för att ställa Ja/Nej Frågor till användaren
    /// </summary>
    /// <param name="prompt"></param>
    /// <param name="errorMessage"></param>
    /// <returns></returns>
    internal static bool UserInputYesNo(string prompt)
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
            else Console.WriteLine("Något gick fel");
        }
    }
    /// <summary>
    /// En metod som skapar ett DateTime men där Datumet sätts och tiden är 00:00:00
    /// </summary>
    /// <returns></returns>
    internal static DateOnly UserCreateDate()
    {
        while (true)
        {
            bool awnser = UserInputYesNo("Vill du använda dagens datum? (ja/nej)");
            if (awnser == true)
            {
                DateOnly today = DateOnly.FromDateTime(DateTime.Now);
                return today;
            }
            else
            {
                int year = UserInputToIntWithLimitations("Ange ett årtal [yyyy]", 9999, 0);
                int month = UserInputToIntWithLimitations("Ange en månad [mm]", 12, 0);
                int maxvalueDate = DateTime.DaysInMonth(year, month);
                int day = UserInputToIntWithLimitations("Ange ett datum[dd]", maxvalueDate, 0);
                DateOnly date = new DateOnly(year, month, day);
                return date;
            }
        }
    }
    /// <summary>
    /// En Metod som låter anvöndaren skapa ett DateTime med både Datum och Tid
    /// </summary>
    /// <returns></returns>
    internal static DateTime UserCreateDateTime()
    {
        while (true)
        {
            bool awnser = UserInputYesNo("Vill du använda dagens datum? (ja/nej)");
            if (awnser == true)
            {
                TimeOnly userTime = UserCreateTime();
                DateOnly today = DateOnly.FromDateTime(DateTime.Now);
                DateTime todayDatetime = today.ToDateTime(userTime);
                return todayDatetime;
            }
            else
            {
                int year = UserInputToIntWithLimitations("Ange ett årtal [åååå]", 9999, 0);
                int month = UserInputToIntWithLimitations("Ange en månad [mm]", 12, 0);
                int maxvalueDate = DateTime.DaysInMonth(year, month);
                int day = UserInputToIntWithLimitations("Ange ett datum[dd]", maxvalueDate, 0);
                int hours = UserInputToIntWithLimitations("Ange timme (24h)", 23, 0);
                int minutes = UserInputToIntWithLimitations("Ange minut", 59, 0);
                DateTime dateTime = new DateTime(year, month, day, hours, minutes, 0);
                return dateTime;
            }
        }
    }
    /// <summary>
    /// Metod för att användaren kan skapa en specifik tid
    /// </summary>
    /// <returns></returns>
    internal static TimeOnly UserCreateTime()
    {
        int hours = UserInputToIntWithLimitations("Ange timme (24h)", 23, 0);
        int minutes = UserInputToIntWithLimitations("Ange minut", 59, 0);
        TimeOnly Time = new TimeOnly(hours, minutes, 0);
        return Time;
    }
}
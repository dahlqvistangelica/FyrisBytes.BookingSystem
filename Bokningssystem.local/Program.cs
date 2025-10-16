using System.Text.Json;

namespace Bokningssystem.local
{
    internal class Program
    {
        public static class Menu
        {
            public static void StartUpScreen()
            {
                int input;
                do
                {
                    Console.Clear();
                    Console.WriteLine("-- Välkommen till bokningsystemet --");
                    Console.Write("[1] Hantera salar.\n[2] Hantera bokningar \n[3] Avsluta programmet \n");
                    Console.Write("Välj: ");
                    int.TryParse(Console.ReadLine(), out input);
                    switch (input)
                    {
                        case 1:
                            ControllRoomScreen();
                            break;
                        case 2:
                            ControllBookingScreen();
                            break;
                        case 3:
                            Console.WriteLine("Programmet kommer nu avslutas.");
                            Console.ReadLine();
                            break;
                        default:
                            Console.WriteLine("Ogiltig inmatning");
                            break;
                    }
                }
                while (input != 3);
            }
            public static void ControllRoomScreen()
            {
                int input;
                do
                {
                    Console.Clear();
                    Console.WriteLine("-- Hantera salar --");
                    Console.Write("[1] Skapa ny lokal. \n" +
                        "[2] Visa befintliga lokaler. \n" +
                        "[3] Ändra lokalsinformation. \n" +
                        "[4] Tillbaka till huvudmenyn\n");
                    Console.Write("Välj: ");
                    int.TryParse(Console.ReadLine(), out input);

                    switch (input)
                    {
                        case 1:
                            Console.Clear();
                            Console.WriteLine("Skapa ny lokal");
                            Console.ReadLine();
                            break;
                        case 2:
                            Console.Clear();
                            Console.WriteLine("Visa befintliga lokaler");
                            Console.ReadLine();
                            break;
                        case 3:
                            Console.Clear();
                            Console.WriteLine("Ändra lokalinformation");
                            Console.ReadLine();
                            break;
                        default:
                            Console.WriteLine("Ogiltigt val, försök igen.");
                            break;
                    }
                } while (input != 4);
            }
            public static void ControllBookingScreen()
            {
                int input;
                do
                {
                    Console.Clear();
                    Console.WriteLine("-- Hantera bokningar --");
                    Console.Write("[1] Skapa ny bokning. \n[2] Uppdatera bokning. \n[3] Ta bort bokning.\n[4] Visa alla bokningar. \n[5] Sök efter bokning. \n[6] Tillbaka till huvudmenyn.\n");
                    Console.Write("Välj: ");
                    int.TryParse(Console.ReadLine(), out input);
                    switch (input)
                    {
                        case 1:
                            Console.Clear();
                            Console.WriteLine("skapa ny bokning");
                            Console.ReadLine();
                            break;
                        case 2:
                            Console.Clear();
                            Console.WriteLine("uppdatera bokning");
                            Console.ReadLine();
                            break;
                        case 3:
                            Console.Clear();
                            Console.WriteLine("ta bort bokning");
                            Console.ReadLine();
                            break;
                        case 4:
                            Console.Clear();
                            Console.WriteLine("visa alla bokningar");
                            Console.ReadLine();
                            break;
                        case 5:
                            Console.Clear();
                            Console.WriteLine("sök efter bokning");
                            Console.ReadLine();
                            break;
                        default:
                            Console.WriteLine("Ogiltigt val, försök igen.");
                            break;
                    }
                } while (input != 6);
            }
        }






        static void ListAllRoomsInSystem() //Tai //TODO: Kom åt korrekt lista över salarna
        {
            List<string> salarLista = new List<string> { "Sal1", "Sal2", "Sal3", "Sal4" };
            //TODO - ^^^ ändra till korrekt lista för salarna ^^^
            Console.WriteLine("Följande salar finns i systemet:");
            foreach (var item in salarLista)
            {
                Console.WriteLine(item);
            }
        }


    }

    /// <summary>
    /// Lokalklass, parent till GroupRoom och ClassRoom. Skapar objekt för varje lokal.
    /// </summary>
    public class Room
    {
        private int _roomID;
        public int RoomID
        {
            get => _roomID;
            set { _roomID = value; }
        }
        private int _seatAmount;
        public virtual int SeatAmount
        {
            get => _seatAmount;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Du måste ha minst en plats i ett rum.");
                _seatAmount = value;
            }
        }
        public virtual bool HandicappedAccessible { get; set; }
        public int EmergencyExits { get; set; }

        public bool WhiteBoard;
        public Room(int idNumb, int seats, bool handAccess, int emergencyExits, bool whiteboard)
        {
            RoomID = idNumb;
            SeatAmount = seats;
            HandicappedAccessible = handAccess;
            EmergencyExits = emergencyExits;
            WhiteBoard = whiteboard;
        }
    }
    /// <summary>
    /// Childclass för grupprum, kan ha max 8 platser annars kastar den error. Ska ha ett id, max 8 sittplatser, kan vara handikappanpassat och ha fler utrymningsvägar.
    /// </summary>
    public class GroupRoom : Room //Grupprum max 8 platser.
    {
        public override int SeatAmount
        {
            get => base.SeatAmount;
            set
            {
                if (value > 8)
                    throw new ArgumentOutOfRangeException(nameof(value), "Grupprum kan inte ha fler än 8 sittplatser.");
                base.SeatAmount = value;
            }
        }
        public GroupRoom() : base(0, 1, false, 0, false)
        { }
    }
    /// <summary>
    /// Childclass för klassrum(sal), måste ha minst 8 platser, måste vara handikappanpassad. Ska ha ett id, minst 8 sittplatser, handikappanpassning, utrymningsvägar. Kan också ha projector och speakersystem.
    /// </summary>
    public class ClassRoom : Room //Sal med minst 8 platser, måste vara handikappanpassad.
    {
        public bool Projector;
        public bool SpeakerSystem;
        public override int SeatAmount
        {
            get => base.SeatAmount;
            set
            {
                if (value < 8)
                    throw new ArgumentOutOfRangeException(nameof(value), "Salar kan inte ha under 8 platser.");
            }
        }
        public override bool HandicappedAccessible
        {
            get => base.HandicappedAccessible;
            set
            {
                if (!value)
                    throw new ArgumentException("Salar måste vara handikappanpassade. ");
                base.HandicappedAccessible = value;
            }
        }
        public ClassRoom(bool projector, bool speaker) : base(0, 10, true, 1, true)
        {
            Projector = projector;
            SpeakerSystem = speaker;
        }

    }

    static class UserInputManager
    {
        internal static int UserInputToInt(string prompt)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                string? input = Console.ReadLine();
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
                string? input = Console.ReadLine();
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
        internal static int UserInputToIntWithLimitations(string prompt, int maxValue, int minValue)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                string? input = Console.ReadLine();
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
                    Console.WriteLine("Input får inte vara null");
                }
            }
        }
        internal static double UserInputToDouble(string prompt, string errorMessage)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                string? input = Console.ReadLine();
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
                string? userInput = Console.ReadLine();
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
                string? input = Console.ReadLine();
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
        internal static DateTime UserCreateDateTime(string prompt, string errorMessage)
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

    }

    public class SaveData
    {
        public static void SaveTofile<T>(List<T> DataToSave, string filePath)
        {
            try
            {
                //Serialize list till JSON string
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(DataToSave, options);

                //Skriv JSON string till fil
                File.WriteAllText(filePath, jsonString);
                Console.WriteLine("Data sparades korrekt");
            }
            catch (Exception e)
            {
                Console.WriteLine("Kunde inte spara data");
            }
        }
    }

}


using System;

public static class Menu
{
    /// <summary>
    /// Huvudmeny som visas vid uppstart av programmet.
    /// </summary>
    public static void StartUpScreen()
    {
        var manager = new BookingManager();
        int input;
        do
        {
            Console.Clear();
            Console.WriteLine("-- Välkommen till bokningsystemet --");
            input = UserInputManager.UserInputToIntWithLimitations("[1] Hantera salar.\n[2] Hantera bokningar \n[3] Avsluta programmet \nVälj: ", 3, 0);
            switch (input)
            {
                case 1:
                    ControllRoomScreen(manager);
                    break;
                case 2:
                    ControllBookingScreen(manager);
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

    /// <summary>
    /// Meny för att hantera lokaler.
    /// </summary>
    public static void ControllRoomScreen(BookingManager bookingManager)
    {
        int input;
        do
        {
            Console.Clear();
            Console.WriteLine("-- Hantera salar --");
            input = UserInputManager.UserInputToIntWithLimitations(
                "[1] Skapa ny lokal. \n" +
                "[2] Visa befintliga lokaler. \n" +
                "[3] Ändra lokalsinformation. \n" +
                "[4] Tillbaka till huvudmenyn\nVälj: ", 4, 0);

            switch (input)
            {
                case 1:
                    Console.Clear();
                    bookingManager.AllRooms.Add(RoomManager.UserCreateRoom());
                    Console.ReadLine();
                    break;
                case 2:
                    Console.Clear();
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
    /// <summary>
    /// Meny för att hantera bokningar i systemet.
    /// </summary>
    public static void ControllBookingScreen(BookingManager bookingManager)
    {
        int input;
        do
        {
            Console.Clear();
            Console.WriteLine("-- Hantera bokningar --");
            input = UserInputManager.UserInputToIntWithLimitations("[1] Skapa ny bokning. \n" +
                "[2] Uppdatera bokning. \n" +
                "[3] Ta bort bokning.\n" +
                "[4] Visa alla bokningar. \n" +
                "[5] Sök efter bokning. \n" +
                "[6] Tillbaka till huvudmenyn.\n" +
                "Välj: ", 6, 0);
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
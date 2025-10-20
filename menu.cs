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
        var checkData = StoreData.ReadFromFile();
        if (checkData != null)
        {
            manager = checkData;
        }
        else
        {
            manager.Developers.Add("Olof Brahm");
            manager.Developers.Add("Angelica Dahlqvist");
            manager.Developers.Add("Filip Gidlöf");
            manager.Developers.Add("Tai Lenke Enarsson");
        }
            do
            {
                Console.Clear();
                Console.WriteLine("-- Välkommen till bokningsystemet --");
                input = UserInputManager.UserInputToIntWithLimitations("[1] Hantera salar.\n[2] Hantera bokningar\n[3] Visa utvecklare \n[4] Avsluta programmet \nVälj: ", 4, 0);
                switch (input)
                {
                    case 1:
                        ControllRoomScreen(manager);
                        break;
                    case 2:
                        ControllBookingScreen(manager);
                        break;
                    case 3:
                        Console.Clear();
                        manager.PrintDevelopers();
                        break;
                    case 4:
                        StoreData.SaveToFile(manager);
                        Console.WriteLine("Programmet kommer nu avslutas.");
                        Console.ReadLine();
                        break;
                    default:
                        Console.WriteLine("Ogiltig inmatning");
                        break;
                }
            }
            while (input != 4);
    }

    /// <summary>
    /// Meny för att hantera lokaler.
    /// </summary>
    public static void ControllRoomScreen(BookingManager manager)
    {
        manager.SortRoomLists();
        int input;
        do
        {
            Console.Clear();
            Console.WriteLine("-- Hantera salar --");
            input = UserInputManager.UserInputToIntWithLimitations(
                "[1] Lägg till rum. \n" +
                "[2] Visa klassrum. \n" +
                "[3] Visa grupprum. \n" +
                "[4] Visa alla rum. \n" +
                "[5] Ändra lokalsinformation. \n" +
                "[6] Tillbaka till huvudmenyn\nVälj: ", 6, 0);

            switch (input)
            {
                case 1:
                    Console.Clear();
                    int seats = RoomManager.GetSeats();
                    if(RoomManager.DetermineRoomType(seats))
                    { manager.AllGroupRooms.Add(RoomManager.CreateGroupRoom(seats, manager)); }
                    else
                    { manager.AllClassRooms.Add(RoomManager.CreateClassRoom(seats, manager)); }
                    break;
                case 2:
                    Console.Clear();
                    RoomManager.DisplayClassRooms(manager);
                    Console.ReadLine();
                    break;
                case 3:
                    Console.Clear();
                    RoomManager.DisplayGroopRooms(manager);
                    Console.ReadLine();
                    break;
                case 4:
                    Console.Clear();
                    RoomManager.DisplayRooms(manager);
                    Console.ReadLine();
                    break;
                case 5:
                    Console.Clear();
                    Console.WriteLine("Ändra lokalinformation");
                    Console.ReadLine();
                    break;
                default:
                    Console.WriteLine("Ogiltigt val, försök igen.");
                    break;
            }
        } while (input != 6);
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
                    Booking.CreateBooking(bookingManager, 0);
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
                    Booking.ListBookings(bookingManager);
                    Console.ReadLine();
                    break;
                case 5:
                    Console.Clear();
                    Console.WriteLine("sök efter bokning");
                    Booking.BookingSearch(bookingManager, UserInputManager.UserInputToInt("Vilket år söker du efter?"));
                    Console.ReadLine();
                    break;
                default:
                    Console.WriteLine("Ogiltigt val, försök igen.");
                    break;
            }
        } while (input != 6);
    }
}
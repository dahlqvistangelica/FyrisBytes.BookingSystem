using System;

public static class Menu
{
    /// <summary>
    /// Huvudmeny som visas vid uppstart av programmet.
    /// </summary>
    public static void StartUpScreen()
    {
        var dataManager = new DataManager();
        int input;
        var checkData = StoreData.ReadFromFile();
        if (checkData != null)
        {
            dataManager = checkData;
            dataManager.RebuildAllRooms();
        }
        else
        {
            dataManager.Developers.Add("Olof Brahm");
            dataManager.Developers.Add("Angelica Dahlqvist");
            dataManager.Developers.Add("Filip Gidlöf");
            dataManager.Developers.Add("Tai Lenke Enarsson");
        }
            do
            {
                Console.Clear();
                Console.WriteLine("-- Välkommen till bokningsystemet --");
                input = UserInputManager.UserInputToIntWithLimitations("[1] Hantera salar.\n[2] Hantera bokningar\n[3] Visa utvecklare \n[4] Avsluta programmet \nVälj: ", 4, 0);
                switch (input)
                {
                    case 1:
                        ControllRoomScreen(dataManager);
                        break;
                    case 2:
                        ControllBookingScreen(dataManager);
                        break;
                    case 3:
                        Console.Clear();
                    BookingManager.PrintDevelopers(dataManager);                  
                    break;
                    case 4:
                        StoreData.SaveToFile(dataManager);
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
    public static void ControllRoomScreen(DataManager dataManager)
    {
        BookingManager.SortRoomLists(dataManager);
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
                    { dataManager.AllGroupRooms.Add(RoomManager.CreateGroupRoom(seats, dataManager)); }
                    else
                    { dataManager.AllClassRooms.Add(RoomManager.CreateClassRoom(seats, dataManager)); }
                    dataManager.RebuildAllRooms();
                    break;
                case 2:
                    Console.Clear();
                    RoomManager.DisplayClassRooms(dataManager);
                    Console.ReadLine();
                    break;
                case 3:
                    Console.Clear();
                    RoomManager.DisplayGroopRooms(dataManager);
                    Console.ReadLine();
                    break;
                case 4:
                    Console.Clear();
                    RoomManager.DisplayRooms(dataManager);
                    Console.ReadLine();
                    break;
                case 5:
                    Console.Clear();
                    Console.WriteLine("Ändra lokalinformation");
                    Console.ReadLine();
                    dataManager.RebuildAllRooms();
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
    public static void ControllBookingScreen(DataManager dataManager)
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
                    //Booking.CreateBooking(dataManager, 0);
                    BookingManager.NewBooking(dataManager);                   
                    Console.ReadLine();
                    dataManager.RebuildAllRooms();
                    break;
                case 2:
                    Console.Clear();
                    Console.WriteLine("uppdatera bokning");
                    Console.ReadLine();
                    dataManager.RebuildAllRooms();
                    break;
                case 3:
                    Console.Clear();
                    Console.WriteLine("ta bort bokning");
                    Console.ReadLine();
                    dataManager.RebuildAllRooms();
                    break;
                case 4:
                    Console.Clear();
                    Console.WriteLine("visa alla bokningar");
                    Booking.ListBookings(dataManager);
                    Console.ReadLine();
                    break;
                case 5:
                    Console.Clear();
                    Console.WriteLine("sök efter bokning");
                    Booking.BookingSearch(dataManager, UserInputManager.UserInputToInt("Vilket år söker du efter?"));
                    Console.ReadLine();
                    break;
                default:
                    Console.WriteLine("Ogiltigt val, försök igen.");
                    break;
            }
        } while (input != 6);
    }
}
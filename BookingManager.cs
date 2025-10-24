using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

/// <summary>
/// Hanterar samtliga bokningar i systemet
/// </summary>
public class BookingManager
{

    /// <summary>
    /// Sorterar listorna med rum efter rumsID. 
    /// </summary>
    public static void SortRoomLists(DataManager manager)
    {
        manager.AllGroupRooms.Sort((r1, r2) => r1.RoomID.CompareTo(r2.RoomID));
        manager.AllClassRooms.Sort((r1, r2) => r1.RoomID.CompareTo(r2.RoomID));
        manager.AllRooms.Sort((r1, r2) => r1.RoomID.CompareTo(r2.RoomID));
    }


    static public void BookingSearchYear(DataManager dataManager, int targetYear)
    {
        int counter = 0;
        foreach (Booking item in dataManager.AllBookings)
        {
            if (item.BookingStart.Year == targetYear)
                counter++;
            Console.WriteLine($"Bokning nummer {counter} {item.BookingStart.ToString("g")}  {item.BookingEnd.ToString("g")}");
        }
    }
    static public void BookingSearchDate(DataManager dataManager, DateOnly targetDate)
    {
        int counter = 0;
        foreach (Booking item in dataManager.AllBookings)
        {
            DateOnly dateonlyItem = DateOnly.FromDateTime(item.BookingStart);
            if (dateonlyItem == targetDate)
                counter++;
            Console.WriteLine($"[{counter}] {item.BookingStart.ToString("g")}  {item.BookingEnd.ToString("g")}");
        }
    }
    public static int ListAllBookings(DataManager dataManager)
    {
        int counter = 0;
        foreach (Booking item in dataManager.AllBookings)
        {
            counter++;
            Console.WriteLine($"[{counter}] {item.Info.ToString()}");
        }
        if (counter <= 0)
            Console.WriteLine("Inga bokningar hittades.");

        return counter;
    }
    /// <summary>
    /// Skapar ny bokning av valfritt rum
    /// </summary>
    /// <param name="dataManager"></param>
    public static void NewBooking(DataManager dataManager) //Tai
    {
        Console.Clear();
        Console.WriteLine("--- Ny Bokning ---");
        Console.WriteLine("Start av bokning:");
        DateTime bookingStart = UserInputManager.UserCreateDateTime();
        Console.WriteLine("Slut av bokning:");
        DateTime bookingEnd = UserInputManager.UserCreateDateTime();

        int initialCount = dataManager.AllBookings.Count;
        bool correctEndTime = bookingEnd > bookingStart ? true : false; //Check om sluttid är efter starttid
        while (correctEndTime == false)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Bokningen kan inte sluta innan den börjar.\nFörsök igen:");
            Console.ForegroundColor = ConsoleColor.White;
            bookingEnd = UserInputManager.UserCreateDateTime();
            if (bookingEnd > bookingStart)
                correctEndTime = true;
        }
        TimeSpan bookedTime = bookingStart - bookingEnd;

        Console.WriteLine("Följande salar är lediga att boka för din angivna tid: ");
        int availableRooms = 0;

        int roomIndex = 0;
        foreach (var room in dataManager.AllRooms)
        {
                if (room.IsAvailable(bookingStart, bookingEnd))
                {
                    Console.WriteLine($"[{roomIndex + 1}] {room.Info}");
                    availableRooms++;
                }
                else
                    break;
            roomIndex++;
        }

        if (availableRooms == 0)
            Console.WriteLine("Det finns inga lediga salar för tiden du angivit.");
        else
        {
            int roomToBook = UserInputManager.UserInputToIntMinus1("\nVälj sal att boka: ");
            Room chosenRoom = dataManager.AllRooms[roomToBook];

            Booking newBooking = new Booking(bookingStart, bookingEnd, chosenRoom);
            dataManager.AllBookings.Add(newBooking);
        }
    }
    /// <summary>
    /// Metod för ändring av tidigare inlagd bokning. Ber användaren om ett tidsspann och listar bokningar inom spannet. Val finns om att uppdatera datum, tid eller rum.
    /// </summary>
    /// <param name="datamanager"></param>
    public static void ChangeBooking(DataManager datamanager) //Tai
    {
        
        Console.WriteLine("--- Uppdatera Bokning ---");
        Console.WriteLine($"Följande bokningar finns i systemet:\n");
        int bookingscount = ListAllBookings(datamanager);
        if (bookingscount > 0)
        {
            int whichBookingToChange = UserInputManager.UserInputToIntMinus1("\nAnge nummer för bokningen du vill uppdatera: ");
            UpdateBookingWhichChange(whichBookingToChange, datamanager); //bestämmer vad som ska skrivas över i angiven bokning och utför överskrivningen 
        }
    }
    /// <summary>
    /// Ber användaren om vilken ändring av bokningen som ska ske
    /// </summary>
    /// <param name="whichBookingToChange"></param>
    /// <param name="dataManager"></param>
    static void UpdateBookingWhichChange(int whichBookingToChange, DataManager dataManager) //Tai
    {
        int inputWhatToChange = UserInputManager.UserInputToIntWithLimitations("\nVad vill du uppdatera i denna bokning?" +
                "\n[1] Datum" +
                "\n[2] Tid" +
                "\n[3] Sal", 3, 1);
        Console.WriteLine();

        switch (inputWhatToChange)
        {
            case 1:
                UpdateBookingDate(whichBookingToChange, dataManager);
                break;
            case 2:
                UpdateBookingTime(whichBookingToChange, dataManager);
                break;
            case 3:
                UpdateBookingRoom(whichBookingToChange, dataManager);
                break;
        }
    }
    /// <summary>
    /// Metod för att uppdatera datum på redan inlagd bokning.
    /// </summary>
    /// <param name="whichBookingToChange"></param>
    /// <param name="bookingManager"></param>
    static void UpdateBookingDate(int whichBookingToChange, DataManager bookingManager) //Tai
    {
        DateTime bookedStart = bookingManager.AllBookings[whichBookingToChange].BookingStart;
        string bookedStartDate = $"{bookedStart:yyyy-MM-dd}";
        string bookedStartTime = $"{bookedStart:HH:mm:ss}";
        Console.WriteLine($"Nuvarande startdag: {bookedStartDate}");
        Console.WriteLine($"Uppdatera startdag för bokning:");
        DateOnly newStartDay = UserInputManager.UserCreateDate();
        DateTime newStart = DateTime.Parse($"{newStartDay:yyyy-MM-dd} {bookedStartTime:HH:mm:ss}");

        DateTime bookedEnd = bookingManager.AllBookings[whichBookingToChange].BookingEnd;
        string bookedEndDate = $"{bookedEnd:yyyy-MM-dd}";
        string bookedEndTime = $"{bookedEnd:HH:mm:ss}";
        Console.WriteLine($"Nuvarande slutdag: {bookedEndDate}");
        Console.WriteLine($"Uppdatera slutdag för bokning:");
        DateOnly newEndDate = UserInputManager.UserCreateDate();
        DateTime newEnd = DateTime.Parse($"{newEndDate:yyyy-MM-dd} {bookedEndTime:HH:mm:ss}");

        bookingManager.AllBookings[whichBookingToChange].BookingStart = newStart;
        bookingManager.AllBookings[whichBookingToChange].BookingEnd = newEnd;
        bookingManager.AllBookings[whichBookingToChange].BookingSpan = newStart - newEnd;
    }
    /// <summary>
    /// Metod för att uppdatera tid på redan inlagd bokning.
    /// </summary>
    /// <param name="whichBookingToChange"></param>
    /// <param name="bookingManager"></param>
    static void UpdateBookingTime(int whichBookingToChange, DataManager bookingManager) //Tai
    {
        DateTime bookedStart = bookingManager.AllBookings[whichBookingToChange].BookingStart;
        string bookedStartDate = $"{bookedStart:yyyy-MM-dd}";
        string bookedStartTime = $"{bookedStart:HH:mm:ss}";
        Console.WriteLine($"Nuvarande starttid: {bookedStartTime}");
        Console.WriteLine("Uppdatera starttid för bokningen: ");
        TimeOnly newStartTime = UserInputManager.UserCreateTime(); 
        DateTime newStart = DateTime.Parse($"{bookedStartDate:yyyy-MM-dd} {newStartTime:HH:mm:ss}");

        DateTime bookedEnd = bookingManager.AllBookings[whichBookingToChange].BookingEnd;
        string bookedEndDate = $"{bookedEnd:yyyy-MM-dd}";
        string bookedEndTime = $"{bookedEnd:HH:mm:ss}";
        Console.WriteLine($"Nuvarande sluttid: {bookedEndTime}");
        Console.WriteLine($"Uppdatera sluttid för bokning:");
        TimeOnly newEndTime = UserInputManager.UserCreateTime();
        DateTime newEnd = DateTime.Parse($"{bookedEndDate:yyyy-MM-dd} {newEndTime:HH:mm:ss}");

        bookingManager.AllBookings[whichBookingToChange].BookingStart = newStart;
        bookingManager.AllBookings[whichBookingToChange].BookingEnd = newEnd;
        bookingManager.AllBookings[whichBookingToChange].BookingSpan = newStart - newEnd;
    }
    /// <summary>
    /// Metod för att uppdatera sal på redan inlagd bokning.
    /// </summary>
    /// <param name="whichBookingToChange"></param>
    /// <param name="dataManager"></param>
    static void UpdateBookingRoom(int whichBookingToChange, DataManager dataManager) //Tai
    {
        DateTime startTime = dataManager.AllBookings[whichBookingToChange].BookingStart;
        DateTime endTime = dataManager.AllBookings[whichBookingToChange].BookingEnd;
        Console.WriteLine("Dessa salar finns tillgängliga under tiden för bokningen:");
        for (int i = 0; i < dataManager.AllBookings.Count; i++)
        {
            if (dataManager.AllRooms[whichBookingToChange].IsAvailable(startTime, endTime) == true)
                Console.WriteLine($"[{i + 1}] {dataManager.AllRooms[i].Info.ToString()}");
            else
                continue;
        }
        int newRoom = UserInputManager.UserInputToIntWithLimitations("Ange vilken sal du vill använda för bokningen: ", dataManager.AllBookings.Count, 0);

        dataManager.AllBookings[whichBookingToChange].BookedRoom = dataManager.AllRooms[newRoom];
    }
    /// <summary>
    /// Listar samtliga bokningar och tar bort ett index i listan
    /// </summary>
    /// <param name="dataManager"></param>
    public static void DeleteBooking(DataManager dataManager)//Tai
    {
        DateTime date = UserInputManager.UserCreateDateTime();
        int index = 0;
        Console.WriteLine("[0] AVBRYT");
        foreach (var item in dataManager.AllBookings)
        {
            Console.WriteLine($"[{index + 1}]" + item.Info.ToString());
            index++;
        }
        int indexToRemove = UserInputManager.UserInputToIntWithLimitations("Vilken bokning skulle du vilja ta bort?", dataManager.AllBookings.Count - 1, 0) - 1;
        if (indexToRemove >= 0)
            dataManager.AllBookings.RemoveAt(indexToRemove);
    }
    /// <summary>
    /// Dublett-metod? Kollar om ett rum är ledigt en viss tid.
    /// </summary>
    /// <param name="wantedStartTime"></param>
    /// <param name="wantedEndTime"></param>
    /// <param name="room"></param>
    /// <returns></returns>
    public bool IsBookable(DateTime wantedStartTime, DateTime wantedEndTime, Room room)
    {
        if (!room.IsAvailable(wantedStartTime, wantedEndTime))
            return false;
        else
            return true;
    }
}


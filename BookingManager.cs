using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

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
            Console.WriteLine($"Bokning nummer {counter} {item.BookingStart.ToString("g")}  {item.BookingEnd.ToString("g")}");
        }
    }
    public static void ListAllBookings(DataManager dataManager)
    {
        foreach (var item in dataManager.AllBookings)
        {
            Console.WriteLine(item.ToString());
        }
    }

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

    public void ChangeBooking(DataManager datamanager) //Tai
    {
        
        Console.WriteLine("--- Uppdatera Bokning ---");
        DateTime date = UserInputManager.UserCreateDateTime();
        Console.WriteLine($"Följande bokningar finns i systemet {date:dddd} {date:D}:");

        for (int i = 0; i < datamanager.AllBookings.Count; i++)
        {
            if (date > datamanager.AllBookings[i].BookingStart && date < datamanager.AllBookings[i].BookingEnd)
                Console.WriteLine($"[{i + 1}] {datamanager.AllBookings[i].Info.ToString()}");
            else
                continue;
        }
        int whichBookingToChange = UserInputManager.UserInputToIntMinus1("Ange nummer för bokningen du vill uppdatera: ");
        UpdateBookingWhichChange(whichBookingToChange, datamanager); //bestämmer vad som ska skrivas över i angiven bokning och utför överskrivningen 
    }
    static void UpdateBookingWhichChange(int whichBookingToChange, DataManager dataManager) //Tai
    {
        int initialCount = dataManager.AllBookings.Count;
        int inputWhatToChange = UserInputManager.UserInputToIntWithLimitations("Vad vill du uppdatera i denna bokning?" +
                "\n[1] Datum" +
                "\n[2] Tid" +
                "\n[3] Sal", 3, 1);

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
    static void UpdateBookingDate(int whichBookingToChange, DataManager bookingManager) //Tai
    {
        //TODO: ta ut tid ur bokningsstart och bokningsslut och lägg in i DateTime (datum + tid)
        Console.WriteLine($"Uppdatera startdag för bokning:");
        DateTime newBookingStartDay = UserInputManager.UserCreateDateTime(); //TODO: Ändra till DateOnly
        Console.WriteLine($"Uppdatera slutdag för bokning:");
        DateTime newBookingEndDay = UserInputManager.UserCreateDateTime(); //TODO: Ändra till DateOnly

        bookingManager.AllBookings[whichBookingToChange].BookingStart = newBookingStartDay;
        bookingManager.AllBookings[whichBookingToChange].BookingEnd = newBookingEndDay;
        bookingManager.AllBookings[whichBookingToChange].BookingSpan = newBookingStartDay - newBookingEndDay;
        //TODO: Korrigera bookingstart/end/span till DateOnly - finns ej i klassen just nu
    }
    static void UpdateBookingTime(int whichBookingToChange, DataManager bookingManager) //Tai
    {
        //TODO: ta ut tid ur bokningsstart och bokningsslut och lägg in i DateTime (datum + tid)
        Console.Write("Ange ny starttid för bokningen: ");
        DateTime bookingStart = UserInputManager.UserCreateDateTime();
        Console.Write("Ange hur länge bokningen ska vara: ");
        DateTime bookingEnd = UserInputManager.UserCreateDateTime();
        int userconfirm = UserInputManager.UserInputToInt
            (
                "Bokningen är satt till:" +
                "\nBokningens start: " + bookingStart.ToString() +
                "\nBokningens slut: " + bookingEnd.ToString() +
                "\nBekräfta tid:" +
                "\n[1] Bekräfta" +
                "\n[2] Ignorera"
            );

        if (userconfirm == 1)
        {
            bookingManager.AllBookings[whichBookingToChange].BookingStart = bookingStart;
            bookingManager.AllBookings[whichBookingToChange].BookingEnd = bookingEnd;
            bookingManager.AllBookings[whichBookingToChange].BookingSpan = bookingStart - bookingEnd;
        }
    }
    static void UpdateBookingRoom(int whichBookingToChange, DataManager dataManager) //Tai
    {
        DateTime startTime = dataManager.AllBooking[whichBookingToChange].BookingStart;
        DateTime endTime = dataManager.AllBookings[whichBookingToChange].BookingEnd;
        Console.WriteLine("Dessa salar finns tillgängliga under tiden för bokningen:");
        for (int i = 0; i < dataManager.AllBookings.Count; i++)
        {
            if (dataManager.AllRooms[whichBookingToChange].IsAvailable(startTime, endTime))
                Console.WriteLine($"[{i + 1}] {dataManager.AllRooms[i].RoomID}");
            else
                continue;
        }
        int newRoom = UserInputManager.UserInputToIntWithLimitations("Ange vilken sal du vill använda för bokningen: ", dataManager.AllBookings.Count, 0);

        dataManager.AllBookings[whichBookingToChange].BookedRoom = dataManager.AllRooms[newRoom];
    }

    public void DeleteBooking(DataManager dataManager)//Tai
    {
        DateTime date = UserInputManager.UserCreateDateTime();
        foreach (var item in dataManager.AllBookings)
        {
            Console.WriteLine(item.Info.ToString());
        }

    }
    public bool IsBookable(DateTime wantedStartTime, DateTime wantedEndTime, Room room)
    {
        if (!room.IsAvailable(wantedStartTime, wantedEndTime))
            return false;
        else
            return true;
    }
}


using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

public class BookingManager { 
    
    /// <summary>
    /// Sorterar listorna med rum efter rumsID. 
    /// </summary>
    public static void SortRoomLists(DataManager manager) 
    {
        manager.AllGroupRooms.Sort((r1, r2) => r1.RoomID.CompareTo(r2.RoomID));
        manager.AllClassRooms.Sort((r1, r2) => r1.RoomID.CompareTo(r2.RoomID));
        manager.AllRooms.Sort((r1, r2) => r1.RoomID.CompareTo(r2.RoomID));
    }
    
    public static void PrintDevelopers(DataManager manager)
    {
        for (int i = 0; i < manager.Developers.Count; i++)
        {
            Console.WriteLine(manager.Developers[i]);
        }
        Console.ReadLine();
    }
    static public void BookingSearchYear(DataManager dataManager, int targetYear)
    {
        int counter = 0;
        foreach (Booking item in dataManager.AllBookings)
        {
            if (item.BookingStart.Year == targetYear)
                counter++;
            Console.WriteLine($"Bokning nummer {counter} {item.BookingStart.ToString("g")}  {item.BookingEnds.ToString("g")}");
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
            Console.WriteLine($"Bokning nummer {counter} {item.BookingStart.ToString("g")}  {item.BookingEnds.ToString("g")}");
        }
    }
    static public void ListBookings(DataManager manager)
    {
        foreach (Booking item in manager.AllBookings)
        {
            Console.WriteLine($" Start tid: {item.BookingStart.ToString("g")} Slut tid: {item.BookingEnds.ToString("g")}  Bokningslängd i timmar:{item.BookingSpan.TotalHours} Rummstyp: {item.BookedRoom}");
        }
    }

    public void NewBooking(DataManager dataManager) //Tai
    {
        Console.Clear();
        Console.WriteLine("--- Ny Bokning ---");
        Console.WriteLine("Start av bokning:");
        DateTime bookingStart = UserInputManager.UserCreateDateTime();
        Console.WriteLine("Slut av bokning:");
        DateTime bookingEnd = UserInputManager.UserCreateDateTime();

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
        for (int i = 0; i < dataManager.AllRooms.Count; i++)
        {
            if (/*//TODO: bookingManager.AllBookings[i].IsBookable(bookingStart, bookingEnd) == true
                 kolla om rummet är bokningsbart den tiden, om true, skriv ut på konsollen*/ IsBookable(bookingStart, bookingEnd) == true)
            {
                Console.WriteLine($"[{i + 1}] ID:{dataManager.AllRooms[i].RoomID} Platser:{dataManager.AllRooms[i].SeatAmount} Handikappsanpassad:{dataManager.AllRooms[i].DisablityAdapted} Nödutgångar:{dataManager.AllRooms[i].EmergencyExits} Whiteboard:{dataManager.AllRooms[i].WhiteBoard}");
                availableRooms++;
            }
            else
                continue;
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

        bool isSuccess = false; //TODO: om bokning lyckades = true, misslyckades = falskt
        ChangeBookingSuccessPrintToScreen(isSuccess);

    }

    public void ChangeBooking(DataManager datamanager) //Tai
    {

        Console.WriteLine("--- Uppdatera Bokning ---");
        DateTime date = UserInputManager.UserCreateDateTime();
        Console.WriteLine($"Följande bokningar finns i systemet {date:dddd} {date:D}:");

        for (int i = 0; i < datamanager.AllBookings.Count; i++)
        {
            Console.WriteLine($"[{i + 1}] {datamanager.AllBookings[i].Info.ToString()}");

            /*Console.WriteLine($"[{i + 1}] ID:{bookingManager.AllRooms.RoomID} Platser:{bookingManager.AllRooms.SeatAmount} Handikappsanpassad:{bookingManager.AllRooms.HandicappedAccessible} Nödutgångar:{bookingManager.AllRooms.EmergencyExits} Whiteboard:{bookingManager.AllRooms.WhiteBoard}");*/
            //TODO: Nå korrekt bokning med egenskaper
            //Format: "datum starttid-sluttid ({tid}h {tid}min) Salnamn: "Notering""
        }
        int whichBookingToChange = UserInputManager.UserInputToIntMinus1("Ange nummer för bokningen du vill uppdatera: ");
        UpdateBookingWhichChange(whichBookingToChange, datamanager); //bestämmer vad som ska skrivas över i angiven bokning och utför överskrivningen 
    }
    static void UpdateBookingWhichChange(int whichBookingToChange, DataManager bookingManager) //Tai
    {
        int inputWhatToChange = UserInputManager.UserInputToIntWithLimitations("Vad vill du uppdatera i denna bokning?" +
                "\n[1] Datum" +
                "\n[2] Tid" +
                "\n[3] Sal", 3, 1);

        bool success = (inputWhatToChange) switch
        {
            1 => success = UpdateBookingDate(whichBookingToChange, bookingManager),
            2 => success = UpdateBookingTime(whichBookingToChange, bookingManager),
            3 => success = UpdateBookingRoom(whichBookingToChange, bookingManager),
            _ => false
        };

        ChangeBookingSuccessPrintToScreen(success);
    }
    static bool UpdateBookingDate(int whichBookingToChange, DataManager bookingManager) //Tai
    {
        //TODO: Kolla av bokningstider om de är samma variabel på dag och tid, eller en och samma variabel?????
        Console.WriteLine($"Uppdatera startdag för bokning:");
        DateTime newBookingStart = UserInputManager.UserCreateDateTime(); //TODO: Ändra till DateOnly
        Console.WriteLine($"Uppdatera slutdag för bokning:");
        DateTime newBookingEnd = UserInputManager.UserCreateDateTime(); //TODO: Ändra till DateOnly

        bookingManager.AllBookings[whichBookingToChange].BookingStart = newBookingStart;
        bookingManager.AllBookings[whichBookingToChange].BookingEnds = newBookingEnd;
        bookingManager.AllBookings[whichBookingToChange].BookingSpan = newBookingStart - newBookingEnd;
        //TODO: Korrigera bookingstart/end/span till DateOnly - finns ej i klassen just nu

        Console.WriteLine("success bool just nu satt till false");
        bool success = false; //TODO: Bool baserad på om tidsöverskrivning lyckades
        return success;
    }
    static bool UpdateBookingTime(int whichBookingToChange, DataManager bookingManager) //Tai
    {
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

        bool isSuccess = false;
        if (userconfirm == 1)
        {
            bookingManager.AllBookings[whichBookingToChange].BookingStart = bookingStart;
            bookingManager.AllBookings[whichBookingToChange].BookingEnds = bookingEnd;
            bookingManager.AllBookings[whichBookingToChange].BookingSpan = bookingStart - bookingEnd;
            isSuccess = true; //TODO: confirm if success
        }
        return isSuccess;
    }
    static bool UpdateBookingRoom(int whichBookingToChange, DataManager dataManager) //Tai
    {

        Console.WriteLine("Dessa salar finns tillgängliga under tiden för bokningen:");
        for (int i = 0; i < dataManager.AllBookings.Count; i++)
        {
            if (dataManager.AllBookings[i].BookingSpan == dataManager.AllBookings[whichBookingToChange].BookingSpan)
                continue;
            else
                Console.WriteLine($"[{i + 1}] {dataManager.AllBookings[i].Info}");

        }
        int newRoom = UserInputManager.UserInputToIntWithLimitations("Ange vilken sal du vill använda för bokningen: ", dataManager.AllBookings.Count, 0);

        Console.WriteLine("Når ej att ändra rum på: " + dataManager.AllBookings[whichBookingToChange]);
        //TODO: Nå att ändra rum i AllBookings???
        //TODO: replace bokningslista[whichBookingToChange] (salen) med (chooseRoom)

        bool success = true; //TODO: confirm if success
        return success;
    }

    public void DeleteBooking(DataManager dataManager)//Tai
    {
        DateTime date = UserInputManager.UserCreateDateTime();
        foreach (var item in dataManager.AllBookings)
        {
            Console.WriteLine(dataManager.AllBookings.ToString());
            //TODO: Fixa formatet som skrivs ut?
        }
    }
    public void ListAllBookings(DataManager dataManager)
    {
        foreach (var item in dataManager.AllBookings)
        {
            Console.WriteLine(item.ToString());
            Console.ReadLine();
        }
    }
    public bool IsBookable(DateTime wantedStartTime, DateTime wantedEndTime) //Tai
    {
        TimeSpan timeSpan = wantedEndTime - wantedStartTime;
        if (wantedStartTime > BookingStart && wantedStartTime < DataManager.AllBookings[i].BookingEnds || wantedEndTime > BookingStart && wantedEndTime < BookingEnds)
            return false;
        else
            return true;
    }
    public void ListAllBookingsWithinTimeframe() //Tai
    {
        //TODO
    }

    public static void ChangeBookingSuccessPrintToScreen(bool success) //Tai
    {
        if (success == true)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Ändringen i systemet utfördes korrekt.");
            Console.ForegroundColor = ConsoleColor.White;

        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Ändringen misslyckades, försök igen.");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}

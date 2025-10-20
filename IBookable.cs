using System;

public interface IBookable //Tai
{
    static void NewBooking(BookingManager bookingManager) //Tai
    {
        Console.WriteLine("--- Ny Bokning ---");
        Console.WriteLine("Start av bokning:");
        DateTime bookingStart = UserInputManager.UserCreateDateTime();
        Console.WriteLine("Slut av bokning:");
        DateTime bookingEnd = UserInputManager.UserCreateDateTime();
        TimeSpan bookedTime = bookingStart - bookingEnd;

        Console.WriteLine("Följande salar är lediga att boka för din angivna tid: ");
        for (int i = 0; i < bookingManager.AllRooms.Count; i++)
        {
            if (bookingManager.AllBookings[i].BookingSpan == bookedTime)
                continue;
            else
            {
                Console.WriteLine($"[{i + 1}] ID:{bookingManager.AllRooms[i].RoomID} Platser:{bookingManager.AllRooms[i].SeatAmount} Handikappsanpassad:{bookingManager.AllRooms[i].DisablityAdapted} Nödutgångar:{bookingManager.AllRooms[i].EmergencyExits} Whiteboard:{bookingManager.AllRooms[i].WhiteBoard}");
            }
        }
        int roomToBook = UserInputManager.UserInputToIntMinus1("\nVälj sal att boka: ");
        Room chosenRoom = bookingManager.AllRooms[roomToBook];

        Booking newBooking = new Booking(bookingStart, bookingEnd, chosenRoom);

        bookingManager.AllBookings.Add(newBooking);
        //TODO: Lägg in bokningen i listan av bokningar


        bool isSuccess = false; //om bokning lyckadexs = true, misslyckades = falskt
        ChangeBookingSuccessPrintToScreen(isSuccess);

    }

    static void ChangeBooking(BookingManager bookingManager) //Tai
    {

        Console.WriteLine("--- Uppdatera Bokning ---");
        DateTime date = UserInputManager.UserCreateDateTime();
        Console.WriteLine($"Följande bokningar finns i systemet {date:dddd} {date:D}:");

        for (int i = 0; i < bookingManager.AllBookings.Count; i++) 
        {
                Console.WriteLine($"[{i + 1}] {bookingManager.AllBookings[i].Info.ToString()}");

            /*Console.WriteLine($"[{i + 1}] ID:{bookingManager.AllRooms.RoomID} Platser:{bookingManager.AllRooms.SeatAmount} Handikappsanpassad:{bookingManager.AllRooms.HandicappedAccessible} Nödutgångar:{bookingManager.AllRooms.EmergencyExits} Whiteboard:{bookingManager.AllRooms.WhiteBoard}");*/
            
            //TODO: Nå korrekt bokning med egenskaper
            //Format: "datum starttid-sluttid ({tid}h {tid}min) Salnamn: "Notering""
        }
        int whichBookingToChange = UserInputManager.UserInputToIntMinus1("Ange nummer för bokningen du vill uppdatera: ");
        UpdateBookingWhichChange(whichBookingToChange, bookingManager); //bestämmer vad som ska skrivas över i angiven bokning och utför överskrivningen 
    }
    static void UpdateBookingWhichChange(int whichBookingToChange, BookingManager bookingManager) //Tai
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
    static bool UpdateBookingDate(int whichBookingToChange, BookingManager bookingManager) //Tai
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
    static bool UpdateBookingTime(int whichBookingToChange, BookingManager bookingManager) //Tai
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
    static bool UpdateBookingRoom(int whichBookingToChange, BookingManager bookingManager) //Tai
    {
        
        Console.WriteLine("Dessa salar finns tillgängliga under tiden för bokningen:");
        for (int i = 0; i < bookingManager.AllBookings.Count; i++)
        {
            if (bookingManager.AllBookings[i].BookingSpan == bookingManager.AllBookings[whichBookingToChange].BookingSpan)
                continue;
            else
                Console.WriteLine($"[{i+1}] {bookingManager.AllBookings[i].Info}"); 
            
        }
        int newRoom = UserInputManager.UserInputToIntWithLimitations("Ange vilken sal du vill använda för bokningen: ", bookingManager.AllBookings.Count, 0);

        Console.WriteLine("Når ej att ändra rum på: " + bookingManager.AllBookings[whichBookingToChange]);
        //TODO: Nå att ändra rum i AllBookings???
        //TODO: replace bokningslista[whichBookingToChange] (salen) med (chooseRoom)

        bool success = true; //TODO: confirm if success
        return success;
    }

    static void DeleteBooking(BookingManager bookingManager)//Tai
    {
        DateTime date = UserInputManager.UserCreateDateTime();
        foreach (var item in bookingManager.AllBookings)
        {
            Console.WriteLine(bookingManager.AllBookings.ToString());
            //TODO: Fixa formatet som skrivs ut?
        }
    }
    static void ListAllBookingsWithinTimeframe() //Tai
    {
        //TODO
    }
    static bool IsBookable() //Tai
    {
        //TODO
        bool answer = false;
        return answer;
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

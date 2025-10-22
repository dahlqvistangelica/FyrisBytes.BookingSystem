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

    public void SortList()
    {
        //Place holder
    }
    public void SearchLists()
    {
        //PLACE HOLDER
    }
    
    public static void PrintDevelopers(DataManager manager)
    {
        for (int i = 0; i < manager.Developers.Count; i++)
        {
            Console.WriteLine(manager.Developers[i]);
        }
        Console.ReadLine();
    }

   /* public static void NewBooking(DataManager dataManager) //Tai
    {
        Console.WriteLine("--- Ny Bokning ---");
        Console.WriteLine("Start av bokning:");
        DateTime bookingStart = UserInputManager.UserCreateDateTime();
        Console.WriteLine("Slut av bokning:");
        DateTime bookingEnd = UserInputManager.UserCreateDateTime();
        TimeSpan bookedTime = bookingStart - bookingEnd;

        Console.WriteLine("Följande salar är lediga att boka för din angivna tid: ");
        for (int i = 0; i < dataManager.AllRooms.Count; i++)
        {
            if (dataManager.AllBookings.Count > 0)
            {
                if (dataManager.AllBookings[i].BookingSpan == bookedTime)
                continue;
            }
            else
            {
                Console.WriteLine($"[{i + 1}] ID:{dataManager.AllRooms[i].RoomID} Platser:{dataManager.AllRooms[i].SeatAmount} Handikappsanpassad:" +
                    $"{dataManager.AllRooms[i].DisablityAdapted} Nödutgångar:{dataManager.AllRooms[i].EmergencyExits} Whiteboard:{dataManager.AllRooms[i].WhiteBoard}");
            }
        }
        int roomToBook = UserInputManager.UserInputToIntMinus1("\nVälj sal att boka: ");
        Room chosenRoom = dataManager.AllRooms[roomToBook];

        Booking newBooking = new Booking(bookingStart, bookingEnd, chosenRoom);

        dataManager.AllBookings.Add(newBooking);
        //TODO: Lägg in bokningen i listan av bokningar


        bool isSuccess = false; //om bokning lyckadexs = true, misslyckades = falskt
        ChangeBookingSuccessPrintToScreen(isSuccess);

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
    }*/
}

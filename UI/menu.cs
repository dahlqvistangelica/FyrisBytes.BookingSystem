using System;
using Bokningssystem.Interfaces;
using Bokningssystem.Persistence;
using Bokningssystem.Models;
using Bokningssystem.Services;

namespace Bokningssystem.UI
{


    /// <summary>
    /// Statisk klass för att hantera menyer med viss kod inbakad.
    /// </summary>
    public static class Menu
    {
        /// <summary>
        /// Huvudmeny som visas vid uppstart av programmet.
        /// </summary>
        public static void StartUpScreen(DataManager dataManager, BookingManager bookingManager, RoomManager rManager, StoreData storeData)
        {
            int input;
            do
            {
                Console.Clear();
                Console.WriteLine("-- Välkommen till bokningsystemet --");
                input = UserInputManager.UserInputToIntWithLimitations("" +
                    "[1] Hantera salar.\n" +
                    "[2] Hantera bokningar\n" +
                    "[3] Visa utvecklare \n" +
                    "[4] Avsluta programmet \n" +
                    "Välj: ", 4, 0);
                switch (input)
                {
                    case 1:
                        ControllRoomScreen(dataManager, rManager, storeData);
                        break;
                    case 2:
                        ControllBookingScreen(bookingManager);
                        break;
                    case 3:
                        Console.Clear();
                        dataManager.PrintDevelopers();
                        break;
                    case 4:
                        storeData.SaveToFile(dataManager);
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
        /// Meny för att hantera rum.
        /// </summary>
        public static void ControllRoomScreen(DataManager dataManager, RoomManager roomManager, IFileStorageProvider storeData)
        {
            dataManager.SortRoomLists();
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
                    "[5] Ta bort befintligt rum. \n" +
                    "[6] Tillbaka till huvudmenyn\nVälj: ", 6, 0);

                switch (input)
                {
                    case 1:
                        Console.Clear();
                        int seats = RoomManager.GetSeats();
                        if (RoomManager.DetermineRoomType(seats))
                        { dataManager.AllGroupRooms.Add(roomManager.CreateGroupRoom(seats)); }
                        else
                        { dataManager.AllClassRooms.Add(roomManager.CreateClassRoom(seats)); }
                        dataManager.RebuildListAllRooms();
                        storeData.SaveToFile(dataManager);
                        break;
                    case 2:
                        Console.Clear();
                        roomManager.DisplayClassRooms();
                        Console.ReadLine();
                        break;
                    case 3:
                        Console.Clear();
                        roomManager.DisplayGroupRooms();
                        Console.ReadLine();
                        break;
                    case 4:
                        Console.Clear();
                        roomManager.DisplayRooms();
                        Console.ReadLine();
                        break;
                    case 5:
                        Console.Clear();
                        roomManager.DeleteRoom(dataManager);
                        Console.ReadLine();
                        dataManager.RebuildListAllRooms();
                        storeData.SaveToFile(dataManager);
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
                        bookingManager.NewBooking();
                        Console.ReadLine();
                        break;
                    case 2:
                        Console.Clear();
                        bookingManager.ChangeBooking();
                        Console.ReadLine();
                        break;
                    case 3:
                        Console.Clear();
                        bookingManager.DeleteBooking();
                        Console.ReadLine();
                        break;
                    case 4:
                        Console.Clear();
                        bookingManager.ListAllBookings();
                        Console.ReadLine();
                        break;
                    case 5:
                        Console.Clear();
                        bookingManager.BookingSearchYear(UserInputManager.UserInputToInt("Vilket år söker du efter?"));
                        Console.ReadLine();
                        break;
                    default:
                        Console.WriteLine("Ogiltigt val, försök igen.");
                        break;
                }
            } while (input != 6);
        }
    }
}
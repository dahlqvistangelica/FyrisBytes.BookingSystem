using System;
using System.Collections.Generic;

public class BookingManager
{
    public List<Booking> AllBookings { get; set; }
    public List<Room> AllRooms { get; set; }
    public List<string> Developers { get; set; }

    public BookingManager()
    {
        //Laddar data ifrån JSON filer, om filen inte finns blir det en tom lista
        AllBookings = StoreData.LoadFromFile<Booking>("allbooking.json");
        AllRooms = StoreData.LoadFromFile<Room>("allrooms.json");
        Developers = StoreData.LoadFromFile<string>("developers.json");

    }
    public bool SaveAllData()
    {
        //definerar base directory, så att filerna alltid sparas på samma ställe
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        string bookingPath = Path.Combine(basePath, "allbooking.json");
        string roomsPath = Path.Combine(basePath, "allRooms.json");
        string devsPath = Path.Combine(basePath, "developers.json");

        //Sparar all data till respektive fil, bool för användar val om fel händer
        bool savedBooking = StoreData.SaveToFile(AllBookings, bookingPath);
        bool savedRooms = StoreData.SaveToFile(AllRooms, roomsPath);
        bool savedDevs = StoreData.SaveToFile(Developers, devsPath);
        return savedBooking && savedBooking && savedDevs;
    }

    public void SortLists()
    {
        //PLACE HOLDER
    }
    public void SearchLists()
    {
        //PLACE HOLDER
    }
}

    
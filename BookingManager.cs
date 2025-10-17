using System;
using System.Collections.Generic;

public class BookingManager
{
    public List<Room> AllBookings { get; set; }
    public List<Room> AllRooms { get; set; }
    public List<string> Developers { get; set; }

    public BookingManager()
    {
        //Laddar data ifrån JSON filer, om filen inte finns blir det en tom lista
        AllBookings = StoreData.LoadFromFile<Room>("allbooking.json");
        AllRooms = StoreData.LoadFromFile<Room>("allrooms.json");
        Developers = StoreData.LoadFromFile<string>("developers.json");

    }
    public bool SaveAllData()
    {
        //Sparar all data till respektive fil, bool för användar val om fel händer
        bool savedBooking = StoreData.SaveToFile(AllBookings, "allbooking.json");
        bool savedRooms = StoreData.SaveToFile(AllRooms, "allrooms.Json");
        bool savedDevs = StoreData.SaveToFile(Developers, "developers.json");
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

    
using System;

public class BookingManager
{

    public List<Room> AllBookings { get; set; }
    public List<Room> AllRooms { get; set; }
    public List<string> Developers { get; set; }

    public BookingManager()
    {
        //Laddar data ifrån JSON filer, om filen inte finns blir det en tom lista
        AllBookings = SaveData.LoadFromFile<Room>("allbooking.json");
        AllRooms = SaveData.LoadFromFile<Room>("allrooms.json");
        Developers = SaveData.LoadFromFile<string>("developers.json");

    }
    public bool SaveAllData()
    {
        //Sparar all data till respektive fil, bool för användar val om fel händer
        bool savedBooking = SaveData.SaveToFile(AllBookings, "allbooking.json");
        bool savedRooms = SaveData.SaveToFile(AllRooms, "allrooms.Json");
        bool savedDevs = SaveData.SaveToFile(Developers, "developers.json");
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

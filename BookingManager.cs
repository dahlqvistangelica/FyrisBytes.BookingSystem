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
        bool successFulFirst = SaveData.SaveToFile(AllBookings, "allbooking.json");
        bool successFulSecond = SaveData.SaveToFile(AllRooms, "allrooms.Json");
        bool successFulThird = SaveData.SaveToFile(Developers, "developers.json");
        return successFulFirst && successFulSecond && successFulThird;

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

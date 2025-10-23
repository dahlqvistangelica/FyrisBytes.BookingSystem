using System;
using System.Text.Json.Serialization;

public class DataManager
{

    public List<Booking> AllBookings { get; set; }
    public List<GroupRoom> AllGroupRooms { get; set; }
    public List<ClassRoom> AllClassRooms { get; set; }
    public List<string> Developers { get; set; }

    //Gör så att serializer ignorerar denna lista vid sparning
    [JsonIgnore]
    public List<Room> AllRooms { get; set; }


    public DataManager()
    {

        AllBookings = new List<Booking>();
        AllGroupRooms = new List<GroupRoom>();
        AllClassRooms = new List<ClassRoom>();
        Developers = new List<string>();
        AllRooms = new List<Room>();

    }

    /// <summary>
    /// Uppdaterar AllRooms listan med nya rum
    /// </summary>
    public void RebuildAllRooms()
    {
        AllRooms.Clear();
        AllRooms.AddRange(AllGroupRooms);
        AllRooms.AddRange(AllClassRooms);
    }

    /// <summary>
    /// Skriver ut alla utvecklare
    /// </summary>
    /// <param name="manager"></param>
    public void PrintDevelopers()
    {
        for (int i = 0; i < Developers.Count; i++)
        {
            Console.WriteLine(Developers[i]);
        }
        Console.ReadLine();
    }

}

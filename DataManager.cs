using System;
using System.Text.Json.Serialization;

public class DataManager
{

    public List<Booking> AllBookings { get; set; }
    public List<GroupRoom> AllGroupRooms { get; set; }
    public List<ClassRoom> AllClassRooms { get; set; }
    public List<string> Developers { get; set; }
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

    public void RebuildAllRooms()
    {
        AllRooms.Clear();
        AllRooms.AddRange(AllGroupRooms);
        AllRooms.AddRange(AllClassRooms);
    }
    
}

using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Xml.Serialization;
namespace bokningssystem
{
    [JsonDerivedType(typeof(GroupRoom), "Group")]
    [JsonDerivedType(typeof(ClassRoom), "Class")]
    internal class Program
    {
        static void Main(string[] args)
        {
            Menu.StartUpScreen();
        }
        
       
        public interface IRoom
        {
            void AddRoom(Room room);
            bool UpdateRoom(Room room, Room room1);
            bool RemoveRoom(Room room);
            Room? GetRoom(Room room);
        }

    }

}


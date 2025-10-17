using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Xml.Serialization;

    [JsonDerivedType(typeof(GroupRoom), "Group")]
    [JsonDerivedType(typeof(ClassRoom), "Class")]
    internal class Program
    {
        static void Main(string[] args)
        {
            Menu.StartUpScreen();
        }
        
       


    }




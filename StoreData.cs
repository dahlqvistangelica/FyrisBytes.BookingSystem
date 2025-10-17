using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

public class StoreData
{
    //Sparar till JSON
    public static bool SaveToFile<T>(List<T> DataToSave, string filePath)
    {
        try
        {
            //Serialize list till JSON string
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(DataToSave, options);

            //Skriv JSON string till fil
            File.WriteAllText(filePath, jsonString);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("Kunde inte spara data");
            return false;
        }
    }

    //Laddar Från JSON, returnar en lista
    public static List<T> LoadFromFile<T>(string filePath)
    {
        //Kollar om filen finns
        if (!File.Exists(filePath))
        {
            return new List<T>();
        }
        try
        {
            //Läser JSON filen
            string jsonString = File.ReadAllText(filePath);
            var options = new JsonSerializerOptions { WriteIndented = true };
            //Returnar en deserialiserad lista om den lyckas, NULL om den misslyckas
            List<T>? loadedData = JsonSerializer.Deserialize<List<T>>(jsonString, options);
            //Null hantering
            return loadedData ?? new List<T>();
        }


        catch (Exception e)
        {
            Console.WriteLine("Kunde inte ladda filen");
            return new List<T>();
        }

    }

}

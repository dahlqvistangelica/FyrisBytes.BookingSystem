using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

public class StoreData
{
   /* private static readonly JsonSerializerOptions CustomOptions = new()
    {
        TypeInfoResolver = new DefaultJsonTypeInfoResolver
        {
            Modifiers = { AddPolymorphism }
        }
    };

    private static void AddPolymorphism(JsonTypeInfo typeinfo)
    {
        if (ty)
    }*/
    //Sparar till JSON
    public static void SaveToFile(DataManager saveInstance)
    {
        var path = FilePath.GetPath();
        //Omvandlar våran instans av bookingmanager till JSON-sträng som vi kan spara
        var jString = JsonSerializer.Serialize<DataManager>(saveInstance, JsonSerializerOptions.Default);

        //skriver innehållet i jstring till en JSON fil
        File.WriteAllText(path, jString);
    }

    //Läser ifrån JSON
    public static DataManager? ReadFromFile()
    {
        var path = FilePath.GetPath();
        try
        {
            //Kollar om filen existerar, om inte return null
            if (!File.Exists(path)) { return null; }

            //returnar en ny instans av objektet BookingManager, fyllt med värdena ifrån JSON filen
            return JsonSerializer.Deserialize<DataManager>(File.ReadAllText(path));

        }
        catch (Exception ex)
        {
            //Console.WriteLine("Filen kunde inte läsas");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n--- DESERIALIZATION ERROR ---");
            Console.WriteLine($"Filen kunde INTE läsas. Anledning: {ex.Message}");
            // If the error is complex, look at the inner exception:
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inre fel: {ex.InnerException.Message}");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadLine();
            return null;
        }

    }

}
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

//Objekten man vill spara måste ha en tom constructor för att deserializern ska fungera
//JSONderivedtype headers för att ge objektetn du sparar en type så att man kan öppna json filen igen och få korrekt objektyp.

/// <summary>
/// Spara och laddda data metoder
/// </summary>
public class StoreData : IFileStorageProvider
{

    private readonly string _path;
    
    public StoreData(string path)
    {
        _path = path;
    }

    /// <summary>
    /// Sparar till JSON, Objekt måste ha en tom constructor om du vill öppna filen igen
    /// </summary>
    /// <param name="saveInstance"></param>
    public void SaveToFile<T>(T saveInstance)
    {
        //Omvandlar våran instans av bookingmanager till JSON-sträng som vi kan spara
        var jString = JsonSerializer.Serialize<T>(saveInstance, JsonSerializerOptions.Default);

        //skriver innehållet i jstring till en JSON fil
        File.WriteAllText(_path, jString);
    }

    /// <summary>
    /// Läser ifrån JSON, om du vill öppna ett objekt ifrån en JSON-fil måste objektet ha sparats med en tom constructor
    /// </summary>
    /// <returns></returns>
    public T? ReadFromFile<T>() where T : class
    {
        //Kollar om filen existerar, om inte return null
        if (!File.Exists(_path)) { return null; }
        try
        {
            //returnar en ny instans av objektet, fyllt med värdena ifrån JSON filen
            return JsonSerializer.Deserialize<T>(File.ReadAllText(_path));

        }
        catch ( Exception ex)
        {
            Console.WriteLine($"Filen kunde inte läsas: {ex.Message}");
            return null;
        }

    }

}
using System;
using System.IO; 


public static class FilePath
{
    private const string _appFolder = "BokningsSystem";
    private const string _fileName = "dataManager.json";

    /// <summary>
    /// Hämtar en filepath på systemet och gör en folder för datan om det inte finns någon
    /// </summary>
    /// <returns></returns>
    public static string GetPath()
    {
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string directoryPath = Path.Combine(appDataPath, _appFolder);

        //Skapar mappen om den inte finns
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        return Path.Combine(directoryPath, _fileName);
    }
}

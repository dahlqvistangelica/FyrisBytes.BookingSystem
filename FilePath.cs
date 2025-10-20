using System;
using System.IO; 

//Statisk filepath så att filerna alltid sparas på samma ställe
public static class FilePath
{
    private const string _appFolder = "BokngingsSystem";
    private const string _fileName = "bookingManager.json";

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

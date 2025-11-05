using System;
using System.IO;

namespace Bokningssystem.Persistence
{
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
            //Hämtar systemets standardplats för applikationsdata
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            //Kombinerar standardplatsen och mappnamnet
            string directoryPath = Path.Combine(appDataPath, _appFolder);

            //Kontrollerar om mappen finns
            if (!Directory.Exists(directoryPath))
            {
                //Om den inte finns, skapas den
                Directory.CreateDirectory(directoryPath);
            }
            //returnar fullständiga sökvägen till datafilen. Kombinerar mappens sökväg med filnamnet
            return Path.Combine(directoryPath, _fileName);
        }
    }
}

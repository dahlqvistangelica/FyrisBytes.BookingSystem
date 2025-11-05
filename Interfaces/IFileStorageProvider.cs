using System;

namespace Bokningssystem.Interfaces
{
    public interface IFileStorageProvider
    {
        /// <summary>
        /// Skapar en instans av typen T
        /// </summary>
        /// <param name="dataInstance"></param>
        void SaveToFile<T>(T dataInstance);

        /// <summary>
        /// Laddar och returnar en instans av typen T
        /// </summary>
        /// <returns></returns>
        T? ReadFromFile<T>() where T : class;
    }
}
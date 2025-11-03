using Bokningssystem.Interfaces;
using Bokningssystem.Persistence;
using Bokningssystem.Models;
using Bokningssystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bokningssystem.UI;

namespace bokningssystem.Services
{
    public class StartUp
    {
        public static void StartUpScreen()
        {
            var filePath = FilePath.GetPath();
            StoreData storeData = new StoreData(filePath);
            var dataManager = storeData.ReadFromFile<DataManager>();
            if (dataManager != null)
            {
                dataManager.RebuildAllRooms();
            }
            else
            {
                dataManager = new DataManager();
                dataManager.Developers.Add("Olof Brahm");
                dataManager.Developers.Add("Angelica Dahlqvist");
                dataManager.Developers.Add("Filip Gidlöf");
                dataManager.Developers.Add("Tai Lenke Enarsson");
            }
            IBookingRepository repository = dataManager;
            BookingManager bookingManager = new BookingManager(repository, storeData);
            RoomManager rManager = new RoomManager(repository, storeData);

            Menu.StartUpScreen(dataManager, bookingManager, rManager, storeData);
        }
    }
}

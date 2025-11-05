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
            //Hämtar en komplett filväg
            var filePath = FilePath.GetPath();

            //Skapar en ny instans av filhanteringen med vår filväg som inparameter
            StoreData storeData = new StoreData(filePath);

            //Försöker hämta ett DataManager objekt ifrån fil med ReadFromFile metoden
            //Returnar antingen den sparade instansen eller NULL om filen inte hittas
            var dataManager = storeData.ReadFromFile<DataManager>();
            //Om sparad data hittas:
            //Återskapar AllRooms listan
            if (dataManager != null)
            {
                dataManager.RebuildListAllRooms();
            }
            //Om ingen sparad data hittades
            else
            {
                //Skapar en ny instans av DataManager
                dataManager = new DataManager();
                
                //Populerar developers listan med utvecklare
                dataManager.Developers.Add("Olof Brahm");
                dataManager.Developers.Add("Angelica Dahlqvist");
                dataManager.Developers.Add("Filip Gidlöf");
                dataManager.Developers.Add("Tai Lenke Enarsson");
            }

            //Skapar en referens till DataManager som ett IBookingRepository
            IBookingRepository repository = dataManager;

            //Skapar en instans av bookingmanager och injecterar våra verktyg som konstruktorn kräver
            BookingManager bookingManager = new BookingManager(repository, storeData);

            //Skapar en instans av roommanager med samma beroende injicerade
            RoomManager rManager = new RoomManager(repository, storeData);

            Menu.StartUpScreen(dataManager, bookingManager, rManager, storeData);
        }
    }
}

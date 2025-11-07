using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Bokningssystem.Interfaces;
using Bokningssystem.Models;
using Bokningssystem.UI;

namespace Bokningssystem.Services
{
    /// <summary>
    /// Hanterar samtliga bokningar i systemet
    /// </summary>
    public class BookingManager
    {
        //Privata fält för att garantera att ingen utanför klassen kan se eller ändra. Och readonly för att garantera att refernsen inte kan ändras efter satt i konstruktorn.

        //Privata fält för att hålla referensen till DataManager
        private readonly IBookingRepository _repository;

        //Privata fält för att hålla referensen till StoreData
        private readonly IFileStorageProvider _storeData;

        //Dependency injection för att ge privata fälten värdet av de instanser av objekten vi vill
        public BookingManager(IBookingRepository repository, IFileStorageProvider storeData)
        {
            _repository = repository;
            _storeData = storeData;
        }

        // ======================================================================================================================

        /// <summary> 
        /// Skapar ny bokning av valfritt rum
        /// </summary>
        /// <param name="_repository"></param>
        public void NewBooking()
        { 
            Console.Clear();
            Console.WriteLine("--- Ny Bokning ---");
            Console.WriteLine("Start av bokning:");
            DateTime bookingStart = UserInputManager.UserCreateDateTime();

            DateTime bookingEnd;
            bool correctEndTime = false;
            do
            {
                Console.WriteLine("Slut av bokning:");
                bookingEnd = UserInputManager.UserCreateDateTime();
                correctEndTime = bookingEnd > bookingStart ? true : false; //Check om sluttid är efter starttid
                if (correctEndTime == false)
                    DisplayRedMessage("Bokningen kan inte sluta innan den börjar.\nFörsök igen:");

            } while (correctEndTime == false);

            TimeSpan bookedTime = bookingStart - bookingEnd;
            List<Room> availableRooms = MakeListOfAvailableRooms(bookingStart, bookingEnd);
            Console.WriteLine();
            int NrOfAvailableRooms = PrintAndCountListOfAvailableRooms(availableRooms, 
                "Följande salar är lediga att boka för din angivna tid:", 
                "Det finns inga lediga salar för tiden du angivit.");

            if (NrOfAvailableRooms != 0)
            {
                bool isValidID = false;
                int roomToBook;
                Room chosenRoom;
                do
                {
                    roomToBook = UserInputManager.UserInputToInt("\nSkriv in ID för rummet du vill boka: ");
                    chosenRoom = availableRooms.FirstOrDefault(r => r.RoomID == roomToBook)!;
                    if (chosenRoom == null)
                        DisplayRedMessage("Ogiltigt ID, försök igen.");
                    else
                        isValidID = true;
                } while (isValidID == false);

                Booking newBooking = new Booking(bookingStart, bookingEnd, chosenRoom!);
                _repository.AllBookings.Add(newBooking);
                chosenRoom!.AddBooking(newBooking);
                DisplayGreenMessage($"Rum {roomToBook} har bokats.");
            }
            _storeData.SaveToFile(_repository);
        }

        /// <summary>
        /// Tar in start och slut-tid för en bokning och returnerar en lista med samtliga tillgängliga rum.
        /// </summary>
        /// <param name="bookingStart"></param>
        /// <param name="bookingEnd"></param>
        /// <returns></returns>
        public List<Room> MakeListOfAvailableRooms(DateTime bookingStart, DateTime bookingEnd)
        {
            List<Room> availableRooms = new List<Room>();
            foreach (var room in _repository.AllRooms)
            {
                if (room.Bookable(bookingStart, bookingEnd))
                    availableRooms.Add(room);
                else
                    continue;
            }
            return availableRooms;
        }

        /// <summary>
        /// Skriver ut alla lediga rum i en lista och returnerar antalet i listan.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public int PrintAndCountListOfAvailableRooms(List<Room> list, string messageAboveList, string messageIfEmpty)
        {
            if (list.Count() == 0)
                DisplayRedMessage(messageIfEmpty);
            else
            {
                Console.WriteLine(messageAboveList);
                foreach (Room room in list)
                    Console.WriteLine(room.Info);
            }
            return list.Count();
        }

        // ======================================================================================================================

        /// <summary>
        /// Metod för ändring av tidigare inlagd bokning. Ber användaren om ett tidsspann och listar bokningar inom spannet. Val finns om att uppdatera datum, tid eller rum.
        /// </summary>
        /// <param name="_repository"></param>
        public void ChangeBooking()
        {
            Console.WriteLine("--- Uppdatera Bokning ---");
            DateOnly dateOnly = UserInputManager.UserCreateDate();
            TimeOnly timeOnly = new TimeOnly(00, 00, 00);
            DateTime date = new DateTime(dateOnly, timeOnly);

            Console.WriteLine($"Följande bokningar finns i systemet {date:dddd} {date:D}:");
            List<Booking> ListOfBookingsOnDate = new List<Booking>();
            int counter = 1;
            foreach (Booking booking in _repository.AllBookings)
            {
                if (date.Date >= booking.BookingStart.Date && date.Date <= booking.BookingEnd.Date)
                {
                    Console.WriteLine($"[{counter}] {booking.Info.ToString()}");
                    counter++;
                    ListOfBookingsOnDate.Add(booking);
                }
                else
                    continue;
            }
            Console.WriteLine();

            int whichBookingToChange = UserInputManager.UserInputToInt("Ange nummer för bokningen du vill uppdatera (0 för att backa): ");
            if (whichBookingToChange <= 0)
                return;

            Booking bookingToChange = ListOfBookingsOnDate[whichBookingToChange - 1]; //Hämtar från listan med tillgängliga bokningar.
            Booking workingCopyOfBookingToChange = new Booking(bookingToChange);

            //Sparar originalinfo från bokningen
            int originalRoomID = bookingToChange.BookedRoomID;
            TimeSpan originalSpan = bookingToChange.BookingSpan;
            DateTime originalStart = bookingToChange.BookingStart;
            DateTime originalEnd = bookingToChange.BookingEnd;

            //Hittar och hämtar index för valda bokningen i bokningslistan + bokningen i originalrummets lista av bokningar
            int bookingToChangeIndexInAllBookings = _repository.AllBookings.FindIndex(b => b.BookingId == bookingToChange.BookingId);
            int originalRoomIndexInAllRooms = _repository.AllRooms.FindIndex(r => r.RoomID == originalRoomID); 
            int bookingToChangeIndexInOriginalRoom = _repository.AllRooms[originalRoomIndexInAllRooms].roomBookings.FindIndex(b => b.BookingId == bookingToChange.BookingId);


            //bestämmer vad som ska skrivas över i angiven bokning och utför överskrivningen på arbetskopian.
            UpdateBookingWhichChange(workingCopyOfBookingToChange); //Ej return pga referensvariabel.


            int indexOfNewRoom = _repository.AllRooms.FindIndex(r => r.RoomID == workingCopyOfBookingToChange.BookedRoomID);
            if (workingCopyOfBookingToChange.BookedRoomID == originalRoomID) //Om samma rum
            {
                _repository.AllRooms[originalRoomIndexInAllRooms].roomBookings[bookingToChangeIndexInOriginalRoom] = workingCopyOfBookingToChange; //Uppdaterar med nya bokningen i *rummets* bokningslista.

                if ((workingCopyOfBookingToChange.BookingSpan != originalSpan) || (workingCopyOfBookingToChange.BookingStart != originalStart) || (workingCopyOfBookingToChange.BookingEnd != originalStart)) //Om ändrade tider
                    DisplayGreenMessage("Bokningens tider ändrades.");
                else
                    DisplayRedMessage("Ingen ändring gjordes.");
            }
            else //Om nytt rum på bokningen
            {
                //Flyttar bokningen till nya rummets lista och tar bort bokningen från gamla rummets lista.
                _repository.AllRooms[indexOfNewRoom].roomBookings.Add(workingCopyOfBookingToChange);
                _repository.AllRooms[originalRoomIndexInAllRooms].roomBookings.RemoveAt(bookingToChangeIndexInOriginalRoom);
                
                DisplayGreenMessage($"Bokningen flyttades till {workingCopyOfBookingToChange.BookedRoomID} från {originalRoomID}");
            }

            //Uppdaterar med nya bokningen i AllBookings-listan, sparar filen.
            _repository.AllBookings[bookingToChangeIndexInAllBookings] = workingCopyOfBookingToChange; 
            _storeData.SaveToFile(_repository);
        }


        /// <summary>
        /// Ber användaren om vilken ändring av bokningen som ska ske
        /// </summary>
        /// <param name="BookingToChange"></param>
        /// <param name="_repository"></param>
        private Booking UpdateBookingWhichChange(Booking BookingToChange)
        {
            int initialCount = _repository.AllBookings.Count;
            int inputWhatToChange = UserInputManager.UserInputToIntWithLimitations("Vad vill du uppdatera i denna bokning?" +
                    "\n[1] Datum" +
                    "\n[2] Tid" +
                    "\n[3] Sal" +
                    "\n[4] Avbryt", 4, 1);
            Console.WriteLine();

            do
            {
                switch (inputWhatToChange)
                {
                    case 1: return UpdateBookingDate(BookingToChange);
                    case 2: return UpdateBookingTime(BookingToChange);
                    case 3: return UpdateBookingRoom(BookingToChange);
                    case 4: return BookingToChange;
                    default:
                        DisplayRedMessage("Ogiltigt val, försök igen.");
                        break;
                }
            } while (inputWhatToChange != 0);
            return BookingToChange;
        }
        /// <summary>
        /// Metod för att uppdatera datum på redan inlagd bokning.
        /// </summary>
        /// <param name="bookingToChange"></param>
        /// <param name="_repository"></param>
        private Booking UpdateBookingDate(Booking bookingToChange)
        {
            string bookedStart = bookingToChange.BookingStart.ToString();
            string bookedStartDate = $"{bookedStart:yyyy-MM-dd}";
            string bookedStartTime = $"{bookedStart:HH:mm:ss}";
            bookingToChange.BookingStart.Deconstruct(out DateOnly OldStartdate, out TimeOnly oldStartTime);

            Console.WriteLine($"Nuvarande startdag: {bookedStartDate}");
            Console.WriteLine($"Uppdatera startdag för bokning:");
            DateOnly newStartDay = UserInputManager.UserCreateDate();
            DateTime newStart = new DateTime(newStartDay, oldStartTime);

            string bookedEnd = bookingToChange.BookingEnd.ToString();
            string bookedEndDate = $"{bookedEnd:yyyy-MM-dd}";
            string bookedEndTime = $"{bookedEnd:HH:mm:ss}";
            bookingToChange.BookingEnd.Deconstruct(out DateOnly oldEndDate, out TimeOnly oldEndTime);

            Console.WriteLine($"Nuvarande slutdag: {bookedEndDate}");
            Console.WriteLine($"Uppdatera slutdag för bokning:");
            DateOnly newEndDate = UserInputManager.UserCreateDate();
            DateTime newEnd = new DateTime(newEndDate, oldEndTime);

            bookingToChange.BookingStart = newStart;
            bookingToChange.BookingEnd = newEnd;
            bookingToChange.BookingSpan = newEnd - newStart;
            return bookingToChange;
        }
        /// <summary>
        /// Metod för att uppdatera tid på redan inlagd bokning.
        /// </summary>
        /// <param name="whichBookingToChange"></param>
        /// <param name="_repository"></param>
        private Booking UpdateBookingTime(Booking bookingToChange)
        {
            string bookedStart = bookingToChange.BookingStart.ToString();
            string bookedStartDate = $"{bookedStart:yyyy-MM-dd}";
            string bookedStartTime = $"{bookedStart:HH:mm:ss}";
            bookingToChange.BookingStart.Deconstruct(out DateOnly OldStartDate, out TimeOnly oldStartTime);
            Console.WriteLine($"Nuvarande starttid: {bookedStartTime}");
            Console.WriteLine("Uppdatera starttid för bokningen: ");
            TimeOnly newStartTime = UserInputManager.UserCreateTime();
            DateTime newStart = new DateTime(OldStartDate, newStartTime);

            Console.WriteLine($"Uppdatera sluttid för bokning:");
            string bookedEnd = bookingToChange.BookingEnd.ToString();
            string bookedEndDate = $"{bookedEnd:yyyy-MM-dd}";
            string bookedEndTime = $"{bookedEnd:HH:mm:ss}";
            bookingToChange.BookingEnd.Deconstruct(out DateOnly OldEndDate, out TimeOnly oldEndTime);
            Console.WriteLine($"Nuvarande sluttid: {bookedEndTime}");
            Console.WriteLine($"Uppdatera sluttid för bokning:");
            TimeOnly newEndTime = UserInputManager.UserCreateTime();
            DateTime newEnd = new DateTime(OldEndDate, newEndTime);

            bookingToChange.BookingStart = newStart;
            bookingToChange.BookingEnd = newEnd;
            bookingToChange.BookingSpan = newEnd - newStart;
            return bookingToChange;
        }
        /// <summary>
        /// Metod för att uppdatera sal på redan inlagd bokning.
        /// </summary>
        /// <param name="whichBookingToChange"></param>
        /// <param name="_repository"></param>
        private Booking UpdateBookingRoom(Booking whichBookingToChange) //Tai
        {
            DateTime startTime = whichBookingToChange.BookingStart;
            DateTime endTime = whichBookingToChange.BookingEnd;
            Console.WriteLine("Dessa salar finns tillgängliga under tiden för bokningen:");
            List<Room> availableRooms = new List<Room>();
            foreach (var room in _repository.AllRooms)
            {
                if (room.Bookable(startTime, endTime))
                {
                    availableRooms.Add(room);
                    Console.WriteLine(room.Info);
                }
                else
                {
                    continue;   
                }
            }
            if (availableRooms.Count > 0)
            {
                int roomToBook = UserInputManager.UserInputToInt("\nSkriv in salID du vill boka: ");
                Room chosenRoom = availableRooms.FirstOrDefault(r => r.RoomID == roomToBook)!;
                while (chosenRoom == null)
                {
                    DisplayRedMessage("Ogiltigt id");
                    roomToBook = UserInputManager.UserInputToInt("\nSkriv in salID du vill boka: ");
                    chosenRoom = availableRooms.FirstOrDefault(r => r.RoomID == roomToBook)!;
                }
                whichBookingToChange.BookedRoomID = chosenRoom.RoomID;
                return whichBookingToChange;
            }
            else
            {
                DisplayRedMessage("Inga salar tillgängliga under din befintliga bokning.");
                Console.WriteLine("Tryck på valfri tangent för att backa.");
                Console.WriteLine();
                return whichBookingToChange;
            }

        }
        // ======================================================================================================================

        /// <summary>
        /// Listar samtliga bokningar och tar bort ett index i listan
        /// </summary>
        /// <param name="_repository"></param>
        public void DeleteBooking()
        {
            int index = 0;
            Console.WriteLine("[0] AVBRYT");
            foreach (var item in _repository.AllBookings)
            {
                Console.WriteLine($"[{index + 1}]" + item.Info.ToString());
                index++;
            }
            int indexToRemove = UserInputManager.UserInputToIntWithLimitations("Vilken bokning skulle du vilja ta bort?", _repository.AllBookings.Count, 0) - 1;
            if (indexToRemove >= 0)
            {
                Booking chosenBooking = _repository.AllBookings[indexToRemove]; //Hämtar från listan med tillgängliga bokningar.
                int roomId = _repository.AllRooms.FindIndex(r => r.RoomID == chosenBooking.BookedRoomID); //Hittar RoomID för bokningen i rummets lista. 
                if ((_repository.AllBookings.Remove(chosenBooking)) && (_repository.AllRooms[roomId].roomBookings.Remove(chosenBooking)))
                {
                    DisplayGreenMessage("Bokning borttagen.");
                }
            }
            _storeData.SaveToFile(_repository);
        }

        // ======================================================================================================================


        public void ListAllUpcomingBookings()
        {
            List<Booking> SortedBookings = OnlyUpcomingBookingsSort();
            int counter = 0;
            foreach (Booking item in SortedBookings)
            {
                counter++;
                Console.WriteLine($"[{counter}] {item.Info.ToString()}");
            }
            if (counter <= 0)
                Console.WriteLine("Inga bokningar hittades.");
        }

        /// <summary>
        /// Sorterar bokningar efter starttid och visar bara bokningar som kommer hända.
        /// </summary>
        /// <returns></returns>
        public List<Booking> OnlyUpcomingBookingsSort()
        {
            DateTime now = DateTime.Now;
            var SortedBookings = _repository.AllBookings
                .Where(b => b.BookingStart >= now)
                .OrderBy(b => b.BookingStart)
                .ToList();
            return SortedBookings;
        }

        // ======================================================================================================================

        public void ListAllBookings()
        {
            List<Booking> SortedBookings = SortAfterStartTimeAllBookings();
            int counter = 0;
            foreach (Booking item in SortedBookings)
            {
                counter++;
                Console.WriteLine($"[{counter}] {item.Info.ToString()}");
            }
            if (counter <= 0)
                Console.WriteLine("Inga bokningar hittades.");
        }

        /// <summary>
        /// Sorterar ALLA bokningar efter starttid.
        /// </summary>
        /// <returns></returns>
        public List<Booking> SortAfterStartTimeAllBookings()
        {
            var SortedBookings = _repository.AllBookings
                .OrderBy(b => b.BookingStart)
                .ToList();
            return SortedBookings;
        }



        // ======================================================================================================================

        public void BookingSearchYear(int targetYear)
        {
            int counter = 0;
            foreach (Booking item in _repository.AllBookings)
            {
                if (item.BookingStart.Year == targetYear)
                {
                    counter++; 
                    Console.WriteLine($"Bokning nummer {counter} {item.BookingStart.ToString("g")}  {item.BookingEnd.ToString("g")}");
                } 

            }
            if (counter <= 0)
            { Console.WriteLine($"Inga bokningar hittades {targetYear}"); }
        }

        // ======================================================================================================================
        //Hjälpmetoder:


        public void DisplayRedMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public void DisplayGreenMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}


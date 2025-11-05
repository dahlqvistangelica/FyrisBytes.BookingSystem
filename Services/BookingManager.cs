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
        private readonly IBookingRepository _repository;
        private readonly IFileStorageProvider _storeData;

        public BookingManager(IBookingRepository repository, IFileStorageProvider storeData)
        {
            _repository = repository;
            _storeData = storeData;
        }


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
        //Tas bort? 
        public void BookingSearchDate(DateOnly targetDate)
        {
            int counter = 0;
            foreach (Booking item in _repository.AllBookings)
            {
                DateOnly dateonlyItem = DateOnly.FromDateTime(item.BookingStart);
                if (dateonlyItem == targetDate)
                    counter++;
                Console.WriteLine($"[{counter}] {item.BookingStart.ToString("g")}  {item.BookingEnd.ToString("g")}");
            }
        }
        public void ListAllBookings()
        {
            List<Booking> SortedBookings = SortAfterUpcomingAllBookings();
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
        /// Skapar ny bokning av valfritt rum
        /// </summary>
        /// <param name="_repository"></param>
        public void NewBooking() //Tai
        {
            Console.Clear();
            Console.WriteLine("--- Ny Bokning ---");
            Console.WriteLine("Start av bokning:");
            DateTime bookingStart = UserInputManager.UserCreateDateTime();
            Console.WriteLine("Slut av bokning:");
            DateTime bookingEnd = UserInputManager.UserCreateDateTime();

            int initialCount = _repository.AllBookings.Count;
            bool correctEndTime = bookingEnd > bookingStart ? true : false; //Check om sluttid är efter starttid
            while (correctEndTime == false)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Bokningen kan inte sluta innan den börjar.\nFörsök igen:");
                Console.ForegroundColor = ConsoleColor.White;
                bookingEnd = UserInputManager.UserCreateDateTime();
                if (bookingEnd > bookingStart)
                    correctEndTime = true;
            }
            TimeSpan bookedTime = bookingStart - bookingEnd;
            
            List<Room> availableRooms = new List<Room>();
            foreach (var room in _repository.AllRooms)
            {
                if (room.Bookable(bookingStart, bookingEnd))
                {
                    availableRooms.Add(room);
                }
                else
                {
                    continue;
                }
            }
            Console.WriteLine();

            if (availableRooms.Count() == 0)
                Console.WriteLine("Det finns inga lediga salar för tiden du angivit.");
            else
            {
                Console.WriteLine("Följande salar är lediga att boka för din angivna tid: ");
                foreach (Room room in availableRooms)
                {
                    Console.WriteLine(room.Info);
                }

                int roomToBook = UserInputManager.UserInputToInt("\nSkriv in rumID du vill boka: ");
                Room chosenRoom = availableRooms.FirstOrDefault(r => r.RoomID == roomToBook);
                while (chosenRoom == null)
                {
                    Console.WriteLine("Ogiltigt id");
                    roomToBook = UserInputManager.UserInputToInt("\nSkriv in rumID du vill boka: ");
                    chosenRoom = availableRooms.FirstOrDefault(r => r.RoomID == roomToBook);
                }
                Booking newBooking = new Booking(bookingStart, bookingEnd, chosenRoom);
                _repository.AllBookings.Add(newBooking);
                chosenRoom.AddBooking(newBooking);
                Console.WriteLine($"Rum {roomToBook} är bokat.");
            }
            _storeData.SaveToFile(_repository);
        }
        /// <summary>
        /// Metod för ändring av tidigare inlagd bokning. Ber användaren om ett tidsspann och listar bokningar inom spannet. Val finns om att uppdatera datum, tid eller rum.
        /// </summary>
        /// <param name="_repository"></param>
        public void ChangeBooking() //Tai
        {

        Console.WriteLine("--- Uppdatera Bokning ---");
        DateOnly dateOnly = UserInputManager.UserCreateDate();
        TimeOnly timeOnly = new TimeOnly(00, 00, 00);
        DateTime date = new DateTime(dateOnly, timeOnly);
        Console.WriteLine($"Följande bokningar finns i systemet {date:dddd} {date:D}:");
        int counter = 1;
        List<Booking> changeBooking = new List<Booking>();
        foreach (Booking booking in _repository.AllBookings)
        {
            if (date.Date >= booking.BookingStart.Date && date.Date <= booking.BookingEnd.Date)
            {
                Console.WriteLine($"[{counter}] {booking.Info.ToString()}");
                counter++;
                changeBooking.Add(booking);
            }
            else
                continue;
        }
        int whichBookingToChange = UserInputManager.UserInputToInt("Ange nummer för bokningen du vill uppdatera (0 för att backa): ");
        if (whichBookingToChange <= 0)
            return;
        Booking chosenBooking = changeBooking[whichBookingToChange - 1]; //Hämtar från listan med tillgängliga bokningar.
        Booking workingCopy = new Booking(chosenBooking);

            int originalRoomID = chosenBooking.BookedRoomID;
            TimeSpan originalSpan = chosenBooking.BookingSpan;
            DateTime originalStart = chosenBooking.BookingStart;
            DateTime originalEnd = chosenBooking.BookingEnd;

            int allBookingsIndex = _repository.AllBookings.FindIndex(b => b.BookingId == chosenBooking.BookingId); //Index för bokningen i allaBokningslistan 
            int originalRoomIndex = _repository.AllRooms.FindIndex(r => r.RoomID == originalRoomID); //Hittar index för bokningen i rummets lista. 
            int originalRoomBookingIndex = _repository.AllRooms[originalRoomIndex].roomBookings.FindIndex(b => b.BookingId == chosenBooking.BookingId);//Hämtar index för bokningen i rummets lista.


            UpdateBookingWhichChange(workingCopy); //bestämmer vad som ska skrivas över i angiven bokning och utför överskrivningen.

            int newRoomIndex = _repository.AllRooms.FindIndex(r => r.RoomID == workingCopy.BookedRoomID);

            if (workingCopy.BookedRoomID == originalRoomID)
            {
                _repository.AllRooms[originalRoomIndex].roomBookings[originalRoomBookingIndex] = workingCopy; //Uppdaterar den gamla bokningen med det nya.
                if ((workingCopy.BookingSpan != originalSpan) || (workingCopy.BookingStart != originalStart) || (workingCopy.BookingEnd != originalStart))
                {
                    Console.WriteLine("Bokningens tider ändrades.");
                }
                else
                {
                    Console.WriteLine("Ingen ändring gjordes.");
                }
            }
            else
            {


                _repository.AllRooms[newRoomIndex].roomBookings.Add(workingCopy);//Flyttar bokningen till nya rummets lista.
                _repository.AllRooms[originalRoomIndex].roomBookings.RemoveAt(originalRoomBookingIndex);//Tar bort bokningen från gamla rummets lista.
                Console.WriteLine($"Bokningen flyttades till {workingCopy.BookedRoomID} från {originalRoomID}");
            }
            _repository.AllBookings[allBookingsIndex] = workingCopy;        //Uppdaterar den gamla bokningen med den nya
            _storeData.SaveToFile(_repository);
        }
        /// <summary>
        /// Ber användaren om vilken ändring av bokningen som ska ske
        /// </summary>
        /// <param name="BookingToChange"></param>
        /// <param name="_repository"></param>
        private Booking UpdateBookingWhichChange(Booking BookingToChange) //Tai
        {
            int initialCount = _repository.AllBookings.Count;
            int inputWhatToChange = UserInputManager.UserInputToIntWithLimitations("Vad vill du uppdatera i denna bokning?" +
                    "\n[1] Datum" +
                    "\n[2] Tid" +
                    "\n[3] Sal", 3, 1);
            Console.WriteLine();

            do
            {
                switch (inputWhatToChange)
                {
                    case 1: return UpdateBookingDate(BookingToChange);
                    case 2: return UpdateBookingTime(BookingToChange);
                    case 3: return UpdateBookingRoom(BookingToChange);
                    case 0: return BookingToChange;
                    default:
                        Console.WriteLine("Ogiltigt val, försök igen.");
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
        private Booking UpdateBookingDate(Booking bookingToChange) //Tai
        {
            string bookedStart = bookingToChange.BookingStart.ToString();
            string bookedStartDate = $"{bookedStart:yyyy-MM-dd}";
            string bookedStartTime = $"{bookedStart:HH:mm:ss}";
            bookingToChange.BookingStart.Deconstruct(out DateOnly OldStartdate, out TimeOnly oldStartTime);
            Console.WriteLine($"Nuvarande startdag: {bookedStartDate}");
            Console.WriteLine($"Uppdatera startdag för bokning:");
            DateOnly newStartDay = UserInputManager.UserCreateDate();
            DateTime newStart = new DateTime(newStartDay, oldStartTime);
            //DateTime newStart = DateTime.Parse($"{newStartDay:yyyy-MM-dd} {bookedStartTime:HH:mm:ss}");

            Console.WriteLine($"Uppdatera slutdag för bokning:");

            string bookedEnd = bookingToChange.BookingEnd.ToString();
            string bookedEndDate = $"{bookedEnd:yyyy-MM-dd}";
            string bookedEndTime = $"{bookedEnd:HH:mm:ss}";
            bookingToChange.BookingEnd.Deconstruct(out DateOnly oldEndDate, out TimeOnly oldEndTime);
            Console.WriteLine($"Nuvarande slutdag: {bookedEndDate}");
            Console.WriteLine($"Uppdatera slutdag för bokning:");
            DateOnly newEndDate = UserInputManager.UserCreateDate();
            DateTime newEnd = new DateTime(newEndDate, oldEndTime);
            //DateTime newEnd = DateTime.Parse($"{newEndDate:yyyy-MM-dd} {bookedEndTime:HH:mm:ss}");

            bookingToChange.BookingStart = newStart;
            bookingToChange.BookingEnd = newEnd;
            bookingToChange.BookingSpan = newStart - newEnd;
            return bookingToChange;
        }
        /// <summary>
        /// Metod för att uppdatera tid på redan inlagd bokning.
        /// </summary>
        /// <param name="whichBookingToChange"></param>
        /// <param name="_repository"></param>
        private Booking UpdateBookingTime(Booking bookingToChange) //Tai
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
            bookingToChange.BookingSpan = newStart - newEnd;
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
                Room chosenRoom = availableRooms.FirstOrDefault(r => r.RoomID == roomToBook);
                while (chosenRoom == null)
                {
                    Console.WriteLine("Ogiltigt id");
                    roomToBook = UserInputManager.UserInputToInt("\nSkriv in salID du vill boka: ");
                    chosenRoom = availableRooms.FirstOrDefault(r => r.RoomID == roomToBook);
                }
                whichBookingToChange.BookedRoomID = chosenRoom.RoomID;
                return whichBookingToChange;
            }
            else
            {
                Console.WriteLine("Inga salar tillgängliga under din befintliga bokning.");
                Console.WriteLine("Tryck på valfri tangent för att backa.");
                Console.WriteLine();
                return whichBookingToChange;
            }

        }
        /// <summary>
        /// Listar samtliga bokningar och tar bort ett index i listan
        /// </summary>
        /// <param name="_repository"></param>
        public void DeleteBooking()//Tai
        {
            DateTime date = UserInputManager.UserCreateDateTime();
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
                    Console.WriteLine("Bokning borttagen.");
                }
            }
            _storeData.SaveToFile(_repository);
        }

        public List<Booking> SortAfterUpcomingFromNow()
        {
            DateTime now = DateTime.Now;

            var SortedBookings = _repository.AllBookings
                .Where(b => b.BookingStart >= now)
                .OrderBy(b => b.BookingStart)
                .ToList();
            return SortedBookings;
        }
        public List<Booking> SortAfterUpcomingAllBookings()
        {
            DateTime now = DateTime.Now;
            var SortedBookings = _repository.AllBookings
                .OrderBy(b => b.BookingStart)
                .ToList();
            return SortedBookings;
        }
    }
}


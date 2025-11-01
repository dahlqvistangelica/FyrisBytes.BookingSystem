using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

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
    public int ListAllBookings()
    {
        int counter = 0;
        foreach (Booking item in _repository.AllBookings)
        {
            counter++;
            Console.WriteLine($"[{counter}] {item.Info.ToString()}");
        }
        if (counter <= 0)
            Console.WriteLine("Inga bokningar hittades.");

        return counter;
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

            int roomToBook = UserInputManager.UserInputToInt("\nSkriv in salID du vill boka: ");
            Room chosenRoom = availableRooms.FirstOrDefault(r => r.RoomID == roomToBook);
            while (chosenRoom == null)
            {
                Console.WriteLine("Ogiltigt id");
                roomToBook = UserInputManager.UserInputToInt("\nSkriv in salID du vill boka: ");
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
        DateTime date = UserInputManager.UserCreateDateTime();
        Console.WriteLine($"Följande bokningar finns i systemet {date:dddd} {date:D}:");
        int counter = 1;
        List<Booking> changeBooking = new List<Booking>();
        foreach(Booking booking in _repository.AllBookings)
        {
            if (date >= booking.BookingStart && date <= booking.BookingEnd)
            {
                Console.WriteLine($"[{counter}] {booking.Info.ToString()}");
                counter++;
                changeBooking.Add(booking);
            }
            else
                continue;
        }
        //Funkar inte.
        //for (int i = 0; i < _repository.AllBookings.Count; i++)
        //{
        //    if (date > _repository.AllBookings[i].BookingStart && date < _repository.AllBookings[i].BookingEnd)
        //        Console.WriteLine($"[{i + 1}] {_repository.AllBookings[i].Info.ToString()}");
        //    else
        //        continue;
        //}
        int whichBookingToChange = UserInputManager.UserInputToIntMinus1("Ange nummer för bokningen du vill uppdatera (0 för att backa): ");
        if (whichBookingToChange <= 0)
            return;
        Booking chosenBooking = changeBooking[whichBookingToChange];
        _repository.AllBookings.Remove(chosenBooking);
        Booking newBooking = UpdateBookingWhichChange(ref chosenBooking); //bestämmer vad som ska skrivas över i angiven bokning och utför överskrivningen 
        int roomIndex = _repository.AllRooms.FindIndex(r => r.RoomID == newBooking.BookedRoomID);
        _repository.AllRooms[roomIndex].roomBookings.Remove(chosenBooking);
        _repository.AllRooms[roomIndex].roomBookings.Add(newBooking);
        _repository.AllBookings.Add(newBooking);
        
        _repository.SortRoomLists();
        _repository.RebuildAllRooms();
        _storeData.SaveToFile(_repository);
    }
    /// <summary>
    /// Ber användaren om vilken ändring av bokningen som ska ske
    /// </summary>
    /// <param name="whichBookingToChange"></param>
    /// <param name="_repository"></param>
    private Booking UpdateBookingWhichChange(ref Booking whichBookingToChange) //Tai
    {
        int initialCount = _repository.AllBookings.Count;
        int inputWhatToChange = UserInputManager.UserInputToIntWithLimitations("Vad vill du uppdatera i denna bokning?" +
                "\n[1] Datum" +
                "\n[2] Tid" +
                "\n[3] Sal", 3, 1);
        Console.WriteLine();

        return inputWhatToChange switch
        {
            1 => UpdateBookingDate(ref whichBookingToChange),
            2 => UpdateBookingTime(ref whichBookingToChange),
            3 => UpdateBookingRoom(ref whichBookingToChange)
        };
        
    }
    /// <summary>
    /// Metod för att uppdatera datum på redan inlagd bokning.
    /// </summary>
    /// <param name="whichBookingToChange"></param>
    /// <param name="_repository"></param>
    private Booking UpdateBookingDate(ref Booking whichBookingToChange) //Tai
    {
        string bookedStart = whichBookingToChange.BookingStart.ToString();
        string bookedStartDate = $"{bookedStart:yyyy-MM-dd}";
        string bookedStartTime = $"{bookedStart:HH:mm:ss}";
        Console.WriteLine($"Nuvarande startdag: {bookedStartDate}");
        Console.WriteLine($"Uppdatera startdag för bokning:");
        DateOnly newStartDay = UserInputManager.UserCreateDate();
        DateTime newStart = DateTime.Parse($"{newStartDay:yyyy-MM-dd} {bookedStartTime:HH:mm:ss}");

        Console.WriteLine($"Uppdatera slutdag för bokning:");

        string bookedEnd = whichBookingToChange.BookingEnd.ToString();
        string bookedEndDate = $"{bookedEnd:yyyy-MM-dd}";
        string bookedEndTime = $"{bookedEnd:HH:mm:ss}";
        Console.WriteLine($"Nuvarande slutdag: {bookedEndDate}");
        Console.WriteLine($"Uppdatera slutdag för bokning:");
        DateOnly newEndDate = UserInputManager.UserCreateDate();
        DateTime newEnd = DateTime.Parse($"{newEndDate:yyyy-MM-dd} {bookedEndTime:HH:mm:ss}");

       whichBookingToChange.BookingStart = newStart;
       whichBookingToChange.BookingEnd = newEnd;
       whichBookingToChange.BookingSpan = newStart - newEnd;
        return whichBookingToChange;
    }
    /// <summary>
    /// Metod för att uppdatera tid på redan inlagd bokning.
    /// </summary>
    /// <param name="whichBookingToChange"></param>
    /// <param name="_repository"></param>
    private Booking UpdateBookingTime(ref Booking whichBookingToChange) //Tai
    {
        string bookedStart = whichBookingToChange.BookingStart.ToString();
        string bookedStartDate = $"{bookedStart:yyyy-MM-dd}";
        string bookedStartTime = $"{bookedStart:HH:mm:ss}";
        Console.WriteLine($"Nuvarande starttid: {bookedStartTime}");
        Console.WriteLine("Uppdatera starttid för bokningen: ");
        TimeOnly newStartTime = UserInputManager.UserCreateTime(); 
        DateTime newStart = DateTime.Parse($"{bookedStartDate:yyyy-MM-dd} {newStartTime:HH:mm:ss}");

        Console.WriteLine($"Uppdatera sluttid för bokning:");
        string bookedEnd = whichBookingToChange.BookingEnd.ToString();
        string bookedEndDate = $"{bookedEnd:yyyy-MM-dd}";
        string bookedEndTime = $"{bookedEnd:HH:mm:ss}";
        Console.WriteLine($"Nuvarande sluttid: {bookedEndTime}");
        Console.WriteLine($"Uppdatera sluttid för bokning:");
        TimeOnly newEndTime = UserInputManager.UserCreateTime();
        DateTime newEnd = DateTime.Parse($"{bookedEndDate:yyyy-MM-dd} {newEndTime:HH:mm:ss}");

        whichBookingToChange.BookingStart = newStart;
        whichBookingToChange.BookingEnd = newEnd;
        whichBookingToChange.BookingSpan = newStart - newEnd;
        return whichBookingToChange;
    }
    /// <summary>
    /// Metod för att uppdatera sal på redan inlagd bokning.
    /// </summary>
    /// <param name="whichBookingToChange"></param>
    /// <param name="_repository"></param>
    private Booking UpdateBookingRoom(ref Booking whichBookingToChange) //Tai
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
            }
            else
            {
                continue;
            }
        }
        foreach (Room room in availableRooms)
        {
            Console.WriteLine(room.Info);
        }

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
            _repository.AllBookings.RemoveAt(indexToRemove);
        

        _storeData.SaveToFile(_repository);
    }
    /// <summary>
    /// Dublett-metod? Kollar om ett rum är ledigt en viss tid.
    /// </summary>
    /// <param name="wantedStartTime"></param>
    /// <param name="wantedEndTime"></param>
    /// <param name="room"></param>
    /// <returns></returns>
    public bool IsBookable(DateTime wantedStartTime, DateTime wantedEndTime, Room room)
    {
        if (!room.IsAvailable(wantedStartTime, wantedEndTime))
            return false;
        else
            return true;
    }
}


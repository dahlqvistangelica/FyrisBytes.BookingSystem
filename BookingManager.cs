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
                counter++;
            Console.WriteLine($"Bokning nummer {counter} {item.BookingStart.ToString("g")}  {item.BookingEnd.ToString("g")}");
        }
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

        Console.WriteLine("Följande salar är lediga att boka för din angivna tid: ");
        int availableRooms = 0;

        int roomIndex = 0;
        foreach (var room in _repository.AllRooms)
        {
                if (room.IsAvailable(bookingStart, bookingEnd))
                {
                    Console.WriteLine($"[{roomIndex + 1}] {room.Info}");
                    availableRooms++;
                }
                else
                    break;
            roomIndex++;
        }

        if (availableRooms == 0)
            Console.WriteLine("Det finns inga lediga salar för tiden du angivit.");
        else
        {
            int roomToBook = UserInputManager.UserInputToIntMinus1("\nVälj sal att boka: ");
            Room chosenRoom = _repository.AllRooms[roomToBook];

            Booking newBooking = new Booking(bookingStart, bookingEnd, chosenRoom);
            _repository.AllBookings.Add(newBooking);
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

        for (int i = 0; i < _repository.AllBookings.Count; i++)
        {
            if (date > _repository.AllBookings[i].BookingStart && date < _repository.AllBookings[i].BookingEnd)
                Console.WriteLine($"[{i + 1}] {_repository.AllBookings[i].Info.ToString()}");
            else
                continue;
        }
        int whichBookingToChange = UserInputManager.UserInputToIntMinus1("Ange nummer för bokningen du vill uppdatera: ");
        UpdateBookingWhichChange(whichBookingToChange); //bestämmer vad som ska skrivas över i angiven bokning och utför överskrivningen 
        _storeData.SaveToFile(_repository);
    }
    /// <summary>
    /// Ber användaren om vilken ändring av bokningen som ska ske
    /// </summary>
    /// <param name="whichBookingToChange"></param>
    /// <param name="_repository"></param>
    private void UpdateBookingWhichChange(int whichBookingToChange) //Tai
    {
        int initialCount = _repository.AllBookings.Count;
        int inputWhatToChange = UserInputManager.UserInputToIntWithLimitations("Vad vill du uppdatera i denna bokning?" +
                "\n[1] Datum" +
                "\n[2] Tid" +
                "\n[3] Sal", 3, 1);
        Console.WriteLine();

        switch (inputWhatToChange)
        {
            case 1:
                UpdateBookingDate(whichBookingToChange);
                break;
            case 2:
                UpdateBookingTime(whichBookingToChange);
                break;
            case 3:
                UpdateBookingRoom(whichBookingToChange);
                break;
        }
    }
    /// <summary>
    /// Metod för att uppdatera datum på redan inlagd bokning.
    /// </summary>
    /// <param name="whichBookingToChange"></param>
    /// <param name="_repository"></param>
    private void UpdateBookingDate(int whichBookingToChange) //Tai
    {
        string bookedStart = _repository.AllBookings[whichBookingToChange].BookingStart.ToString();
        string bookedStartDate = $"{bookedStart:yyyy-MM-dd}";
        string bookedStartTime = $"{bookedStart:HH:mm:ss}";
        Console.WriteLine($"Nuvarande startdag: {bookedStartDate}");
        Console.WriteLine($"Uppdatera startdag för bokning:");
        DateOnly newStartDay = UserInputManager.UserCreateDate();
        DateTime newStart = DateTime.Parse($"{newStartDay:yyyy-MM-dd} {bookedStartTime:HH:mm:ss}");

        Console.WriteLine($"Uppdatera slutdag för bokning:");

        string bookedEnd = _repository.AllBookings[whichBookingToChange].BookingEnd.ToString();
        string bookedEndDate = $"{bookedEnd:yyyy-MM-dd}";
        string bookedEndTime = $"{bookedEnd:HH:mm:ss}";
        Console.WriteLine($"Nuvarande slutdag: {bookedEndDate}");
        Console.WriteLine($"Uppdatera slutdag för bokning:");
        DateOnly newEndDate = UserInputManager.UserCreateDate();
        DateTime newEnd = DateTime.Parse($"{newEndDate:yyyy-MM-dd} {bookedEndTime:HH:mm:ss}");

        _repository.AllBookings[whichBookingToChange].BookingStart = newStart;
        _repository.AllBookings[whichBookingToChange].BookingEnd = newEnd;
        _repository.AllBookings[whichBookingToChange].BookingSpan = newStart - newEnd;
    }
    /// <summary>
    /// Metod för att uppdatera tid på redan inlagd bokning.
    /// </summary>
    /// <param name="whichBookingToChange"></param>
    /// <param name="_repository"></param>
    private void UpdateBookingTime(int whichBookingToChange) //Tai
    {
        string bookedStart = _repository.AllBookings[whichBookingToChange].BookingStart.ToString();
        string bookedStartDate = $"{bookedStart:yyyy-MM-dd}";
        string bookedStartTime = $"{bookedStart:HH:mm:ss}";
        Console.WriteLine($"Nuvarande starttid: {bookedStartTime}");
        Console.WriteLine("Uppdatera starttid för bokningen: ");
        TimeOnly newStartTime = UserInputManager.UserCreateTime(); 
        DateTime newStart = DateTime.Parse($"{bookedStartDate:yyyy-MM-dd} {newStartTime:HH:mm:ss}");

        Console.WriteLine($"Uppdatera sluttid för bokning:");
        string bookedEnd = _repository.AllBookings[whichBookingToChange].BookingEnd.ToString();
        string bookedEndDate = $"{bookedEnd:yyyy-MM-dd}";
        string bookedEndTime = $"{bookedEnd:HH:mm:ss}";
        Console.WriteLine($"Nuvarande sluttid: {bookedEndTime}");
        Console.WriteLine($"Uppdatera sluttid för bokning:");
        TimeOnly newEndTime = UserInputManager.UserCreateTime();
        DateTime newEnd = DateTime.Parse($"{bookedEndDate:yyyy-MM-dd} {newEndTime:HH:mm:ss}");

        _repository.AllBookings[whichBookingToChange].BookingStart = newStart;
        _repository.AllBookings[whichBookingToChange].BookingEnd = newEnd;
        _repository.AllBookings[whichBookingToChange].BookingSpan = newStart - newEnd;
    }
    /// <summary>
    /// Metod för att uppdatera sal på redan inlagd bokning.
    /// </summary>
    /// <param name="whichBookingToChange"></param>
    /// <param name="_repository"></param>
    private void UpdateBookingRoom(int whichBookingToChange) //Tai
    {
        DateTime startTime = _repository.AllBookings[whichBookingToChange].BookingStart;
        DateTime endTime = _repository.AllBookings[whichBookingToChange].BookingEnd;
        Console.WriteLine("Dessa salar finns tillgängliga under tiden för bokningen:");
        for (int i = 0; i < _repository.AllBookings.Count; i++)
        {
            if (_repository.AllRooms[whichBookingToChange].IsAvailable(startTime, endTime))
                Console.WriteLine($"[{i + 1}] {_repository.AllRooms[i].RoomID}");
            else
                continue;
        }
        int newRoom = UserInputManager.UserInputToIntWithLimitations("Ange vilken sal du vill använda för bokningen: ", _repository.AllBookings.Count, 0);

        _repository.AllBookings[whichBookingToChange].BookedRoom = _repository.AllRooms[newRoom];
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
        int indexToRemove = UserInputManager.UserInputToIntWithLimitations("Vilken bokning skulle du vilja ta bort?", _repository.AllBookings.Count - 1, 0) - 1;
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


using System;
using System.Diagnostics;


public class Booking
{
    public DateTime BookingStart { get; set; }
    private DateTime _BookingStart;
    public DateTime BookingEnds { get; set; }
    private DateTime _BookingEnds;
    private TimeSpan _BookingSpan;
    public TimeSpan BookingSpan
    {
        get { return _BookingSpan; }
        set { _BookingSpan = BookingEnds - BookingStart; }
    }
    //public string Info { get; set; }
    Room BookedRoom { get; set; }
    public Booking(DateTime start, DateTime ends, Room room)
    {
        BookingStart = start;
        BookingEnds = ends;
        BookingSpan = ends - start;
        BookedRoom = room;
    }
    static public void CreateBooking(BookingManager manager, int i)
    {
        if (UserInputManager.UserInputYesNo("Vill du boka ett grupprum?"))
        {
            Booking booking = new Booking(UserInputManager.UserCreateDateTime(), UserInputManager.UserCreateDateTime(), manager.AllGroupRooms[i]);
            manager.AllBookings.Add(booking);
        }
        else
        {
            Booking booking = new Booking(UserInputManager.UserCreateDateTime(), UserInputManager.UserCreateDateTime(), manager.AllClassRooms[i]);
            manager.AllBookings.Add(booking);
        }
    }
    static public void BookingSearch(BookingManager manager, int targetYear)
    {
        int counter = 0;
        foreach (Booking item in manager.AllBookings)
        {
            if (item.BookingStart.Year == targetYear)
                counter++;
                Console.WriteLine($"Bokning nummer {counter} {item.BookingStart.ToString("g")}  {item.BookingEnds.ToString("g")}");
        }
    }
    static public void ListBookings(BookingManager manager)
    {
        foreach (Booking item in manager.AllBookings)
        {
            Console.WriteLine($" Start tid: {item.BookingStart.ToString("g")} Slut tid: {item.BookingEnds.ToString("g")}  Bokningslängd i timmar:{item.BookingSpan.TotalHours} Rummstyp: {item.BookedRoom}");
        }
    }

}

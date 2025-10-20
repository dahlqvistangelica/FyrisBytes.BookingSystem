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
       
        Booking booking = new Booking(UserInputManager.UserCreateDateTime(), UserInputManager.UserCreateDateTime(), manager.AllGroupRooms[i]);
        Console.WriteLine(booking.BookingSpan);
        Console.WriteLine(booking.BookedRoom.RoomID);
    }

}

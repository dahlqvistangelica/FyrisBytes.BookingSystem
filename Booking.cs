using System;
using System.Diagnostics;


public class Booking
{
    public DateTime BookingStart { get; set; }
    public DateTime BookingEnds { get; set; }
    public TimeSpan BookingSpan { get; set; }
    public Room BookedRoom { get; set; }
    
    public Booking(DateTime start, DateTime ends, Room room)
    {
        BookingStart = start;
        BookingEnds = ends;
        BookingSpan = ends - start;
        BookedRoom = room;
    }
    static public void CreateBooking(DataManager dataManager, int i)
    {
        if (UserInputManager.UserInputYesNo("Vill du boka ett grupprum?"))
        {
            Booking booking = new Booking(UserInputManager.UserCreateDateTime(), UserInputManager.UserCreateDateTime(), dataManager.AllGroupRooms[i]);
            dataManager.AllBookings.Add(booking);
        }
        else
        {
            Booking booking = new Booking(UserInputManager.UserCreateDateTime(), UserInputManager.UserCreateDateTime(), dataManager.AllClassRooms[i]);
            dataManager.AllBookings.Add(booking);
        }
    }
}

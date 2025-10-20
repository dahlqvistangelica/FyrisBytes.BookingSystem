using System;
using System.Diagnostics;


public class Booking
{
    public DateTime BookingStart { get; set; }
    public DateTime BookingEnds { get; set; }
    public TimeSpan BookingSpan { get; set; }
    //public string Info { get; set; }
    public Room BookedRoom { get; set; }
    
    public Booking() { }
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
    static public void BookingSearch(DataManager dataManager, int targetYear)
    {
        foreach (Booking item in dataManager.AllBookings)
        {
            if (item.BookingStart.Year == targetYear)
                Console.WriteLine($"{item.BookingStart} {item.BookingEnds}");
        }
    }
    static public void ListBookings(DataManager manager)
    {
        foreach (Booking item in manager.AllBookings)
        {
            string roomType = item.BookedRoom.GetType().Name;
            Console.WriteLine($" Start tid: {item.BookingStart} Slut tid: {item.BookingEnds}  Bokningslängd i timmar:{item.BookingSpan.Hours} Rummstyp: {roomType}");
        }
    }

}

using System;
using System.Diagnostics;


public class Booking
{
    public DateTime BookingStart { get; set; }
    public DateTime BookingEnd { get; set; }
    public TimeSpan BookingSpan { get; set; }
    public string Info
    {
        get => Info;
        set
        {
            Info = "Bokat rumm: " + BookedRoom.RoomID.ToString() + " " + BookingStart.ToString("g") +  " " + BookingEnd.ToString("g") + " " + "(" + BookingSpan.TotalHours.ToString() + ")";
        }
    } 
    public Room BookedRoom { get; set; }
    
    public Booking(DateTime start, DateTime ends, Room room)
    {
        BookingStart = start;
        BookingEnd = ends;
        BookingSpan = ends - start;
        BookedRoom = room;
    }
    //static public void CreateBooking(DataManager dataManager, int i)
    //{
    //    if (UserInputManager.UserInputYesNo("Vill du boka ett grupprum?"))
    //    {
    //        Booking booking = new Booking(UserInputManager.UserCreateDateTime(), UserInputManager.UserCreateDateTime(), dataManager.AllGroupRooms[i]);
    //        dataManager.AllBookings.Add(booking);
    //    }
    //    else
    //    {
    //        Booking booking = new Booking(UserInputManager.UserCreateDateTime(), UserInputManager.UserCreateDateTime(), dataManager.AllClassRooms[i]);
    //        dataManager.AllBookings.Add(booking);
    //    }
    //}
}

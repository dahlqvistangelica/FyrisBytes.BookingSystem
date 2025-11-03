using System;
using System.Diagnostics;
using System.Linq;
using Bokningssystem.UI;
using Bokningssystem.Services;

namespace Bokningssystem.Models
{
    public class Booking
    {
        private static int _nextId = 0;
        public DateTime BookingStart { get; set; }
        public DateTime BookingEnd { get; set; }
        public TimeSpan BookingSpan { get; set; }
        public readonly int _bookingId;
        public int BookingId { get => _bookingId; }
        public string Info
        {
            get
            {
                string Info = $"Bokat rum: {BookedRoomID.ToString()}, {BookingStart.ToString("g")} - {BookingEnd.ToString("g")} ({BookingSpan.ToString("hh':'mm")})";
                return Info;
            }
            set
            {
            }
        }
        public int BookedRoomID { get; set; }

        //Tom constructor för deserializer
        public Booking() { }
        public Booking(DateTime start, DateTime ends, Room room)
        {
            _nextId++;
            _bookingId = _nextId;
            BookingStart = start;
            BookingEnd = ends;
            BookingSpan = ends - start;
            BookedRoomID = room.RoomID;

        }
        public Booking(Booking original)
        {
            this._bookingId = original.BookingId;
            this.BookingStart = original.BookingStart;
            this.BookingEnd = original.BookingEnd;
            this.BookingSpan = original.BookingSpan;
            this.BookedRoomID = original.BookedRoomID;
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
}

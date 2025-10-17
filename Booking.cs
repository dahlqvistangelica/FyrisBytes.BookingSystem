using System;


    internal class Booking
    {
        public DateTime BookingStart { get; set; }
        private DateTime _BookingStart;
        public DateTime BookingEnds { get; set; }
        private DateTime _BookingEnds;
        private TimeSpan _BookingSpan;
        public TimeSpan BookingSpan
        {
            get { return _BookingSpan; }
            set { _BookingSpan = BookingStart - BookingEnds; }
        }
        public string Info { get; set; }
        Room BookedRoom { get; set; }
        public Booking(DateTime start, DateTime ends, Room room)
        {
            BookingStart = start;
            BookingEnds = ends;
            BookedRoom = room;
        }
    }

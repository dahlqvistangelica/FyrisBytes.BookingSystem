using System;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using Bokningssystem.Interfaces;
using Bokningssystem.Services;

namespace Bokningssystem.Models
{
    [JsonDerivedType(typeof(GroupRoom), typeDiscriminator: "GroupRoom")]
    [JsonDerivedType(typeof(ClassRoom), typeDiscriminator: "ClassRoom")]
    /// <summary>
    /// Lokalklass, parent till GroupRoom och ClassRoom. Skapar objekt för varje lokal.
    /// </summary>
    public class Room : IBookable
    {
        public List<Booking> roomBookings { get; set; } = new List<Booking>(); //Lista för att hålla rummets bokningar.
        private int _roomID; //Inkapsling av RoomID för att skydda mot yttre åtkomst.
        public int RoomID
        {
            get => _roomID;
            init { _roomID = value; }
        }
        private int _seatAmount; //Inkapsling av SeatAmout för att skydda mot yttre åtkomst.
        public virtual int SeatAmount
        {
            get => _seatAmount;
            init
            {
                _seatAmount = value;
            }
        }
        public virtual string Info
        {
            get
            {
                string AllProperties = $"Rum id: {this.RoomID} " +
                    $"Antal platser: {this.SeatAmount} " +
                    $"Funktionsanpassat: {this.DisablityAdapted} " +
                    $"Nödutgångar: {this.EmergencyExits} " +
                    $"Whiteboard: {this.WhiteBoard} ";
                return AllProperties;
            }

        }
        public virtual bool DisablityAdapted { get; init; } //Funktionsanpassat rum true = ja, false = nej.
        public int EmergencyExits { get; init; } //Hur många nödutgångar har rummet.
        public bool WhiteBoard { get; init; } //Finns whiteboard.
        public Room(int idNumb, int seats, bool disabilityAccess, int emergencyExits, bool whiteboard)
        {
            RoomID = idNumb;
            SeatAmount = seats;
            DisablityAdapted = disabilityAccess;
            EmergencyExits = emergencyExits;
            WhiteBoard = whiteboard;
            List<Booking> roomBookings = new List<Booking>();
        }
        public Room() : this(0, 1, false, 0, false) { } //Standardvärden för rum.
                                                        //IBookable metoder
        /// <summary>
        /// Returnerar en bool utifrån om rummet går att boka eller inte.
        /// </summary>
        /// <param name="bookingStart"></param>
        /// <param name="bookingEnd"></param>
        /// <param name="manager"></param>
        /// <returns></returns>
        public bool Bookable(DateTime bookingStart, DateTime bookingEnd)
        {
            if (bookingStart >= bookingEnd) //Om starttiden är efter sluttiden.
            { return false; }
            if (!IsAvailable(bookingStart, bookingEnd)) //Kontrollerar tillgängligheten.
            { return false; }
            return true;
        }
        /// <summary>
        /// Tar bort bokning från room's lista.
        /// </summary>
        /// <param name="booking"></param>
        /// <param name="manager"></param>
        public void CancelBooking(Booking booking, DataManager manager)
        {
            roomBookings.Remove(booking);
            manager.AllBookings.Remove(booking);
        }
        /// <summary>
        /// Kontrollerar om det finns en bokning på rummets lista den tiden. Returnerar false om det krockar, annars true.
        /// </summary>
        /// <param name="bookingStart"></param>
        /// <param name="bookingEnd"></param>
        /// <returns></returns>
        public bool IsAvailable(DateTime bookingStart, DateTime bookingEnd)
        {
            foreach (Booking booking in roomBookings)
                if (bookingStart < booking.BookingEnd && bookingEnd > booking.BookingStart) //Kontrollerar krock med annan bokning. Om bokningsstarten är mindre (13.00) än bef. bokningsslut (14.00) och bokningsslut (14.00) är större än bef. bookingsstart (13.00) går det inte att boka. 
                { return false; }
            return true;
        }

        public void AddBooking(Booking booking)
        {
            roomBookings.Add(booking);
        }


    }
    /// <summary>
    /// Subclass av room för grupprum. Ska ha ett id, max 8 sittplatser, kan vara handikappanpassat och ha fler utrymningsvägar.
    /// </summary>
    public class GroupRoom : Room //Grupprum max 8 platser.
    {
        public override string Info
        {
            get
            {
                string AllPropteties = base.Info;
                return AllPropteties;
            }
        }
        public override int SeatAmount
        {
            get => base.SeatAmount;
            init
            {
                if (value > 9)
                    Console.WriteLine("Grupprum kan inte ha fler än 8 platser. ");
                else
                    base.SeatAmount = value;
            }
        }
        public GroupRoom() : base() { }
        public GroupRoom(int idNumb, int seats, bool disabilityAccess, int emergencyExits, bool whiteboard) : base(idNumb, seats, disabilityAccess, emergencyExits, whiteboard) { }
    }
    /// <summary>
    /// Childclass för klassrum(sal), måste ha minst 8 platser, måste vara handikappanpassad. Ska ha ett id, minst 8 sittplatser, handikappanpassning, utrymningsvägar. Kan också ha projector och speakersystem.
    /// </summary>
    public class ClassRoom : Room  //Sal med minst 9 platser, måste vara handikappanpassad.
    {
        public bool Projector { get; init; }
        public bool SpeakerSystem { get; init; }

        public override string Info
        {
            get
            {
                string AllPropteties = base.Info;
                AllPropteties +=
                    $"Projektor: {this.Projector} " +
                    $"Högtalare: {this.SpeakerSystem} ";
                return AllPropteties;
            }

        }

        public override int SeatAmount
        {
            get => base.SeatAmount;
            init
            {
                if (value < 9)
                    Console.WriteLine("Klassrum måste ha minst 8 platser.");
                else
                    base.SeatAmount = value;
            }
        }
        public override bool DisablityAdapted
        {
            get => base.DisablityAdapted;
            init
            {
                if (!value)
                    Console.WriteLine("Klassrum måste vara funktionsanpassade.");
                else
                    base.DisablityAdapted = value;
            }
        }
        public ClassRoom() : base() { }
        public ClassRoom(int idNumb, int seats, bool disabilityAccess, int emergencyExits, bool whiteboard, bool projector, bool speaker) : base(idNumb, seats, disabilityAccess, emergencyExits, whiteboard)
        {
            Projector = projector;
            SpeakerSystem = speaker;
        }

    }
}


using System;

/// <summary>
/// Lokalklass, parent till GroupRoom och ClassRoom. Skapar objekt för varje lokal.
/// </summary>
public class Room
    {
        private int _roomID;
        public int RoomID
        {
            get => _roomID;
            init { _roomID = value; }
        }
        private int _seatAmount;
        public virtual int SeatAmount
        {
            get => _seatAmount;
            init
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Du måste ha minst en plats i ett rum.");
                _seatAmount = value;
            }
        }
        public virtual bool HandicappedAccessible { get; init; }
        public int EmergencyExits { get; init; }

        public bool WhiteBoard { get; init; }
        public Room(int idNumb, int seats, bool handAccess, int emergencyExits, bool whiteboard)
        {
            RoomID = idNumb;
            SeatAmount = seats;
            HandicappedAccessible = handAccess;
            EmergencyExits = emergencyExits;
            WhiteBoard = whiteboard;
        }
        public Room() : this(0, 1, false, 0, false) { }

    public static bool CheckRoomID(int roomID, BookingManager bookingManager)
    {
        foreach (Room room in bookingManager.AllRooms)
            if (roomID == room.RoomID)
                return false;
        return true;
    }
    public static int GetSeats()
    {
        int seats = UserInputManager.UserInputToInt("Hur många platser har rummet?");
        return seats;
    }
    public static int GetID()
    {
        int roomId = UserInputManager.UserInputToInt("Vad har rummet för id?");
        return roomId;
    }
    public static int GetEmergencyExits()
    {
        int emergencyExit = UserInputManager.UserInputToInt("Hur många nödutgångar har rummet?");
        return emergencyExit;
    }
    public static bool GetHandicappedAccess()
    {
        bool handicappedAccess = UserInputManager.UserInputYesNo("Är rummet handikappanpassat?");
        return handicappedAccess;
    }
    public static bool GetWhiteBoard()
    {
        bool whiteboard = UserInputManager.UserInputYesNo("Finns det en whiteboard?");
        return whiteboard;
    }
}
    /// <summary>
    /// Childclass för grupprum, kan ha max 8 platser annars kastar den error. Ska ha ett id, max 8 sittplatser, kan vara handikappanpassat och ha fler utrymningsvägar.
    /// </summary>
    public class GroupRoom : Room //Grupprum max 8 platser.
    {
        public override int SeatAmount
        {
            get => base.SeatAmount;
            init
            {
                if (value > 8)
                    throw new ArgumentOutOfRangeException(nameof(value), "Grupprum kan inte ha fler än 8 sittplatser.");
                base.SeatAmount = value;
            }
        }
        public GroupRoom(int idNumb, int seats, bool handAccess, int emergencyExits, bool whiteboard) : base(idNumb, seats, handAccess, emergencyExits, whiteboard)
        { }
    }
    /// <summary>
    /// Childclass för klassrum(sal), måste ha minst 8 platser, måste vara handikappanpassad. Ska ha ett id, minst 8 sittplatser, handikappanpassning, utrymningsvägar. Kan också ha projector och speakersystem.
    /// </summary>
    public class ClassRoom : Room  //Sal med minst 8 platser, måste vara handikappanpassad.
    {
        public bool Projector { get; init; }
        public bool SpeakerSystem { get; init; }
        public override int SeatAmount
        {
            get => base.SeatAmount;
            init
            {
                if (value < 8)
                    throw new ArgumentOutOfRangeException(nameof(value), "Salar kan inte ha under 8 platser.");
            base.SeatAmount = value;
            }
        }
        public override bool HandicappedAccessible
        {
            get => base.HandicappedAccessible;
            init
            {
                if (!value)
                    throw new ArgumentException("Salar måste vara handikappanpassade. ");
                base.HandicappedAccessible = value;
            }
        }
        public ClassRoom(int idNumb, int seats, bool handAccess, int emergencyExits, bool whiteboard, bool projector, bool speaker) : base(idNumb, seats, handAccess, emergencyExits, whiteboard)
        {
            Projector = projector;
            SpeakerSystem = speaker;
        }
    public static bool GetProjector()
    {
        bool projector = UserInputManager.UserInputYesNo("Finns det projector?");
        return projector;
    }
    public static bool GetSpeaker()
    {
        bool speaker = UserInputManager.UserInputYesNo("Finns det högtalarsystem?");
        return speaker;
    }
}


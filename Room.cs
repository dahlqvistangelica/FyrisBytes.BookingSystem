using System;
using System.Text.Json.Serialization;

/// <summary>
/// Lokalklass, parent till GroupRoom och ClassRoom. Skapar objekt för varje lokal.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(GroupRoom), typeDiscriminator: "group")]
[JsonDerivedType(typeof(ClassRoom), typeDiscriminator: "class")]
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
                
                _seatAmount = value;
            }
        }

        public virtual bool DisablityAdapted { get; init; }
        public int EmergencyExits { get; init; }
        public bool WhiteBoard { get; init; }
        public Room(int idNumb, int seats, bool disabilityAccess, int emergencyExits, bool whiteboard)
        {
            RoomID = idNumb;
            SeatAmount = seats;
            DisablityAdapted = disabilityAccess;
            EmergencyExits = emergencyExits;
            WhiteBoard = whiteboard;
        }
        public Room() : this(0, 1, false, 0, false) { }


   
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
                
                base.SeatAmount = value;
            }
        }
    public GroupRoom() : base() { }
    public GroupRoom(int idNumb, int seats, bool disabilityAccess, int emergencyExits, bool whiteboard) : base(idNumb, seats, disabilityAccess, emergencyExits, whiteboard) { }
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
            base.SeatAmount = value;
            }
        }
        public override bool DisablityAdapted
        {
            get => base.DisablityAdapted;
            init
            {
                
                base.DisablityAdapted = value;
            }
        }
    public ClassRoom() : base (){ }
    public ClassRoom(int idNumb, int seats, bool disabilityAccess, int emergencyExits, bool whiteboard, bool projector, bool speaker) : base(idNumb, seats, disabilityAccess, emergencyExits, whiteboard)
    {
        Projector = projector;
        SpeakerSystem = speaker;
    }
}


using System;

public static class RoomManager
{

    public static Room UserCreateRoom(BookingManager manager)
    {
        int seats = GetSeats();
        if (seats < 8)
        { return CreateGroupRoom(seats, manager); }
        return CreateClassRoom(seats, manager);
    }
    public static int ValidRoomID(BookingManager manager)
    {
        int roomID = GetID();
        bool checkID = CheckRoomID(roomID, manager);
        while (!checkID)
        {
            Console.WriteLine($"ID {roomID} finns redan. Ange ett annat ID.");
            roomID = GetID();
            checkID = CheckRoomID(roomID, manager);
        }
        return roomID;
    }
    public static GroupRoom CreateGroupRoom(int seats, BookingManager manager)
    {
        int roomID = ValidRoomID(manager);
        int emergencyExits = GetEmergencyExits();
        bool disabilityAccess = GetDisabilityAccess();
        bool whiteboard = GetWhiteBoard();
        return new GroupRoom
        {
            RoomID = roomID,
            SeatAmount = seats,
            DisablityAdapted = disabilityAccess,
            EmergencyExits = emergencyExits,
            WhiteBoard = whiteboard,

        };
    }
    public static ClassRoom CreateClassRoom(int seats, BookingManager manager)
    {
        int roomID = ValidRoomID(manager);
        int emergencyExits = GetEmergencyExits();
        bool disablityAccess = GetDisabilityAccess();
        while (!disablityAccess)
        {
            Console.WriteLine("Rum med över 8 sittplatser måste vara tillgänglighetsanpassade.");
            disablityAccess = GetDisabilityAccess();
        }
        bool whiteboard = GetWhiteBoard();
        bool projector = GetProjector();
        bool speaker = GetSpeaker();
        return new ClassRoom
        {
            RoomID = roomID,
            SeatAmount = seats,
            DisablityAdapted = disablityAccess,
            EmergencyExits = emergencyExits,
            WhiteBoard = whiteboard,
            Projector = projector,
            SpeakerSystem = speaker
        };
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
    public static bool GetDisabilityAccess()
    {
        bool disabilityAccess = UserInputManager.UserInputYesNo("Är rummet handikappanpassat?");
        return disabilityAccess;
    }
    public static bool GetWhiteBoard()
    {
        bool whiteboard = UserInputManager.UserInputYesNo("Finns det en whiteboard?");
        return whiteboard;
    }
    public static bool CheckRoomID(int roomID, BookingManager bookingManager)
    {
        foreach (Room room in bookingManager.AllRooms)
            if (roomID == room.RoomID)
                return false;
        return true;
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

    public static void DisplayRooms(BookingManager manager)
    {
        Console.WriteLine("-- Tillgängliga rum --");
        Console.WriteLine("ID \t Platser \t Nödutgångar \t Whiteboard \t Handikappanpassning \t Projector \t Speaker");
        foreach (var room in manager.AllRooms)
        {
            
            Console.Write($"{room.RoomID} \t {room.SeatAmount} \t\t {room.EmergencyExits} \t\t {(room.WhiteBoard ? "ja" : "nej")} \t\t {(room.DisablityAdapted ? "ja" : "nej")} \t \t");
            if (room is ClassRoom classRoom)
            {
                Console.Write($"{(classRoom.Projector ? "ja" : "nej")} \t \t {(classRoom.SpeakerSystem ? "ja" : "nej")}");
            }
                Console.WriteLine();
        }
    }
    public static void EditRooms(BookingManager manager)
    {
        DisplayRooms(manager);
        int roomID = UserInputManager.UserInputToInt("Ange ID på rum du önskar ändra: ");
    }
}
    


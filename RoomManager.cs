using System;

public static class RoomManager
{

    public static Room UserCreateRoom(BookingManager manager)
    {
        int seats = Room.GetSeats();
        if (seats < 8)
        { return CreateGroupRoom(seats, manager); }
        return CreateClassRoom(seats, manager);
    }
    public static GroupRoom CreateGroupRoom(int seats, BookingManager manager)
    {
        int roomID = Room.GetID();
        bool checkID = Room.CheckRoomID(roomID, manager);
        while (!checkID)
        {
            Console.WriteLine($"ID {roomID} finns redan. Ange ett annat ID.");
            roomID = Room.GetID();
            checkID = Room.CheckRoomID(roomID, manager);
        }
        int emergencyExits = Room.GetEmergencyExits();
        bool handicappAccess = Room.GetHandicappedAccess();
        bool whiteboard = Room.GetWhiteBoard();
        return new GroupRoom(roomID, seats, handicappAccess, emergencyExits, whiteboard);
    }
    public static ClassRoom CreateClassRoom(int seats, BookingManager manager)
    {
        int roomID = Room.GetID();
        bool checkID = Room.CheckRoomID(roomID, manager);
        while(!checkID)
        {
            Console.WriteLine($"ID {roomID} finns redan. Ange ett annat ID.");
            roomID = Room.GetID();
            checkID = Room.CheckRoomID(roomID, manager);
        } 
        int emergencyExits = Room.GetEmergencyExits();
        bool handicappAccess = Room.GetHandicappedAccess();
        bool whiteboard = Room.GetWhiteBoard();
        bool projector = ClassRoom.GetProjector();
        bool speaker = ClassRoom.GetSpeaker();
        return new ClassRoom(roomID, seats, handicappAccess, emergencyExits, whiteboard, projector, speaker);
    }
    public static void DisplayRooms(BookingManager manager)
    {
        Console.WriteLine("-- Tillgängliga rum --");
        Console.WriteLine("ID \t Platser \t Nödutgångar \t Whiteboard \t Handikappanpassning \t Projector \t Speaker");
        foreach (var room in manager.AllRooms)
        {
            
            Console.Write($"{room.RoomID} \t {room.SeatAmount} \t\t {room.EmergencyExits} \t\t {(room.WhiteBoard ? "ja" : "nej")} \t\t {(room.HandicappedAccessible ? "ja" : "nej")} \t");
            if (room is ClassRoom classRoom)
            {
                Console.Write($"{(classRoom.Projector ? "ja" : "nej")} \t {(classRoom.SpeakerSystem ? "ja" : "nej")}");
            }
                Console.WriteLine();
        }
    }
}
    


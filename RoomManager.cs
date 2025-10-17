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
        foreach (Room room in manager.AllRooms)
        {
            Console.WriteLine($"ID: {room.RoomID}");
            Console.WriteLine($"Platser: {room.SeatAmount}");
        }
    }
}
    


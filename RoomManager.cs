using System;

public static class RoomManager
{
    
    public static Room UserCreateRoom()
    {
        int seats = UserInputManager.UserInputToInt("Hur många platser har rummet?");
        if (seats < 8)
            return CreateGroupRoom(seats);
        else
            return CreateClassRoom(seats);
    }

    public static GroupRoom CreateGroupRoom(int seats)
    {
        int roomId = UserInputManager.UserInputToInt("Vad har rummet för id?");
        int emergencyExit = UserInputManager.UserInputToInt("Hur många nödutgångar har rummet?");
        bool handicappedAccess = UserInputManager.UserInputYesNo("Är rummet handikappanpassat?");
        bool whiteboard = UserInputManager.UserInputYesNo("Finns det en whiteboard?");
        return new GroupRoom(roomId, seats, handicappedAccess, emergencyExit, whiteboard);
    }
    public static ClassRoom CreateClassRoom(int seats)
    {
        int roomId = UserInputManager.UserInputToInt("Vad har rummet för id?");
        int emergencyExit = UserInputManager.UserInputToInt("Hur många nödutgångar har rummet?");
        bool handicappedAccess = UserInputManager.UserInputYesNo("Är rummet handikappanpassat?");
        bool whiteboard = UserInputManager.UserInputYesNo("Finns det en whiteboard?");
        bool projector = UserInputManager.UserInputYesNo("Finns det projector?");
        bool speaker = UserInputManager.UserInputYesNo("Finns det högtalarsystem?");
        return new ClassRoom(roomId, seats, handicappedAccess, emergencyExit, whiteboard, projector, speaker);
    }
    public static void DisplayRooms(BookingManager manager)
    {
        foreach(Room room in manager.AllRooms)
        { Console.WriteLine($"ID: {room.RoomID}");
          Console.WriteLine($"Platser: {room.SeatAmount}");
        }
    }
    }


using System;

public interface IClassRoom
{
    public bool GetProjector()
    {
        bool projector = UserInputManager.UserInputYesNo("Finns det projector?");
        return projector;
    }
    public bool GetSpeaker()
    {
        bool speaker = UserInputManager.UserInputYesNo("Finns det högtalarsystem?");
        return speaker;
    }
    
}
public interface IRoom

{
    public bool CheckRoomID(int roomID, BookingManager bookingManager)
    {
        foreach (Room room in bookingManager.AllRooms)
            if (roomID == room.RoomID)
                return false;
        return true;
    }
    public int GetSeats()
    {
        int seats = UserInputManager.UserInputToInt("Hur många platser har rummet?");
        return seats;
    }
    public int GetID()
    {
        int roomId = UserInputManager.UserInputToInt("Vad har rummet för id?");
        return roomId;
    }
    public int GetEmergencyExits()
    {
        int emergencyExit = UserInputManager.UserInputToInt("Hur många nödutgångar har rummet?");
        return emergencyExit;
    }
    public bool GetHandicappedAccess()
    {
        bool handicappedAccess = UserInputManager.UserInputYesNo("Är rummet handikappanpassat?");
        return handicappedAccess;
    }
    public bool GetWhiteBoard()
    {
        bool whiteboard = UserInputManager.UserInputYesNo("Finns det en whiteboard?");
        return whiteboard;
    }
}


using System;

public class RoomManager
{

    public void UserCreateRoom()
    {
        int seats = UserInputManager.UserInputToInt("Hur många platser har rummet?");
        if (seats < 8)
            CreateGroupRoom(seats);
        else
            CreateClassRoom(seats);
    }
    public GroupRoom CreateGroupRoom(int seats)
    {
        int roomId = UserInputManager.UserInputToInt("Vad har rummet för id?");
        int emergencyExit = UserInputManager.UserInputToInt("Hur många nödutgångar har rummet?");
        bool handicappedAccess = UserInputManager.UserInputYesNo("Är rummet handikappanpassat?", "Du måste svara ja eller nej.");
        bool whiteboard = UserInputManager.UserInputYesNo("Finns det en whiteboard?", "Du måste svara ja eller nej.");

        var room = new GroupRoom(roomId, seats, handicappedAccess, emergencyExit, whiteboard);
        return room;
    }
    public ClassRoom CreateClassRoom(int seats)
    {
        int roomId = UserInputManager.UserInputToInt("Vad har rummet för id?");
        int emergencyExit = UserInputManager.UserInputToInt("Hur många nödutgångar har rummet?");
        bool handicappedAccess = UserInputManager.UserInputYesNo("Är rummet handikappanpassat?", "Du måste svara ja eller nej.");
        bool whiteboard = UserInputManager.UserInputYesNo("Finns det en whiteboard?", "Du måste svara ja eller nej.");
        bool projector = UserInputManager.UserInputYesNo("Finns det projector?", "Du måste svara ja eller nej.");
        bool speaker = UserInputManager.UserInputYesNo("Finns det högtalarsystem?", "Du måste svara ja eller nej.");
        ClassRoom room = new ClassRoom(roomId, seats, handicappedAccess, emergencyExit, whiteboard, projector, speaker);
        return room;
    }
}

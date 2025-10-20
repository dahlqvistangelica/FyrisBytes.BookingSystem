using System;

public static class RoomManager
{
    /// <summary>
    /// Skapa room utifrån vilken underklass som är bäst. Kontroll mot manager.
    /// </summary>
    /// <param name="manager"></param>
    /// <returns></returns>
    /// 
    public static bool DetermineRoomType(int seats)
    {
        if (seats < 9) //Om användaren anger 8 eller mindre platser så returnar den true annars false för att avgöra vilket typ av rum som ska skapas.
        { return true; }
        else return false;
    }
    /// <summary>
    /// Tar in id från användaren, kontrollerar det och tvingar användaren att ange ett ID som inte finns om det redan finns. 
    /// </summary>
    /// <param name="manager"></param>
    /// <returns></returns>
    public static int ValidRoomID(BookingManager manager)
    {
        int roomID = GetID();
        bool checkID = CheckRoomID(roomID, manager);
        while (checkID)
        {
            Console.WriteLine($"ID {roomID} finns redan. Ange ett annat ID.");
            roomID = GetID();
            checkID = CheckRoomID(roomID, manager);
        }
        return roomID;
    }
    /// <summary>
    /// Jämför det inmatade roomID med de som finns i listorna. Returns true om id finns.
    /// </summary>
    /// <param name="roomID"></param>
    /// <param name="bookingManager"></param>
    /// <returns></returns>
    public static bool CheckRoomID(int roomID, BookingManager bookingManager)
    {
        bool classrooms = false;
        bool grouprooms = false;
        foreach (ClassRoom room in bookingManager.AllClassRooms)
            if (roomID == room.RoomID)
                classrooms = true;
        foreach (GroupRoom room in bookingManager.AllGroupRooms)
            if (roomID == room.RoomID)
                grouprooms = true;
        if (grouprooms == true || classrooms == true)
            return true;
        else return false;
    }
    public static int CheckEmergencyExits()
    {
        int emergencyExits;
        do
        {
            emergencyExits = GetEmergencyExits();
            {
                if (emergencyExits <= 0)
                    Console.WriteLine("Klassrum måste innehålla minst en utrymningsväg.");
            }
        } while (emergencyExits <= 0);
        return emergencyExits;
    }
    /// <summary>
    /// Skapa grupprum, tar emot parametrar för platser och från BookingManager för att kunna validera id. 
    /// </summary>
    /// <param name="seats"></param>
    /// <param name="manager"></param>
    /// <returns></returns>
    public static GroupRoom CreateGroupRoom(int seats, BookingManager manager)
    {
        int roomID = ValidRoomID(manager);
        int emergencyExits = GetEmergencyExits();
        bool disabilityAccess = GetDisabilityAccess();
        bool whiteboard = GetWhiteBoard();
        return new GroupRoom(roomID, seats, disabilityAccess, emergencyExits, whiteboard);
    }
    /// <summary>
    /// Skapar klassrum, tar emot parametrar för antal platser och manager för att validera id.
    /// </summary>
    /// <param name="seats"></param>
    /// <param name="manager"></param>
    /// <returns></returns>
    public static ClassRoom CreateClassRoom(int seats, BookingManager manager)
    {
        int roomID = ValidRoomID(manager);
        int emergencyExits = CheckEmergencyExits();

        bool disablityAccess = GetDisabilityAccess();
        while (!disablityAccess)
        {
            Console.WriteLine("Rum med över 8 sittplatser måste vara tillgänglighetsanpassade.");
            disablityAccess = GetDisabilityAccess();
        }
        bool whiteboard = GetWhiteBoard();
        bool projector = GetProjector();
        bool speaker = GetSpeaker();
        return new ClassRoom(roomID, seats, disablityAccess, emergencyExits, whiteboard, projector, speaker);

    }

    //Metoder för att ta emot input för att skapa room.
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
        DisplayClassRooms(manager);
        Console.WriteLine();
        DisplayGroopRooms(manager);
    }
    public static void DisplayClassRooms(BookingManager manager)
    {
        Console.WriteLine("-- Befintliga klassrum --");
        Console.WriteLine("ID \t Platser \t Nödutgångar \t Whiteboard \t Handikappanpassning \t Projector \t Speaker");
        
        foreach (ClassRoom room in manager.AllClassRooms)
        {

            Console.Write($"{room.RoomID} \t {room.SeatAmount} \t\t {room.EmergencyExits} \t\t {(room.WhiteBoard ? "ja" : "nej")} \t\t {(room.DisablityAdapted ? "ja" : "nej")} \t\t\t {(room.Projector ? "ja" : "nej")} \t\t {(room.SpeakerSystem ? "ja" : "nej")}");

            Console.WriteLine();
        }
    }
    public static void DisplayGroopRooms(BookingManager manager)
    {
        
        Console.WriteLine("-- Befintliga grupprum --");
        Console.WriteLine("ID \t Platser \t Nödutgångar \t Whiteboard \t Handikappanpassning \t ");
        foreach (GroupRoom room in manager.AllGroupRooms)
        {

            Console.Write($"{room.RoomID} \t {room.SeatAmount} \t\t {room.EmergencyExits} \t\t {(room.WhiteBoard ? "ja" : "nej")} \t\t {(room.DisablityAdapted ? "ja" : "nej")} \t \t");
            Console.WriteLine();
        }
    }
    public static void EditRooms(BookingManager manager)
    {
        DisplayRooms(manager);
        int roomID = UserInputManager.UserInputToInt("Ange ID på rum du önskar ändra: ");
        if(CheckRoomID(roomID, manager))
        { foreach (GroupRoom room in manager.AllGroupRooms)
                if (roomID == room.RoomID)
                { Console.WriteLine("Vad vill du ändra?"); }
        }

    }
}
    


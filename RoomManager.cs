using System;

public class RoomManager
{
    /// <summary>
    /// Skapar room utifrån vilken underklass som är bäst.
    /// </summary>
    /// <param name="manager"></param>
    /// <returns></returns>
    /// 
    private readonly IBookingRepository _repository;
    private readonly IFileStorageProvider _storeData;

    public RoomManager(IBookingRepository repository, IFileStorageProvider storeData)
    {
        _repository = repository;
        _storeData = storeData;
    }
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
    public int ValidRoomID()
    {
        int roomID = GetID();
        bool checkID = CheckRoomID(roomID);
        while (checkID)
        {
            Console.WriteLine($"ID {roomID} finns redan. Ange ett annat ID.");
            roomID = GetID();
            checkID = CheckRoomID(roomID);
        }
        return roomID;
    }
    /// <summary>
    /// Jämför det inmatade roomID med de som finns i listorna. Returns true om id finns.
    /// </summary>
    /// <param name="roomID"></param>
    /// <param name="dataManager"></param>
    /// <returns></returns>
    public bool CheckRoomID(int roomID)
    {
        bool classrooms = false;
        bool grouprooms = false;
        bool allrooms = false;
        foreach (Room room in _repository.AllRooms)
            if (roomID == room.RoomID)
                allrooms = true;
        foreach (ClassRoom room in _repository.AllClassRooms)
            if (roomID == room.RoomID)
                classrooms = true;
        foreach (GroupRoom room in _repository.AllGroupRooms)
            if (roomID == room.RoomID)
                grouprooms = true;
        if ((grouprooms == true && allrooms == true) || (classrooms == true && allrooms== true))
            return true;
        else return false;
    }
    /// <summary>
    /// Kontrollerar att klassrum innehåller minst en utrymningsväg.
    /// </summary>
    /// <returns></returns>
    public int CheckEmergencyExits()
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
    /// Skapa grupprum, tar emot parametrar för platser och från DataManager för att kunna validera id. 
    /// </summary>
    /// <param name="seats"></param>
    /// <param name="manager"></param>
    /// <returns></returns>
    public GroupRoom CreateGroupRoom(int seats)
    {
        int roomID = ValidRoomID();
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
    public ClassRoom CreateClassRoom(int seats)
    {
        int roomID = ValidRoomID();
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
    public void DisplayRooms()
    {
        DisplayClassRooms();
        Console.WriteLine();
        DisplayGroopRooms();
    }
    public void DisplayClassRooms()
    {
        const int ID_WIDTH = -10;
        const int AMOUNT_WIDTH = 15;
        const int LONG_BOOL_WIDTH = -20;
        const int BOOL_WIDTH = -15;
        Console.WriteLine($"{"===== KLASSRUM =====", 50}");
        Console.WriteLine($"{"Rum ID",ID_WIDTH*-1}{"Sittplatser",AMOUNT_WIDTH}{"Nödutgångar",AMOUNT_WIDTH}{"Whiteboard",BOOL_WIDTH*-1}{"Handikappanpassat",LONG_BOOL_WIDTH*-1}{"Projektor", BOOL_WIDTH*-1}{"Högtalare",BOOL_WIDTH*-1}");
        
        foreach (ClassRoom room in _repository.AllClassRooms)
        {

            Console.Write($"{room.RoomID, ID_WIDTH*-1}{room.SeatAmount, AMOUNT_WIDTH}{room.EmergencyExits, AMOUNT_WIDTH}{(room.WhiteBoard ? "ja" : "nej"), BOOL_WIDTH*-1}{(room.DisablityAdapted ? "ja" : "nej"), LONG_BOOL_WIDTH*-1}{(room.Projector ? "ja" : "nej"),BOOL_WIDTH*-1}{(room.SpeakerSystem ? "ja" : "nej"), BOOL_WIDTH*-1}");

            Console.WriteLine();
        }
    }
    public void DisplayGroopRooms()
    {
        const int ID_WIDTH = -10;
        const int AMOUNT_WIDTH = 15;
        const int LONG_BOOL_WIDTH = -20;
        const int BOOL_WIDTH = -15;

        Console.WriteLine($"{"===== GRUPPRUM =====", 50}");
        Console.WriteLine($"{"Rum ID",ID_WIDTH * -1}{"Sittplatser",AMOUNT_WIDTH}{"Nödutgångar",AMOUNT_WIDTH}{"Whiteboard",BOOL_WIDTH * -1}{"Handikappanpassat",LONG_BOOL_WIDTH * -1}");
        foreach (GroupRoom room in _repository.AllGroupRooms)
        {

            Console.Write($"{room.RoomID,ID_WIDTH * -1}{room.SeatAmount,AMOUNT_WIDTH}{room.EmergencyExits,AMOUNT_WIDTH}{(room.WhiteBoard ? "ja" : "nej"),BOOL_WIDTH * -1}{(room.DisablityAdapted ? "ja" : "nej"),LONG_BOOL_WIDTH * -1}");
            Console.WriteLine();
        }
    }
    /// <summary>
    /// Metod för att kunna ändra tillagda rum.
    /// </summary>
    public void DeleteRoom(DataManager manager)
    {
        DisplayRooms();
        Console.WriteLine();
        int idToRemove = UserInputManager.UserInputToInt("Ange ID på rum du önskar ta bort (0 om du ångrar dig): ");
        if (idToRemove == 0)
        { Console.WriteLine("Återgår till menyn."); return; }
        int removedCount = _repository.AllRooms.RemoveAll(r => r.RoomID == idToRemove);
        if(removedCount > 0)
        {   _repository.AllGroupRooms.RemoveAll(r=>r.RoomID == idToRemove);
            _repository.AllClassRooms.RemoveAll(r => r.RoomID == idToRemove);
            Console.WriteLine($"Rum med id {idToRemove} togs bort ur systemet.");
        }
        else
        { Console.WriteLine($"Fel: Rum med id {idToRemove} hittades inte."); }
        _storeData.SaveToFile(_repository);

    }
    #region InputMetoder
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
    #endregion

}



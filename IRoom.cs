using System;

public interface IRoom
{
    void AddRoom(Room room);

    bool UpdateRoom(Room room, Room room1);
    bool RemoveRoom(Room room);
    Room? GetRoom(Room room);

}

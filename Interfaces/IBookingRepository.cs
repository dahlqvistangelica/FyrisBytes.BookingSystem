using System;
using Bokningssystem.Models;

namespace Bokningssystem.Interfaces
{
    /// <summary>
    /// Interface för vad data hanteraren måste innehålla
    /// </summary>
    public interface IBookingRepository
    {
        List<Booking> AllBookings { get; }
        List<Room> AllRooms { get; }
        List<GroupRoom> AllGroupRooms { get; }
        List<ClassRoom> AllClassRooms { get; }
        void RebuildListAllRooms();
        void SortRoomLists();
    }
}
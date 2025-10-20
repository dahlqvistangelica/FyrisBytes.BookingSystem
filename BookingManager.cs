using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

public class BookingManager
{
    public List<Booking> AllBookings { get; set; }
    public List<GroupRoom> AllGroupRooms { get; set; }
    public List<ClassRoom> AllClassRooms { get; set; }
    public List<string> Developers { get; set; }


    public BookingManager()
    {

        AllBookings = new List<Booking>();
        AllGroupRooms = new List<GroupRoom>();
        AllClassRooms = new List<ClassRoom>();
        Developers = new List<string>();

    }


    //Sortering för bokad tid, bara place holder har inte tänkt längre än två sekunder på denna
    public void SortList()
    {
        for (int i = 0; i < AllBookings.Count; i++)
        {
            for (int j = 0; j < AllBookings.Count - 1 - i; j++)
            {
                if (AllBookings[j].BookingStart > AllBookings[j + 1].BookingStart)
                {
                    Booking tempState = AllBookings[j];
                    AllBookings[j] = AllBookings[j + 1];
                    AllBookings[j + 1] = tempState;
                }

            }
        }
    }
    public void SearchLists()
    {
        //PLACE HOLDER
    }
}

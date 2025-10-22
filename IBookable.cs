using System;


public interface IBookable //Tai
{
    public void NewBooking(DataManager dataManager);
    public void ListAllBookings(DataManager dataManager);
    public void ChangeBooking(DataManager datamanager);
    public void DeleteBooking(DataManager dataManager);
    public void ListAllBookingsWithinTimeframe();
    public bool IsBookable(DateTime wantedStartTime, DateTime wantedEndTime);
}
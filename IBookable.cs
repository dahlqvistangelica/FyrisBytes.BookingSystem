using System;


public interface IBookable
{
    public bool Book(DateTime bookingStart, DateTime bookingEnd, DataManager manager);
    public void CancelBooking(Booking booking, DataManager manager);
    public bool IsAvailable(DateTime bookingStart, DateTime bookingEnd);
    //public void NewBooking(DataManager dataManager);
    //public void ListAllBookings(DataManager dataManager);
    //public void ChangeBooking(DataManager datamanager);
    //public void DeleteBooking(DataManager dataManager);
    //public void ListAllBookingsWithinTimeframe();
    //public bool IsBookable(DateTime wantedStartTime, DateTime wantedEndTime);
}
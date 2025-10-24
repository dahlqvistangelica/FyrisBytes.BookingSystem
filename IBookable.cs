using System;


public interface IBookable
{
    public bool Book(DateTime bookingStart, DateTime bookingEnd, DataManager manager);
    public void CancelBooking(Booking booking, DataManager manager);
    public bool IsAvailable(DateTime bookingStart, DateTime bookingEnd);

}
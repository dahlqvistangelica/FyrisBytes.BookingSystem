using System;


public interface IBookable
{
    public bool Bookable(DateTime bookingStart, DateTime bookingEnd);
    public void CancelBooking(Booking booking, DataManager manager);
    public bool IsAvailable(DateTime bookingStart, DateTime bookingEnd);

}
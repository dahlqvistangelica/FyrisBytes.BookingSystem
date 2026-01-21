## Room Booking System - Group Project
This application was developed as a collaborative group project during the foundational C# course. The goal was to build a robust system for managing hotel rooms, bookings, and customer interactions while applying core Object-Oriented Programming (OOP) principles.

### My Contributions

While we worked as a team, my primary technical focus was on the backend structure and data management.

- **Room & RoomManager:** Main responsibility for the core logic regarding room administration.
- **BookingManager:** Developed solutions for filtering and room search functionality.
- **Advanced Logic:** Implemented the Copy Constructor in the Booking class to ensure data integrity and proper object handling.

### Known Limitations & Future Improvements

As this was a learning project within a set timeframe, we identified some areas for further development:

- **Room Deletion Impact:** Currently, removing a room also deletes all associated bookings. A more robust solution would allow the user to reassign these bookings to a different room before deletion.
- **Booking Management at Scale: **When removing a booking, the system currently lists all existing records. For larger organizations, a search or filter function within the deletion menu would be necessary for better usability.
- **Search Edge Cases: **The search function for "booking year" does not currently display bookings that span across multiple years (e.g., a stay from Dec 2025 to Jan 2026). Refining this logic would be the next step to ensure total search accuracy.

Readme
Vid start kommer användaren till huvudmeny. 
Där får användaren välja att hantera lokaler eller bokningar genom att ange nr 1/2. 
Nr 3 avslutar programmet och sparar datan. 

Undermeny - Hantera lokaler
Under hantera lokaler kan användaren lägga till och visa rum.
När användaren lägger till rum avgör programmet vilken rumstyp som användaren försöker skapa genom att först ta reda på antal platser i rummet. 
Därefter får användaren svara på olika frågor för att spara info om rummet. 
Användaren kan bara lägga till grupprum eller klassrum vilka har olika villkor som programmet meddelar om användaren bryter mot under tillägg. 
Användaren kan visa alla rum och rummen visas då i två tabeller. 

Undermeny - Hantera bokningar


Ansvarsfördelning:
Angelica Dahlqvist - Room och RoomManager
Olof Brahm - Persistence, DataManager, IBookingRepository, IFileStorageProvider
Tai Lenke Enarsson - BookingManager och IBooking
Filip Gidlöf - Booking och UserInputManager

Readme
Vid start kommer användaren till huvudmeny. 
Där får användaren välja att hantera lokaler eller bokningar genom att ange nr 1/2. 
Variabeln exitnumber avslutar programmet och ser till att datan sparas. 

Undermeny - Hantera lokaler
Under hantera lokaler kan användaren lägga till, ta bort och visa rum.
När användaren lägger till rum avgör programmet vilken rumstyp som användaren försöker skapa genom att först ta reda på antal platser i rummet. 
Därefter får användaren svara på olika frågor för att skapa info om rummet som sparas.
Användaren kan bara lägga till grupprum eller klassrum vilka har olika villkor som programmet meddelar om användaren bryter mot under tillägg. 
Användaren kan visa alla rum och rummen visas då i två tabeller, eller visa de olika rumstyperna separat. Rummen är sorterade efter rums-id från lägsta till högsta nr. 
Användaren kan även ta bort befintliga rum och då tas även befintliga bokningar på det rummet bort ur systemet. 

Undermeny - Hantera bokningar
Här kan användaren lägga till, ändra, ta bort och visa bokningar. 
Användaren visas tillgängliga rum att boka när användaren angett datum och tid för önskad bokning och efter valt rum bekräftas bokningen. 
När användaren uppdaterar ett bokning får användaren först välja mellan tillgängliga bokningar det datum den önskat samt därefter välja bokning att ändra.
När användaren valt bokning att ändra ges hen valet att ändra datum, tid eller sal och därefter får användaren knappa in sina ändringar och programmet bekräftar om ändringarna genomförst eller inte.
Alla bokningar listas utifrån två val, alla kommande bokningar eller alla befintliga bokningar i systemet. Dessa är även sorterade utifrån datum och tid i kronologisk ordning. 
För att ta bort en bokning visas alla bokningar och användaren får därefter välja vilken bokning hen önskar ta bort. Därefter bekräftas att bokningen blivit borttagen ur systemet. 

Sparning
Programmet skapar en mapp i användarens AppData\Roaming som heter BokningsSystem och i denna läggs filen dataManager där all data kring rum och bokningar sparas.

Begränsningar:
Vid borttagning av rum tas även befintliga bokningar på rummet bort, önskvärt kanske hade varit att dessa bokningar fick tilldelas ett nytt rum av användaren. 
Vid borttagning av bokning listas alla befintliga bokningar inlagda i systemet. Vid en större organisation behövs det en funktion för att filtrera bokningarna på något sätt. 
Vid sökning av bokningsår så visas inte bokningar som sträcker sig före till efter det valda året som sökts.

Ansvarsfördelning:
Angelica Dahlqvist - Room och RoomManager
Olof Brahm - Persistence, DataManager, IBookingRepository, IFileStorageProvider
Tai Lenke Enarsson - BookingManager och IBooking
Filip Gidlöf - Booking och UserInputManager

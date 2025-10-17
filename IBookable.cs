using System;
using BookingSystem.BookingManager;

namespace IBookableInterface
{
    public interface IBookable //Tai
    {
        static void NewBooking() //Tai
        {
            var bookingList = BookingManager.AllBookings;
            Console.WriteLine("--- Ny Bokning ---");
            Console.WriteLine("Start av bokning:");
            DateTime bookingStart = UserInputManager.UserCreateDateTime();
            Console.WriteLine("Slut av bokning:");
            DateTime bookingEnd = UserInputManager.UserCreateDateTime();

            Console.WriteLine("Följande salar är lediga att boka för din angivna tid: ");
            for (int i = 0; i < salarLista.Count; i++)
            {
                if (salarLista[i] == "")//tiden som valts.booked == true)
                    continue;
                else
                {
                    Console.WriteLine($"[{i + 1}] {salarLista[i]}"); //byt ut salarLista[i] till namn på salen, med dess egenskaper

                }
            }
            int roomToBook = UserInputManager.UserInputToIntMinus1("\nVälj sal att boka: ");


            //TODO: Lägg in bokningen i listan av bokningar


            bool isSuccess = false; //om bokning lyckadexs = true, misslyckades = falskt
            ChangeBookingSuccessPrintToScreen(isSuccess);

        }

        static void ChangeBooking(List<Booking> AllBookings) //Tai
        {

            Console.WriteLine("--- Uppdatera Bokning ---");
            DateTime date = UserInputManager.UserCreateDateTime();
            Console.WriteLine($"Följande bokningar finns i systemet {date:dddd} {date:D}:");
            for (int i = 0; i < AllBookings.Count; i++) //TODO: Nå korrekt bokningslista
            {
                Console.WriteLine($"[{i + 1}] ({AllBookings[i].timespan}) {AllBookings[i].name}:  \"{AllBookings[i].note}\"");
                //TODO: Nå korrekt bokning med egenskaper
                //Format: "datum starttid-sluttid ({tid}h {tid}min) Salnamn: "Notering""

            }
            int whichBookingToChange = UserInputManager.UserInputToIntMinus1("Ange nummer för bokningen du vill uppdatera: ");
            UpdateBookingWhichChange(whichBookingToChange); //bestämmer vad som ska skrivas över i angiven bokning och utför överskrivningen 

            bool isSuccess = false; //TODO: Lyckades ändringen? Misslyckades ändringen?
            ChangeBookingSuccessPrintToScreen(isSuccess);
        }
        static void UpdateBookingWhichChange(int whichBookingToChange) //Tai
        {
            int inputWhatToChange = UserInputManager.UserInputToIntWithLimitations("Vad vill du uppdatera i denna bokning?" +
                    "\n[1] Datum" +
                    "\n[2] Tid" +
                    "\n[3] Sal", 3, 1);

            bool success = (inputWhatToChange) switch
            {
                1 => success = UpdateBookingDate(whichBookingToChange),
                2 => success = UpdateBookingTime(whichBookingToChange),
                3 => success = UpdateBookingRoom(whichBookingToChange),
                _ => false
            };

            ChangeBookingSuccessPrintToScreen(success);
        }
        static bool UpdateBookingDate(int whichBookingToChange) //Tai
        {
            DateTime date = UserInputManager.UserCreateDateTime();
            //TODO: replace i bokningslista[whichBookingToChange]
            bool success = true; //TODO: confirm if success
            return success;
        }
        static bool UpdateBookingTime(int whichBookingToChange) //Tai
        {
            Console.Write("Ange ny starttid för bokningen: ");
            DateTime bookingStart = UserInputManager.UserCreateDateTime();
            Console.Write("Ange hur länge bokningen ska vara: ");
            DateTime bookingEnd = UserInputManager.UserCreateDateTime();
            int userconfirm = UserInputManager.UserInputToInt
                (
                    "Bokningen är satt till:" +
                    "\nBokningens start: " + bookingStart.ToString() +
                    "\nBokningens slut: " + bookingEnd.ToString() +
                    "\nBekräfta tid:" +
                    "\n[1] Bekräfta" +
                    "\n[2] Ignorera"
                );
            if (userconfirm == 1)
            {
                //replace i bokningslista[whichBookingToChange]
                //Lägg in ny tid i bokningslistan[whichBookingToChange]
            }
            bool success = true; //confirm if success
            return success;
        }
        static bool UpdateBookingRoom(int whichBookingToChange) //Tai
        {
            List<string> AllRooms = new List<string> { "Sal1", "Sal2", "Sal3", "Sal4" };
            //TODO - ^^^ ändra till korrekt lista för salarna ^^^
            Console.WriteLine("Dessa salar finns tillgängliga under tiden för bokningen:");
            for (int i = 0; i < AllRooms.Count; i++)
            {
                /*if (salarLista[i+1] is booked)
                    continue;
                else
                    Console.WriteLine($"[{i+1}] {bokning}"); 
                */
            }
            Console.Write("Ange vilken sal du vill använda för bokningen: ");
            int chooseRoom = UserInputManager.UserInputToIntWithLimitations("Ange vilken sal du vill använda för bokningen: ", AllRooms.Count, 0);

            //replace bokningslista[whichBookingToChange] (salen) med (chooseRoom)

            bool success = true; //confirm if success
            return success;
        }

        static void DeleteBooking()//Tai
        {
            DateTime date = UserInputManager.UserCreateDateTime();
            BookingsPrintList(date);
        }
        static void ListAllBookingsWithinTimeframe() //Tai
        {
            //TODO
        }
        static bool IsBookable() //Tai
        {
            //TODO
            bool answer = false;
            return answer;
        }

        static void BookingsPrintList(DateTime date) //Tai
        {
            foreach (var item in AllBookings)
            {
                if (item == null)
                    continue;
                else
                    Console.WriteLine($"[{AllBookings.SpotInList}] {AllBookings.date} ({AllBookings.timespan}) {AllBookings.name}:  \"{AllBookings.note}\"");
            }
            //TODO Korrigera AllBookings formatet ^
        }
        public static void ChangeBookingSuccessPrintToScreen(bool success) //Tai
        {
            if (success == true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Ändringen i systemet utfördes korrekt.");
                Console.ForegroundColor = ConsoleColor.White;

            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ändringen misslyckades, försök igen.");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
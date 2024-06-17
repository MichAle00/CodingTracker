using System.Globalization;

namespace CodingTracker;

internal class UserInput
{
    internal void Menu()
    {
        Console.Clear();
        bool closeApp = true;

        while (closeApp)
        {
            Console.WriteLine("Main Menu:");
            Console.WriteLine("What do you like to do?");
            Console.WriteLine("Type 0 to Close Application");
            Console.WriteLine("Type 1 to View All Records");
            Console.WriteLine("Type 2 to Insert Record");
            Console.WriteLine("Type 3 to Delete Record");
            Console.WriteLine("Type 4 to Update Record");
            Console.WriteLine("--------------------------------------\n");

            string? command = Console.ReadLine();

            switch (command)
            {
                case "0":
                    Console.WriteLine("Bye!");
                    break;

                case "1":
                    //CRUD.viewRecords();
                    break;

                case "2":
                    break;

                case "3":
                    break;

                case "4":
                    break;

                default:
                    break;
            }
        }
    }

    internal DateTime[] GetDates()
    {
        Console.Clear();

        Console.WriteLine("Please input your start date: (Format dd-MM-yy hh:mm)");
        string? start = Console.ReadLine();

        Console.WriteLine("Please input your end date: (Format dd-MM-yy hh:mm)");
        string? end = Console.ReadLine();

        DateTime startDate;
        DateTime endDate;
        

        while (!DateTime.TryParseExact(start, "dd-MM-yy HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out startDate))
        {
            Console.WriteLine("Invalid date. Format is: dd-MM-yy HH:mm. Please try again");
            start = Console.ReadLine();
        }

        while (!DateTime.TryParseExact(end, "dd-MM-yy HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out endDate))
        {
            Console.WriteLine("Invalid date. Format is: dd-MM-yy HH:mm. Please try again");
            end = Console.ReadLine();
        }

        return new DateTime[] { startDate, endDate };
    }
}

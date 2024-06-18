using Spectre.Console;
using System.Globalization;
using System.Runtime.InteropServices;

namespace CodingTracker;

internal class UserInput
{
    internal void Menu()
    {
        CRUD op = new();
        op.CreateDB();

        bool closeApp = true;

        while (closeApp)
        {
            //AnsiConsole.Clear();
            var panel = new Panel("[skyblue3]Type 0 to Close Application[/]" +
                "\n[royalblue1]Type 1 to View All Records[/]" +
                "\n[springgreen3]Type 2 to Insert Record[/]" +
                "\n[red]Type 3 to Delete Record[/]" +
                "\n[turquoise2]Type 4 to Update Record[/]");
            panel.Header = new PanelHeader("Main Menu:");
            panel.Padding = new Padding(1, 1, 1, 1);
            panel.BorderColor<Panel>(Color.Aquamarine3);
            panel.Border = BoxBorder.Rounded;
            panel.Expand = false;
            AnsiConsole.Write(panel);


            string? command = Console.ReadLine();

            switch (command)
            {
                case "0":
                    Console.WriteLine("Bye!");
                    break;

                case "1":
                    CRUD.viewRecords();
                    break;

                case "2":
                    op.addToTable();
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

        string? start = AnsiConsole.Ask<string>("[darkturquoise]Please input your start date[/] [red](Format dd-MM-yy hh:mm)[/]:");

        string? end = AnsiConsole.Ask<string>("[darkturquoise]Please input your end date[/] [red](Format dd-MM-yy hh:mm)[/]:");

        DateTime startDate;
        DateTime endDate;
        

        while (!DateTime.TryParseExact(start, "dd-MM-yy HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out startDate))
        {
            start = AnsiConsole.Ask<string>("[darkturquoise]Invalid start date,please try again[/] [red](Format dd-MM-yy hh:mm)[/]:");
        }

        while (!DateTime.TryParseExact(end, "dd-MM-yy HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out endDate))
        {
            end = AnsiConsole.Ask<string>("[darkturquoise]Invalid end date,please try again[/] [red](Format dd-MM-yy hh:mm)[/]:");
        }

        return new DateTime[] { startDate, endDate };
    }
}

using Spectre.Console;
using System.Globalization;

namespace CodingTracker;

internal class UserInput
{
    internal void Menu()
    {
        CRUD.CreateDB();

        bool closeApp = true;

        while (closeApp)
        {
            var menu = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("What do you want to do with the records?")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to select an option)[/]")
            .AddChoices(new[] {
                "View All", "Add",
                "Delete", "Update",
                "Leave",
            }));

            switch (menu)
            {
                case "Leave":
                    AnsiConsole.MarkupLine("[chartreuse2]Bye![/]");
                    closeApp = false;
                    break;

                case "View All":
                    CRUD.ViewRecords();
                    break;

                case "Add":
                    CRUD.AddToTable();
                    break;

                case "Delete":
                    CRUD.DeleteRecords();
                    break;

                case "Update":
                    CRUD.UpdateRecords();
                    break;

                default:
                    AnsiConsole.MarkupLine("[red]That's not a valid option![/]");
                    break;
            }
        }
    }

    internal static DateOnly GetDate()
    {
        string? date = AnsiConsole.Ask<string>("[darkturquoise]Please input the date[/] [red](Format dd-MM-yy)[/]:");

        DateOnly Date;

        while (!DateOnly.TryParseExact(date, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out Date))
        {
            date = AnsiConsole.Ask<string>("[darkturquoise]Invalid date,please try again[/] [red](Format dd-MM-yy)[/]:");
        }
        
        return Date;
    }

    internal static TimeSpan[] GetTime()
    {
        string? start = AnsiConsole.Ask<string>("[darkturquoise]Please input your start time[/] [red](Format hh:mm)[/]:");

        string? end = AnsiConsole.Ask<string>("[darkturquoise]Please input your end time[/] [red](Format hh:mm)[/]:");

        TimeSpan startTime;
        TimeSpan endTime;

        while (!TimeSpan.TryParseExact(start, "g", new CultureInfo("en-US"),TimeSpanStyles.None, out startTime))
        {
            start = AnsiConsole.Ask<string>("[darkturquoise]Incorrect start time, Please try again[/] [red](Format hh:mm)[/]:");
        }

        while (!TimeSpan.TryParseExact(end, "g", new CultureInfo("en-US"), TimeSpanStyles.None, out endTime))
        {
            end = AnsiConsole.Ask<string>("[darkturquoise]Incorrect end time, Please try again[/] [red](Format hh:mm)[/]:");
        }

        return new TimeSpan[] { startTime, endTime };
    }

    internal static int GetId()
    {
        CRUD.ViewRecords();

        string? id = AnsiConsole.Ask<string>("Type the Id of the record you want: ");

        return Convert.ToInt32(id);
    }
}

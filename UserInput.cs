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
                "View All", "Add", "Start session now",
                "Delete", "Update", "Filter",
                "Report", "Leave",
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

                case "Start session now":
                    CRUD.Session();
                    break;

                case "Filter":
                    //CRUD.FilterBy();
                    break;

                case "Report":
                    CRUD.Report();
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

        while (!TimeSpan.TryParseExact(end, "g", new CultureInfo("en-US"), TimeSpanStyles.None, out endTime) || !Validation.CorrectTime(startTime, endTime))
        {
            end = AnsiConsole.Ask<string>("[darkturquoise]Incorrect end time, end time must be in [red](Format hh:mm)[/] and can't be lower than the start time, Please try again[/] :");
        }

        return new TimeSpan[] { startTime, endTime };
    }

    internal static int GetId()
    {
        CRUD.ViewRecords();

        string? id = AnsiConsole.Ask<string>("Type the Id of the record you want to modify: ");

        while(!Int32.TryParse(id, out _))
        {
            AnsiConsole.MarkupLine("[slateblue1]Thay's not a valid number, please try again[/]");
            id = AnsiConsole.Ask<string>("Type the Id of the record you want: ");
        }


        return Convert.ToInt32(id);
    }

    internal static void GoalsMenu()
    {
        var menu = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("What do you want to do?")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to select an option)[/]")
            .AddChoices(new[] {
                "Set Goal", "See progress"
            }));

        switch (menu)
        {
            case "Set Goal":
                CRUD.SetGoals();
                break;

            case "See progress":
                CRUD.Progress();
                break;
        }
    }

    internal static string[] Period()
    {
        var period = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title("Select the time period you want for the goal")
        .PageSize(10)
        .MoreChoicesText("[grey](Move up and down to select an option)[/]")
        .AddChoices(new[] {
            "Week", "Month"
        }));

        string? quantity = AnsiConsole.Ask<string>($"How many {period}s do you want to set?: ");

        string? time = AnsiConsole.Ask<string>("How many hours?: ");

        return new string[] { period, quantity, time };
    }
}

using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Globalization;
using CodingTracker.Model;
using Dapper;
using Spectre.Console;

namespace CodingTracker;

internal class CRUD
{
    static readonly string? con = ConfigurationManager.AppSettings.Get("connectionString");

    internal static void CreateDB()
    {
        using (var connection = new SQLiteConnection(con))
        {
            connection.Open();

            string sql = @"CREATE TABLE IF NOT EXISTS codingSessions (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    Start TEXT,
                    End TEXT,
                    Duration TEXT)";

            var cmd = connection.Execute(sql);

            connection.Close();
        }
    }

    internal static void AddToTable()
    {
        AnsiConsole.Clear();
        DateOnly date = UserInput.GetDate();
        TimeSpan[] times = UserInput.GetTime();
        string duration = CalculateDuration(times[0], times[1]);

        using (var connection = new SQLiteConnection(con))
        {
            connection.Open();

            string sql =
                @$"INSERT INTO codingSessions (Date, Start, End, Duration)
                    VALUES ('{date.ToString("dd-MM-yyyy")}', '{times[0]}', '{times[1]}', '{duration}')";

            var cmd = connection.Execute(sql);

            connection.Close();

            AnsiConsole.MarkupLine("[springgreen3_1]Records added succesfully![/]");
        }
    }

    internal static string CalculateDuration(TimeSpan start, TimeSpan end)
    {
        TimeSpan dur = end - start;
        return dur.ToString("c");
    }

    internal static void ViewRecords()
    {
        AnsiConsole.Clear();
        using (var connection = new SQLiteConnection(con))
        {
            connection.Open();

            IDataReader reader = connection.ExecuteReader("SELECT * FROM codingSessions");
            
            List<CodingSession> sessions = new List<CodingSession>();

            var exists = connection.ExecuteScalar<bool>("SELECT COUNT(*) FROM codingSessions");

            if (!exists)
            {
                AnsiConsole.MarkupLine("[red]This table is empty![/]");
                connection.Close();
                return;
            }

            while (reader.Read())
            {
                sessions.Add(
                new CodingSession
                {
                    Id = reader.GetInt32(0),
                    Date = DateOnly.Parse(reader.GetString(1)),
                    StartTime = TimeSpan.Parse(reader.GetString(2)),
                    EndTime = TimeSpan.Parse(reader.GetString(3)),
                    Duration = TimeSpan.Parse(reader.GetString(4))
                }); ;
            }

            connection.Close();

            var table = new Table().Centered();
            table.BorderColor<Table>(Color.Cyan3);
            table.Border = TableBorder.Horizontal;

            AnsiConsole.Live(table)
            .Start(ctx =>
            {
                table.AddColumn("[steelblue1_1]ID[/]");
                ctx.Refresh();
                Thread.Sleep(850);

                table.AddColumn("[mediumturquoise]Date[/]");
                ctx.Refresh();
                Thread.Sleep(850);

                table.AddColumn("[blue]Start Time[/]");
                ctx.Refresh();
                Thread.Sleep(850);

                table.AddColumn("[red]End Time[/]");
                ctx.Refresh();
                Thread.Sleep(850);

                table.AddColumn("[darkmagenta_1]Duration[/]");
                ctx.Refresh();
                Thread.Sleep(850);

                foreach (var session in sessions)
                {
                    table.AddRow($"[steelblue1_1]{session.Id}[/]", $"[mediumturquoise]{session.Date}[/]" , $"[blue]{session.StartTime}[/]", $"[red]{session.EndTime}[/]", $"[darkmagenta_1]{session.Duration}[/]");
                    ctx.Refresh();
                    Thread.Sleep(850);
                }
            });
        }
    }

    internal static void DeleteRecords()
    {
        int id = UserInput.GetId();

        if (!Validation.IsInTable(id))
        {
            AnsiConsole.MarkupLine("[lightslateblue]The Id doesn't exists in the table[/]");
            return;
        }

        using (var connection = new SQLiteConnection(con))
        {
            connection.Open();

            var sql = $"DELETE FROM codingSessions WHERE Id = {id}";

            var cmd = connection.Execute(sql);

            connection.Close();

            AnsiConsole.MarkupLine("[mediumpurple3_1]Records deleted succesfully![/]");
        }
    }

    internal static void UpdateRecords()
    {
        int id = UserInput.GetId();
        DateOnly date = UserInput.GetDate();
        TimeSpan[] time = UserInput.GetTime();
        string? duration = CalculateDuration(time[0], time[1]);

        if (!Validation.IsInTable(id))
        {
            AnsiConsole.MarkupLine("[lightslateblue]The Id doesn't exists in the table[/]");
            return;
        }

        using (var connection = new SQLiteConnection(con))
        {
            connection.Open();

            string sql = @$"UPDATE codingSessions SET 
                Date = '{date.ToString("dd-MM-yyyy")}',
                Start = '{time[0]}',
                End = '{time[1]}',
                Duration = '{duration}'
            WHERE Id = {id}";

            var cmd = connection.Execute(sql);

            connection.Close();

            AnsiConsole.MarkupLine("[springgreen3_1]Records updated succesfully![/]");
        }
    }

    internal static void Session()
    {
        AnsiConsole.Clear();

        Stopwatch stopwatch = new();
        TimeSpan start = DateTime.Now.TimeOfDay;
        stopwatch.Start();
        DateOnly today = DateOnly.FromDateTime(DateTime.Today);
        TimeSpan end = DateTime.Now.TimeOfDay; ;
        string? duration;
        bool stop = false;

        while (!stop)
        {
            string? menu = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Press stop when you're finished:")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to select an option)[/]")
            .AddChoices(new[] {
                "STOP"
            }));

            if (menu == "STOP")
            {
                stopwatch.Stop();
                end = DateTime.Now.TimeOfDay;
                stop = true;
            }
        }

        duration = stopwatch.Elapsed.ToString(@"hh\:mm\:ss");

        using (var connection = new SQLiteConnection(con))
        {
            connection.Open();

            string sql = @$"INSERT INTO codingSessions (Date, Start, End, Duration)
                    VALUES ('{today.ToString("dd-MM-yyyy")}', '{start.ToString(@"hh\:mm\:ss")}', '{end.ToString(@"hh\:mm\:ss")}', '{duration}')";

            var cmd = connection.Execute(sql);

            connection.Close();

            AnsiConsole.MarkupLine("[springgreen3_1]Records added succesfully![/]");
        }
    }

    //internal static void FilterBy()
    //{
    //    throw NotImplementedException();
        //AnsiConsole.Clear();
        //string[] filters = UserInput.Filter();

        //using (var connection = new SQLiteConnection(con))
        //{
        //    connection.Open();

        //    List<CodingSession> sessions = new();

        //    var exists = connection.ExecuteScalar<bool>("SELECT COUNT(*) FROM codingSessions");

        //    if (!exists)
        //    {
        //        AnsiConsole.MarkupLine("[red]This table is empty![/]");
        //        connection.Close();
        //        return;
        //    }

        //    var reader = connection.ExecuteReader($"SELECT * FROM codingSessions");

        //    while (reader.Read())
        //    {
        //        sessions.Add(
        //        new CodingSession
        //        {
        //            Id = reader.GetInt32(0),
        //            Date = DateOnly.Parse(reader.GetString(1)),
        //            StartTime = TimeSpan.Parse(reader.GetString(2)),
        //            EndTime = TimeSpan.Parse(reader.GetString(3)),
        //            Duration = TimeSpan.Parse(reader.GetString(4))
        //        }); ;
        //    }

        //    connection.Close();

        //    foreach (var session in sessions)
        //    {
                
        //        AnsiConsole.WriteLine($"{session.Id} {session.Date} {session.StartTime} {session.EndTime} {session.Duration}");
        //    }

            //switch (filters[0])
            //{
            //    case "Day":
            //        sessions.Sort(.Date);
            //        break;

            //    case "Week":
                    
            //        break;

            //    case "Month":
                    
            //        break;

            //    case "Year":
                    
            //        break;
            //}
        //}

    internal static void Report()
    {
        AnsiConsole.Clear();
        using (var connection = new SQLiteConnection(con))
        {
            connection.Open();

            IDataReader reader = connection.ExecuteReader("SELECT * FROM codingSessions");

            List<CodingSession> sessions = new();

            var exists = connection.ExecuteScalar<bool>("SELECT COUNT(*) FROM codingSessions");

            if (!exists)
            {
                AnsiConsole.MarkupLine("[red]This table is empty![/]");
                connection.Close();
                return;
            }

            while (reader.Read())
            {
                sessions.Add(
                new CodingSession
                {
                    Id = reader.GetInt32(0),
                    Date = DateOnly.Parse(reader.GetString(1)),
                    StartTime = TimeSpan.Parse(reader.GetString(2)),
                    EndTime = TimeSpan.Parse(reader.GetString(3)),
                    Duration = TimeSpan.Parse(reader.GetString(4))
                }); ;
            }

            connection.Close();

            System.Globalization.Calendar myCal = new CultureInfo("en-US").Calendar;

            Dictionary<int, List<TimeSpan>> weeks = new();

            Dictionary<int, List<TimeSpan>> months = new();

            foreach (var session in sessions)
            {
                int weekNum = myCal.GetWeekOfYear(session.Date.ToDateTime(new TimeOnly(0, 0)), new CultureInfo("en-US").DateTimeFormat.CalendarWeekRule, new CultureInfo("en-US").DateTimeFormat.FirstDayOfWeek);
                int monthNum = myCal.GetMonth(session.Date.ToDateTime(new TimeOnly(0, 0)));
                if (weeks.ContainsKey(weekNum))
                {
                    weeks[weekNum].Add(session.Duration);
                }
                else
                {
                    weeks.Add(weekNum, new() { session.Duration });
                }

                if (months.ContainsKey(monthNum))
                {
                    months[monthNum].Add(session.Duration);
                }
                else
                {
                    months.Add(monthNum, new() { session.Duration});
                }
            }

            TimeSpan sum = new();

            var table = new Table().Centered();
            table.BorderColor<Table>(Color.Cyan3);
            table.Border = TableBorder.Horizontal;

            AnsiConsole.Live(table)
            .Start(ctx =>
            {
                table.AddColumn("[steelblue1_1]Week #[/]");
                ctx.Refresh();
                Thread.Sleep(850);

                table.AddColumn("[mediumturquoise]Total hours[/]");
                ctx.Refresh();
                Thread.Sleep(850);

                table.AddColumn("[blue]Average hours[/]");
                ctx.Refresh();
                Thread.Sleep(850);

                foreach (var week in weeks)
                {
                    for (int i = 0; i < week.Value.Count; i++)
                    {
                        sum += week.Value[i];
                    }
                    table.AddRow($"[steelblue1_1]{week.Key}[/]", $"[mediumturquoise]{sum}[/]", $"[blue]{sum / week.Value.Count}[/]");
                    ctx.Refresh();
                    Thread.Sleep(850);
                }

                foreach (var month in months)
                {
                    for (int i = 0; i < month.Value.Count; i++)
                    {
                        sum += month.Value[i];
                    }
                    new CultureInfo("en-US").DateTimeFormat.GetMonthName(month.Key);
                }
            });

            var table2 = new Table().Centered();
            table2.BorderColor<Table>(Color.Cyan3);
            table2.Border = TableBorder.Horizontal;

            AnsiConsole.Live(table2)
            .Start(ctx =>
            {
                table2.AddColumn("[steelblue1_1]Month[/]");
                ctx.Refresh();
                Thread.Sleep(850);

                table2.AddColumn("[mediumturquoise]Total hours[/]");
                ctx.Refresh();
                Thread.Sleep(850);

                table2.AddColumn("[blue]Average hours[/]");
                ctx.Refresh();
                Thread.Sleep(850);

                foreach (var month in months)
                {
                    for (int i = 0; i < month.Value.Count; i++)
                    {
                        sum += month.Value[i];
                    }
                    table2.AddRow($"[steelblue1_1]{new CultureInfo("en-US").DateTimeFormat.GetMonthName(month.Key)}[/]", $"[mediumturquoise]{sum}[/]", $"[blue]{sum / month.Value.Count}[/]");
                    ctx.Refresh();
                    Thread.Sleep(850);
                }
            });
        }
    }
}
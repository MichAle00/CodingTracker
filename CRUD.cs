using System.Configuration;
using System.Data.SQLite;
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
                    VALUES ('{date}', '{times[0]}', '{times[1]}', '{duration}')";

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

            var reader = connection.ExecuteReader("SELECT * FROM codingSessions");
            
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

        using (var connection = new SQLiteConnection(con))
        {
            connection.Open();

            string sql = @$"UPDATE codingSessions SET 
                Date = '{date}',
                Start = '{time[0]}',
                End = '{time[1]}',
                Duration = '{duration}'
            WHERE Id = {id}";

            var cmd = connection.Execute(sql);

            connection.Close();

            AnsiConsole.MarkupLine("[springgreen3_1]Records updated succesfully![/]");
        }
    }
}

using System.Configuration;
using System.Data.SQLite;
using CodingTracker.Model;
using Dapper;
using Spectre.Console;

namespace CodingTracker;


internal class CRUD
{
    static readonly string? con = ConfigurationManager.AppSettings.Get("connectionString");

    UserInput input = new();
    internal void CreateDB()
    {
        using (var connection = new SQLiteConnection(con))
        {
            connection.Open();

            string sql = @"CREATE TABLE IF NOT EXISTS codingSessions (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Start TEXT,
                    End TEXT,
                    Duration TEXT)";

            var cmd = connection.Execute(sql);

            connection.Close();
        }
    }

    internal void addToTable()
    {
        DateTime[] dates = input.GetDates();
        string duration = CalculateDuration(dates[0], dates[1]);

        using (var connection = new SQLiteConnection(con))
        {
            connection.Open();

            string sql =
                @$"INSERT INTO codingSessions (Start, End, Duration)
                    VALUES ('{dates[0]}', '{dates[1]}', '{duration}')";

            var cmd = connection.Execute(sql);

            connection.Close();

            AnsiConsole.Markup("[springgreen3_1]Records added succesfully![/]\n");
        }
    }

    internal string CalculateDuration(DateTime start, DateTime end)
    {
        TimeSpan dur = end - start;
        return dur.ToString("c");
    }

    internal static void viewRecords()
    {
        AnsiConsole.Clear();
        using (var connection = new SQLiteConnection(con))
        {
            connection.Open();

            var reader = connection.ExecuteReader("SELECT * FROM codingSessions");
            
            List<CodingSession> sessions = new List<CodingSession>();

            while (reader.Read())
            {
                sessions.Add(
                new CodingSession
                {
                    Id = reader.GetInt32(0),
                    StartTime = DateTime.Parse(reader.GetString(1)),
                    EndTime = DateTime.Parse(reader.GetString(2)),
                    Duration = reader.GetString(3)
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
                Thread.Sleep(1000);

                table.AddColumn("[blue]Start Time[/]");
                ctx.Refresh();
                Thread.Sleep(1000);

                table.AddColumn("[red]End Time[/]");
                ctx.Refresh();
                Thread.Sleep(1000);

                table.AddColumn("[darkmagenta_1]Duration[/]");
                ctx.Refresh();
                Thread.Sleep(1000);

                foreach (var session in sessions)
                {
                    table.AddRow($"[steelblue1_1]{session.Id}[/]", $"[blue]{session.StartTime}[/]", $"[red]{session.EndTime}[/]", $"[darkmagenta_1]{session.Duration}[/]");
                    ctx.Refresh();
                    Thread.Sleep(700);
                }
            });
        }
    }
}

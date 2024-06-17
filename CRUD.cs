using CodingTracker.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;
using System.Configuration;
using System.Data.SQLite;

namespace CodingTracker;

internal class CRUD
{
    string? config = ConfigurationManager.AppSettings.Get("connectionString");
    UserInput input = new();
    internal void CreateDB()
    {
        using (var connection = new SQLiteConnection(config))
        {
            connection.Open();

            var cmd = connection.CreateCommand();

            cmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS codingSessions (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Start TEXT,
                    End TEXT,
                    Duration TEXT)";

            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    internal void addToTable()
    {
        DateTime[] dates = input.GetDates();
        string duration = CalculateDuration(dates[0], dates[1]);

        using (var connection = new SQLiteConnection(config))
        {
            connection.Open();
            var cmd = connection.CreateCommand();

            cmd.CommandText =
                @$"INSERT INTO codingSessions (Start, End, Duration)
                    VALUES ('{dates[0]}', '{dates[1]}', '{duration}')";

            cmd.ExecuteNonQuery();

            connection.Close();
        }
    }

    internal string CalculateDuration(DateTime start, DateTime end)
    {
        TimeSpan dur = end - start;
        //TimeSpan final;
        //TimeSpan.TryParseExact(dur, "hh:mm::ss", new CultureInfo("en-US"), TimeSpanStyles.None, out final);
        return dur;
    }
}

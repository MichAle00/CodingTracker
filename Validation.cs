using Dapper;
using Spectre.Console;
using System.Configuration;
using System.Data.SQLite;

namespace CodingTracker;

internal class Validation
{
    static readonly string? con = ConfigurationManager.AppSettings.Get("connectionString");
    internal static bool CorrectTime(TimeSpan start, TimeSpan end)
    {
        if (TimeSpan.Compare(start, end) == 1)
        {
            return false;
        }
        return true;
    }

    internal static bool IsInTable(int id)
    {
        using (var connection = new SQLiteConnection(con))
        {
            connection.Open();

            List<int> ids = new();

            var reader = connection.ExecuteReader("SELECT Id FROM codingSessions");

            var exists = connection.ExecuteScalar<bool>("SELECT COUNT(Id) FROM codingSessions");

            if (!exists)
            {
                AnsiConsole.MarkupLine("[red]This table is empty![/]");
                connection.Close();
                return false;
            }

            while (reader.Read())
            {
                ids.Add(reader.GetInt32(0));
            }

            connection.Close();

            if (ids.Contains(id))
            {
                return true;
            }
            return false;
        }
    }
}

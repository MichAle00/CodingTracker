using CodingTracker;
using System.Xml.Linq;

namespace coding_tracker;

internal class CodingTracker()
{
    internal static void Main()
    {
        CRUD XD = new();
        DateTime x = new DateTime(2024,06,17,8,24,45);
        DateTime y = DateTime.Now;
        
        Console.WriteLine(XD.CalculateDuration(x, y));
    }
}
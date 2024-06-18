using CodingTracker;
using System.Xml.Linq;

namespace coding_tracker;

internal class CodingTracker()
{
    internal static void Main()
    {
        UserInput menu = new UserInput();

        menu.Menu();
    }
}
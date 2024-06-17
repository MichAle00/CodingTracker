using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker
{
    internal class UserInput
    {
        public UserInput()
        {
            Console.Clear();
            bool closeApp = true;

            while (closeApp)
            {
                Console.WriteLine("Main Menu:");
                Console.WriteLine("What do you like to do?");
                Console.WriteLine("Type 0 to Close Application");
                Console.WriteLine("Type 1 to View All Records");
                Console.WriteLine("Type 2 to Insert Record");
                Console.WriteLine("Type 3 to Delete Record");
                Console.WriteLine("Type 4 to Update Record");
                Console.WriteLine("--------------------------------------\n");

                string? command = Console.ReadLine();

                switch (command)
                {
                    case "0":
                        break;

                    case "1":
                        break;

                    case "2":
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
    }
}

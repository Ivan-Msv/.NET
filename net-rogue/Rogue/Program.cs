using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue
{
    class Program
    {
        static void Main()
        {
            bool again = true;
            while (again)
            {
                Game rogue = new Game();
                rogue.Run();

                again = false;
            }
        }
    }
}

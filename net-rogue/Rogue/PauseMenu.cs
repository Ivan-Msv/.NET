using RayGuiCreator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroElectric.Vinculum;

namespace Rogue
{
    internal class PauseMenu
    {
        public event EventHandler BackButtonPressedEvent;
        public void DrawMenu()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.BLACK);
            MenuCreator pauseMenu = new MenuCreator(0, 0, 50, 50);
            if (pauseMenu.Button("Return"))
            {
                BackButtonPressedEvent.Invoke(this, EventArgs.Empty);
            }
            Raylib.EndDrawing();
        }
    }
}

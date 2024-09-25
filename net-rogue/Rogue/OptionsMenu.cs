using RayGuiCreator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroElectric.Vinculum;

namespace Rogue
{
    internal class OptionsMenu
    {
        public event EventHandler BackButtonPressedEvent;
        public bool randomizerBool;
        public bool screenshakeBool;
        public bool noclipBool;
        public void DrawMenu()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.BLACK);
            MenuCreator pauseMenu = new MenuCreator(0, 0, 50, 50);
            if (pauseMenu.Button("Return"))
            {
                BackButtonPressedEvent.Invoke(this, EventArgs.Empty);
            }
            pauseMenu.Label("Fun mode", Raylib.BLUE);
            pauseMenu.Checkbox("Screenshake (found by accident)", ref screenshakeBool);
            pauseMenu.Checkbox("No wall collision", ref noclipBool);
            pauseMenu.Checkbox("Randomize sprites (Restart level to see the effect)", ref randomizerBool);
            Raylib.EndDrawing();
        }
    }
}

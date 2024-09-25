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
        public event EventHandler MainMenuButtonPressedEvent;
        public event EventHandler RestartButtonPressedEvent;
        public event EventHandler OptionsMenuPressedEvent;
        public void DrawMenu()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.BLACK);
            MenuCreator pauseMenu = new MenuCreator(0, 0, 50, 50, 5);
            if (pauseMenu.Button("Return"))
            {
                BackButtonPressedEvent.Invoke(this, EventArgs.Empty);
            }
            if (pauseMenu.Button("Restart Level"))
            {
                RestartButtonPressedEvent.Invoke(this, EventArgs.Empty);
            }
            if (pauseMenu.Button("Options"))
            {
                OptionsMenuPressedEvent.Invoke(this, EventArgs.Empty);
            }
            if (pauseMenu.Button("Back to Menu"))
            {
                MainMenuButtonPressedEvent.Invoke(this, EventArgs.Empty);
            }
            Raylib.EndDrawing();
        }
    }
}

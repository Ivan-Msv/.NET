using CommunityToolkit.HighPerformance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ZeroElectric.Vinculum;

namespace RayLibTesti
{
    internal class Test
    {
        const int windowWidth = 600;
        const int windowHeight = 400;

        float snowmanX;
        float snowmanY;
        public void Run()
        {
            snowmanX = 20;
            snowmanY = 150;

            Raylib.InitWindow(windowWidth, windowHeight, "Test");
            Raylib.SetTargetFPS(60);

            while (!Raylib.WindowShouldClose())
            {
                Update();
                Draw();
            }
            Raylib.CloseWindow();
        }
        private void Draw()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.BLACK);

            for (int i = 0; i < 3; i++)
            {
                Raylib.DrawCircle((int)snowmanX, (int)snowmanY - i * 40, 40 - i*8, Raylib.WHITE);
            }

            Raylib.EndDrawing();
        }
        private void Update()
        {
            snowmanX += 1f;
            snowmanY += 0.5f;
        }
    }
}

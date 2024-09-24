using System.Diagnostics;
using System.Numerics;
using ZeroElectric.Vinculum;

namespace LunarLander
{
    internal class Lander
    {
        static void Main(string[] args)
        {
            Lander game = new Lander();
            game.Init();
            game.GameLoop();
        }

        /////////////////////////////////////

        // Pelaajan sijainti
        // x on aluksen keskikohta
        // y on aluksen pohja
        float x = 120;
        float y = 16;

        // Onko moottori päällä
        bool engine_on = false;

        // Pelaajan nopeus, polttoaine ja polttonopeus
        float velocity = 0;
        float fuel = 100;
        float fuel_consumption = 10.0f;

        // Laskeutumisalustan katon sijainti y-akselilla. Y kasvaa alaspäin.
        int landing_y = 125;

        // Ruudunpäivitykseen menevä aika (oletus)
        float delta_time = 1.0f / 60.0f;

        // Moottorin voimakkuus
        float acceleration = 20.9f;

        // Painovoiman voimakkuus
        float gravity = 9.89f;

        // Pelialueen ja ikkunan mittasuhteet
        int game_width = 240;
        int game_height = 136;

        int screen_width = 1280;
        int screen_height = 720;


        RenderTexture game_screen;
        Texture ship_texture;

        void Init()
        {
            Raylib.InitWindow(screen_width, screen_height, "Lunar Lander");

            game_screen = Raylib.LoadRenderTexture(game_width, game_height);
            ship_texture = Raylib.LoadTexture("Images/ship.png");
            Raylib.SetTargetFPS(60);
        }

        void GameLoop()
        {
            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginTextureMode(game_screen);
                Raylib.BeginDrawing();
                Draw();
                Raylib.EndTextureMode();
                Raylib.ClearBackground(Raylib.BLANK);
                DrawScaled();
                Update();
                Raylib.EndDrawing();
            }
            Raylib.UnloadRenderTexture(game_screen);
            Raylib.UnloadTexture(ship_texture);
            Raylib.CloseWindow();
        }

        void Update()
        {
            // TODO: Kysy Raylibiltä miten pitkään yksi ruudunpäivitys kesti
            delta_time = Raylib.GetFrameTime(); // Kirjoita funktiokutsu 0.0f tilalle.

            // Lisää painovoiman vaikutus
            velocity += gravity * delta_time;

            // TODO: Kun pelaaja painaa nappia (esim nuoli ylös)
            // ja polttoainetta on jäljellä, lisää
            // kiihtyvyys nopeuteen
            if (Raylib.IsKeyDown(KeyboardKey.KEY_UP) && fuel > 0)
            {
                velocity -= acceleration * delta_time;
                fuel -= fuel_consumption * delta_time;
                engine_on = true;
            }
            else
            {
                engine_on = false;
            }
            // Liikuta alusta
            y += velocity * delta_time;
        }
        void Draw()
        {
            Raylib.ClearBackground(Raylib.BLANK);

            int plat_x = (int)x - 30;
            int plat_y = landing_y;
            int plat_w = 60;
            int plat_h = 10;

            Raylib.DrawRectangle(plat_x, plat_y, plat_w, plat_h, Raylib.SKYBLUE);

            Raylib.DrawTexture(ship_texture, (int)x - 28, (int)y - 38, Raylib.SKYBLUE);

            // Piirrä moottorin liekki
            if (engine_on)
            {
                Raylib.DrawTriangle(new Vector2(x - 5, y), new Vector2(x, y + 32), new Vector2(x + 5, y), Raylib.YELLOW);

            }

            // Piirrä polttoaineen tilanne
            Raylib.DrawRectangle(9, 9, 102, 12, Raylib.BLUE);
            Raylib.DrawRectangle(10, 10, (int)fuel, 10, Raylib.YELLOW);
            Raylib.DrawText("FUEL", 11, 11, 12, Raylib.DARKBLUE);

            // Piirrä debug tietoja
            Raylib.DrawText($"V:{velocity}", 11, 31, 8, Raylib.WHITE);
            Raylib.DrawText($"dt:{delta_time}", 11, 41, 8, Raylib.WHITE);
        }
        void DrawScaled()
        {
            int draw_width = screen_width;
            int draw_height = screen_height;
            float scale = Math.Min((float)draw_width / game_width, (float)draw_height / game_height);

            Rectangle source = new Rectangle(0.0f, 0.0f, game_screen.texture.width, game_screen.texture.height * -1.0f);
            Rectangle destination = new Rectangle((draw_width - (float)game_width * scale) * 0.5f, (draw_height - (float)game_height * scale) * 0.5f, game_width * scale, game_height * scale);
            Raylib.DrawTexturePro(game_screen.texture, source, destination, new Vector2(0, 0), 0.0f, Raylib.WHITE);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using ZeroElectric.Vinculum;
using static System.Net.Mime.MediaTypeNames;

namespace Rogue
{
    class Game
    {
        PlayerCharacter player;
        Map level01;
        Texture mapImage;
        public static readonly int imagesPerRow = 12;
        public static readonly int tileSize = 16;
        int screen_width = 1280;
        int screen_height = 720;

        int game_width;
        int game_height;
        RenderTexture game_screen;
        public void Run()
        {
            Init();
            GameLoop();
        }
        private void Init()
        {
            Console.CursorVisible = false;
            Console.WindowWidth = 100;
            Console.WindowHeight = 26;

            CreateCharacter();

            MapLoader loader = new MapLoader();
            level01 = loader.ReadMapFromFile("Maps/level01.json");


            Raylib.InitWindow(screen_width, screen_height, "Rogue");
            mapImage = Raylib.LoadTexture("data/images/tilemap.png");

            ClassTexture();
            EnemyAndItemTexture();

            Raylib.SetWindowState(ConfigFlags.FLAG_WINDOW_RESIZABLE);
            game_width = 480;
            game_height = 270;
            game_screen = Raylib.LoadRenderTexture(game_width, game_height);

            Raylib.SetTextureFilter(game_screen.texture, TextureFilter.TEXTURE_FILTER_POINT);
            Raylib.SetWindowMinSize(game_width, game_height);
            Raylib.SetTargetFPS(30);
        }
        private void GameLoop()
        {
            while (!Raylib.WindowShouldClose())
            {
                DrawGameToTexture();
                PlayerMovement();
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_ESCAPE))
                {
                    break;
                }
            }
            Raylib.CloseWindow();
            Raylib.UnloadRenderTexture(game_screen);
            Console.Clear();
        }
        private PlayerCharacter CreateCharacter()
        {
            player = new PlayerCharacter(Raylib.GREEN);
            player.pName = AskName();
            player.pRace = AskRace();
            player.pClass = AskClass();
            player.position = new Vector2(1, 1);
            player.currentMoney = player.StartingMoney(player.pRace);
            return player;
        }
        private static string AskName()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("What is your name?");
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;
                string nameanswer = Console.ReadLine();
                if (nameanswer.Length >= 3 && nameanswer.Length <= 10 && !nameanswer.Any(char.IsNumber))
                {
                    return nameanswer;
                }
                else if (nameanswer.Any(char.IsNumber))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"Name (");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(nameanswer);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(") is not supposed to contain any numbers, try again.");
                    continue;
                }
                else if (nameanswer.Length >= 10)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Name too long, try again.");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Name too short, try again.");
                }
            }
        }
        private static Race AskRace()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Please select your race.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("1. Human");
            Console.WriteLine("2. Elf");
            Console.WriteLine("3. Goblin");

            int raceAnswer;
            while (true)
            {
                char raceAnswerLine = Console.ReadKey(true).KeyChar;
                if (int.TryParse(raceAnswerLine.ToString(), out int integer) && Enumerable.Range(1, 3).Contains(integer))
                {
                    raceAnswer = integer;
                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Choose using the numbers.");
                    Console.ResetColor();
                    continue;
                }
            }

            switch (raceAnswer)
            {
                case 1:
                    return Race.Human;
                case 2:
                    return Race.Elf;
                case 3:
                    return Race.Goblin;
                default:
                    Console.WriteLine("Something went wrong...");
                    return Race.Human;
            }
        }
        private static Class AskClass()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Please select your class.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("1. Knight");
            Console.WriteLine("2. Archer");
            Console.WriteLine("3. Mage");

            int classAnswer;
            while (true)
            {
                char classAnswerLine = Console.ReadKey(true).KeyChar;
                if (int.TryParse(classAnswerLine.ToString(), out int integer) && Enumerable.Range(1, 3).Contains(integer))
                {
                    classAnswer = integer;
                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Choose using the numbers.");
                    Console.ResetColor();
                    continue;
                }
            }

            switch (classAnswer)
            {
                case 1:
                    return Class.Knight;
                case 2:
                    return Class.Archer;
                case 3:
                    return Class.Mage;
                default:
                    Console.WriteLine("Something went wrong...");
                    return Class.Knight;
            }
        }
        private void ClassTexture()
        {
            switch (player.pClass)
            {
                case Class.Knight:
                    player.SetPlayerImageAndIndex(mapImage, 12, new Vector2(0, 8));
                    Console.WriteLine("Knight?");
                    break;
                case Class.Archer:
                    player.SetPlayerImageAndIndex(mapImage, 12, new Vector2(1, 7));
                    Console.WriteLine("archer");
                    break;
                case Class.Mage:
                    player.SetPlayerImageAndIndex(mapImage, 12, new Vector2(0, 7));
                    Console.WriteLine("mage");
                    break;
            }
        }
        private void EnemyAndItemTexture()
        {
            foreach (var enemy in level01.enemies)
            {
                switch (enemy.name.ToLower())
                {
                    case "thief":
                        enemy.SetEnemyImageAndIndex(mapImage, imagesPerRow, new Vector2(4, 8));
                        break;
                    case "troll":
                        enemy.SetEnemyImageAndIndex(mapImage, imagesPerRow, new Vector2(1, 9));
                        break;
                }
            }
            foreach (var item in level01.items)
            {
                switch (item.name.ToLower())
                {
                    case "pickaxe":
                        item.SetItemImageAndIndex(mapImage, imagesPerRow, new Vector2(9, 9));
                        break;
                    case "sword":
                        item.SetItemImageAndIndex(mapImage, imagesPerRow, new Vector2(8, 8));
                        break;
                }
            }
        }
        private void DrawEnemyAndItems()
        {
            foreach (var enemy in level01.enemies)
            {
                enemy.Draw();
            }
            foreach (var item in level01.items)
            {
                item.Draw();
            }
        }
        private void ScanEnemiesAndItems()
        {
            Color color = Raylib.MAGENTA;

            Enemy enemyScan = level01.GetEnemyAt(player.position);
            Item itemScan = level01.GetItemAt(player.position);

            if (enemyScan != null)
            {
                switch (enemyScan.name.ToLower())
                {
                    case "troll":
                        Raylib.DrawText($"You hit an enemy: <{enemyScan.name}>", 0, 7 * tileSize, tileSize, color);
                        break;
                    case "thief":
                        Raylib.DrawText($"You encounter a <{enemyScan.name}>. \nHe steals half of your money.", 0, 7 * tileSize, tileSize, color);
                        player.currentMoney /= 2;
                        level01.DeleteEnemyOrItem(enemyScan); // only deletes the enemy itself but the icon ($) stays
                        break;
                }
            }
            else if (itemScan != null)
            {
                Raylib.DrawText($"You find an item: <{itemScan.name}>", 0, 7 * tileSize, tileSize, color);
            }
            else
            {
                Raylib.DrawRectangle(0, 7 * tileSize, tileSize * 20, tileSize * 2, Raylib.BLACK);
            }
        }
        private void DrawGameToTexture()
        {
            Raylib.BeginTextureMode(game_screen);
            level01.Draw(mapImage, 0);
            ScanEnemiesAndItems();
            DrawEnemyAndItems();
            player.Draw();
            ShowMoney(player.currentMoney);
            Raylib.EndTextureMode();
            DrawGameScaled();
        }
        private void DrawGameScaled()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.BLACK);

            int draw_width = Raylib.GetScreenWidth();
            int draw_height = Raylib.GetScreenHeight();
            float scale = Math.Min((float)draw_width / game_width, (float)draw_height / game_height);
            
            Rectangle source = new Rectangle(0.0f, 0.0f, game_screen.texture.width, game_screen.texture.height * -1.0f);
            Rectangle destination = new Rectangle((draw_width - (float)game_width * scale) * 0.5f, (draw_height - (float)game_height * scale) * 0.5f, game_width * scale, game_height * scale);

            Raylib.DrawTexturePro(game_screen.texture, source, destination, new Vector2(0,0), 0.0f, Raylib.WHITE);
            Raylib.EndDrawing();
        }
        private void PlayerMovement()
        {
            int moveX = 0;
            int moveY = 0;
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_UP))
            {
                moveY = -1;
            }
            else if (Raylib.IsKeyPressed(KeyboardKey.KEY_DOWN))
            {
                moveY = 1;
            }
            else if (Raylib.IsKeyPressed(KeyboardKey.KEY_LEFT))
            {
                moveX = -1;
            }
            else if (Raylib.IsKeyPressed(KeyboardKey.KEY_RIGHT))
            {
                moveX = 1;
            }

            int index = (int)player.position.X + moveX + ((int)player.position.Y + moveY) * level01.mapWidth;
            int wallCheck = level01.layers[0].mapTiles[index];
            if (wallCheck != 2)
            {
                player.Move(moveX, moveY);
            }
        }
        private void ShowMoney(int money)
        {
            Raylib.DrawRectangle(10 * tileSize, 0, tileSize * 10, tileSize, Raylib.BLACK);
            Raylib.DrawText($"Gold: {money}", 10 * tileSize, 0, tileSize, Raylib.YELLOW);
        }
    }
}

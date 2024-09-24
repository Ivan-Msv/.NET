using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using ZeroElectric.Vinculum;
using RayGuiCreator;
using System.Reflection.Emit;

namespace Rogue
{
    enum GameState
    {
        MainMenu, GameLoop, CharacterCreation
    }
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
        ZeroElectric.Vinculum.Font game_font;
        private bool trollDefeated;

        Sound thiefLaugh;
        Sound defeatedTroll;
        Sound wallKnock;
        Sound itemPickup;

        GameState currentGameState;

        bool canMove = true;
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

            MapLoader loader = new MapLoader();
            level01 = loader.ReadMapFromFile("Maps/RogueTiled.tmj");

            Raylib.InitWindow(screen_width, screen_height, "Rogue");
            mapImage = Raylib.LoadTexture("data/images/tilemap.png");
            game_font = Raylib.LoadFontEx("data/fonts/Adventurer.ttf", 16, 0);


            currentGameState = GameState.MainMenu;

            Raylib.InitAudioDevice();
            trollDefeated = false;
            defeatedTroll = Raylib.LoadSound("data/audio/defeated_troll.wav");
            thiefLaugh = Raylib.LoadSound("data/audio/thief_laugh.mp3");
            wallKnock = Raylib.LoadSound("data/audio/wallknock.mp3");
            itemPickup = Raylib.LoadSound("data/audio/item_pickup.wav");

            Raylib.SetSoundVolume(thiefLaugh, 0.5f);
            Raylib.SetSoundVolume(defeatedTroll, 0.5f);
            Raylib.SetSoundVolume(itemPickup, 0.4f);

            //ClassTexture();
            //EnemyAndItemTexture();

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
                switch (currentGameState)
                {
                    case GameState.MainMenu:
                        DrawMainMenu();
                        break;
                    case GameState.GameLoop:
                        DrawGameToTexture();
                        PlayerMovement();
                        break;
                    case GameState.CharacterCreation:
                        CreateCharacter();
                        break;
                }
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_ESCAPE))
                {
                    break;
                }
            }
            Raylib.UnloadRenderTexture(game_screen);
            Raylib.UnloadFont(game_font);
            Raylib.UnloadSound(thiefLaugh);
            Raylib.UnloadSound(defeatedTroll);
            Raylib.UnloadSound(wallKnock);
            Raylib.UnloadSound(itemPickup);
            Raylib.CloseAudioDevice();
            Raylib.CloseWindow();
            Console.Clear();
        }
        private PlayerCharacter CreateCharacter()
        {
            player = new PlayerCharacter(Raylib.GREEN);
            player.pName = AskNameVisual();
            player.pRace = AskRace();
            player.pClass = AskClass();
            player.position = new Vector2(1, 1);
            player.currentMoney = player.StartingMoney(player.pRace);

            ClassTexture();
            EnemyAndItemTexture();
            return player;
        }
        private static string AskName()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("What is your name?");
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;
                var nameanswer = Console.ReadLine();
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
        private static string AskNameVisual()
        {
            TextBoxEntry nameEntry = new TextBoxEntry(500);
            string wrongName = null;
            while (true)
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Raylib.BLACK);
                MenuCreator characterCreator = new MenuCreator(Raylib.GetScreenWidth() / 3, Raylib.GetScreenHeight() / 4, 20, 40, 5);
                characterCreator.Label("What is your name?");
                characterCreator.TextBox(nameEntry);
                if (characterCreator.Button("Confirm"))
                {
                    wrongName = $"test {nameEntry}";
                }
                characterCreator.Label(wrongName); // korjaa huomen
                Raylib.EndDrawing();
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
                    break;
                case Class.Archer:
                    player.SetPlayerImageAndIndex(mapImage, 12, new Vector2(1, 7));
                    break;
                case Class.Mage:
                    player.SetPlayerImageAndIndex(mapImage, 12, new Vector2(0, 7));
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
                        enemy.SetEnemyImageAndIndex(mapImage, imagesPerRow, (int)MapTile.Thief);
                        break;
                    case "troll":
                        enemy.SetEnemyImageAndIndex(mapImage, imagesPerRow, (int)MapTile.Troll);
                        break;
                }
            }
            foreach (var item in level01.items)
            {
                switch (item.name.ToLower())
                {
                    case "potion":
                        item.SetItemImageAndIndex(mapImage, imagesPerRow, (int)MapTile.Potion);
                        break;
                    case "sword":
                        item.SetItemImageAndIndex(mapImage, imagesPerRow, (int)MapTile.Sword);
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
            Vector2 textPos = new Vector2(0, game_height - 50);
            string text;

            if (enemyScan != null)
            {
                string name = enemyScan.name.ToLower();
                if (name == "thief")
                {
                    canMove = false;
                    text = $"You encounter a <{enemyScan.name}>.\nHe steals half of your money and runs away.\n[Enter to continue]";

                    Raylib.DrawTextEx(game_font, text, textPos, game_font.baseSize, 0, color);
                    if (Raylib.IsKeyDown(KeyboardKey.KEY_ENTER))
                    {
                        Raylib.PlaySound(thiefLaugh);
                        player.currentMoney /= 2;
                        level01.enemies.Remove(enemyScan);
                        canMove = true;
                    }
                }
                else if (name == "troll")
                {
                    canMove = false;
                    text = $"You hit an enemy: <{enemyScan.name}>,\nHe dies rather quickly...\n[Enter to continue]";
                    Raylib.DrawTextEx(game_font, text, textPos, game_font.baseSize, 0, color);
                    switch (trollDefeated)
                    {
                        case false:
                            Raylib.PlaySound(defeatedTroll);
                            trollDefeated = true;
                            break;
                    }
                    if (Raylib.IsKeyDown(KeyboardKey.KEY_ENTER))
                    {
                        level01.enemies.Remove(enemyScan);
                        canMove = true;
                    }
                }
            }
            else if (itemScan != null)
            {
                canMove = false;
                text = $"You find an item: <{itemScan.name}> \n[Enter to continue]";
                Raylib.DrawTextEx(game_font, text, textPos, game_font.baseSize, 0, color);
                if (Raylib.IsKeyDown(KeyboardKey.KEY_ENTER))
                {
                    Raylib.PlaySound(itemPickup);
                    level01.items.Remove(itemScan);
                    canMove = true;
                }
            }
            else
            {
                Raylib.DrawRectangle((int)textPos.X, (int)textPos.Y, tileSize * 30, tileSize * 3, Raylib.BLACK);
            }
        }
        private void DrawGameToTexture()
        {
            Raylib.BeginTextureMode(game_screen);
            level01.Draw(mapImage);
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
            if (canMove)
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

                int index = (int)player.position.X + moveX + ((int)player.position.Y + moveY) * level01.tileMap.layers[0].width;
                MapTile tile = level01.GetTileAt(index);

                if (tile == MapTile.Floor)
                {
                    player.Move(moveX, moveY);
                }
                else
                {
                    Raylib.PlaySound(wallKnock);
                }
            }
        }
        public void DrawMainMenu()
        {
            // Tyhjennä ruutu ja aloita piirtäminen
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.BLACK);


            // Laske ylimmän napin paikka ruudulla.
            int button_width = 100;
            int button_height = 20;
            int button_x = Raylib.GetScreenWidth() / 2 - button_width / 2;
            int button_y = Raylib.GetScreenHeight() / 2 - button_height / 2;

            MenuCreator c = new MenuCreator(button_x, button_y - button_height * 2, button_height, button_width, button_height);
            // Piirrä pelin nimi nappien yläpuolelle
            c.Label("Rogue");

            if (c.Button("Start Game"))
            {
                currentGameState = GameState.CharacterCreation;
            }

            if (c.Button("Options"))
            {
                // Go to options somehow
            }

            if (c.Button("Quit"))
            {
                // Quit the game
            }

            Raylib.EndDrawing();
        }
        private static void ShowMoney(int money)
        {
            Raylib.DrawRectangle(10 * tileSize, 0, tileSize * 10, tileSize, Raylib.BLACK);
            Raylib.DrawText($"Gold: {money}", 10 * tileSize, 0, tileSize, Raylib.YELLOW);
        }
    }
}

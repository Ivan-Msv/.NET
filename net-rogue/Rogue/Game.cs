using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using ZeroElectric.Vinculum;
using RayGuiCreator;
using System.Reflection.Emit;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Net.Http.Headers;
using TurboMapReader;

namespace Rogue
{
    enum GameState
    {
        MainMenu, GameLoop, CharacterCreation, OptionsMenu, PauseMenu
    }
    class Game
    {
        PlayerCharacter player;
        Map level01;
        Texture mapImage;
        public int imagesPerRow { get; private set; } = 12;
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
        Stack<GameState> stateStack;
        PauseMenu myPauseMenu;
        OptionsMenu myOptionsMenu;

        bool canMove = true;
        Random random = new Random();
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

            stateStack = new Stack<GameState>();
            currentGameState = GameState.MainMenu;
            stateStack.Push(currentGameState);
            myPauseMenu = new PauseMenu();
            myOptionsMenu = new OptionsMenu();

            myOptionsMenu.BackButtonPressedEvent += this.OnBackButtonPressed;
            myPauseMenu.BackButtonPressedEvent += this.OnBackButtonPressed;
            myPauseMenu.MainMenuButtonPressedEvent += this.OnMainMenuButtonPressed;
            myPauseMenu.RestartButtonPressedEvent += this.OnRestartButtonPressed;
            myPauseMenu.OptionsMenuPressedEvent += this.OnOptionsButtonPressed;

            Raylib.InitAudioDevice();
            trollDefeated = false;
            defeatedTroll = Raylib.LoadSound("data/audio/defeated_troll.wav");
            thiefLaugh = Raylib.LoadSound("data/audio/thief_laugh.mp3");
            wallKnock = Raylib.LoadSound("data/audio/wallknock.mp3");
            itemPickup = Raylib.LoadSound("data/audio/item_pickup.wav");

            Raylib.SetSoundVolume(thiefLaugh, 0.5f);
            Raylib.SetSoundVolume(defeatedTroll, 0.5f);
            Raylib.SetSoundVolume(itemPickup, 0.4f);

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
                    case GameState.CharacterCreation:
                        CreateCharacter();
                        break;
                    case GameState.MainMenu:
                        DrawMainMenu();
                        break;
                    case GameState.OptionsMenu:
                        myOptionsMenu.DrawMenu(); 
                        break;
                    case GameState.PauseMenu:
                        myPauseMenu.DrawMenu();
                        break;
                    case GameState.GameLoop:
                        DrawGameToTexture();
                        PlayerMovement();
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
            player.pName = AskName();
            player.pRace = AskRace();
            player.pClass = AskClass();
            player.position = new Vector2(1, 1);
            player.currentMoney = player.StartingMoney(player.pRace);

            currentGameState = GameState.GameLoop;
            stateStack.Push(currentGameState);

            if (myOptionsMenu.randomizerBool)
            {
                imagesPerRow = random.Next(3, 40);
            }
            else
            {
                imagesPerRow = 12;
            }

            ClassTexture();
            EnemyAndItemTexture();
            return player;
        }
        private string AskName()
        {
            TextBoxEntry nameEntry = new TextBoxEntry(10);
            string wrongName = null;
            while (true)
            {
                if (!Raylib.WindowShouldClose())
                {
                    Raylib.BeginDrawing();
                    Raylib.ClearBackground(Raylib.BLACK);
                    MenuCreator nameMenu = new MenuCreator(50, Raylib.GetScreenHeight() / 3, 45, 45, 1, 0, game_font);
                    nameMenu.Label("What is your name?", Raylib.YELLOW);
                    nameMenu.TextBox(nameEntry);
                    if (nameMenu.Button("Confirm"))
                    {
                        if (nameEntry.ToString().Any(char.IsNumber))
                        {
                            wrongName = $"Name ({nameEntry}) is not supposed to contain any numbers";
                        }
                        else if (nameEntry.ToString().Length <= 3)
                        {
                            wrongName = $"Name ({nameEntry}) is too short.";
                        }
                        else
                        {
                            return nameEntry.ToString();
                        }
                    }
                    nameMenu.Label(wrongName, Raylib.RED);
                    Raylib.EndDrawing();
                }
                else
                {
                    Raylib.CloseWindow();
                }
            }
        }
        private Race AskRace()
        {
            string[] choices = ["Human", "Elf", "Goblin"];
            MultipleChoiceEntry raceChoice = new MultipleChoiceEntry(choices);
            while (true)
            {
                if (!Raylib.WindowShouldClose())
                {
                    Raylib.BeginDrawing();
                    Raylib.ClearBackground(Raylib.BLACK);
                    MenuCreator raceMenu = new MenuCreator(50, Raylib.GetScreenHeight() / 3, 45, 400, 1, 0, game_font);
                    raceMenu.Label("Please select your race.", Raylib.YELLOW);
                    raceMenu.ToggleGroup(raceChoice);
                    if (RayGui.GuiButton(new Rectangle(50, Raylib.GetScreenHeight() / 3 + 300, 300, 45), "Confirm") == 1)
                    {
                        switch (raceChoice.ToString())
                        {
                            case "Human":
                                return Race.Human;
                            case "Elf":
                                return Race.Elf;
                            case "Goblin":
                                return Race.Goblin;
                            default:
                                Console.WriteLine("Something went wrong...");
                                return Race.Human;
                        }
                    }
                    Raylib.EndDrawing();
                }
                else
                {
                    Raylib.CloseWindow();
                }
            }
        }
        private Class AskClass()
        {
            string[] choices = ["Knight", "Archer", "Mage"];
            MultipleChoiceEntry classChoice = new MultipleChoiceEntry(choices);
            while (true)
            {
                if (!Raylib.WindowShouldClose())
                {
                    if (Raylib.WindowShouldClose())
                    {
                        Raylib.CloseWindow();
                    }
                    Raylib.BeginDrawing();
                    Raylib.ClearBackground(Raylib.BLACK);
                    MenuCreator classMenu = new MenuCreator(50, Raylib.GetScreenHeight() / 3, 45, 400, 1, 0, game_font);
                    classMenu.Label("Please select your class.", Raylib.YELLOW);
                    classMenu.ToggleGroup(classChoice);
                    DrawClassImage(classChoice.ToString());
                    if (RayGui.GuiButton(new Rectangle(50, Raylib.GetScreenHeight() / 3 + 250, 300, 45), "Confirm") == 1)
                    {
                        switch (classChoice.ToString())
                        {
                            case "Knight":
                                return Class.Knight;
                            case "Archer":
                                return Class.Archer;
                            case "Mage":
                                return Class.Mage;
                            default:
                                Console.WriteLine("Something went wrong...");
                                return Class.Mage;
                        }
                    }
                    Raylib.EndDrawing();
                }
                else
                {
                    Raylib.CloseWindow();
                }
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
                enemy.SetEnemyImageAndIndex(mapImage, imagesPerRow);
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
            level01.Draw(mapImage, imagesPerRow);
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

            if (myOptionsMenu.screenshakeBool)
            {
                Raylib.DrawTexturePro(game_screen.texture, source, destination, new Vector2(random.Next(10), random.Next(10)), 0.0f, Raylib.WHITE);
            }
            else
            {
                Raylib.DrawTexturePro(game_screen.texture, source, destination, new Vector2(0, 0), 0.0f, Raylib.WHITE);
            }
            DrawPauseMenuButton();
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
                else if (tile != MapTile.Floor && myOptionsMenu.noclipBool)
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
            int button_width = 140;
            int button_height = 40;
            int button_x = Raylib.GetScreenWidth() / 2 - button_width / 2;
            int button_y = Raylib.GetScreenHeight() / 2 - button_height / 2;

            MenuCreator c = new MenuCreator(button_x, button_y - button_height * 2, button_height, button_width, button_height);
            // Piirrä pelin nimi nappien yläpuolelle
            c.Label("Rogue");

            if (c.Button("Start Game"))
            {
                currentGameState = GameState.CharacterCreation;
                stateStack.Push(currentGameState);
            }

            if (c.Button("Options"))
            {
                currentGameState = GameState.OptionsMenu;
                stateStack.Push(currentGameState);
            }

            if (c.Button("Quit"))
            {
                Raylib.CloseWindow();
                Environment.Exit(0);
            }

            Raylib.EndDrawing();
        }
        private void OnBackButtonPressed(object sender, EventArgs args)
        {
            stateStack.Pop();
            currentGameState = stateStack.Peek();
        }
        private void OnMainMenuButtonPressed(object sender, EventArgs args)
        {
            stateStack.Clear();
            currentGameState = GameState.MainMenu;
            stateStack.Push(currentGameState);
        }
        private void OnRestartButtonPressed(object sender, EventArgs args)
        {
            if (myOptionsMenu.randomizerBool)
            {
                imagesPerRow = random.Next(3, 40);
            }
            else
            {
                imagesPerRow = 12;
            }
            MapLoader loader = new MapLoader();
            level01 = loader.ReadMapFromFile("Maps/RogueTiled.tmj");
            player.position = new Vector2(1, 1);
            player.currentMoney = player.StartingMoney(player.pRace);
            EnemyAndItemTexture();
            stateStack.Pop();
            currentGameState = stateStack.Peek();
        }
        private void OnOptionsButtonPressed(object sender, EventArgs args)
        {
            currentGameState = GameState.OptionsMenu;
            stateStack.Push(currentGameState);
        }
        private void DrawPauseMenuButton()
        {
            if (RayGui.GuiButton(new Rectangle(Raylib.GetScreenWidth() - 270, Raylib.GetScreenHeight() - 80, 270, 80), "Pause Menu") == 1)
            {
                currentGameState = GameState.PauseMenu;
                stateStack.Push(currentGameState);
            }
        }
        private void DrawClassImage(string playerClass)
        {
            Vector2 position;
            switch (playerClass)
            {
                case "Knight":
                    position = new Vector2(0, 8);
                    break;
                case "Archer":
                    position = new Vector2(1, 7);
                    break;
                case "Mage":
                    position = new Vector2(0, 7);
                    break;
                default:
                    position = new Vector2(0, 0);
                    break;
            }
            int index = (int)position.X + (int)position.Y * imagesPerRow;
            int imagePixelX = (index % imagesPerRow) * Game.tileSize;
            int imagePixelY = (index / imagesPerRow) * Game.tileSize;

            Rectangle imageRect = new Rectangle(imagePixelX, imagePixelY, Game.tileSize, Game.tileSize);
            Raylib.DrawTexturePro(mapImage, imageRect, new Rectangle(Raylib.GetScreenWidth() / 3 + 50, Raylib.GetScreenHeight() / 2 - 50, 100, 100), Vector2.Zero, 0, Raylib.WHITE);
        }
        private static void ShowMoney(int money)
        {
            Raylib.DrawRectangle(10 * tileSize, 0, tileSize * 10, tileSize, Raylib.BLACK);
            Raylib.DrawText($"Gold: {money}", 10 * tileSize, 0, tileSize, Raylib.YELLOW);
        }
    }
}

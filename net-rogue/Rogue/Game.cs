using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Rogue
{
    class Game
    {
        PlayerCharacter player = new PlayerCharacter('@', ConsoleColor.Green);
        Map level01;

        public void Run()
        {
            Console.CursorVisible = false;
            Console.WindowWidth = 60;
            Console.WindowHeight = 26;

            AskName();
            AskRace();
            AskClass();
            player.playerPos = new Vector2(1, 1);

            MapLoader loader = new MapLoader();
            level01 = loader.ReadMapFromFile("Maps/level01.json");

            Console.Clear();
            level01.Draw(ConsoleColor.Gray, 0, ".", "#");
            level01.Draw(ConsoleColor.DarkGreen, 1, "?", "!");
            level01.Draw(ConsoleColor.Red, 2, "$", "O");
            player.Draw();

            bool game_running = true;
            while (game_running)
            {
                int moveX = 0;
                int moveY = 0;
                ConsoleKeyInfo key = Console.ReadKey(true);
                    switch (key.Key)
                    {
                        case ConsoleKey.UpArrow:
                            moveY = -1;
                            break;
                        case ConsoleKey.DownArrow:
                            moveY = 1;
                            break;
                        case ConsoleKey.LeftArrow:
                            moveX = -1;
                            break;
                        case ConsoleKey.RightArrow:
                            moveX = 1;
                            break;
                        case ConsoleKey.Escape:
                            game_running = false;
                            break;
                        default:
                            break;
                    }
                int index = (int)player.playerPos.X + moveX + ((int)player.playerPos.Y + moveY) * level01.mapWidth;
                int wallCheck = level01.layers[0].mapTiles[index];
                if (wallCheck != 2)
                {
                    player.Move(moveX, moveY);
                }
                Console.Clear();
                level01.Draw(ConsoleColor.Gray, 0, ".", "#");
                level01.Draw(ConsoleColor.DarkGreen, 1, "?", "!");
                level01.Draw(ConsoleColor.Red, 2, "$", "O");
                ScanEnemiesAndItems();
                player.Draw();
            }
        }
        private void AskName()
        {
            Console.WriteLine("What is your name?");
            while (true)
            {
                string nameanswer = Console.ReadLine();
                if (nameanswer.Length >= 3 && nameanswer.Length <= 10 && !nameanswer.Any(char.IsNumber))
                {
                    player.pName = nameanswer;
                    break;
                }
                else if (nameanswer.Any(char.IsNumber))
                {
                    Console.WriteLine($"Name ({nameanswer}) is not supposed to contain any numbers, try again.");
                    continue;
                }
                else if (nameanswer.Length >= 10)
                {
                    Console.WriteLine("Name too long, try again.");
                }
                else
                {
                    Console.WriteLine("Name too short, try again.");
                }
            }
        }
        private void AskRace()
        {
            Console.WriteLine("Please select your race.");
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
                    Console.WriteLine("Choose using the numbers.");
                    continue;
                }
            }

            switch (raceAnswer)
            {
                case 1:
                    player.pRace = Race.Human;
                    break;
                case 2:
                    player.pRace = Race.Elf;
                    break;
                case 3:
                    player.pRace = Race.Goblin;
                    break;
                default:
                    Console.WriteLine("Something went wrong...");
                    break;
            }
        }

        private void AskClass()
        {
            Console.WriteLine("Please select your class.");
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
                    Console.WriteLine("Choose using the numbers.");
                    continue;
                }
            }

            switch (classAnswer)
            {
                case 1:
                    player.pClass = Class.Knight;
                    break;
                case 2:
                    player.pClass = Class.Archer;
                    break;
                case 3:
                    player.pClass = Class.Mage;
                    break;
                default:
                    Console.WriteLine("Something went wrong...");
                    break;
            }
        }
        private void ScanEnemiesAndItems()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.SetCursorPosition(10, 6);

            Enemy enemyScan = level01.GetEnemyAt((int)player.playerPos.X, (int)player.playerPos.Y);
            Item itemScan = level01.GetItemAt((int)player.playerPos.X, (int)player.playerPos.Y);

            if (enemyScan != null)
            {
                switch (enemyScan.name.ToLower())
                {
                    case "thief":
                        Console.WriteLine($"You hit an enemy: <{enemyScan.name}>");
                        break;
                    case "troll":
                        Console.WriteLine($"You hit an enemy: <{enemyScan.name}>");
                        break;
                    default:
                        break;
                }
            }
            if (itemScan != null)
            {
                Console.WriteLine($"You find an item: <{itemScan.name}>");
            }
            Console.ResetColor();
        }
    }
}

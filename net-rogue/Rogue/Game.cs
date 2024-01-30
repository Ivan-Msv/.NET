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
        public void Run()
        {
            int mapWidth = 8;
            int[] mapTiles = new int[]
            {
                2, 2, 2, 2, 2, 2, 2, 2, 
                2, 1, 1, 2, 1, 1, 1, 2,
                2, 1, 1, 2, 1, 1, 1, 2,
                2, 1, 1, 1, 1, 1, 2, 2,
                2, 2, 2, 2, 1, 1, 1, 2,
                2, 1, 1, 1, 1, 1, 1, 2,
                2, 2, 2, 2, 2, 2, 2, 2
            };  

            Console.CursorVisible = false;
            Console.WindowWidth = 60;
            Console.WindowHeight = 26;



            AskName();
            AskRace();
            player.playerPos = new Vector2(10, 10);

            Console.Clear();
            player.Draw();

            bool game_running = true;
            while (game_running)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        player.Move(0, -1);
                        break;
                    case ConsoleKey.DownArrow:
                        player.Move(0, 1);
                        break;
                    case ConsoleKey.LeftArrow:
                        player.Move(-1, 0);
                        break;
                    case ConsoleKey.RightArrow:
                        player.Move(1, 0);
                        break;
                    case ConsoleKey.Escape:
                        game_running = false;
                        break;
                    default:
                        break;
                }
                Console.Clear();
                player.Draw();
            }
        }
        private void AskName()
        {
            Console.WriteLine("What is your name?");
            while (true)
            {
                string nameanswer = Console.ReadLine();
                if (nameanswer.Length <= 10 && !nameanswer.Any(char.IsNumber))
                {
                    player.pName = nameanswer;
                    break;
                }
                else if (nameanswer.Any(char.IsNumber))
                {
                    Console.WriteLine($"Name ({nameanswer}) is not supposed to contain any numbers, try again.");
                    continue;
                }
                else
                {
                    Console.WriteLine("Name too long, try again.");
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
    }
}

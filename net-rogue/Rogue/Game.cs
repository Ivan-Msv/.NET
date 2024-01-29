using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue
{
    class Game
    {
        PlayerCharacter player = new PlayerCharacter();
        public void Run()
        {
            AskName();
            AskRace();

            while (true)
            {

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

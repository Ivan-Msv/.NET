using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Rogue
{
    public enum Race
    {
        Human,
        Elf,
        Goblin
    }
    public enum Class
    {
        Knight,
        Archer,
        Mage
    }
    internal class PlayerCharacter
    {
        public string pName;
        public Race pRace;
        public Class pClass;

        public Vector2 playerPos;
        public int currentMoney;

        private char image;
        private ConsoleColor color;

        public PlayerCharacter(char image, ConsoleColor color)
        {
            this.image = image;
            this.color = color;
        }

        public void Move(int moveX, int moveY)
        {
            playerPos.X += moveX;
            playerPos.Y += moveY;
            playerPos.X = Math.Min(Math.Max(playerPos.X, 0), Console.WindowWidth - 1);
            playerPos.Y = Math.Min(Math.Max(playerPos.Y, 0), Console.WindowHeight - 1);
        }

        public void Draw()
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(Convert.ToInt32(playerPos.X), Convert.ToInt32(playerPos.Y));
            Console.Write(image);
        }
        public int StartingMoney(Race race)
        {
            int money = 0;
            switch (race)
            {
                case Race.Human:
                    money = 50;
                    break;
                case Race.Elf:
                    money = 75;
                    break;
                case Race.Goblin:
                    money = 15;
                    break;
                default:
                    money = 10;
                    break;
            }
            return money;
        }
    }
}

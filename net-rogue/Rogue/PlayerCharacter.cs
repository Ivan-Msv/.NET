using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using ZeroElectric.Vinculum;

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

        public Vector2 position;
        public int currentMoney;

        private char image;
        private Color color;

        public PlayerCharacter(char image, Color color)
        {
            this.image = image;
            this.color = color;
        }

        public void Move(int moveX, int moveY)
        {
            position.X += moveX;
            position.Y += moveY;
            position.X = Math.Min(Math.Max(position.X, 0), Console.WindowWidth - 1);
            position.Y = Math.Min(Math.Max(position.Y, 0), Console.WindowHeight - 1);
        }

        public void Draw()
        {
            int convertedX = Convert.ToInt32(position.X * Game.tileSize);
            int convertedY = Convert.ToInt32(position.Y * Game.tileSize);
            Raylib.DrawRectangle(convertedX, convertedY, Game.tileSize, Game.tileSize, Raylib.BLACK);
            Raylib.DrawText(image.ToString(), convertedX, convertedY, Game.tileSize, color);
        }
        public int StartingMoney(Race race)
        {
            int money;
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

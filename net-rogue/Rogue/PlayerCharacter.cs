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

        Texture image;
        Color color;

        int imagePixelX;
        int imagePixelY;

        public PlayerCharacter(Color color)
        {
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

            Rectangle imageRect = new Rectangle(imagePixelX, imagePixelY, Game.tileSize, Game.tileSize);
            Raylib.DrawTextureRec(image, imageRect, position * Game.tileSize, Raylib.WHITE);
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
        public void SetPlayerImageAndIndex(Texture atlasImage, int imagesPerRow, Vector2 position)
        {
            int index = (int)position.X + (int)position.Y * imagesPerRow;
            image = atlasImage;
            imagePixelX = (index % imagesPerRow) * Game.tileSize;
            imagePixelY = (index / imagesPerRow) * Game.tileSize;
        }
    }
}

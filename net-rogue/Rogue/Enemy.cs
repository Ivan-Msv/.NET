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
    public class Enemy
    {
        public string name;
        public Vector2 position;
        public int index;
        int imagePixelX;
        int imagePixelY;
        Texture image;
        public Enemy(string name, Vector2 position, int index)
        {
            this.name = name;
            this.index = index;
            this.position = position;
        }
        public Enemy(Enemy copyFrom)
        {
            this.name = copyFrom.name;
            this.position = copyFrom.position;
            this.index = copyFrom.index;
        }
        public Enemy() { }

        public override string ToString()
        {
            return $"{name} — ID ( {index} )";
        }

        public void Draw()
        {
            Rectangle imageRect = new Rectangle(imagePixelX, imagePixelY, Game.tileSize, Game.tileSize);
            Raylib.DrawTextureRec(image, imageRect, position * Game.tileSize, Raylib.WHITE);
        }

        public void SetEnemyImageAndIndex(Texture atlasImage, int imagesPerRow)
        {
            index--;
            image = atlasImage;
            imagePixelX = (index % imagesPerRow) * Game.tileSize;
            imagePixelY = (index / imagesPerRow) * Game.tileSize;
        }
    }
}

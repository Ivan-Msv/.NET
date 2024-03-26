using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using ZeroElectric.Vinculum;

namespace Rogue
{
    class Item
    {
        public string name;
        public Vector2 position;
        int imagePixelX;
        int imagePixelY;
        Texture image;

        public Item(string name, Vector2 position)
        {
            this.name = name;
            this.position = position;
        }

        public void Draw()
        {
            Rectangle imageRect = new Rectangle(imagePixelX, imagePixelY, Game.tileSize, Game.tileSize);
            Raylib.DrawTextureRec(image, imageRect, position * Game.tileSize, Raylib.WHITE);
        }

        public void SetItemImageAndIndex(Texture atlasImage, int imagesPerRow, int index)
        {
            index--;
            image = atlasImage;
            imagePixelX = (index % imagesPerRow) * Game.tileSize;
            imagePixelY = (index / imagesPerRow) * Game.tileSize;
        }
    }
}

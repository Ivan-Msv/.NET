using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using ZeroElectric.Vinculum;
using TurboMapReader;

namespace Rogue
{
    public enum MapTile : int
    {
        Floor = 1,
        StoneLUCorner = 58,
        StoneRUCorner = 60,
        StoneWall = 41,
        StoneWindowWall = 29,
        BrokenStoneWall = 15,
        VerticalStoneWall = 59,

        Thief = 87,
        Troll = 110,

        Sword = 105,
        Potion = 115,
    }
    class Map
    {
        public int mapWidth;
        public MapLayer[] layers { get; set; }
        public int[] mapTiles;
        public TiledMap tileMap { get; private set; }

        public List<Enemy> enemies;
        public List<Item> items;

        public Map(TiledMap map)
        {
            this.tileMap = map;
        }

        //public void OldDraw(Texture image, int layerIndex)
        //{
        //    int map_start_row = 0;
        //    int map_start_col = 0;

        //    for (int row = 0; row < layers[0].mapTiles.Length / mapWidth; row++)
        //    {
        //        for (int col = 0; col < mapWidth; col++)
        //        {
        //            int mapIndex = row * mapWidth + col;

        //            int tileId = layers[layerIndex].mapTiles[mapIndex];
        //            int posX = (map_start_col + col) * Game.tileSize;
        //            int posY = (map_start_row + row) * Game.tileSize;
        //            Vector2 mapPos = new Vector2(posX, posY);
        //            int atlasIndex = 0;

        //            switch (tileId)
        //            {
        //                case 1:
        //                    atlasIndex = 1 + 4 * Game.imagesPerRow;
        //                    break;
        //                case 2:
        //                    atlasIndex = 4 + 3 * Game.imagesPerRow;
        //                    break;
        //                default:
        //                    break;
        //            }

        //            int imagePixelX = atlasIndex % Game.imagesPerRow * Game.tileSize;
        //            int imagePixelY = atlasIndex / Game.imagesPerRow * Game.tileSize;
        //            Rectangle mapRect = new Rectangle(imagePixelX, imagePixelY, Game.tileSize, Game.tileSize);

        //            Raylib.DrawTextureRec(image, mapRect, mapPos, Raylib.WHITE);
        //        }
        //    }
        //}

        public void Draw(Texture image)
        {
            TurboMapReader.MapLayer tileGround = tileMap.layers[0];
            for (int row = 0; row < tileGround.height; row++)
            {
                for (int col = 0; col < tileGround.width; col++)
                {
                    int mapIndex = row * tileGround.width + col;
                    Vector2 mapPos = new Vector2(col, row);
                    int tileId = tileMap.layers[0].data[mapIndex];
                    if (tileId == 0)
                    {
                        continue;
                    }
                    int tileIndex = tileId - 1;

                    int imagePixelX = tileIndex % Game.imagesPerRow * Game.tileSize;
                    int imagePixelY = tileIndex / Game.imagesPerRow * Game.tileSize;
                    Rectangle mapRect = new Rectangle(imagePixelX, imagePixelY, Game.tileSize, Game.tileSize);

                    Raylib.DrawTextureRec(image, mapRect, mapPos * Game.tileSize, Raylib.WHITE);
                }
            }
        }
        public void LoadEnemiesAndItems()
        {
            enemies = new List<Enemy>();
            items = new List<Item>();

            TurboMapReader.MapLayer itemLayer = tileMap.layers[1];
            TurboMapReader.MapLayer enemyLayer = tileMap.layers[2];

            int[] enemyTiles = enemyLayer.data;
            int[] itemTiles = itemLayer.data;

            int mapHeight = enemyLayer.height;
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < enemyLayer.width; x++)
                {
                    Vector2 enemyPosition = new Vector2(x, y);
                    Vector2 itemPosition = new Vector2(x, y);
                    int index = x + y * enemyLayer.width;
                    int enemyTileId = enemyTiles[index];
                    int itemTileId = itemTiles[index];
                    switch(enemyTileId)
                    {
                        case 0:
                            break;
                        case (int)MapTile.Thief:
                            enemies.Add(new Enemy("Thief", enemyPosition));
                            break;
                        case (int)MapTile.Troll:
                            enemies.Add(new Enemy("Troll", enemyPosition));
                            break;
                    }
                    switch (itemTileId)
                    {
                        case 0:
                            break;
                        case (int)MapTile.Sword:
                            items.Add(new Item("Sword", itemPosition));
                            break;
                        case (int)MapTile.Potion:
                            items.Add(new Item("Potion", itemPosition));
                            break;
                    }
                }
            }
        }
        public Enemy GetEnemyAt(Vector2 pos)
        {
            int index = -1;

            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].position == pos)
                {
                    index = i;
                    break;
                }
            }

            if (index != -1)
            {
                return enemies[index];
            }
            else
            {
                return null;
            }
        }
        public Item GetItemAt(Vector2 pos)
        {
            int index = -1;


            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].position == pos)
                {
                    index = i;
                    break;
                }
            }

            if (index != -1)
            {
                return items[index];
            }
            else
            {
                return null;
            }
        }
        public MapTile GetTileAt(int index)
        {
            MapTile tileName = (MapTile)tileMap.layers[0].data[index];
            return tileName;
        }
    }
}

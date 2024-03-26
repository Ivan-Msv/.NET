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
    class Map
    {
        public int mapWidth;
        public MapLayer[] layers { get; set; }
        public int[] mapTiles;
        public MapLayer itemLayer;
        public MapLayer enemyLayer;
        public TiledMap tileMap { get; private set; }

        public List<Enemy> enemies;
        public List<Item> items;

        public Map(TiledMap map)
        {
            this.tileMap = map;
        }

        //public void Draw()
        //{
        //    groundLayer = layers[0];
        //    Console.ForegroundColor = ConsoleColor.Gray;
        //    int map_start_row = 0;
        //    int map_start_col = 0;

        //    for (int row = 0; row < groundLayer.mapTiles.Length / mapWidth; row++)
        //    {
        //        for (int col = 0; col < mapWidth; col++)
        //        {
        //            int mapIndex = row * mapWidth + col;

        //            int tileId = groundLayer.mapTiles[mapIndex];

        //            Console.SetCursorPosition(map_start_col + col, map_start_row + row);

        //            switch (tileId)
        //            {
        //                case 1:
        //                    Console.Write(".");
        //                    break;
        //                case 2:
        //                    Console.Write("#");
        //                    break;
        //                default:
        //                    Console.Write(" ");
        //                    break;
        //            }
        //        }
        //    }
        //} // OLD DRAW

        public void Draw(Texture image, int layerIndex)
        {
            int map_start_row = 0;
            int map_start_col = 0;

            for (int row = 0; row < layers[0].mapTiles.Length / mapWidth; row++)
            {
                for (int col = 0; col < mapWidth; col++)
                {
                    int mapIndex = row * mapWidth + col;

                    int tileId = layers[layerIndex].mapTiles[mapIndex];
                    int posX = (map_start_col + col) * Game.tileSize;
                    int posY = (map_start_row + row) * Game.tileSize;
                    Vector2 mapPos = new Vector2(posX, posY);
                    int atlasIndex = 0;

                    switch (tileId)
                    {
                        case 1:
                            atlasIndex = 1 + 4 * Game.imagesPerRow;
                            break;
                        case 2:
                            atlasIndex = 4 + 3 * Game.imagesPerRow;
                            break;
                        default:
                            break;
                    }

                    int imagePixelX = atlasIndex % Game.imagesPerRow * Game.tileSize;
                    int imagePixelY = atlasIndex / Game.imagesPerRow * Game.tileSize;
                    Rectangle mapRect = new Rectangle(imagePixelX, imagePixelY, Game.tileSize, Game.tileSize);

                    Raylib.DrawTextureRec(image, mapRect, mapPos, Raylib.WHITE);
                }
            }
        }
        public void LoadEnemiesAndItems()
        {
            enemies = new List<Enemy>();
            items = new List<Item>();

            itemLayer = layers[1];
            enemyLayer = layers[2];

            int[] enemyTiles = enemyLayer.mapTiles;
            int[] itemTiles = itemLayer.mapTiles;

            int mapHeight = enemyTiles.Length / mapWidth;
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    Vector2 enemyPosition = new Vector2(x, y);
                    Vector2 itemPosition = new Vector2(x, y);
                    int index = x + y * mapWidth;
                    int enemyTileId = enemyTiles[index];
                    int itemTileId = itemTiles[index];
                    switch(enemyTileId)
                    {
                        case 0:
                            break;
                        case 1:
                            enemies.Add(new Enemy("Thief", enemyPosition));
                            break;
                        case 2:
                            enemies.Add(new Enemy("Troll", enemyPosition));
                            break;
                    }
                    switch(itemTileId)
                    {
                        case 0:
                            break;
                        case 1:
                            items.Add(new Item("Sword", itemPosition));
                            break;
                        case 2:
                            items.Add(new Item("Pickaxe", itemPosition));
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        public void DeleteEnemyOrItem(Enemy enemyScan)
        {
            enemies.Remove(enemyScan);
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using ZeroElectric.Vinculum;
using TurboMapReader;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;

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
        public List<Enemy> enemyTypes;

        public List<Enemy> enemies;
        public List<Item> items;

        public Map(TiledMap map)
        {
            this.tileMap = map;
        }
        /// <summary>
        /// Piirtää kartan annetulla spriteillä
        /// Tämä metodi lukee kartan ja renderöi ne näytölle ID:n mukaan
        /// </summary>
        /// <param name="image">Kuva, joka sisältää spritet</param>
        /// <param name="imagesPerRow">Yksittäisten spriten määrät jokasen rivin kuvassa</param>
        public void Draw(Texture image, int imagesPerRow)
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

                    int imagePixelX = tileIndex % imagesPerRow * Game.tileSize;
                    int imagePixelY = tileIndex / imagesPerRow * Game.tileSize;
                    Rectangle mapRect = new Rectangle(imagePixelX, imagePixelY, Game.tileSize, Game.tileSize);

                    Raylib.DrawTextureRec(image, mapRect, mapPos * Game.tileSize, Raylib.WHITE);
                }
            }
        }
        public void LoadEnemiesAndItems()
        {
            enemies = new List<Enemy>();
            items = new List<Item>();

            LoadEnemyTypes("data/enemies.json");

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

                    if (enemyTileId > 0)
                    {
                        var newEnemy = GetEnemyBySpriteID(enemyTileId);
                        Console.WriteLine(enemyPosition);
                        newEnemy.position = enemyPosition;

                        enemies.Add(newEnemy);
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
        /// <summary>
        /// Lataa kaikki olemassa olevat vihollismallit
        /// </summary>
        /// <param name="fileName">Json tiedoston nimi</param>
        public void LoadEnemyTypes(string fileName)
        {
            enemyTypes = new List<Enemy>();
            //Tarkista että tiedosto on olemassa.
            if (File.Exists(fileName))
            {
                string fileContents;
                try
                {
                    using (StreamReader enemyReader = new StreamReader(fileName))
                    {
                        fileContents = enemyReader.ReadToEnd();
                    }

                    List<Enemy> deserializedContent = JsonConvert.DeserializeObject<List<Enemy>>(fileContents);
                    enemyTypes = deserializedContent;
                }
                catch (FileNotFoundException e)
                {
                    Console.WriteLine("Error message: " + e.Message);
                }
            }
        }
        /// <summary>
        /// Käy joka ikisen mallin läpi, ja jos vihollinenmalli löytyy, niin luo uusi vihollinen
        /// </summary>
        /// <param name="spriteID">Sprite kuvan ID</param>
        private Enemy GetEnemyBySpriteID(int spriteID)
        {
            foreach (Enemy template in enemyTypes)
            {
                // Onko tällä enemyllä sama spriteId kuin mitä on saatiin parametrina
                if (template.index == spriteID)
                {
                    return new Enemy(template);
                }
            }
            // Jos sopivaa ei löytyny, näytä virheilmoitus ja luo testivihollinen
            Console.WriteLine($"Error, no enemy found with id: {spriteID}");
            return new Enemy("testEnemy", new Vector2(0, 0), spriteID);
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

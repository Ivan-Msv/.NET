﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Rogue
{
    class Map
    {
        public int mapWidth;
        public MapLayer[] layers;
        public int[] mapTiles;
        public MapLayer itemLayer;
        public MapLayer enemyLayer;

        List<Enemy> enemies;
        List<Item> items;

        //public void Draw()
        //{
        //    groundLayer = layers[0];
        //    itemLayer = layers[1];
        //    enemyLayer = layers[2];
        //    Console.ForegroundColor = ConsoleColor.Gray;
        //    int map_start_row = 0;
        //    int map_start_col = 0;

        //    for (int row = 0; row < groundLayer.mapTiles.Length / mapWidth; row++)
        //    {
        //        for (int col = 0; col < mapWidth; col++)
        //        {
        //            int mapIndex = row * mapWidth + col;

        //            int tileId = groundLayer.mapTiles[mapIndex];
        //            int itemId = itemLayer.mapTiles[mapIndex];
        //            int enemyId = enemyLayer.mapTiles[mapIndex];

        //            Console.SetCursorPosition(map_start_col + col, map_start_row + row);
        //            switch (itemId)
        //            {
        //                case 1:
        //                    Console.Write("?");
        //                    break;
        //                case 2:
        //                    Console.Write("!");
        //                    break;
        //                default:
        //                    break;
        //            }

        //            switch (enemyId)
        //            {
        //                case 1:
        //                    Console.Write("$");
        //                    break;
        //                case 2:
        //                    Console.Write("O");
        //                    break;
        //                default:
        //                    break;
        //            }

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

        public void Draw(ConsoleColor color, int layerIndex, string case1, string case2)
        {
            Console.ForegroundColor = color;
            int map_start_row = 0;
            int map_start_col = 0;

            for (int row = 0; row < layers[0].mapTiles.Length / mapWidth; row++)
            {
                for (int col = 0; col < mapWidth; col++)
                {
                    int mapIndex = row * mapWidth + col;

                    int tileId = layers[layerIndex].mapTiles[mapIndex];
                    Console.SetCursorPosition(map_start_col + col, map_start_row + row);

                    switch (tileId)
                    {
                        case 1:
                            Console.Write(case1);
                            break;
                        case 2:
                            Console.Write(case2);
                            break;
                        default:
                            break;
                    }
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

        public Enemy GetEnemyAt(int x, int y)
        {
            int index = x + y * mapWidth;
            int enemyCheck = layers[2].mapTiles[index];
            if (enemyCheck != 0)
            {
                return enemies[enemyCheck-1]; // outputting wrong index or something, fix later
            }
            else
            {
                return null;
            }
        }
        public Item GetItemAt(int x, int y)
        {
            int index = x + y * mapWidth;
            int itemCheck = layers[1].mapTiles[index];
            if (itemCheck != 0)
            {
                return items[itemCheck - 1];
            }
            else
            {
                return null;
            }
        }
    }
}

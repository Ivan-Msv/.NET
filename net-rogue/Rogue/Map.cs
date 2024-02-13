using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue
{
    class Map
    {
        public int mapWidth;
        public int[] mapTiles;
        public void Draw()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            int map_start_row = 0;
            int map_start_col = 0;
            for (int row = 0; row < mapTiles.Length / mapWidth; row++)
            {
                for (int col = 0; col < mapWidth; col++)
                {
                    int tileId = mapTiles[row * mapWidth + col];
                    Console.SetCursorPosition(map_start_col + col, map_start_row + row);
                    switch (tileId)
                    {
                        case 1:
                            Console.Write(".");
                            break;
                        case 2:
                            Console.Write("#");
                            break;
                        default:
                            Console.Write(" ");
                            break;
                    }
                }
            }
        }
    }
}

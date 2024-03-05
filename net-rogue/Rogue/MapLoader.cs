using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue
{
    internal class MapLoader
    {
        public Map LoadTestMap()
        {
            Map test = new Map();
            test.mapWidth = 8;
            int[] mapTiles = new int[]
            {
                2, 2, 2, 2, 2, 2, 2, 2,
                2, 1, 1, 2, 1, 1, 1, 2,
                2, 1, 1, 2, 1, 1, 1, 2,
                2, 2, 2, 2, 2, 2, 2, 2,
                2, 2, 2, 2, 1, 1, 1, 2,
                2, 1, 1, 1, 1, 1, 1, 2,
                2, 2, 2, 2, 2, 2, 2, 2
            };
            test.mapTiles = mapTiles;
            return test;
        }
        public Map ReadMapFromFile(string fileName)
        {
            bool fileFound = File.Exists(fileName);
            if (!fileFound)
            {
                Console.WriteLine($"File {fileName} not found");
                return LoadTestMap();
            }

            string fileContents;

            using (StreamReader reader = File.OpenText(fileName))
            {
                fileContents = reader.ReadToEnd();
            }

            Map loadedMap = JsonConvert.DeserializeObject<Map>(fileContents);

            loadedMap.LoadEnemiesAndItems();

            return loadedMap;
        }
    }
}

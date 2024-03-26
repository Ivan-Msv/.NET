using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurboMapReader;

namespace Rogue
{
    internal class MapLoader
    {

        public Map ReadMapFromFile(string fileName)
        {
            TiledMap loadedTileMap = TurboMapReader.MapReader.LoadMapFromFile(fileName);
            return CreateMapObject(loadedTileMap);
        }

        private Map CreateMapObject(TiledMap loadedTileMap)
        {
            Map map = new Map(loadedTileMap);
            return map;
        }
    }
}

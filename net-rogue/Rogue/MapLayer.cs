﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue
{
    class MapLayer
    {
        public string name;
        public int[] mapTiles;

        public MapLayer(string name, int[] maptiles)
        {
            this.name = name;
            mapTiles = maptiles;
        }
    }
}

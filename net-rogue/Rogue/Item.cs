using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Rogue
{
    class Item
    {
        public string name;
        public Vector2 position;

        public Item(string name, Vector2 position)
        {
            this.name = name;
            this.position = position;
        }
    }
}

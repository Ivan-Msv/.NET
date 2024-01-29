using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Rogue
{
    public enum Race
    {
        Human,
        Elf,
        Goblin
    }
    public enum Class
    {
        Knight,
        Archer,
        Mage
    }
    internal class PlayerCharacter
    {
        public string pName;
        public Race pRace;
        public Class pClass;

        public Vector2 playerPos;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewMapBuilder
{
    class Tile :ITilable<Tile>
    {
        private readonly int x, y;
        private readonly int id;
        private readonly Tile[] neighbours;
        public int ID => id;
        public Tile[] Neighbours => neighbours;
    }
}

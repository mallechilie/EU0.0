using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewMapBuilder
{
    class Province : ITilableWithBase<Province, Tile>
    {
        private readonly Tile[] tiles;
        private readonly int id;
        private readonly Province[] neighbours;
        public Tile[] Tiles => tiles;
        public int ID => id;
        public Province[] Neighbours => neighbours;

    }
}

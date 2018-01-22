using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewMapBuilder
{
    class ProvinceMap : ITileMapWithBase<Province, Tile>
    {
        private readonly Province[] tiles;
        private readonly TileMap map;
        public Province[] Tiles
        {
            get { return tiles; }
        }
        public ITileMap<Tile> Map
        {
            get { return map; }
        }
    }
}

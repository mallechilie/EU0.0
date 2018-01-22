using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewMapBuilder
{
    class NationMap: ITileMapWithBase<Nation, Province>
    {
        private readonly Nation[] tiles;
        private readonly ProvinceMap map;
        public Nation[] Tiles
        {
            get { return tiles; }
        }
        public ITileMap<Province> Map
        {
            get { return map; }
        }
    }
}

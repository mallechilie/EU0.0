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
        public Nation this[int index]
        {
            get => index < 0 || index >= tiles.Length ? null : tiles[index];
            private set
            {
                if (index > 0 && index <= tiles.Length) tiles[index] = value;
            }
        }
        private readonly ProvinceMap map;
        public Nation[] Tiles
        {
            get { return tiles; }
        }
        public ITileMap<Province> Map
        {
            get { return map; }
        }


        public NationMap(ProvinceMap map, int nations)
        {
            this.map = map;
            tiles = new Nation[nations];
            //TODO: initialize nations
        }
    }
}

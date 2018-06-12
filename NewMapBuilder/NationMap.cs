using System.Collections.Generic;
using System.Linq;

namespace NewMapBuilder
{
    public class NationMap : ITileMapWithBase<Nation, Province>
    {
        private readonly Nation[] tiles;

        public Nation this[int index]
        {
            //TODO: out of range => null?
            get => index < 0 || index >= tiles.Length ? null : tiles[index];
            private set
            {
                if (index > 0 && index <= tiles.Length)
                    tiles[index] = value;
            }
        }

        private readonly ProvinceMap map;
        private ProvinceMap provinceMap;

        public Nation[] Tiles => tiles;
        public ITileMap<Province> Map => map;


        public NationMap(ProvinceMap map)
        {
            this.map = map;
            //TODO: initialize privinces
            Dictionary<int, List<Province>> nations = this.GenerateTileGroup();
            tiles = new Nation[nations.Count];
            for (int i = 0; i < tiles.Length; i++)
                tiles[i] = new Nation(nations[nations.Keys.ElementAt(i)].ToArray(), i);
            for (int i = 0; i < tiles.Length; i++)
                tiles[i].GetNeighbours(this);
        }
    }
}

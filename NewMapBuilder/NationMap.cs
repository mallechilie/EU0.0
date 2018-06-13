using System.Collections.Generic;
using System.Linq;

namespace NewMapBuilder
{
    public class NationMap : ITileMapWithBase<Nation, Province>
    {
        public Nation this[int index] => index < 0 || index >= Tiles.Length ? null : Tiles[index];

        private readonly ProvinceMap map;

        public Nation[] Tiles
        {
            get;
        }

        public ITileMap<Province> Map => map;


        public NationMap(ProvinceMap map)
        {
            this.map = map;
            //TODO: initialize privinces
            Dictionary<int, List<Province>> nations = this.GenerateTileGroup();
            Tiles = new Nation[nations.Count];
            for (int i = 0; i < Tiles.Length; i++)
                Tiles[i] = new Nation(nations[nations.Keys.ElementAt(i)].ToArray(), i);
            for (int i = 0; i < Tiles.Length; i++)
                Tiles[i].GetNeighbours(this);
        }
    }
}

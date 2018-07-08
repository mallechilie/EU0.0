using System.Collections.Generic;
using System.Linq;

namespace NewMapBuilder
{
    public class ProvinceMap : ITileMapWithBase<Province, Tile>
    {
        private readonly TileMap map;
        public Province this[int index] => index < 0 || index >= Tiles.Length ? null : Tiles[index];

        public Province[] Tiles
        {
            get;
        }

        public ITileMap<Tile> Map => map;


        public ProvinceMap(TileMap map)
        {
            this.map = map;
            //TODO: initialize privinces
            Dictionary<int, List<Tile>> provinces = this.GenerateTileGroup();
            Tiles = provinces.Select((n, i) => new Province(n.Value, i)).ToArray();
            for (int i = 0; i < Tiles.Length; i++)
                Tiles[i].GetNeighbours(this);
        }
    }
}

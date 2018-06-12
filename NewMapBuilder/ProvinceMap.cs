using System.Collections.Generic;
using System.Linq;

namespace NewMapBuilder
{
    public class ProvinceMap : ITileMapWithBase<Province, Tile>
    {
        private readonly Province[] tiles;
        private readonly TileMap map;
        public Province this[int index]
        {
            // TODO: out of range => null?
            get => index < 0 || index >= tiles.Length ? null : tiles[index];
            private set
            {
                if (index > 0 && index <= tiles.Length) tiles[index] = value;
            }
        }
        public Province[] Tiles => tiles;

        public ITileMap<Tile> Map => map;


        public ProvinceMap(TileMap map)
        {
            this.map = map;
            //TODO: initialize privinces
            Dictionary<int, List<Tile>> provinces = MapExtentions<Province, Tile>.GenerateTileGroup(this);
            tiles = new Province[provinces.Count];
            for (int i = 0; i < tiles.Length; i++)
                tiles[i] = new Province(provinces[provinces.Keys.ElementAt(i)].ToArray(), i);
        }
    }
}

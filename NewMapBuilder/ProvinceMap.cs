namespace NewMapBuilder
{
    class ProvinceMap : ITileMapWithBase<Province, Tile>
    {
        private readonly Province[] tiles;
        private readonly TileMap map;
        public Province this[int index] => tiles[index];
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

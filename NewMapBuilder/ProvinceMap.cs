namespace NewMapBuilder
{
    class ProvinceMap : ITileMapWithBase<Province, Tile>
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
        public Province[] Tiles
        {
            get { return tiles; }
        }
        public ITileMap<Tile> Map
        {
            get { return map; }
        }


        public ProvinceMap(TileMap map, int provinces)
        {
            this.map = map;
            tiles = new Province[provinces];
            //TODO: initialize privinces
        }
    }
}

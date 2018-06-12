namespace NewMapBuilder
{
    class NationMap: ITileMapWithBase<Nation, Province>
    {
        private readonly Nation[] tiles;
        private readonly ProvinceMap map;

        public Nation this[int index] => tiles[index];

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

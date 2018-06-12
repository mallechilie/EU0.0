namespace NewMapBuilder
{
    public class Province : ITilableWithBase<Province, Tile>
    {
        private readonly Tile[] tiles;
        private readonly int id;
        private readonly Province[] neighbours;

        public Province(Tile[] tiles, int id)
        {
            this.tiles = tiles;
            foreach (var tile in tiles)
            {
                tile.ProvinceID = id;
            }
            this.id = id;
        }

        public Tile[] Tiles => tiles;
        public int ID => id;
        public Province[] Neighbours => neighbours;

    }
}

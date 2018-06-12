namespace NewMapBuilder
{
    public class Nation : ITilableWithBase<Nation,Province>
    {
        private readonly int id;
        private readonly Province[] tiles;
        public int ID => id;

        public Nation(Province[] tiles, int id)
        {
            this.tiles = tiles;
            foreach (Province tile in tiles)
            {
                tile.ParentID = id;
            }
            this.id = id;
        }

        public Nation[] Neighbours
        {
            get;
            set;
        }
        public Province[] Tiles => tiles;
    }
}

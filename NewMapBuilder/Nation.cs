namespace NewMapBuilder
{
    public class Nation : ITilableWithBase<Nation,Province>
    {
        public int ID
        {
            get;
        }

        public Nation(Province[] tiles, int id)
        {
            Tiles = tiles;
            foreach (Province tile in tiles)
            {
                tile.ParentID = id;
            }
            ID = id;
        }

        public Nation[] Neighbours
        {
            get;
            set;
        }
        public Province[] Tiles
        {
            get;
        }
    }
}

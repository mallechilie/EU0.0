namespace NewMapBuilder
{
    public class Province : ITilableWithBase<Province, Tile>, ITilableWithParent<Province>
    {
        public Province(Tile[] tiles, int id)
        {
            ParentID = -1;
            Tiles = tiles;
            foreach (Tile tile in tiles)
            {
                tile.ParentID = id;
            }
            ID = id;
        }

        public Tile[] Tiles
        {
            get;
        }

        public int ID
        {
            get;
        }

        public Province[] Neighbours
        {
            get;
            set;
        }

        public int ParentID
        {
            get;
            set;
        }
    }
}

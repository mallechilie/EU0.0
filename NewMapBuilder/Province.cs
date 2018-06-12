using System.Collections.Generic;
using System.Linq;

namespace NewMapBuilder
{
    public class Province : ITilableWithBase<Province, Tile>, ITilableWithParent<Province>
    {
        private readonly Tile[] tiles;
        private readonly int id;

        public Province(Tile[] tiles, int id)
        {
            ParentID = -1;
            this.tiles = tiles;
            foreach (Tile tile in tiles)
            {
                tile.ParentID = id;
            }
            this.id = id;
        }

        public Tile[] Tiles => tiles;
        public int ID => id;
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

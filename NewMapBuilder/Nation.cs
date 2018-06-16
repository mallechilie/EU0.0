using System.Collections.Generic;

namespace NewMapBuilder
{
    public class Nation : ITilableWithBase<Nation,Province>
    {
        public int ID
        {
            get;
        }

        public Nation(List<Province> tiles, int id)
        {
            Tiles = tiles;
            foreach (Province tile in tiles)
            {
                tile.Parent = this;
            }
            ID = id;
        }

        public Nation[] Neighbours
        {
            get;
            set;
        }
        public List<Province> Tiles
        {
            get;
        }
    }
}

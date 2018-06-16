using System;
using System.Collections.Generic;

namespace NewMapBuilder
{
    public class Province : ITilableWithBase<Province, Tile>, ITilableWithParent<Province, Nation>
    {
        public ProvinceInfo ProvinceInfo
        {
            get;
            internal set;
        }

        public Province(List<Tile> tiles, int id)
        {
            Tiles = tiles;
            foreach (Tile tile in tiles)
            {
                tile.Parent = this;
            }
            ID = id;
        }

        public List<Tile> Tiles
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
        
        public Nation Parent
        {
            get;
            set;
        }
    }
}

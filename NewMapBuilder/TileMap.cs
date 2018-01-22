using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewMapBuilder
{
    class TileMap : ITileMap<Tile>
    {
        private readonly Tile[] tiles;
        public Tile[] Tiles
        {
            get { return tiles; }
        }
        
    }
}

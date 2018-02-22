using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewMapBuilder
{
    public class Tile : ITilable<Tile>
    {
        private readonly int x, y;
        private readonly int id;
        private Tile[] neighbours;
        public int ID => id;
        public Tile[] Neighbours => neighbours;

        internal Tile(int x, int y, int id)
        {
            this.x = x;
            this.y = y;
            this.id = id;
        }


        internal void GetNeighbours(TileMap map)
        {
            if(map[x,y]!=this)
                throw new ArgumentException("The wrong map was given as input.");
            List<Tile> tiles = new List<Tile>();
            tiles.Add(map[x - 1, y]);
            tiles.Add(map[x, y - 1]);
            tiles.Add(map[x + 1, y]);
            tiles.Add(map[x, y + 1]);
            neighbours = tiles.Where(t => t != null).ToArray();
        }
    }
}

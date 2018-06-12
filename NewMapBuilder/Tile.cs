using System;
using System.Collections.Generic;
using System.Linq;
using TerrainGeneration;

namespace NewMapBuilder
{
    public class Tile : ITilable<Tile>
    {
        public Tile(int x, int y, CoordinateSystem coordinateSystem, TileMap tileMap)
        {
            this.tileMap = tileMap;
            this.id = x * coordinateSystem.height + y;
            neighbours = coordinateSystem.GetNeightbours(x, y);
        }

        private readonly int id;
        private readonly Coordinate[] neighbours;
        private readonly TileMap tileMap;
        public int ID => id;

        public Tile[] Neighbours
        {
            get
            {
                Tile[] tiles = new Tile[neighbours.Length];
                for (int i = 0; i < neighbours.Length; i++)
                    tiles[i] = tileMap[neighbours[i]];
                return tiles;
            }
        }
    }
}

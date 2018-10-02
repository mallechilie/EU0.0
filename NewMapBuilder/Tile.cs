using TerrainGeneration;

namespace NewMapBuilder
{
    public class Tile : ITilableWithParent<Tile>
    {
        public Tile(int x, int y, float height, CoordinateSystem coordinateSystem, TileMap tileMap, double waterHeight = 0)
        {
            ParentID = -1;
            this.x = x;
            this.y = y;
            this.tileMap = tileMap;
            Height = height;
            ID = x * coordinateSystem.Height + y;
            neighbours = coordinateSystem.GetDirectNeightbours(x, y);
            WaterHeight = waterHeight;
        }

        private readonly int x, y;
        private readonly Coordinate[] neighbours;
        private readonly TileMap tileMap;
        public readonly double WaterHeight;
        public readonly float Height;
        public int ID
        {
            get;
        }

        public Tile[] Neighbours
        {
            get
            {
                Tile[] tiles = new Tile[neighbours.Length];
                for (int i = 0; i < neighbours.Length; i++)
                    tiles[i] = tileMap[neighbours[i]];
                return tiles;
            }
            set
            {
            }
        }

        public override string ToString()
        {
            return $"{x} {y}";
        }

        public int ParentID
        {
            get;
            set;
        }
    }
}

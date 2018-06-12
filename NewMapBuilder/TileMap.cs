using TerrainGeneration;

namespace NewMapBuilder
{
    class TileMap : ITileMap<Tile>
    {
        private readonly Tile[] tiles;
        public readonly int Width, Height;
        public TileMap(CoordinateSystem coordinateSystem)
        {
            Width = coordinateSystem.width;
            Height = coordinateSystem.height;
            tiles = new Tile[Width * Height];
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    tiles[x * Height + y] = new Tile(x, y, coordinateSystem, this);
        }
        public Tile this[int index] => tiles[index];

        public Tile this[Coordinate coordinate] => tiles[coordinate.X * Height + coordinate.Y];

        public Tile[] Tiles => tiles;
    }
}

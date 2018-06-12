using TerrainGeneration;

namespace NewMapBuilder
{
    public class TileMap : ITileMap<Tile>
    {
        public readonly int Width, Height;
        public int Size => tiles.Length;
        private readonly Tile[] tiles;
        public TileMap(CoordinateSystem coordinateSystem)
        {
            Width = coordinateSystem.width;
            Height = coordinateSystem.height;
            tiles = new Tile[Width * Height];
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    tiles[x * Height + y] = new Tile(x, y, coordinateSystem, this);
        }
        public Tile this[int index]
        {
            //TODO: out of range => null?
            get => index < 0 || index >= tiles.Length ? null : tiles[index];
            private set
            {
                if (index >= 0 && index < tiles.Length)
                    tiles[index] = value;
            }
        }

        public Tile this[Coordinate coordinate] => tiles[coordinate.X * Height + coordinate.Y];

        public Tile this[int x, int y] => this[x * Height + y];

        public Tile[] Tiles => tiles;
    }
}

using TerrainGeneration;

namespace NewMapBuilder
{
    public class TileMap : ITileMap<Tile>
    {
        private readonly int height;

        public TileMap(GenerateHeight heightMap)
        {
            int width = heightMap.Cs.Width;
            height = heightMap.Cs.Height;
            Tiles = new Tile[width * height];
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    Tiles[x * height + y] = new Tile(x, y, heightMap.HeightMap[x,y], heightMap.Cs, this);
        }
        public TileMap(GenerateWater waterMap)
        {
            int width = waterMap.HeightMap.Cs.Width;
            height = waterMap.HeightMap.Cs.Height;
            Tiles = new Tile[width * height];
            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                Tiles[x * height + y] = new Tile(x, y, waterMap.HeightMap.HeightMap[x, y], waterMap.HeightMap.Cs, this, waterMap.WaterHeights[x,y]);
        }

        public Tile this[int index] => index < 0 || index >= Tiles.Length ? null : Tiles[index];

        public Tile this[Coordinate coordinate] => Tiles[coordinate.X * height + coordinate.Y];

        public Tile this[int x, int y] => this[x * height + y];

        public Tile[] Tiles
        {
            get;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewMapBuilder
{
    class TileMap : ITileMap<Tile>
    {
        public readonly int Width, Height;
        public int Size => tiles.Length;
        private readonly Tile[] tiles;
        public Tile[] Tiles => tiles;
        public Tile this[int index]
        {
            get => index < 0 || index >= tiles.Length ? null : tiles[index];
            private set
            {
                if (index >= 0 && index < tiles.Length)
                    tiles[index] = value;
            }
        }
        public Tile this[int x, int y]
        {
            get => this[x + y * Width];
            set => this[x + y * Width] = value;
        }


        public TileMap(int width, int height)
        {
            Width = width;
            Height = height;
            tiles = new Tile[width * height];
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    this[x, y] = new Tile(x, y, x + y * width);
            for (int i = 0; i < Size; i++)
                this[i].GetNeighbours(this);
        }


    }
}

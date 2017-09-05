using System.Linq;
using System;
using TerrainGeneration;

namespace MapBuilder
{
	public class Map
	{
		public static Random R;
		public enum topology { square, cilinder, sphere, torus }
		public readonly Tile[,] Tiles;
		public readonly topology MapTopology;
		public readonly bool regular = true;
		public Province[] Provinces;
		public Nation[] Nations;

		internal Map(Tile.topology topology, int x, int y, int provinces = 0, int nations = 0, Random r = null)
		{
			R = r ?? new Random();
			MapTopology = Map.topology.square;

			Tiles = new Tile[x, y];
			WaterGeneration GH = new WaterGeneration(new GenerateHeight(x, y, 3).HeightMap);
			for (x = 0; x < Tiles.GetLength(0); x++)
				for (y = 0; y < Tiles.GetLength(1); y++)
					Tiles[x, y] = new Tile(topology, x, y,1 + x + y * Tiles.GetLength(0), GH.HeightMap[x, y], GH.water[x, y]);
			for (x = 0; x < Tiles.GetLength(0); x++)
				for (y = 0; y < Tiles.GetLength(1); y++)
					for (int n = 0; n < Tiles[x, y].Neighbours.Length; n++)
						Tiles[x, y].Neighbours[n] = GetNeighbour(x, y, n);

			if (provinces == 0)
				provinces = x;
			Provinces = new Province[provinces];
			for (int n = 0; n < provinces; n++)
				Provinces[n] = new Province(this, x * y / provinces * 2, n);
			Provinces = Provinces.Where(p => p.Tiles[0] != null).ToArray();
			for (int n = 0; n < Provinces.Length; n++)
				Provinces[n].Id = n;


			if (nations == 0)
				nations = (int)Math.Sqrt(provinces);
			Nations = new Nation[nations];
			for (int n = 0; n < nations; n++)
				Nations[n] = new Nation(this, provinces / nations * 2, n);
			Nations = Nations.Where(p => p.Provinces.Count >0).ToArray();
			for (int n = 0; n < Nations.Length; n++)
				Nations[n].Id = n;
		}

		public static Map GenerateMap(Tile.topology topology, int width, int height, int provinces, int nations, Random r = null)
		{
			return new Map(topology, width, height, provinces, nations, r);
		}

		private Tile GetNeighbour(int x, int y, int n)
		{
			switch (Tiles[x, y].TileTopology)
			{
				case Tile.topology.triangle:
					switch (n)
					{
						case 0:
							if (x > 0)
								return Tiles[x - 1, y];
							break;
						case 1:
							if ((x + y) % 2 == 0)
							{
								if (y > 0)
									return Tiles[x, y - 1];
							}
							else
							{
								if (y < Tiles.GetLength(1) - 1)
									return Tiles[x, y + 1];
							}
							break;
						case 2:
							if (x < Tiles.GetLength(0) - 1)
								return Tiles[x + 1, y];
							break;
						default: break;
					}
					break;
				case Tile.topology.square:
					switch (n)
					{
						case 0:
							if (x > 0)
								return Tiles[x - 1, y];
							break;
						case 1:
							if (y > 0)
								return Tiles[x, y - 1];
							break;
						case 2:
							if (x < Tiles.GetLength(0) - 1)
								return Tiles[x + 1, y];
							break;
						case 3:
							if (y < Tiles.GetLength(1) - 1)
								return Tiles[x, y + 1];
							break;
						default: break;
					}
					break;
				case Tile.topology.hexagon:
					switch (n)
					{
						case 0:
							if (x % 2 == 0)
							{
								if (x > 0 && y > 0)
									return Tiles[x - 1, y - 1];
							}
							else
							{
								if (x > 0)
									return Tiles[x - 1, y];
							}
							break;
						case 1:
							if (y > 0)
								return Tiles[x, y - 1];
							break;
						case 2:
							if (x % 2 == 0)
							{
								if (x < Tiles.GetLength(0) - 1 && y > 0)
									return Tiles[x + 1, y - 1];
							}
							else
							{
								if (x < Tiles.GetLength(0) - 1)
									return Tiles[x + 1, y];
							}
							break;
						case 3:
							if (x % 2 == 0)
							{
								if (x < Tiles.GetLength(0) - 1)
									return Tiles[x + 1, y];
							}
							else
							{
								if (x < Tiles.GetLength(0) - 1 && y < Tiles.GetLength(1) - 1)
									return Tiles[x + 1, y + 1];
							}
							break;
						case 4:
							if (y < Tiles.GetLength(1) - 1)
								return Tiles[x, y + 1];
							break;
						case 5:
							if (x % 2 == 0)
							{
								if (x > 0)
									return Tiles[x - 1, y];
							}
							else
							{
								if (x > 0 && y < Tiles.GetLength(1) - 1)
									return Tiles[x - 1, y + 1];
							}
							break;
						default: break;
					}
					break;
				default: break;
			}
			return default(Tile);
		}

		public override string ToString()
		{
			string s = "";
			switch (Tiles[0, 0].TileTopology)
			{
				case Tile.topology.square:
					{
						for (int y = 0; y < Tiles.GetLength(1); y++)
						{
							for (int x = 0; x < Tiles.GetLength(0); x++)
								s = string.Concat(s, Tiles[x, y] + " ");
							s = string.Concat(s, "\n");
						}
						break;
					}
				case Tile.topology.hexagon:
					{
						for (int y = 0; y < Tiles.GetLength(1); y++)
						{
							for (int x = 0; x < Tiles.GetLength(0); x++)
								if(x%2==0)
								s = string.Concat(s, Tiles[x, y] + "     ");
							s = string.Concat(s, "\n   ");
							for (int x = 0; x < Tiles.GetLength(0); x++)
								if (x % 2 != 0)
									s = string.Concat(s, Tiles[x, y] + "     ");
							s = string.Concat(s, "\n");
						}
						break;
					}
				case Tile.topology.triangle:
					{
						for (int y = 0; y < Tiles.GetLength(1); y++)
						{
							if (y % 2 == 0)
							{
								for (int x = 0; x < Tiles.GetLength(0); x++)
									if (x % 2 == 0)
										s = string.Concat(s, Tiles[x, y] + "   ");
								s = string.Concat(s, "\n  ");
								for (int x = 0; x < Tiles.GetLength(0); x++)
									if (x % 2 != 0)
										s = string.Concat(s, Tiles[x, y] + "   ");
								s = string.Concat(s, "\n");
							}
							if (y % 2 != 0)
							{
								s = string.Concat(s, "  ");
								for (int x = 0; x < Tiles.GetLength(0); x++)
									if (x % 2 != 0)
										s = string.Concat(s, Tiles[x, y] + "   ");
								s = string.Concat(s, "\n");
								for (int x = 0; x < Tiles.GetLength(0); x++)
									if (x % 2 == 0)
										s = string.Concat(s, Tiles[x, y] + "   ");
								s = string.Concat(s, "\n");
							}
						}
						break;
					}
			}
			return s;
		}
	}
}
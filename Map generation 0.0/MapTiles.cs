using System.Linq;

namespace MapGeneration0_0
{
	public class MapTiles
	{
		public enum topology { square, cilinder, sphere, torus }
		public readonly Tile[,] Tiles;
		public readonly topology MapTopology;

		public MapTiles(Tile.topology topology, int x, int y)
		{
			MapTopology = MapTiles.topology.square;
			Tiles = new Tile[x, y];
			for (x = 0; x < Tiles.GetLength(0); x++)
				for (y = 0; y < Tiles.GetLength(1); y++)
					Tiles[x, y] = new Tile(topology, 1 + x + y * Tiles.GetLength(0));
			for (x = 0; x < Tiles.GetLength(0); x++)
				for (y = 0; y < Tiles.GetLength(1); y++)
					for (int n = 0; n < Tiles[x, y].Neighbours.Length; n++)
						Tiles[x, y].Neighbours[n] = GetNeighbour(x, y, n);
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

	public class Tile
	{
		public enum topology { triangle, square, hexagon}
		public Tile[] Neighbours;
		public topology TileTopology;
		public int Id;
		public ProvinceTile Province;
		public bool HasProvince { get { return Province != null; } }
		
		public Tile(topology topology, int id, Tile[] tiles=null)
		{
			Id = id;
			TileTopology = topology;
			Neighbours = TileTopology == topology.triangle ? new Tile[3] :
						 TileTopology == topology.square ? new Tile[4] :
						 TileTopology == topology.hexagon ? new Tile[6] : null;
			
			if (tiles != null)
				for (int x = 0; x < Neighbours.Length; x++)
					Neighbours[x] = tiles[x];
		}

		public override string ToString()
		{
			return Province == null ? 0.ToString() : Province.Id.ToString(); // Neighbours.Count(t => t.Id != 0).ToString();
		}
		public bool Equals(Tile tile)
		{
			return Id==tile.Id;
		}
	}
}
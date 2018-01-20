using System.Linq;
using Enumeration;

namespace MapBuilder
{
	public class Tile
	{
		public readonly float Height;
		public readonly float WaterHeight;
		public Tile[] Neighbours;
		public int EmptyNeighbours
		{
			get {
				return Neighbours.Where(t => t!=null && !t.HasProvince).Count();
			}
		}
		public readonly TileShape TileTopology;
		public int Id;
		public Province Province;
		// Could be made internal? (Every tile should have a province.)
		public bool HasProvince { get { return Province != null; } }
		public readonly int X, Y;
		
		internal Tile(TileShape topology, int x, int y, int id, float height, float waterHeight, Tile[] tiles=null)
		{
			WaterHeight = waterHeight;
			X = x; Y = y;
			Id = id;
			TileTopology = topology;
			Neighbours = TileTopology == TileShape.Triangular ? new Tile[3] :
						 TileTopology == TileShape.Square ? new Tile[4] :
						 TileTopology == TileShape.Hex ? new Tile[6] : null;
			
			if (tiles != null)
				for (int n = 0; n < Neighbours.Length; n++)
					Neighbours[n] = tiles[n];

			Height = height;
		}

		public override string ToString()
		{
			return Id.ToString(); // Province == null ? 0.ToString() : Province.Id.ToString(); //  Neighbours.Count(t => t!=null && t.Id != 0).ToString(); //
		}
		public bool Equals(Tile tile)
		{
			return Id==tile.Id;
		}
	}
}
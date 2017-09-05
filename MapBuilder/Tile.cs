using System.Linq;

namespace MapBuilder
{
	public class Tile
	{
		public readonly float Height;
		public readonly float WaterHeight;
		public enum topology { triangle, square, hexagon}
		public Tile[] Neighbours;
		public int EmptyNeighbours
		{
			get {
				return Neighbours.Where(t => t!=null && !t.HasProvince).Count();
			}
		}
		public topology TileTopology;
		public int Id;
		public Province Province;
		// Could be made internal? (Every tile should have a province.)
		public bool HasProvince { get { return Province != null; } }
		public readonly int x, y;
		
		internal Tile(topology topology, int x, int y, int id, float Height, float WaterHeight, Tile[] tiles=null)
		{
			this.WaterHeight = WaterHeight;
			this.x = x; this.y = y;
			Id = id;
			TileTopology = topology;
			Neighbours = TileTopology == topology.triangle ? new Tile[3] :
						 TileTopology == topology.square ? new Tile[4] :
						 TileTopology == topology.hexagon ? new Tile[6] : null;
			
			if (tiles != null)
				for (int n = 0; n < Neighbours.Length; n++)
					Neighbours[n] = tiles[n];

			this.Height = Height;
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
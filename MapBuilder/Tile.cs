using System.Linq;

namespace MapBuilder
{
	public class Tile
	{
		public readonly float Height;
		public readonly float WaterHeight;
		public enum Topology { Triangle, Square, Hexagon}
		public Tile[] Neighbours;
		public int EmptyNeighbours
		{
			get {
				return Neighbours.Where(t => t!=null && !t.HasProvince).Count();
			}
		}
		public Topology TileTopology;
		public int Id;
		public Province Province;
		// Could be made internal? (Every tile should have a province.)
		public bool HasProvince { get { return Province != null; } }
		public readonly int X, Y;
		
		internal Tile(Topology topology, int x, int y, int id, float height, float waterHeight, Tile[] tiles=null)
		{
			this.WaterHeight = waterHeight;
			this.X = x; this.Y = y;
			Id = id;
			TileTopology = topology;
			Neighbours = TileTopology == Topology.Triangle ? new Tile[3] :
						 TileTopology == Topology.Square ? new Tile[4] :
						 TileTopology == Topology.Hexagon ? new Tile[6] : null;
			
			if (tiles != null)
				for (int n = 0; n < Neighbours.Length; n++)
					Neighbours[n] = tiles[n];

			this.Height = height;
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
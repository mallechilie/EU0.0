using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TerrainGeneration
{
	public struct Coordinate
	{
		public int x, y;

		public Coordinate(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public override string ToString()
		{
			return x.ToString() + ", " + y.ToString();
		}
	}
	public class CoordinateSystem
	{
		int maxx, maxy;

		public CoordinateSystem(int maxx, int maxy)
		{
			this.maxx = maxx;
			this.maxy = maxy;
		}

		public Coordinate[] GetNeightbours(Coordinate c)
		{
			Coordinate[] coordinates = new[] { new Coordinate(c.x - 1, c.y), new Coordinate(c.x + 1, c.y), new Coordinate(c.x, c.y - 1), new Coordinate(c.x, c.y + 1) , new Coordinate(c.x - 1, c.y - 1), new Coordinate(c.x + 1, c.y + 1), new Coordinate(c.x + 1, c.y - 1), new Coordinate(c.x - 1, c.y + 1) };
			return coordinates.Where(cc => cc.x >= 0 && cc.y >= 0 && cc.x < maxx && cc.y < maxy).ToArray();
		}
		public override string ToString()
		{
			return maxx.ToString() + ", " + maxy.ToString();
		}
	}
}


using System;
using System.Collections.Generic;
using System.Linq;

namespace TerrainGeneration
{
	public class Water
	{
		public readonly StaticState StaticState;
		public DynamicState Previous;
		public DynamicState Next;



	}

	public class StaticState
	{
        private StaticTileState[,] tiles;

	}

	public class DynamicState
	{
        private DynamicTileState[,] tiles;

	}


	public class StaticTileState 
	{
		public readonly int X, Y;
		public readonly float BaseHeight;
		public readonly Vector Slope;
		public readonly StaticTileState[] Neighbours;

		public StaticTileState(StaticTileState[,] states, int x, int y)
		{
			this.X = x;
			this.Y = y;
			Neighbours = GetNeighbours(states).ToArray();
			Slope = GetSlope();
		}

		private Vector GetSlope()
		{
			float lu = Mean(0);
			float ru = Mean(1);
			float ld = Mean(2);
			float rd = Mean(3);
			float xy = lu - rd;
			float yx = ru - ld;
			return new Vector(xy - yx, xy + yx) / 2;
		}
		private float Mean(int location)
		{
			var n = new[] { Neighbours[0], Neighbours[1], Neighbours[3], Neighbours[4] };
			switch(location)
			{
				case 0:
					n = new[] { Neighbours[0], Neighbours[1], Neighbours[3], Neighbours[4] };
					break;
				case 1:
					n = new[] { Neighbours[1], Neighbours[2], Neighbours[4], Neighbours[5] };
					break;
				case 2:
					n = new[] { Neighbours[3], Neighbours[4], Neighbours[6], Neighbours[7] };
					break;
				case 3:
					n = new[] { Neighbours[4], Neighbours[5], Neighbours[7], Neighbours[8] };
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			return n.Where(x => x != null).Average(s => s.BaseHeight);
		}
		private IEnumerable<StaticTileState> GetNeighbours(StaticTileState[,] states)
		{
			Coordinate[] cs = new[] { new Coordinate(X-1, Y - 1), new Coordinate(X, Y - 1), new Coordinate(X + 1, Y - 1),
				new Coordinate(X-1, Y), new Coordinate(X, Y), new Coordinate(X + 1, Y),
				new Coordinate(X-1, Y + 1), new Coordinate(X, Y + 1), new Coordinate(X + 1, Y + 1)};
			foreach (Coordinate c in cs)
			{
				if (c.X < 0 || c.Y < 0 || c.X >= states.GetLength(0) || c.Y >= states.GetLength(1))
					yield return null;
				yield return states[c.X, c.Y];
			}
		}
	}

	public class DynamicTileState
	{
		public float WaterHeight;
		public Vector Speed;




		public void AddWater(DynamicTileState state, float percentage)
		{
			WaterHeight += state.WaterHeight * percentage;
			Speed = Speed * (WaterHeight - state.WaterHeight * percentage) / WaterHeight + state.Speed * state.WaterHeight * percentage / WaterHeight;
		}
	}

	public struct Vector
	{
        private float x;
        private float y;

		public Vector(float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		public static Vector operator +(Vector v, Vector w)
		{
			return Function(v, w, (a, b) => a + b);
		}
		public static Vector operator -(Vector v, Vector w)
		{
			return Function(v, w, (a, b) => a - b);
		}
		public static float operator *(Vector v, Vector w)
		{
			return Function(v, w, (a, b) => a * b).Sum();
		}
		public static Vector operator *(Vector v, float w)
		{
			return Function(v, w, (a, b) => a * b);
		}
		public static Vector operator /(Vector v, float w)
		{
			return Function(v, w, (a, b) => a / b);
		}
		public float Norm(float n=2)
		{
			return (float)Math.Pow(Math.Pow(x, n) + Math.Pow(y, n), 1 / n);
		}

		private static Vector Function(Vector v, Vector w, Func<float, float, float> f)
		{
			return new Vector(f(v.x, w.x), f(v.y, w.y));
		}
		private static Vector Function(Vector v, float w, Func<float, float, float> f)
		{
			return new Vector(f(v.x, w), f(v.y, w));
		}
		private float Sum()
		{
			return x + y;
		}
	}
}

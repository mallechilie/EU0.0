﻿using System.Linq;
using Enumeration;

namespace TerrainGeneration
{

    public abstract class CoordinateSystem
    {
        public readonly Shape Topology;
        protected readonly int Maxx, Maxy;

        public CoordinateSystem(int maxx, int maxy, Shape topology)
        {
            Maxx = maxx;
            Maxy = maxy;
            Topology = topology;
        }

        public Coordinate[] GetNeightbours(int x, int y)
        {
            return GetNeightbours(new Coordinate(x, y));
        }
        public abstract Coordinate[] GetNeightbours(Coordinate c);
        public abstract Coordinate[] GetDistancedNeighbours(int x, int y, bool even, int distance);
        protected Coordinate[] TrimNeighbours(Coordinate[] coordinates)
        {
            return coordinates.Where(cc => cc.X >= 0 && cc.Y >= 0 && cc.X < Maxx && cc.Y < Maxy).ToArray();
        }
        public override string ToString()
        {
            return $"{Maxx}, {Maxy}, {Topology}";
        }
    }

	public class SquareCoordinateSystem : CoordinateSystem
	{
	    public SquareCoordinateSystem(int maxx, int maxy) : base(maxx, maxy, Shape.Square)
	    {
	    }

        public override Coordinate[] GetNeightbours(Coordinate c)
		{
			Coordinate[] coordinates = { new Coordinate(c.X - 1, c.Y), new Coordinate(c.X + 1, c.Y), new Coordinate(c.X, c.Y - 1), new Coordinate(c.X, c.Y + 1) ,
			                        new Coordinate(c.X - 1, c.Y - 1), new Coordinate(c.X + 1, c.Y + 1), new Coordinate(c.X + 1, c.Y - 1), new Coordinate(c.X - 1, c.Y + 1) };
		    return TrimNeighbours(coordinates);
		}
	    public override Coordinate[] GetDistancedNeighbours(int x, int y, bool even, int distance)
	    {
	        Coordinate[] neighbours = new Coordinate[4];
	        if (even)
	        {
	            neighbours[0] = new Coordinate(x - distance, y);
	            neighbours[1] = new Coordinate(x + distance, y);
	            neighbours[2] = new Coordinate(x, y - distance);
	            neighbours[3] = new Coordinate(x, y + distance);
	        }
	        else
	        {
	            neighbours[0] = new Coordinate(x - distance, y - distance);
	            neighbours[1] = new Coordinate(x + distance, y - distance);
	            neighbours[2] = new Coordinate(x - distance, y + distance);
	            neighbours[3] = new Coordinate(x + distance, y + distance);
	        }

	        return TrimNeighbours(neighbours);
	    }
    }

	public struct Coordinate
	{
		public int X, Y;

		public Coordinate(int x, int y)
		{
			X = x;
			Y = y;
		}

		public override string ToString()
		{
			return $"{X}, {Y}";
		}
	}
}


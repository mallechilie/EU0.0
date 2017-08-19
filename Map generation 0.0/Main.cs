using System;
using System.Runtime.InteropServices;

namespace MapGeneration0_0
{

	public static class Program
    {
		internal static Random R;

		public static void Main()
		{
			R = new Random();
			int width = 8, height = 8;

			Console.WriteLine(GenerateMap(Tile.topology.square, width, height));
			Console.WriteLine(GenerateMap(Tile.topology.hexagon, width, height));
			Console.WriteLine(GenerateMap(Tile.topology.triangle, width, height));

			Console.ReadKey();
		}

		public static MapTiles GenerateMap(Tile.topology topology, int width, int height)
		{
			MapTiles mt = new MapTiles(topology, width, height);
			ProvinceTile P = new ProvinceTile(mt, 1);
			P = new ProvinceTile(mt, 2);
			P = new ProvinceTile(mt, 3);
			P = new ProvinceTile(mt, 4);
			P = new ProvinceTile(mt, 5);
			P = new ProvinceTile(mt, 6);
			P = new ProvinceTile(mt, 7);
			P = new ProvinceTile(mt, 8);
			P = new ProvinceTile(mt, 9);

			return mt;
		}
    }
}

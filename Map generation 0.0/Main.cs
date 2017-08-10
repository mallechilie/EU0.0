using System;
using System.Runtime.InteropServices;

namespace MapGeneration0_0
{

	static class Program
    {
		private enum output { Console, WPF }
		private static output Output = output.Console;
		public static Random R;
		public static void Main()
		{
			R = new Random();
			int width = 20, height = 20;
			draw(GenerateMap(Tile.topology.square, width, height));
			draw(GenerateMap(Tile.topology.hexagon, width, height));
			draw(GenerateMap(Tile.topology.triangle, width, height));

			Console.ReadKey();
		}

		private static MapTiles GenerateMap(Tile.topology topology, int width, int height)
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
		private static void draw(MapTiles mt)
		{
			switch (Output)
			{
				case output.Console:
					Console.WriteLine(mt);
					break;
				case output.WPF:
					break;
			}

		}
    }
}

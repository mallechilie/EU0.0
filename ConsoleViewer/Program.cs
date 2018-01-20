using System;

namespace ConsoleViewer
{


	public static class Program
	{
		internal static Random R;

		public static void Main()
		{
			R = new Random();
			int width = 8, height = 8;

			//Console.WriteLine(Map.GenerateMap(Tile.topology.square, width, height));
			//Console.WriteLine(Map.GenerateMap(Tile.topology.hexagon, width, height));
			//Console.WriteLine(Map.GenerateMap(Tile.topology.triangle, width, height));

			Console.ReadKey();
		}

	}
}

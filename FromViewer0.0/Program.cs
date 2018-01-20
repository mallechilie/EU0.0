using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MapBuilder;


namespace FromViewer
{
    internal static class Program
	{
		public static Map Map;
        private static int size = 100;
        private static Viewer v;


		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
        private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(v = new Viewer());
		}
		internal static void Generatemap()
		{
			v.Selected = new List<Province>();
			Map = Map.GenerateMap(Tile.Topology.Square, size, size, size * 9 / 10, 0);
		}
	}
}

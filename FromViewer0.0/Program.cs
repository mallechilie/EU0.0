using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MapBuilder;


namespace FromViewer0_0
{
	static class Program
	{
		public static Map map;
		static int size = 100;
		static Viewer v;


		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(v=new Viewer());
		}
		internal static void generatemap()
		{
			v.Selected = new List<Province>();
			map = Map.GenerateMap(Tile.topology.square, size, size, size *9/10);
		}
	}
}

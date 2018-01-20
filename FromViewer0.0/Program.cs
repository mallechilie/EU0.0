using System;
using System.Windows.Forms;
using MapBuilder;


namespace FromViewer
{
    internal static class Program
	{
	    private static Map Map;
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
			Application.Run(v = new Viewer(new ProvinceViewer(size, size)));
		}
	}
}

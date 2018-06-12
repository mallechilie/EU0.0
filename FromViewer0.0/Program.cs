using System;
using System.Windows.Forms;


namespace FromViewer
{
    internal static class Program
    {
        private static int size = 1000;


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Viewer(new NationViewer(size, size)));
        }
    }
}

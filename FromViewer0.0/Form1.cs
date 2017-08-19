using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MapBuilder;

namespace FromViewer0_0
{
	public partial class Viewer : Form
	{
		Graphics graphics;
		Color[] ProvinceColors;
		int x = 0, y = 0;
		int mx = 0, my = 0;
		Map map
		{
			get
			{
				return Program.map;
			}
		}
		public List<Province> Selected;

		public Viewer()
		{
			InitializeComponent();
			MouseMove += new MouseEventHandler(DrawMouse);
			graphics = CreateGraphics();
			Selected = new List<Province>();
		}

		
		public void Draw()
		{
			graphics = CreateGraphics();
			graphics.Clear(SystemColors.Window);
			Program.generatemap();
			ProvinceColors = new Color[Program.map.Provinces.Length];
			for (int n = 0; n < ProvinceColors.Length; n++)
				ProvinceColors[n] = Color.FromArgb(200 * n % 127 + 90, 500 * n % 127 + 90, 300 * n % 127 + 90);
			if (Program.map.regular)
				DrawTiles(graphics);
		}

		public void DrawMouse(Object o, MouseEventArgs mea)
		{
			if (Program.map == null)
			{
				Program.generatemap();
				ProvinceColors = new Color[Program.map.Provinces.Length];
				for (int n = 0; n < ProvinceColors.Length; n++)
					ProvinceColors[n] = Color.FromArgb(200 * n % 127 + 90, 500 * n % 127 + 90, 300 * n % 127 + 90);
			}
			Map map = Program.map;
			x = PointToClient(Cursor.Position).X * (map.Tiles.GetLength(0) + 1) / ClientSize.Width;
			if (x >= map.Tiles.GetLength(0) || x < 0) 
				return;
			y = PointToClient(Cursor.Position).Y * (map.Tiles.GetLength(1) + 1) / ClientSize.Height;
			if (y >= map.Tiles.GetLength(1) || y < 0) 
				return;
			SelectNeighbours();
			//UnselectProvince();
			mx = x;
			my = y;
		}

		public void SelectNeighbours()
		{
			if (map.Tiles[x, y].HasProvince)
				foreach (var p in map.Tiles[x, y].Province.Neighbours)
					SelectProvince(p.Tiles[0]);
		}
		public void SelectProvince(Tile tile = null)
		{
			if (tile == null)
			{
				if (map.Tiles[x, y].HasProvince)
				{
					if(!Selected.Contains(map.Tiles[x, y].Province))
					{
						Selected.Add(map.Tiles[x, y].Province);
						DrawSelection();
					}
				}
			}
			else
				if (tile.HasProvince)
				{
					if (!Selected.Contains(tile.Province))
					{
						Selected.Add(tile.Province);
						DrawSelection();
					}
				}
		}
		public void DrawSelection(Province[] provinces=null, bool selected = true)
		{
			if (provinces == null)
				for (int p = 0; p < Selected.Count; p++)
					for (int t = 0; t < Selected[p].Tiles.Length; t++)
						DrawTile(graphics, Selected[p].Tiles[t], selected);
			else
				for (int p = 0; p < provinces.Length; p++)
					for (int t = 0; t < provinces[p].Tiles.Length; t++)
						DrawTile(graphics, provinces[p].Tiles[t], selected);
		}
		public void UnselectProvince(Tile tile = null)
		{
			if (tile == null)
			{
				if (!map.Tiles[mx, my].HasProvince)
				return;
			if (map.Tiles[mx, my].Province.Tiles.Contains(map.Tiles[x, y]))
				return;
			DrawSelection(new[] { map.Tiles[mx, my].Province }, false);
			}
			else
			{
				if (tile.HasProvince)
					DrawSelection(new[] { tile.Province }, false);
			}
		}

		private void DrawTiles(Graphics graphics)
		{
			for (int x = 0; x < map.Tiles.GetLength(0); x++)
				for (int y = 0; y < map.Tiles.GetLength(1); y++)
					DrawTile(graphics, map.Tiles[x, y]);
		}
		private void DrawTile(Graphics graphics, Tile tile, bool dark=false)
		{
			switch (tile.TileTopology)
			{
				case Tile.topology.square:
					{
						Rectangle rect = new Rectangle((int)((float)ClientSize.Width / (map.Tiles.GetLength(0) + 1) * tile.x),
							(int)((float)ClientSize.Height / (map.Tiles.GetLength(1) + 1) * tile.y),
							(int)((float)ClientSize.Width / (map.Tiles.GetLength(0) + 1)),
							(int)((float)ClientSize.Height / (map.Tiles.GetLength(1) + 1)));
						dark = dark || (x ==tile.x) && (y==tile.y);
						//graphics.DrawRectangle(Pens.Black, rect);
						if (tile.HasProvince)
						{
							Color c = ProvinceColors[tile.Province.Id];
							int i = Array.IndexOf(tile.Province.Tiles, tile)/5;
							if (dark)
								c = Color.FromArgb((c.R + i) -50, (c.G + i) -50, (c.B + i) -50);
							else
								c = Color.FromArgb(c.R + i, c.G + i, c.B + i);
							graphics.FillRectangle(new SolidBrush(c), rect);
						}
						//if (tile.HasProvince && tile.Province.Tiles[0] == tile)
						//	graphics.DrawString("S", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, rect, (new StringFormat{ LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center }));
						//else if(tile.HasProvince)
						//	graphics.DrawString(tile.Province.Id.ToString(), new Font("Arial", 8), Brushes.Black, rect, (new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center }));

					}
					break;
				default:
					break;
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Draw();
		}
	}
}

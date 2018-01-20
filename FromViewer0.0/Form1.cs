using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MapBuilder;

namespace FromViewer
{
	public partial class Viewer : Form
	{
        private Graphics graphics;
        private Color[] provinceColors;
        private int x, y;
        private int mx, my;

        private Map Map
		{
			get
			{
				return Program.Map;
			}
		}
		public List<Province> Selected;
		public enum MapMode { Terrain, Province, Nation}

        private MapMode mapMode = MapMode.Terrain;

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
			Program.Generatemap();
			provinceColors = new Color[Program.Map.Provinces.Length];
			for (int n = 0; n < provinceColors.Length; n++)
				provinceColors[n] = Color.FromArgb(200 * n % 127 + 90, 500 * n % 127 + 90, 300 * n % 127 + 90);
			if (Program.Map.Regular)
			{
				graphics.Clear(SystemColors.Window);
				DrawTiles(graphics);
			}
		}

		public void DrawMouse(Object o, MouseEventArgs mea)
		{
			if (Program.Map == null)
			{
				Program.Generatemap();
				provinceColors = new Color[Program.Map.Provinces.Length];
				for (int n = 0; n < provinceColors.Length; n++)
					provinceColors[n] = Color.FromArgb(200 * n % 127 + 90, 500 * n % 127 + 90, 300 * n % 127 + 90);
			}
			Map map = Program.Map;
			x = PointToClient(Cursor.Position).X * (map.Tiles.GetLength(0) + 1) / ClientSize.Width;
			if (x >= map.Tiles.GetLength(0) || x < 0) 
				return;
			y = PointToClient(Cursor.Position).Y * (map.Tiles.GetLength(1) + 1) / ClientSize.Height;
			if (y >= map.Tiles.GetLength(1) || y < 0) 
				return;
			//SelectNeighbours();
			SelectNewProvince();
			mx = x;
			my = y;
		}

		public void SelectNeighbours()
		{
			if (Map.Tiles[x, y].HasProvince)
				foreach (var p in Map.Tiles[x, y].Province.Neighbours)
					SelectProvince(p.Tiles[0]);
		}
		public void SelectNewProvince(Tile tile = null)
		{
			if (tile == null)
			{
				if (Map.Tiles[x, y].HasProvince)
				{
					if (!Selected.Contains(Map.Tiles[x, y].Province))
					{
						Selected = new List<Province>();
						Selected.Add(Map.Tiles[x, y].Province);
						DrawSelection();
					}
				}
			}
			else
			if (tile.HasProvince)
			{
				if (!Selected.Contains(tile.Province))
				{
					Selected = new List<Province>();
					Selected.Add(tile.Province);
					DrawSelection();
				}
			}
		}
		public void SelectProvince(Tile tile = null)
		{
			if (tile == null)
			{
				if (Map.Tiles[x, y].HasProvince)
				{
					if(!Selected.Contains(Map.Tiles[x, y].Province))
					{
						Selected.Add(Map.Tiles[x, y].Province);
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
				if (!Map.Tiles[mx, my].HasProvince)
				return;
			if (Map.Tiles[mx, my].Province.Tiles.Contains(Map.Tiles[x, y]))
				return;
			DrawSelection(new[] { Map.Tiles[mx, my].Province }, false);
			}
			else
			{
				if (tile.HasProvince)
					DrawSelection(new[] { tile.Province }, false);
			}
		}

		private void DrawTiles(Graphics graphics)
		{
			for (int x = 0; x < Map.Tiles.GetLength(0); x++)
				for (int y = 0; y < Map.Tiles.GetLength(1); y++)
					DrawTile(graphics, Map.Tiles[x, y]);
		}
		private void DrawTile(Graphics graphics, Tile tile, bool dark=false)
		{
			switch (tile.TileTopology)
			{
				case Tile.Topology.Square:
					{
						Rectangle rect = new Rectangle((int)((float)ClientSize.Width / (Map.Tiles.GetLength(0) + 1) * tile.X),
							(int)((float)ClientSize.Height / (Map.Tiles.GetLength(1) + 1) * tile.Y),
							(int)((float)ClientSize.Width / (Map.Tiles.GetLength(0) + 1))+1,
							(int)((float)ClientSize.Height / (Map.Tiles.GetLength(1) + 1))+1);
						//graphics.DrawRectangle(Pens.Black, rect);
							graphics.FillRectangle(new SolidBrush(TileColor(tile)), rect);
					}
					break;
				default:
					break;
			}
		}
		private Color TileColor(Tile tile)
		{
			Color terrainColor = Viewer.WaterColor(tile);
			switch (mapMode)
			{
				case MapMode.Province:
					if (tile.HasProvince)
					{
						int h = (int)tile.Height;
						Color c = provinceColors[tile.Province.Id];
						return MeanColor(terrainColor, c);
					}
					else
						return terrainColor;
				case MapMode.Nation:
					if (tile.HasProvince && tile.Province.HasNation)
					{
						if (tile.X != 0)
						{
							Tile above = Map.Tiles[tile.X - 1, tile.Y];
							if (above.HasProvince && above.Province.HasNation && above.Province.Nation != tile.Province.Nation)
								return Color.Black;
						}
						if (tile.Y != 0)
						{
							Tile left = Map.Tiles[tile.X, tile.Y - 1];
							if (left.HasProvince && left.Province.HasNation && left.Province.Nation != tile.Province.Nation)
								return Color.Black;
						}
						int h = (int)tile.Height;
						Color c = provinceColors[tile.Province.Nation.Id];
						return MeanColor(terrainColor, c);
					}
					break;
				case MapMode.Terrain:
					return terrainColor;
			}
			return default(Color);
		}
		#region CalculateColors
		private static Color WaterColor(Tile tile)
		{
			double whiteness = (tile.Height + tile.WaterHeight) / 255;
			return tile.WaterHeight > 0.1 ? MeanColor(Color.Black, Color.White, 1 - whiteness, whiteness) :
				TerrainColor(tile);
		}
		private static Color TerrainColor(Tile tile)
		{
			Color[] colors = new[] {  Color.Blue, Color.Cyan, Color.Green, Color.Yellow, Color.Red };
			//colors = new[] { Color.Black, Color.White };
			double index = tile.Height / 255 * (colors.Length - 1.000001);
			//return colors[(int)index];
			return MeanColor(colors[(int)index], colors[(int)index + 1], 1 - (index - (int)index), index - (int)index);
		}
		private static Color MeanColor(Color a, Color b)
		{
			return FromFormula(a, b, (c, d) => (c + d) / 2);
		}
		private static Color MeanColor(Color a, Color b, double weightA, double weightB)
		{
			return FromFormula(a, b, (c, d) => (int)((c * weightA + d * weightB) / (weightA + weightB)));
		}
		private static Color FromFormula(Color a, Color b, Func<int, int, int> formula)
		{
			return Color.FromArgb(formula(a.A, b.A), formula(a.R, b.R), formula(a.G, b.G), formula(a.B, b.B));
		}
		#endregion

		private void button1_Click(object sender, EventArgs e)
		{
			Draw();
		}
	}
}

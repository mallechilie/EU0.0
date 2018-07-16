using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NewMapBuilder;
using TerrainGeneration;
using Enumeration;

namespace FromViewer
{
    internal abstract class MapViewer
    {
        public int Width, Height;
        public readonly TileShape TileTopology;
        public readonly bool Torus;


        protected MapViewer(int width, int height, TileShape tileTopology, bool torus)
        {
            Width = width;
            Height = height;
            TileTopology = tileTopology;
            Torus = torus;
            ResetMap();
        }

        public Bitmap GetBitmap()
        {
            Bitmap bitmap = new Bitmap(Width, Height);
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    bitmap.SetPixel(x, y, GetColor(x, y));
            return bitmap;
        }
        public abstract Color GetColor(int x, int y);
        public abstract void ResetMap();
    }

    internal class TerrainViewer : MapViewer
    {
        private GenerateHeight heightMap;


        public TerrainViewer(int width, int height, bool torus) : base(width, height, TileShape.Square, torus)
        {
        }


        public override Color GetColor(int x, int y)
        {
            return ColorCalc.TerrainColor(heightMap.HeightMap[x, y], true);
        }
        public override void ResetMap()
        {
            heightMap = new GenerateHeight(Width, Height, Torus);
        }
    }
    internal class WaterViewer : MapViewer
    {
        private GenerateWater waterMap;


        public WaterViewer(int width, int height, bool torus) : base(width, height, TileShape.Square, torus)
        {
        }


        public override Color GetColor(int x, int y)
        {
            float waterHeigt = waterMap.WaterHeights[x, y];
            switch ((int)waterHeigt / 15)
            {
                case 0:
                    return ColorCalc.TerrainColor(waterMap.HeightMap.HeightMap[x, y], waterMap.WaterHeights[x, y], true);
                case 1:
                    return Color.Aquamarine;
                case 2:
                    return Color.Aqua;
                case 3:
                    return Color.DeepSkyBlue;
                case 4:
                    return Color.Blue;
                case 5:
                    return Color.DarkBlue;
                default:
                    return Color.MidnightBlue;
            }
        }
        public override void ResetMap()
        {
            waterMap = new GenerateWater(new GenerateHeight(Width, Height, Torus));
        }
    }

    internal class ProvinceViewer : MapViewer
    {
        private ProvinceMap map;
        public List<Province> Selected;
        private Color[] provinceColors;

        public ProvinceViewer(int width, int height, bool torus) : base(width, height, TileShape.Square, torus)
        {
        }

        public override Color GetColor(int x, int y)
        {
            int id = x * Height + y;
            return map.Map[id].ParentID != -1
                ? provinceColors[map.Map[id].ParentID]
                : ColorCalc.TerrainColor(map.Map[id], true);
        }
        public sealed override void ResetMap()
        {
            map = new ProvinceMap(new TileMap(new GenerateHeight(Width, Height, Torus)));

            Selected = new List<Province>();
            provinceColors = new Color[map.Tiles.Length];
            for (int n = 0; n < provinceColors.Length; n++)
                provinceColors[n] = Color.FromArgb(200 * n % 127 + 90, 500 * n % 127 + 90, 300 * n % 127 + 90);
        }


        /*
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
        */
    }
    internal class NationViewer : MapViewer
    {
        private NationMap map;
        public List<Province> Selected;
        private Color[] NationColors;

        public NationViewer(int width, int height, bool torus) : base(width, height, TileShape.Square, torus)
        {
        }

        public override Color GetColor(int x, int y)
        {
            int id = x * Height + y;
            int provinceID = ((ProvinceMap)map.Map).Map[id].ParentID;
            if (provinceID == -1)
                return ColorCalc.TerrainColor(((ProvinceMap)map.Map).Map[id], true);
            int nationID = map.Map[provinceID].ParentID;
            return nationID != -1
                ? NationColors[nationID]
                : ColorCalc.TerrainColor(((ProvinceMap)map.Map).Map[id], true);
        }
        public sealed override void ResetMap()
        {
            map = new NationMap(new ProvinceMap(new TileMap(new GenerateHeight(Width, Height, Torus))));

            Selected = new List<Province>();
            NationColors = new Color[map.Tiles.Length];
            for (int n = 0; n < NationColors.Length; n++)
                NationColors[n] = Color.FromArgb(200 * n % 127 + 90, 500 * n % 127 + 90, 300 * n % 127 + 90);
        }


        /*
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
        */
    }
}

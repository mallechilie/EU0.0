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
            return ColorCalc.TerrainColor(waterMap.HeightMap.HeightMap[x, y], waterMap.WaterHeights[x, y], true);
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
            return map.Map[id].ParentID<Tile, Province>() != -1
                ? map.Map[id].Neighbours.Any(n => n.Parent != map.Map[id].Parent)
                ? ColorCalc.MeanColor(ColorCalc.TerrainColor(map.Map[id], true), provinceColors[map.Map[id].ParentID<Tile, Province>()])
                : provinceColors[map.Map[id].ParentID<Tile, Province>()]
                : ColorCalc.TerrainColor(map.Map[id], true);
        }
        public override sealed void ResetMap()
        {
            map = new ProvinceMap(new TileMap(new GenerateHeight(Width, Height, Torus)));

            Selected = new List<Province>();
            provinceColors = new Color[map.Tiles.Length];
            for (int n = 0; n < provinceColors.Length; n++)
                provinceColors[n] = Color.FromArgb(31 * n % 200 + 55, 113 * n % 200 + 55, 67 * n % 200 + 55);
        }
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
            int provinceID = ((ProvinceMap)map.Map).Map[id].ParentID<Tile, Province>();
            if(provinceID == -1 )
                return ColorCalc.TerrainColor(((ProvinceMap)map.Map).Map[id], true);
            int nationID = map.Map[provinceID].ParentID<Province, Nation>();
            if (nationID == -1)
                return ColorCalc.TerrainColor(((ProvinceMap) map.Map).Map[id], true);
            if (((ProvinceMap) map.Map).Map[id].Neighbours.Any(n => n.Parent != ((ProvinceMap) map.Map).Map[id].Parent))
                return ColorCalc.MeanColor(ColorCalc.TerrainColor(((ProvinceMap) map.Map).Map[id], true), NationColors[nationID]);
            return  NationColors[nationID];
        }
        public override sealed void ResetMap()
        {
            map = new NationMap(new ProvinceMap(new TileMap(new GenerateHeight(Width, Height, Torus))));

            Selected = new List<Province>();
            NationColors = new Color[map.Tiles.Length];
            for (int n = 0; n < NationColors.Length; n++)
                NationColors[n] = Color.FromArgb(31 * n % 200 + 55, 113 * n % 200 + 55, 67 * n % 200 + 55);
        }
    }
}

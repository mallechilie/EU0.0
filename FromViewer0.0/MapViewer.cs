﻿using System;
using System.Collections.Generic;
using System.Drawing;
using MapBuilder;
using TerrainGeneration;
using Enumeration;

namespace FromViewer
{
    internal abstract class MapViewer
    {
        public readonly int Width, Height;
        public readonly TileShape TileTopology;


        protected MapViewer(int width, int height, TileShape tileTopology)
        {
            Width = width;
            Height = height;
            TileTopology = tileTopology;
            ResetMap();
        }


        public abstract Color GetColor(int x, int y);
        public abstract void ResetMap();
    }

    internal class TerrainViewer : MapViewer
    {
        private GenerateHeight heightMap;


        public TerrainViewer(int width, int height) : base(width, height, TileShape.Square)
        {
        }


        public override Color GetColor(int x, int y)
        {
            return ColorCalc.TerrainColor(heightMap.HeightMap[x, y]);
        }
        public override void ResetMap()
        {
            heightMap = new GenerateHeight(Width, Height);
        }
    }

    internal class ProvinceViewer : MapViewer
    {
        private Map map;
        public List<Province> Selected;
        private Color[] provinceColors;

        public ProvinceViewer(int width, int height) : base(width, height, TileShape.Square)
        {
        }

        public override Color GetColor(int x, int y)
        {
            return map.Tiles[x, y].HasProvince
                ? provinceColors[map.Tiles[x, y].Province.Id]
                : ColorCalc.TerrainColor(map.Tiles[x, y], true);
        }
        public sealed override void ResetMap()
        {
            map = Map.GenerateMap(TileTopology, Width, Height, (Width + Height) / 2, (int)Math.Sqrt(Width + Height));

            Selected = new List<Province>();
            provinceColors = new Color[map.Provinces.Length];
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
}

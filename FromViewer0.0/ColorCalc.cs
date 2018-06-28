using System;
using System.Drawing;
using NewMapBuilder;

namespace FromViewer
{
    internal static class ColorCalc
    {
        public static Color WaterColor(Tile tile)
        {
            double whiteness = (tile.Height + tile.WaterHeight) / 255;
            return tile.WaterHeight > 0.1 ? MeanColor(Color.Black, Color.White, 1 - whiteness, whiteness) :
                TerrainColor(tile);
        }
        public static Color TerrainColor(float height, bool shadesOfGray = false)
        {
            Color[] colors = shadesOfGray
                ? new[] {Color.Black, Color.White}
                : new[] {Color.Blue, Color.Cyan, Color.Green, Color.Yellow, Color.Red};
            double index = height / 255 * (colors.Length - 1.000001);
            //return colors[(int)index];
            return MeanColor(colors[(int)index], colors[(int)index + 1], 1 - (index - (int)index), index - (int)index);
        }
        public static Color TerrainColor(float height, float WaterHeight, bool shadesOfGray = false)
        {
            Color baseColor = TerrainColor(height, shadesOfGray);
            Color water = Color.FromArgb(0, 0, (int) WaterHeight);
            return FromFormula(baseColor, water, (c, d) => c + d);
        }
        public static Color TerrainColor(Tile tile, bool shadesOfGray = false)
        {
            return TerrainColor(tile.Height, shadesOfGray);
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
            int CorrectedFunc(int i, int i1) => Math.Min(255, Math.Max(0, formula(i, i1)));
            return Color.FromArgb(CorrectedFunc(a.A, b.A), CorrectedFunc(a.R, b.R), CorrectedFunc(a.G, b.G), CorrectedFunc(a.B, b.B));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapBuilder;

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
        public static Color TerrainColor(float height, bool ShadesOfGray = false)
        {
            Color[] colors = ShadesOfGray
                ? new[] {Color.Black, Color.White}
                : new[] {Color.Blue, Color.Cyan, Color.Green, Color.Yellow, Color.Red};
            double index = height / 255 * (colors.Length - 1.000001);
            //return colors[(int)index];
            return MeanColor(colors[(int)index], colors[(int)index + 1], 1 - (index - (int)index), index - (int)index);
        }
        public static Color TerrainColor(Tile tile, bool ShadesOfGray = false)
        {
            return TerrainColor(tile.Height, ShadesOfGray);
        }
        public static Color MeanColor(Color a, Color b)
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
    }
}

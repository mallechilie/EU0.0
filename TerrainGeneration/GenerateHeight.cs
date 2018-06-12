using System;
using System.Linq;
using Superbest_random;
#pragma warning disable 1584,1711,1572,1581,1580

namespace TerrainGeneration
{
    public class GenerateHeight
    {
        public float[,] HeightMap;

        /// <summary>
        /// Deviation from the mean value between other cells.
        /// </summary>
        private float deviation;

        /// <summary>
        /// The difference in <paramref name="deviation"/> between iterations. A high delta will mean the deviation stays big, so a steep landscape, where a small delta will result in a flatter landscape.
        /// <remarks>delta should be between 0 and 1.</remarks>
        /// </summary>
        private readonly float delta;
        
        /// <summary>
        /// Iteration value, smaller initiatin will give a landscape where there are multiple highs and lows.
        /// <remarks>This value shoud stay between 1 and the smallest dimension of <paramref name="HeightMap"/>.</remarks>
        /// </summary>
        private int distance;
        public readonly CoordinateSystem Cs;

        /// <summary>
        /// Generates a <paramref name="HeightMap"/>.
        /// </summary>
        /// <param name="width">The width of the map.</param>
        /// <param name="height">The height of the map.</param>
        /// <param name="delta">A value between 0 and 1 that indicates the steepness: 0 is flat and 1 steep.</param>
        public GenerateHeight(int width, int height, float delta = 0.5f)
        {
            this.delta = delta;
            Cs = new SquareCoordinateSystem(width, height);
            deviation = delta;
            HeightMap = new float[width, height];
            // ReSharper disable once PossibleLossOfFraction
            distance = (int)Math.Pow(2, (int)Math.Log(Math.Min(width, height), 2) / 2);
            GenerateHeights();
            CompensateMean();
            Round();
        }


        private void CompensateMean(int minimum = 0, int maximum = 255)
        {
            // Calculate the min and max value
            float min = HeightMap[0, 0], max = HeightMap[0, 0];
            for (int x = 0; x < HeightMap.GetLength(0); x++)
                for (int y = 0; y < HeightMap.GetLength(1); y++)
                {
                    min = Math.Min(min, HeightMap[x, y]);
                    max = Math.Max(max, HeightMap[x, y]);
                }

            // Convert to minimum-maximum range.
            for (int x = 0; x < HeightMap.GetLength(0); x++)
                for (int y = 0; y < HeightMap.GetLength(1); y++)
                {
                    HeightMap[x, y] -= min;
                    HeightMap[x, y] *= (maximum - minimum) / (max - min);
                    HeightMap[x, y] += minimum;
                }

        }
        private void GenerateHeights(Random r = null)
        {
            r = r ?? new Random();
            // Set all starting points.
            for (int x = 0; x < HeightMap.GetLength(0); x += distance)
                for (int y = 0; y < HeightMap.GetLength(1); y += distance)
                    HeightMap[x, y] = (float)r.NextGaussian(0, deviation);

            // Set the other points.
            for (; distance > 0; distance /= 2)
            {
                Generate(r, false);
                Generate(r, true);
                deviation *= delta;
            }
        }
        /// <summary>
        /// Rounds the cell height to the mean of it and its neighbours.
        /// </summary>
        private void Round()
        {
            float[,] rounded = (float[,])HeightMap.Clone();
            for (int x = 0; x < HeightMap.GetLength(0); x++)
                for (int y = 0; y < HeightMap.GetLength(1); y++)
                    rounded[x, y] = Cs.GetNeightbours(x, y).Select(c => HeightMap[c.X, c.Y]).Concat(new[] { HeightMap[x, y] }).Average();
            HeightMap = rounded;
        }

        private void Generate(Random r, bool even)
        {
            for (int x = 0; x < HeightMap.GetLength(0); x += distance)
                for (int y = x % 2 == 0 && !even ? distance : 0; y < HeightMap.GetLength(0); y += distance)
                {
                    float mean = GetMean(x, y, even);
                    if (Math.Abs(mean - 0) > float.Epsilon * 10)
                        HeightMap[x, y] = (float)r.NextGaussian(mean, deviation);
                }
        }
        private float GetMean(int x, int y, bool even)
        {
            // Get Neighbours (not equal to zero)
            float[] v = Cs.GetDistancedNeighbours(x, y, even, distance).Select(c => HeightMap[c.X, c.Y]).Where(f => Math.Abs(f) > 10 * float.Epsilon).ToArray();
            // Middle them out.
            return v.Length != 0 ? v.Sum() / v.Length : 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerrainGeneration
{
    public class GenerateWater
    {
        public GenerateHeight heightMap;
        private CoordinateSystem CoordinateSystem => heightMap.Cs;
        public float[,] waterHeights;

        public GenerateWater(GenerateHeight heightMap)
        {
            this.heightMap = heightMap;
            waterHeights = new float[CoordinateSystem.Width, CoordinateSystem.Height];
            StartingWater(20, 2);
            for (int x = 0; x < 50; x++)
                IterateWater();
        }

        private void StartingWater(float amount, float deviation = 0)
        {
            if (deviation == 0)
                for (int x = 0; x < waterHeights.GetLength(0); x++)
                    for (int y = 0; y < waterHeights.GetLength(0); y++)
                        waterHeights[x, y] = amount;
            else
            {
                Random r = new Random();
                for (int x = 0; x < waterHeights.GetLength(0); x++)
                    for (int y = 0; y < waterHeights.GetLength(1); y++)
                        waterHeights[x, y] = (float)Superbest_random.RandomExtensions.NextGaussian(r, amount, deviation);
            }
        }

        private void IterateWater()
        {
            float[,] newWater = new float[CoordinateSystem.Width, CoordinateSystem.Height];
            for (int x = 0; x < waterHeights.GetLength(0); x++)
                for (int y = 0; y < waterHeights.GetLength(1); y++)
                {
                    Dictionary<Coordinate, float> neighbourWater = IterateWaterTile(x, y, CoordinateSystem.GetNeightbours(x, y));
                    foreach (KeyValuePair<Coordinate, float> keyValuePair in neighbourWater)
                        newWater[keyValuePair.Key.X, keyValuePair.Key.Y] += keyValuePair.Value;
                }
        }

        private Dictionary<Coordinate, float> IterateWaterTile(int x, int y, Coordinate[] neighbours)
        {
            Dictionary<Coordinate, float> water = new Dictionary<Coordinate, float>(neighbours.Length + 1);
            for (int i = 0; i < neighbours.Length; i++)
                water.Add(neighbours[i], waterHeights[neighbours[i].X, neighbours[i].Y] + heightMap.HeightMap[neighbours[i].X, neighbours[i].Y]);
            Coordinate tile = new Coordinate(x, y);
            water.Add(tile, heightMap.HeightMap[x, y]);
            float left = waterHeights[x, y];

            while (Math.Abs(left) > float.Epsilon * 100)
            {
                KeyValuePair<Coordinate, float>[] temp = water.OrderBy(t => t.Value).ToArray();
                int low = temp.Count(t => Math.Abs(t.Value - temp.First().Value) < float.Epsilon * 100);
                float amount = low == temp.Length ? left + 1 : temp[low].Value - temp[0].Value;
                if (left < amount * low)
                {
                    for (int i = 0; i < low; i++)
                        water[temp[i].Key] += left / low;
                    left = 0;
                }
                else
                    for (int i = 0; i < low; i++)
                    {
                        water[temp[i].Key] += amount;
                        left -= amount;
                    }
            }

            return water.Select(t => new KeyValuePair<Coordinate, float>(t.Key, t.Key == tile ? t.Value - heightMap.HeightMap[t.Key.X, t.Key.Y] : t.Value - heightMap.HeightMap[t.Key.X, t.Key.Y] - waterHeights[t.Key.X, t.Key.Y])).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}
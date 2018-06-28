using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerrainGeneration
{
    public class GenerateWater
    {
        public GenerateHeight HeightMap;
        private CoordinateSystem CoordinateSystem => HeightMap.Cs;
        public float[,] WaterHeights;

        public GenerateWater(GenerateHeight heightMap)
        {
            this.HeightMap = heightMap;
            WaterHeights = new float[CoordinateSystem.Width, CoordinateSystem.Height];
            StartingWater(20, 2);
            for (int x = 0; x < 50; x++)
                IterateWater();
        }

        private void StartingWater(float amount, float deviation = 0)
        {
            if (deviation == 0)
                for (int x = 0; x < WaterHeights.GetLength(0); x++)
                    for (int y = 0; y < WaterHeights.GetLength(0); y++)
                        WaterHeights[x, y] = amount;
            else
            {
                Random r = new Random();
                for (int x = 0; x < WaterHeights.GetLength(0); x++)
                    for (int y = 0; y < WaterHeights.GetLength(1); y++)
                        WaterHeights[x, y] = (float)Superbest_random.RandomExtensions.NextGaussian(r, amount, deviation);
            }
        }

        private void IterateWater()
        {
            float[,] newWater = new float[CoordinateSystem.Width, CoordinateSystem.Height];
            for (int x = 0; x < WaterHeights.GetLength(0); x++)
                for (int y = 0; y < WaterHeights.GetLength(1); y++)
                {
                    Dictionary<Coordinate, float> neighbourWater = IterateWaterTile(x, y, CoordinateSystem.GetNeightbours(x, y));
                    foreach (KeyValuePair<Coordinate, float> keyValuePair in neighbourWater)
                        newWater[keyValuePair.Key.X, keyValuePair.Key.Y] += keyValuePair.Value;
                }
            waterHeights = newWater;
        }

        private Dictionary<Coordinate, float> IterateWaterTile(int x, int y, Coordinate[] neighbours)
        {
            Dictionary<Coordinate, float> water = new Dictionary<Coordinate, float>(neighbours.Length + 1);
            for (int i = 0; i < neighbours.Length; i++)
                water.Add(neighbours[i], WaterHeights[neighbours[i].X, neighbours[i].Y] + HeightMap.HeightMap[neighbours[i].X, neighbours[i].Y]);
            Coordinate tile = new Coordinate(x, y);
            water.Add(tile, HeightMap.HeightMap[x, y]);
            float left = WaterHeights[x, y];

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

            return water.Select(t => new KeyValuePair<Coordinate, float>(t.Key, t.Key == tile ? t.Value - HeightMap.HeightMap[t.Key.X, t.Key.Y] : t.Value - HeightMap.HeightMap[t.Key.X, t.Key.Y] - WaterHeights[t.Key.X, t.Key.Y])).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}
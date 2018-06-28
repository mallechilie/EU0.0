using System;
using System.Collections.Generic;
using System.Drawing;
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
        private Vector[,] velocities;

        public GenerateWater(GenerateHeight heightMap)
        {
            this.heightMap = heightMap;
            waterHeights = new float[CoordinateSystem.Width, CoordinateSystem.Height];
            velocities = new Vector[CoordinateSystem.Width, CoordinateSystem.Height];
            StartingWater(10, 2);
            for (int x = 0; x < 50; x++)
                IterateWaterAccelerate();
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

        private void IterateWaterFill()
        {
            float[,] newWater = new float[CoordinateSystem.Width, CoordinateSystem.Height];
            for (int x = 0; x < WaterHeights.GetLength(0); x++)
                for (int y = 0; y < WaterHeights.GetLength(1); y++)
                {
                    Dictionary<Coordinate, float> neighbourWater = IterateWaterFillTile(x, y, CoordinateSystem.GetNeightbours(x, y));
                    foreach (KeyValuePair<Coordinate, float> keyValuePair in neighbourWater)
                        newWater[keyValuePair.Key.X, keyValuePair.Key.Y] += keyValuePair.Value;
                }
            waterHeights = newWater;
        }
        private void IterateWaterAccelerate()
        {
            float[,] newWater = new float[CoordinateSystem.Width, CoordinateSystem.Height];
            Vector[,] newVelocities = new Vector[CoordinateSystem.Width, CoordinateSystem.Height];
            for (int x = 0; x < waterHeights.GetLength(0); x++)
                for (int y = 0; y < waterHeights.GetLength(1); y++)
                {
                    Vector velocity = IterateWaterAccelerateTile(x, y, CoordinateSystem.GetNeightbours(x, y));
                    //TODO: implement max velocity.
                    int X = velocity.X >= 0 ? x : x - 1;
                    int Y = velocity.Y >= 0 ? y : y - 1;
                    velocity = new Vector(velocity.X >= 0 ? velocity.X : 1 - velocity.X,
                                          velocity.Y >= 0 ? velocity.Y : 1 - velocity.Y);
                    float c;
                    for (int i = 0; i < 4; i++)
                    {
                        c = (i % 2 == 0 ? 1 - velocity.X : velocity.X) * (i / 2 == 0 ? 1 - velocity.Y : velocity.Y);
                        newWater[X + i % 2, Y + i / 2] += waterHeights[x, y] * c;
                        newVelocities[X + i % 2, Y + i / 2] += waterHeights[X + i % 2, Y + i / 2] * velocity * c;
                    }
                }
            for (int x = 0; x < waterHeights.GetLength(0); x++)
                for (int y = 0; y < waterHeights.GetLength(1); y++)
                    newVelocities[x, y] /= newWater[x, y];
            waterHeights = newWater;
            velocities = newVelocities;
        }

        private Dictionary<Coordinate, float> IterateWaterFillTile(int x, int y, Coordinate[] neighbours)
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
        private Vector IterateWaterAccelerateTile(int x, int y, Coordinate[] neighbours)
        {
            float leftSum = neighbours.Where(coordinate => coordinate.X == x - 1).Sum(coordinate => waterHeights[coordinate.X, coordinate.Y]);
            float rightSum = neighbours.Where(coordinate => coordinate.X == x + 1).Sum(coordinate => waterHeights[coordinate.X, coordinate.Y]);
            float upperSum = neighbours.Where(coordinate => coordinate.Y == y - 1).Sum(coordinate => waterHeights[coordinate.X, coordinate.Y]);
            float lowerSum = neighbours.Where(coordinate => coordinate.Y == y + 1).Sum(coordinate => waterHeights[coordinate.X, coordinate.Y]);
            Vector acceleration = new Vector(leftSum - rightSum, upperSum - lowerSum);

            return velocities[x, y] + acceleration / waterHeights[x, y];
        }
    }

    internal struct Vector
    {
        public float X, Y;
        public Vector(float x, float y)
        {
            Y = y;
            X = x;
        }
        public static Vector operator +(Vector v, Vector w)
        {
            return new Vector(v.X + w.X, v.Y + w.Y);
        }
        public static Vector operator -(Vector v, Vector w)
        {
            return new Vector(v.X - w.X, v.Y - w.Y);
        }
        public static Vector operator *(Vector v, float a)
        {
            return new Vector(v.X * a, v.Y * a);
        }
        public static Vector operator *(float a, Vector v)
        {
            return new Vector(v.X * a, v.Y * a);
        }
        public static Vector operator /(Vector v, float a)
        {
            return new Vector(v.X / a, v.Y / a);
        }
        public override string ToString()
        {
            return $"{X} {Y}";
        }
    }
}
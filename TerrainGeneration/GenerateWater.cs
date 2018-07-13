using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Superbest_random;

namespace TerrainGeneration
{
    public class GenerateWater
    {
        public GenerateHeight HeightMap;
        private CoordinateSystem CoordinateSystem => HeightMap.Cs;
        public float[,] WaterHeights;
        private Vector[,] velocities;

        public GenerateWater(GenerateHeight heightMap)
        {
            HeightMap = heightMap;
            WaterHeights = new float[CoordinateSystem.Width, CoordinateSystem.Height];
            velocities = new Vector[CoordinateSystem.Width, CoordinateSystem.Height];
            StartingWater((float)new Random().NextGaussian(10, 3), 2);
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
                        WaterHeights[x, y] = (float)r.NextGaussian(amount, deviation);
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
            WaterHeights = newWater;
        }
        private void IterateWaterAccelerate()
        {
            float[,] newWater = new float[CoordinateSystem.Width, CoordinateSystem.Height];
            Vector[,] newVelocities = new Vector[CoordinateSystem.Width, CoordinateSystem.Height];
            for (int x = 0; x < WaterHeights.GetLength(0); x++)
                for (int y = 0; y < WaterHeights.GetLength(1); y++)
                {
                    Vector velocity = IterateWaterAccelerateTile(x, y, CoordinateSystem.GetNeightbours(x, y));
                    //TODO: implement max velocity.
                    int X = (int)Math.Floor(velocity.X + x);
                    int Y = (int)Math.Floor(velocity.Y + y);
                    Coordinate v = CoordinateSystem.TrimCoordinate(new Coordinate(X, Y));
                    velocity = new Vector(velocity.X >= 0 ? velocity.X % 1 : 1 + velocity.X % 1,
                                          velocity.Y >= 0 ? velocity.Y % 1 : 1 + velocity.Y % 1);
                    for (int i = 0; i < 4; i++)
                    {
                        float c = (i % 2 == 0 ? 1 - velocity.X : velocity.X) * (i / 2 == 0 ? 1 - velocity.Y : velocity.Y);
                        Coordinate w = CoordinateSystem.TrimCoordinate(new Coordinate(v.X + i % 2, v.Y + i / 2));
                        if (Math.Abs(c) > float.Epsilon * 100)
                        {
                            newWater[w.X, w.Y] += WaterHeights[x, y] * c;
                            newVelocities[w.X, w.Y] += WaterHeights[w.X, w.Y] * velocity * c;
                        }
                    }
                }
            for (int x = 0; x < WaterHeights.GetLength(0); x++)
                for (int y = 0; y < WaterHeights.GetLength(1); y++)
                    newVelocities[x, y] /= newWater[x, y]+1;
            WaterHeights = newWater;
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
            float total = neighbours.Sum(coordinate => HeightMap.HeightMap[coordinate.X, coordinate.Y] + WaterHeights[coordinate.X, coordinate.Y]) + 1;
            float leftSum = neighbours.Where(coordinate => coordinate.X != (x + 1 + CoordinateSystem.Width) % CoordinateSystem.Width)
                                      .Sum(coordinate => HeightMap.HeightMap[coordinate.X, coordinate.Y] + WaterHeights[coordinate.X, coordinate.Y]);
            float rightSum = neighbours.Where(coordinate => coordinate.X != (x - 1 + CoordinateSystem.Width) % CoordinateSystem.Width)
                                       .Sum(coordinate => HeightMap.HeightMap[coordinate.X, coordinate.Y] + WaterHeights[coordinate.X, coordinate.Y]);
            float upperSum = neighbours.Where(coordinate => coordinate.Y != (y + 1 + CoordinateSystem.Height) % CoordinateSystem.Height)
                                       .Sum(coordinate => HeightMap.HeightMap[coordinate.X, coordinate.Y] + WaterHeights[coordinate.X, coordinate.Y]);
            float lowerSum = neighbours.Where(coordinate => coordinate.Y != (y - 1 + CoordinateSystem.Height) % CoordinateSystem.Height)
                                       .Sum(coordinate => HeightMap.HeightMap[coordinate.X, coordinate.Y] + WaterHeights[coordinate.X, coordinate.Y]);
            Vector acceleration = new Vector(leftSum - rightSum, upperSum - lowerSum);

            return velocities[x, y] * 0.9f + acceleration / total * 10;
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
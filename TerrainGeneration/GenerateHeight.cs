using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Superbest_random;

namespace TerrainGeneration
{
    public class GenerateHeight
    {
		public float[,] HeightMap;
		float delta;
		int distance;
		CoordinateSystem cs;

		public GenerateHeight(int x, int y, int distance = 0)
		{
			cs = new CoordinateSystem(x, y);
			delta = 1;
			HeightMap = new float[x, y];
			this.distance = (int)Math.Pow(2, (int)Math.Log(Math.Min(x, y), 2) / 2);// - distance);
			GenerateHeights();
			CompensateMean();
			Round();
		}

		public void CompensateMean()
		{
			float min= HeightMap[0, 0], max = HeightMap[0, 0];
			for (int x = 0; x < HeightMap.GetLength(0); x++)
				for (int y = 0; y < HeightMap.GetLength(1); y++)
				{
					min = Math.Min(min, HeightMap[x, y]);
					max = Math.Max(max, HeightMap[x, y]);
				}
			for (int x = 0; x < HeightMap.GetLength(0); x++)
				for (int y = 0; y < HeightMap.GetLength(1); y++)
				{
					HeightMap[x, y] -= min;
					HeightMap[x, y] *= 255 / (max - min);
				}

		}
		public void GenerateHeights(Random R = null)
		{
			R = R ?? new Random();
			for (int x = 0; x < HeightMap.GetLength(0); x += distance)
				for (int y = 0; y < HeightMap.GetLength(1); y += distance)
					HeightMap[x, y] = (float)R.NextGaussian(0, delta);
			for (; distance > 0; distance /= 2)
			{
				GenerateOdd(R);
				GenerateEven(R);
				delta /= 2;
			}
		}
		public void Round(Random R = null)
		{
			R = R ?? new Random();
			float[,] Rounded = (float[,])HeightMap.Clone();
			for (int x = 0; x < HeightMap.GetLength(0); x++)
				for (int y = 0; y < HeightMap.GetLength(1); y++)
					Rounded[x, y] = (float)cs.GetNeightbours(new Coordinate(x, y)).Select(c => HeightMap[c.x, c.y]).Concat(new[] { HeightMap[x, y] }).Average();
			HeightMap = Rounded;
		}

		public void GenerateEven(Random R)
		{
			for (int x = 0; x < HeightMap.GetLength(0); x += distance)
				for (int y = 0; y < HeightMap.GetLength(0); y += distance)
				{
					float mean = getMean(x, y, true);
					if (Math.Abs(mean - 0) > float.Epsilon * 10)
						HeightMap[x, y] = (float)R.NextGaussian(mean, delta);
				}
		}
		public void GenerateOdd(Random R)
		{
			for (int x = 0; x < HeightMap.GetLength(0); x += distance)
				for (int y = x % 2 == 0 ? distance : 0; y < HeightMap.GetLength(0); y += distance)
				{
					float mean = getMean(x, y, false);
					if (Math.Abs(mean - 0) > float.Epsilon * 10)
						HeightMap[x, y] = (float)R.NextGaussian(mean, delta);
				}
		}

		public float getMean(int x, int y, bool even)
		{
			float[] neighbours = new float[4];
			if (even)
			{
				if (x - distance >= 0)
					neighbours[0] = HeightMap[x - distance, y];
				if (x + distance < HeightMap.GetLength(0))
					neighbours[1] = HeightMap[x + distance, y];
				if (y - distance >= 0)
					neighbours[2] = HeightMap[x, y - distance];
				if (y + distance < HeightMap.GetLength(1))
					neighbours[3] = HeightMap[x, y + distance];
			}
			else
			{
				if (x - distance >= 0 && y - distance >= 0)
					neighbours[0] = HeightMap[x - distance, y - distance];
				if (x + distance < HeightMap.GetLength(0) && y - distance >= 0)
					neighbours[1] = HeightMap[x + distance, y - distance];
				if (x - distance >= 0 && y + distance < HeightMap.GetLength(1))
					neighbours[2] = HeightMap[x - distance, y + distance];
				if (x + distance < HeightMap.GetLength(0) && y + distance < HeightMap.GetLength(1))
					neighbours[3] = HeightMap[x + distance, y + distance];
			}

			var v = neighbours.Where(f => Math.Abs(f) > 10 * float.Epsilon);
			if (v.Count() != 0)
				return v.Sum() / v.Count();
			else
				return 0;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerrainGeneration
{
	public class WaterGeneration
	{
		readonly float WaterPerTile;
		float RainPercentage = 0.8f;
		public float[,] HeightMap;
		public float[,] water;
		float[,] waterPreviousIteration;

		public WaterGeneration(float[,] heightMap, float WaterPerTile = 10)
		{
			this.WaterPerTile = WaterPerTile;
			HeightMap = heightMap;
			water = new float[HeightMap.GetLength(0), HeightMap.GetLength(1)];
			for (int x = 0; x < HeightMap.GetLength(0); x++)
				for (int y = 0; y < HeightMap.GetLength(0); y++)
					water[x, y] = this.WaterPerTile;
			waterPreviousIteration = water;
			GetWater();
		}


		public void GetWater()
		{
			for (int x = 0; x < 100; x++)
			{
				waterPreviousIteration = (float[,])water.Clone();
				water = new float[HeightMap.GetLength(0), HeightMap.GetLength(1)];
				IterateWater();
				//waterPreviousIteration = (float[,])water.Clone();
				//Rain();
			}
			for (int x = 0; x < 20; x++)
			{
				waterPreviousIteration = (float[,])water.Clone();
				water = new float[HeightMap.GetLength(0), HeightMap.GetLength(1)];
				IterateWater();
			}
		}
		public void Rain()
		{
			float total = 0;
			for (int x = 0; x < water.GetLength(0); x++)
				for (int y = 0; y < water.GetLength(1); y++)
				{
					water[x, y] = Math.Max(waterPreviousIteration[x, y] * (1 - RainPercentage), waterPreviousIteration[x, y] - WaterPerTile * 0.2f);
					total += waterPreviousIteration[x, y] - water[x, y];
				}
			total /= water.GetLength(0) * water.GetLength(1);
			for (int x = 0; x < water.GetLength(0); x++)
				for (int y = 0; y < water.GetLength(1); y++)
					water[x, y] += total;
		}
		public void IterateWater()
		{
			for (int x = 0; x < water.GetLength(0); x++)
				for (int y = 0; y < water.GetLength(1); y++)
				{
					float z;
					if (waterPreviousIteration[x, y] < 0)
						z = waterPreviousIteration[x, y];
					IterateWater(new Coordinate(x, y));
				}
		}
		private void IterateWater(Coordinate c)
		{
			float total = waterPreviousIteration[c.x, c.y];
			float percentage = 0.5f;
			Coordinate[] Neighbours = GetNeightbours(c);
			for (int x = 0; x < Neighbours.Length; x++)
			{
				float delta;
				if (totalHeight(c) > totalHeight(Neighbours[x]))
					delta = Math.Min(waterPreviousIteration[c.x, c.y], totalHeight(c) - totalHeight(Neighbours[x]));
				else
					delta = Math.Max(-waterPreviousIteration[Neighbours[x].x, Neighbours[x].y], totalHeight(c) - totalHeight(Neighbours[x]));
				water[Neighbours[x].x, Neighbours[x].y] += percentage * delta / Neighbours.Length / 2;
				total -= percentage * delta / Neighbours.Length / 2;
			}
			if (total < 0)
				total = total;
			water[c.x, c.y] += total;
		}
		private float totalHeight(Coordinate c)
		{
			return waterPreviousIteration[c.x, c.y] + HeightMap[c.x, c.y];
		}

		private Coordinate[] GetNeightbours(Coordinate c)
		{
			Coordinate[] coordinates = new[] { new Coordinate(c.x - 1, c.y), new Coordinate(c.x + 1, c.y) , new Coordinate(c.x, c.y - 1) , new Coordinate(c.x, c.y + 1) };
			return coordinates.Where(cc => cc.x >= 0 && cc.y >= 0 && cc.x < water.GetLength(0) && cc.y < water.GetLength(1)).ToArray();
		}
		private struct Coordinate
		{
			public int x, y;

			public Coordinate(int x, int y)
			{
				this.x = x;
				this.y = y;
			}
		}
	}
}

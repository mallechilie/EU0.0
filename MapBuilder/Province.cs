using System.Collections.Generic;
using System.Linq;

namespace MapBuilder
{
	public class Province
    {
		public Tile[] Tiles { get; private set; }
		public ProvinceInfo ProvinceInfo;
		public Province[] Neighbours
		{
			get
			{
				if (neighbours != null)
					return neighbours;
				GetNeighbours();
				return neighbours;
			}
			set => neighbours = value;
		}
		public int Id;
		private Province[] neighbours;
		public int EmptyNeighbours
		{
			get
			{
				return Neighbours.Where(t => t != null && !t.HasNation).Count();
			}
		}
		public Nation Nation;
		public bool HasNation { get { return Nation != null; } }

		public Province(Tile[] tiles, int id, ProvinceInfo provinceInfo = null)
		{
			ProvinceInfo = provinceInfo;
			Id = id;
			Tiles = tiles;
			for (int n = 0; n < Tiles.Length; n++)
				Tiles[n].Province = this;
		}
		public Province(Map map, int size, int id, ProvinceInfo provinceInfo = null)
		{
			ProvinceInfo = provinceInfo;
			Id = id;
			GetTiles(map, size);
		}

		
		public void GetNeighbours()
		{
			Neighbours = Tiles.SelectMany(t => t.Neighbours.Where(u => u!=null && u.HasProvince && u.Province!=this).Select(u => u.Province)).Distinct().ToArray();
		}
		public void GetTiles(Map map, int size)
		{
			Tiles = new Tile[size];
			Tile first = null;
			for (int x=0;x<100;x++)
			{
				Tile f = map.Tiles[Map.R.Next(map.Tiles.GetLength(0)), Map.R.Next(map.Tiles.GetLength(1))];
				if (!f.HasProvince && f.Height > 10) 
					if (f.EmptyNeighbours==f.Neighbours.Length)
					{
						first = f;
						break;
					}
			}
			if (first == null)
				return;
			Tiles[0] = first;
			Tiles[0].Province = this;

			Dictionary<Tile, int> tileScores = first.Neighbours.Where(t => t != null && !t.HasProvince && t.Height > 5).
				ToDictionary(t => t, t => GetScore(t));
			for (int n = 1; n < Tiles.Length && tileScores.Count > 0; n++)
			{
				Tile add = SelectTile(tileScores);
				Tiles[n] = add;
				add.Province = this;
				tileScores.Remove(add);
				foreach (Tile t in add.Neighbours.Where(t => t != null && !t.HasProvince && t.Height > 5))
					if (!tileScores.ContainsKey(t) && t != null)
						tileScores.Add(t, GetScore(t));
					else if (t != null)
						tileScores[t] = GetScore(t);

			}
			if (Tiles.Contains(null))
				Tiles = Tiles;
			if (Tiles.ToList().Where(t => t != null).Count() != Tiles.ToList().Where(t => t != null).Distinct().Count())
				Tiles = Tiles;
			Tiles = Tiles.Where(t => t != null).ToArray();
		}
		
		private int GetScore(Tile tile)
		{

			return (tile.Neighbours.Where(t => t != null).Count() - tile.EmptyNeighbours) *
				tile.Neighbours.Where(t => t != null && t.Province == this).Count() *
				tile.Neighbours.Where(t => t != null && t.Province == this).Count();
		}
		private Tile SelectTile(Dictionary<Tile, int> tileScores)
		{
			int total = tileScores.Sum(kvp => kvp.Value);
			int choice = Map.R.Next(total);
			foreach (KeyValuePair<Tile, int> kvp in tileScores)
				if (choice < kvp.Value)
					return kvp.Key;
				else
					choice -= kvp.Value;
			return null;
		}

		public bool Equals(Province province)
		{
			return Id == province.Id;
		}
	}
	public abstract class ProvinceInfo
	{
		public Province Province;

		protected ProvinceInfo(Province province)
		{
			Province = province;
		}
	}
}

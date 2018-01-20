using System.Collections.Generic;
using System.Linq;

namespace MapBuilder
{
	public class Nation
	{
		private Nation[] neighbours;
		public NationInfo NationInfo;
		private List<Province> provinces;
		public Nation[] Neighbours
		{
			get
			{
				GetNeighbours();
				return neighbours;
			}
			set => neighbours = value;
		}
		public List<Province> Provinces
		{
			get => provinces;
			set
			{
				GetNeighbours(); provinces = value;
			}
		}
		public int Id;

		public Nation(Province[] provinces, int id, NationInfo nationInfo = null)
		{
			this.NationInfo = nationInfo;
			Id = id;
			Provinces = provinces.ToList();
			for (int n = 0; n < Provinces.Count; n++)
				Provinces[n].Nation = this;
		}
		public Nation(Map map, int size, int id, NationInfo nationInfo = null)
		{
			this.NationInfo = nationInfo;
			Id = id;
			GetTiles(map, size);
		}


		public void GetNeighbours()
		{
			Neighbours = Provinces.SelectMany(t => t.Neighbours.Where(u => u != null && u.HasNation).Select(u => u.Nation)).ToArray();
		}
		public void GetTiles(Map map, int size)
		{
			provinces = new List<Province>(size);
			Province first = null;
			for (int x = 0; x < 100; x++)
			{
				Province f = map.Provinces[Map.R.Next(map.Provinces.Length)];
				if (!f.HasNation && f.EmptyNeighbours == f.Neighbours.Length)
				{
					first = f;
					break;
				}
			}
			if (first == null)
				return;
			provinces.Add(first);
			provinces[0].Nation = this;

			Dictionary<Province, int> tileScores = first.Neighbours.Where(t => t != null && !t.HasNation).
				ToDictionary(t => t, t => GetScore(t));
			for (int n = 1; n < size && tileScores.Count > 0; n++)
			{
				Province add = SelectProvince(tileScores);
				provinces.Add(add);
				add.Nation = this;
				tileScores.Remove(add);
				foreach (Province t in add.Neighbours.Where(t => t != null && !t.HasNation))
					if (!tileScores.ContainsKey(t) && t != null)
						tileScores.Add(t, GetScore(t));
					else if (t != null)
						tileScores[t] = GetScore(t);

			}
			provinces = provinces.Where(t => t != null).ToList();
		}

		private int GetScore(Province tile)
		{

			return (tile.Neighbours.Where(t => t != null).Count() - tile.EmptyNeighbours) *
				tile.Neighbours.Where(t => t != null && t.Nation == this).Count() *
				tile.Neighbours.Where(t => t != null && t.Nation == this).Count();
		}

		public void Add(Province province)
		{
			if(province.Nation.Remove(province))
			{
				provinces.Add(province);
			}

		}

		private bool Remove(Province province)
		{
			return provinces.Remove(province);
		}

		private Province SelectProvince(Dictionary<Province, int> tileScores)
		{
			int total = tileScores.Sum(kvp => kvp.Value);
			int choice = Map.R.Next(total);
			foreach (KeyValuePair<Province, int> kvp in tileScores)
				if (choice < kvp.Value)
					return kvp.Key;
				else
					choice -= kvp.Value;
			return null;
		}

		public bool Equals(Nation nation)
		{
			return Id == nation.Id;
		}
	}
	public abstract class NationInfo
	{
		public Nation Nation;

		protected NationInfo(Nation nation)
		{
			this.Nation = nation;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapBuilder
{
	class Nation
	{
		public List<Province> Provinces;
		public readonly int ID;



		private void GetProvinces(Map map, int size)
		{

			Provinces = new List<Province>();
			Province first = null;
			for (int x = 0; x < 100; x++)
			{
				Province f = map.Provinces[Map.R.Next(map.Provinces.Length)];
				if (!f.HasNation)
					if (f.EmptyNeighbours == f.Neighbours.Length)
					{
						first = f;
						break;
					}
			}
			if (first == null)
				return;
			Provinces[0] = first;
			Provinces[0].Nation = this;

			Dictionary<Province, int> ProvinceScores = first.Neighbours.Where(t => t != null && !t.HasNation).
				ToDictionary(t => t, t => GetScore(t));
			for (int n = 1; n < Provinces.Count() && ProvinceScores.Count > 0; n++)
			{
				Province add = SelectProvince(ProvinceScores);
				Provinces[n] = add;
				add.Nation = this;
				ProvinceScores.Remove(add);
				foreach (Tile t in add.Neighbours.Where(t => t != null && !t.HasNation))
					if (!ProvinceScores.ContainsKey(t) && t != null)
						ProvinceScores.Add(t, GetScore(t));
					else if (t != null)
						ProvinceScores[t] = GetScore(t);

			}
			Provinces = Provinces.Where(t => t != null).ToList();
		}
	}
}

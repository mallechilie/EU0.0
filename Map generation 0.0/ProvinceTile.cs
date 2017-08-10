using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapGeneration0_0
{
    public class ProvinceTile
    {
		public Tile[] Tiles { get; private set; }
		public int Id;

		public ProvinceTile(Tile[] tiles, int id)
		{
			Id = id;
			Tiles = tiles;
			for (int n = 0; n < Tiles.Length; n++)
				Tiles[n].Province = this;
		}
		public ProvinceTile(MapTiles map, int id)
		{
			Id = id;
			GetTiles(map);
		}


		public void GetTiles(MapTiles map)
		{
			Tiles = new Tile[40];
			Tile first;
			while ((first = map.Tiles[Program.R.Next(map.Tiles.GetLength(0)), Program.R.Next(map.Tiles.GetLength(1))]).HasProvince) ;
			Tiles[0] = first;
			Tiles[0].Province = this;

			Dictionary<Tile, int> TileScores = first.Neighbours.Where(t => t != null && !t.HasProvince).
				ToDictionary(t => t, t => GetScore(t));
			for (int n = 1; n < Tiles.Length && TileScores.Count > 0; n++)
			{
				Tile add = SelectTile(TileScores);
				Tiles[n] = add;
				add.Province = this;
				TileScores.Remove(add);
				foreach(Tile t in add.Neighbours.Where(t => t != null && !t.HasProvince))
					if(!TileScores.ContainsKey(t))
						TileScores.Add(t, GetScore(t));
			}
		}

		private int GetScore(Tile t)
		{

			return t.Neighbours.Where(u => u == null || u.HasProvince).Count() * 
					t.Neighbours.Where(u => u != null && u.Province == this).Count();
		}
		private Tile SelectTile(Dictionary<Tile, int> tileScores)
		{
			int total = tileScores.Sum(kvp => kvp.Value);
			int choice = Program.R.Next(total);
			foreach (KeyValuePair<Tile, int> kvp in tileScores)
				if (choice < kvp.Value)
					return kvp.Key;
				else
					choice -= kvp.Value;
			return null;
		}

		public bool Equals(ProvinceTile province)
		{
			return Id == province.Id;
		}
	}
}

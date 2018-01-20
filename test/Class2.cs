namespace test
{
    internal class Map2 : TileGroup<TileGroup<TileGroup<TileBase>>>
	{

		public Map2(Nation2 nation)
		{
			Tiles = new[] { nation };
			tileGroup = null;
		}
	}

    internal class Anotherclass : TileGroup<TileGroup<TileBase>>
	{ }

    internal class Nation2 : TileGroup<TileGroup<TileBase>>
	{

		public Nation2(Province2 province, Map2 map)
		{
			Tiles = new[] { province };
			tileGroup = map;
		}
	}

    internal class Province2 : TileGroup<TileBase>
	{

		public Province2(Tile2 tile, Nation2 nation)
		{
			Tiles = new[] { tile };
			tileGroup = nation;
		}
	}

    internal class Tile2 : TileBase
	{

		public Tile2(Province2 province2)
		{
			TileGroup = province2;
		}
	}

}

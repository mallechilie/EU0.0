namespace test
{
	class Map2 : TileGroup<TileGroup<TileGroup<TileBase>>>
	{

		public Map2(Nation2 nation)
		{
			tiles = new[] { nation };
			tileGroup = null;
		}
	}
	class anotherclass : TileGroup<TileGroup<TileBase>>
	{ }
	class Nation2 : TileGroup<TileGroup<TileBase>>
	{

		public Nation2(Province2 province, Map2 map)
		{
			tiles = new[] { province };
			tileGroup = map;
		}
	}
	class Province2 : TileGroup<TileBase>
	{

		public Province2(Tile2 tile, Nation2 nation)
		{
			tiles = new[] { tile };
			tileGroup = nation;
		}
	}
	class Tile2 : TileBase
	{

		public Tile2(Province2 Province2)
		{
			tileGroup = Province2;
		}
	}

}

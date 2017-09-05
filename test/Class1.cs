namespace test
{
	class Map
    {
		int ID;
		Nation[] nations;
    }
	class Nation
	{
		int ID;
		Map map;
		Province[] provinces;
	}
	class Province
	{
		int ID;
		Nation nation;
		Tile[] tiles;
	}
	class Tile
	{
		int ID;
		Province province;
	}

}

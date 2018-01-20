namespace test
{
    internal class Map
    {
        private int id;
        private Nation[] nations;
    }

    internal class Nation
	{
        private int id;
        private Map map;
        private Province[] provinces;
	}

    internal class Province
	{
        private int id;
        private Nation nation;
        private Tile[] tiles;
	}

    internal class Tile
	{
        private int id;
        private Province province;
	}

}

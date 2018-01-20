namespace test
{
    internal abstract class TileGroup<T>
	{
		protected T[] Tiles;
		protected TileGroup<TileGroup<T>> tileGroup;
	}

    internal abstract class TileBase
	{
		protected int Id;
		protected TileGroup<TileBase> TileGroup;
	}
}

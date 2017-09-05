namespace test
{
	abstract class TileGroup<T>
	{
		protected T[] tiles;
		protected TileGroup<TileGroup<T>> tileGroup;
	}
	abstract class TileBase
	{
		protected int ID;
		protected TileGroup<TileBase> tileGroup;
	}
}

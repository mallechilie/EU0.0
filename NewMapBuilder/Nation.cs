namespace NewMapBuilder
{
    class Nation : ITilableWithBase<Nation,Province>
    {
        private readonly int id;
        private readonly Nation[] neighbours;
        private readonly Province[] tiles;
        public int ID
        {
            get { return id; }
        }
        public Nation[] Neighbours
        {
            get { return neighbours; }
        }
        public Province[] Tiles
        {
            get { return tiles; }
        }
    }
}

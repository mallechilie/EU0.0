namespace NewMapBuilder
{
    class Nation : ITilableWithBase<Nation,Province>
    {
        private readonly int id;
        private readonly Nation[] neighbours;
        private readonly Province[] tiles;
        public int ID => id;

        public Nation[] Neighbours => neighbours;
        public Province[] Tiles => tiles;
    }
}

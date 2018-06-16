namespace NewMapBuilder
{
    public abstract class ProvinceInfo
    {
        public readonly Province Province;

        protected ProvinceInfo(Province province)
        {
            Province = province;
            Province.ProvinceInfo = this;
        }
    }
}
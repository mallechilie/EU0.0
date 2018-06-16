using System.Collections.Generic;

namespace NewMapBuilder
{
    public interface ITilable<TSelf>
        where TSelf : ITilable<TSelf>
    {
        int ID { get; }
        TSelf[] Neighbours
        {
            get;
            set;
        }
    }

    public interface ITilableWithBase<TSelf, TBase> : ITilable<TSelf> 
        where TBase : ITilable<TBase>
        where TSelf : ITilable<TSelf>
    {
        List<TBase> Tiles { get; }
    }

    public interface ITilableWithParent<TSelf, TParent> : ITilable<TSelf>
        where TSelf : ITilable<TSelf>
        where TParent : ITilableWithBase<TParent, TSelf>
    {
        TParent Parent
        {
            get;
        }
    }

    public interface ITileMap<TTile> where TTile : ITilable<TTile>
    {
        TTile this[int index]
        {
            get;
        }
        TTile[] Tiles { get; }
    }
    public interface ITileMapWithBase<TTile, TBase> : ITileMap<TTile> 
        where TTile : ITilableWithBase<TTile, TBase> 
        where TBase : ITilable<TBase>
    {
        ITileMap<TBase> Map { get; }
    }
}

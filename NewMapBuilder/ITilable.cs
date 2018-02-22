using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NewMapBuilder
{
    public interface ITilable<TSelf>
    {
        int ID { get; }
        TSelf[] Neighbours { get; }

    }

    public interface ITilableWithBase<TSelf, TBase> : ITilable<TSelf> where TBase : ITilable<TBase>
    {
        TBase[] Tiles { get; }
    }

    public interface ITileMap<TTile> where TTile : ITilable<TTile>
    {
        TTile this[int index]
        {
            get;
        }
        TTile[] Tiles { get; }
    }
    public interface ITileMapWithBase<TTile, TBase> : ITileMap<TTile> where TTile : ITilableWithBase<TTile, TBase> where TBase : ITilable<TBase>
    {
        ITileMap<TBase> Map { get; }
    }
}

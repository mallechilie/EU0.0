using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewMapBuilder
{
    public static class MapExtentions<TTile, TBase> where TTile : ITilableWithBase<TTile, TBase>
                                                    where TBase : ITilable<TBase>
    {
        public static int[][] InitTileGroup(ITileMapWithBase<TTile, TBase> map, ITileMap<TBase> baseMap)
        {





            return null;
        }
    }
}

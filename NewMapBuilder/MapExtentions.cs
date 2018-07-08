using System;
using System.Collections.Generic;
using System.Linq;

namespace NewMapBuilder
{
    public static class MapExtentions
    {
        public static Dictionary<int, List<TBase>> GenerateTileGroup<TTile, TBase>(this ITileMapWithBase<TTile, TBase> map)
            where TTile : ITilableWithBase<TTile, TBase>
            where TBase : ITilable<TBase>
        {
            Dictionary<int, TBase> freeBases = map.Map.Tiles.ToDictionary(t => t.ID);
            Random r = new Random();
            Dictionary<int, List<TBase>> tiles = new Dictionary<int, List<TBase>>();
            int tileSize = (int)Math.Sqrt(map.Map.Tiles.Length);
            for (int counter = 0; freeBases.Count != 0; counter++)
            {
                int first = freeBases.Keys.ElementAt(r.Next(freeBases.Count));
                tiles.Add(counter, new List<TBase> { freeBases[first] });
                freeBases.Remove(first);
                List<TBase> neighbours = new List<TBase>(tiles[counter][0].Neighbours
                    .Where(t => freeBases.ContainsKey(t.ID)));

                for (int i = 0; i < tileSize; i++)
                {
                    //TODO: add weighting.
                    if (neighbours.Count == 0)
                        break;
                    int next = r.Next(neighbours.Count);
                    tiles[counter].Add(neighbours[next]);
                    freeBases.Remove(neighbours[next].ID);
                    neighbours.AddRange(neighbours[next].Neighbours
                        .Where(t => freeBases.ContainsKey(t.ID)));
                    neighbours.RemoveAll(t => t.ID == neighbours[next].ID);
                    if (neighbours.Intersect(tiles[counter]).Count() != 0) ;
                }

            }
            tiles = tiles.Where(kvp => kvp.Value.Count > tileSize / 10).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            return tiles;
        }

        public static void GetNeighbours<TTile, TBase>(this TTile tile, ITileMapWithBase<TTile, TBase> map)
            where TTile : ITilableWithBase<TTile, TBase>
            where TBase : ITilable<TBase>, ITilableWithParent<TBase, TTile>
        {
            Dictionary<int, TTile> neighbours = new Dictionary<int, TTile>();
            foreach (TBase @base in tile.Tiles)
                foreach (TBase tileNeighbour in @base.Neighbours)
                    if (tileNeighbour.ParentID<TBase, TTile>() != -1 &&
                        !neighbours.ContainsKey(tileNeighbour.ParentID<TBase, TTile>()))
                    {
                        neighbours.Add(
                                       tileNeighbour.ParentID<TBase, TTile>(),
                                       map.Tiles[tileNeighbour.ParentID<TBase, TTile>()]);
                    }
            neighbours.Remove(tile.ID);
            tile.Neighbours = neighbours.Values.ToArray();
        }

        public static void Add<TTile, TBase>(this TTile tile, TBase tbase)
            where TTile : ITilableWithBase<TTile, TBase>
            where TBase : ITilableWithParent<TBase, TTile>
        {
            tbase.Parent?.Remove(tbase);
            tile.Tiles.Add(tbase);
            tbase.Parent = tile;
        }

        public static bool Remove<TTile, TBase>(this TTile tile, TBase tbase)
            where TTile : ITilableWithBase<TTile, TBase>
            where TBase : ITilableWithParent<TBase, TTile>
        {
            return tile.Tiles.Remove(tbase);
        }

        public static int ParentID<TSelf, TParent>(this TSelf self)
            where TSelf : ITilableWithParent<TSelf, TParent>
            where TParent : ITilableWithBase<TParent, TSelf>
        {
            return self.Parent == null ?
                0 :
                self.Parent.ID;
        }
    }
}

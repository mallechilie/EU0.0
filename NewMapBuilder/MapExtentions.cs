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
        public static Dictionary<int, List<TBase>> GenerateTileGroup(ITileMapWithBase<TTile, TBase> map)
        {
            List<TBase> freeBases = new List<TBase>(map.Map.Tiles);
            Random r = new Random();
            Dictionary<int, List<TBase>> tiles = new Dictionary<int, List<TBase>>();
            int tileSize = (int)Math.Sqrt(map.Map.Tiles.Length);
            for (int counter = 0; freeBases.Count != 0; counter++)
            {
                int first = r.Next(freeBases.Count);
                tiles.Add(counter, new List<TBase> { freeBases[first] });
                freeBases.RemoveAt(first);
                List<TBase> neighbours = new List<TBase>(tiles[counter][0].Neighbours.Where(t => freeBases.Contains(t)));

                for (int i = 0; i < tileSize; i++)
                {
                    //TODO: add weighting.
                    if (neighbours.Count == 0)
                        break;
                    int next = r.Next(neighbours.Count);
                    tiles[counter].Add(neighbours[next]);
                    freeBases.Remove(neighbours[next]);
                    neighbours.AddRange(neighbours[next].Neighbours
                        .Where(t => freeBases.Contains(t) && !neighbours.Contains(t)));
                    neighbours.RemoveAt(next);
                    if (neighbours.Distinct().Count() != neighbours.Count);
                    if (neighbours.Intersect(tiles[counter]).Count() != 0);
                }

            }
            tiles = tiles.Where(kvp => kvp.Value.Count > tileSize / 10).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            return tiles;
        }
    }
}

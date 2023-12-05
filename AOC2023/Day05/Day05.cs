using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AOC2023
{
    internal class Day05
    {
        public class MapEntry
        {
            public long DestStart { get; private set; }
            public long SourceStart { get; private set; }
            public long Range { get; private set; }
            public long SourceEnd
            {
                get
                {
                    return SourceStart + Range - 1;
                }
            }

            public MapEntry(long destStart, long sourceStart, long range)
            {
                DestStart = destStart;
                SourceStart = sourceStart;
                Range = range;
            }

            public bool IsInSource(long source)
            {
                return source >= SourceStart && source < SourceStart + Range;
            }

            public long Get(long source)
            {
                return DestStart + (source - SourceStart);
            }
        }

        public record LongRange(long start, long range)
        {
            public long GetStart()
            {
                return start;
            }

            public long GetEnd()
            {
                return start + range - 1;
            }
        }

        public class Map
        {
            public List<MapEntry> MapEntries { get; private set; }
            public string Source { get; private set; }
            public string Destination { get; private set; }

            public Map(string source, string destination)
            {
                MapEntries = new List<MapEntry>();
                Source = source;
                Destination = destination;
            }

            public void Sort()
            {
                MapEntries.Sort((a, b) => a.SourceStart.CompareTo(b.SourceStart));
            }

            public long Get(long source)
            {
                foreach (var entry in MapEntries)
                {
                    if (entry.IsInSource(source))
                    {
                        return entry.Get(source);
                    }
                }
                return source;
            }

            public List<LongRange> GetRange(LongRange source)
            {
                List<LongRange> result = new List<LongRange>();
                List<MapEntry> includedEntries = new List<MapEntry>();

                foreach (var entry in MapEntries)
                {
                    if (entry.IsInSource(source.GetStart()) || (source.GetStart() <= entry.SourceStart && source.GetEnd() >= entry.SourceStart))
                    {
                        includedEntries.Add(entry);
                    }
                }
                long i = source.GetStart();
                while (i <= source.GetEnd())
                {
                    MapEntry? entry = includedEntries.FirstOrDefault();

                    if (entry == null)
                    {
                        var nextI = source.GetEnd();
                        result.Add(new LongRange(i, nextI - i + 1));
                        break;
                    }
                    if (entry.SourceStart > i)
                    {
                        var nextI = Math.Min(entry.SourceStart, source.GetEnd());
                        result.Add(new LongRange(i, nextI - i + 1));
                        i = nextI + 1;
                    }
                    if (entry.IsInSource(i))
                    {
                        var resultStart = entry.Get(i);
                        var nextI = Math.Min(entry.SourceEnd, source.GetEnd());
                        var resultEnd = entry.Get(nextI);
                        result.Add(new LongRange(resultStart, resultEnd - resultStart + 1));
                        i = nextI + 1;
                        includedEntries.Remove(entry);
                    }
                }
                return result;
            }
        }

        public record MapKey(string source, string dest);

        public static void Solve1()
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
            var lines = File.ReadAllLines($"{className}/input.txt");
            long result = long.MaxValue;

            List<long> seeds = new List<long>();
            MapKey? mapKey = null;
            Dictionary<string, Map> maps = new Dictionary<string, Map>();

            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }
                else
                if (line.StartsWith("seeds:"))
                {
                    seeds = line.Split(' ').Skip(1).Select(long.Parse).ToList();
                    continue;
                }
                else
                if (line.Contains("map"))
                {
                    var split = line.Split(' ')[0].Split('-');
                    mapKey = new MapKey(split[0], split[2]);
                    maps[mapKey.source] = new Map(mapKey.source, mapKey.dest);
                    continue;
                }
                else
                {
                    var split = line.Split(' ').Select(long.Parse).ToList();
                    var entry = new MapEntry(split[0], split[1], split[2]);
                    maps[mapKey.source].MapEntries.Add(entry);
                }
            }

            foreach (var seed in seeds)
            {
                string currentCategory = "seed";
                long current = seed;
                var map = maps[currentCategory];
                while (!currentCategory.Equals("location"))
                {
                    current = map.Get(current);
                    currentCategory = map.Destination;
                    if (maps.ContainsKey(currentCategory))
                    {
                        map = maps[currentCategory];
                    }
                }
                result = Math.Min(result, current);
            }

            Console.WriteLine($"{className}.1: " + result);
        }

        public static void Solve2()
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
            var lines = File.ReadAllLines($"{className}/input.txt");
            long result = long.MaxValue;

            List<long> seeds = new List<long>();
            MapKey? mapKey = null;
            Dictionary<string, Map> maps = new Dictionary<string, Map>();

            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }
                else
                if (line.StartsWith("seeds:"))
                {
                    seeds = line.Split(' ').Skip(1).Select(long.Parse).ToList();
                    continue;
                }
                else
                if (line.Contains("map"))
                {
                    var split = line.Split(' ')[0].Split('-');
                    mapKey = new MapKey(split[0], split[2]);
                    maps[mapKey.source] = new Map(mapKey.source, mapKey.dest);
                    continue;
                }
                else
                {
                    var split = line.Split(' ').Select(long.Parse).ToList();
                    var entry = new MapEntry(split[0], split[1], split[2]);
                    maps[mapKey.source].MapEntries.Add(entry);
                }
            }

            foreach (var map in maps)
            {
                map.Value.Sort();
            }

            for (int i = 0; i < seeds.Count; i+=2)
            {
                LongRange seedRange = new LongRange(seeds[i], seeds[i+1]);
                string currentCategory = "seed";
                List<LongRange> current = new List<LongRange>() { seedRange };
                var map = maps[currentCategory];

                while (!currentCategory.Equals("location"))
                {
                    List<LongRange> newCurrent = new List<LongRange>();
                    foreach (var c in current)
                    {
                        newCurrent.AddRange(map.GetRange(c));
                    }
                    current = newCurrent;
                    currentCategory = map.Destination;
                    if (maps.ContainsKey(currentCategory))
                    {
                        map = maps[currentCategory];
                    }
                }
                foreach (var c in current)
                {
                    result = Math.Min(result, c.GetStart());
                }
            }

            Console.WriteLine($"{className}.2: " + result);
        }
    }
}

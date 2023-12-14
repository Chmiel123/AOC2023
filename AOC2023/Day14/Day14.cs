using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static AOC2023.Day05;

namespace AOC2023
{
    internal class Day14
    {
        private const string INPUT = "input.txt";

        public static void Solve1()
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
            var lines = File.ReadAllLines($"{className}/{INPUT}");
            long result = 0;

            List<List<char>> map = new List<List<char>>();
            foreach (var line in lines)
            {
                map.Add(line.ToList());
            }
            for (int i = 0; i < map.Count; i++)
            {
                for (int j = 0; j < map[i].Count; j++)
                {
                    if (map[i][j] == 'O')
                    {
                        int k = i - 1;
                        while (k >= 0 && map[k][j] == '.')
                        {
                            k--;
                        }
                        if (k + 1 < i)
                        {
                            map[k + 1][j] = 'O';
                            map[i][j] = '.';
                        }
                    }
                }
            }
            for (int i = 0; i < map.Count; i++)
            {
                for (int j = 0; j < map[i].Count; j++)
                {
                    //Console.Write(map[i][j]);
                    if (map[i][j] == 'O')
                    {
                        result += map.Count - i;
                    }
                }
                //Console.WriteLine();
            }

            Console.WriteLine($"{className}.1: " + result);
        }

        public static List<List<char>> Cycle(List<List<char>> map)
        {
            List<List<char>> result = new List<List<char>>();

            for (int i = 0; i < map.Count; i++)
            {
                result.Add(new List<char>());
                for (int j = 0; j < map[i].Count; j++)
                {
                    result[i].Add(map[i][j]);
                }
            }

            //North
            for (int i = 0; i < result.Count; i++)
            {
                for (int j = 0; j < result[i].Count; j++)
                {
                    if (result[i][j] == 'O')
                    {
                        int k = i - 1;
                        while (k >= 0 && result[k][j] == '.')
                        {
                            k--;
                        }
                        if (k + 1 < i)
                        {
                            result[k + 1][j] = 'O';
                            result[i][j] = '.';
                        }
                    }
                }
            }
            //West
            for (int j = 0; j < result[0].Count; j++)
            {
                for (int i = 0; i < result.Count; i++)
                {
                    if (result[i][j] == 'O')
                    {
                        int k = j - 1;
                        while (k >= 0 && result[i][k] == '.')
                        {
                            k--;
                        }
                        if (k + 1 < j)
                        {
                            result[i][k + 1] = 'O';
                            result[i][j] = '.';
                        }
                    }
                }
            }
            //South
            for (int i = result.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < result[i].Count; j++)
                {
                    if (result[i][j] == 'O')
                    {
                        int k = i + 1;
                        while (k < result.Count && result[k][j] == '.')
                        {
                            k++;
                        }
                        if (k - 1 > i)
                        {
                            result[k - 1][j] = 'O';
                            result[i][j] = '.';
                        }
                    }
                }
            }
            //East
            for (int j = result[0].Count - 1; j >= 0; j--)
            {
                for (int i = 0; i < result.Count; i++)
                {
                    if (result[i][j] == 'O')
                    {
                        int k = j + 1;
                        while (k < result[0].Count && result[i][k] == '.')
                        {
                            k++;
                        }
                        if (k - 1 > j)
                        {
                            result[i][k - 1] = 'O';
                            result[i][j] = '.';
                        }
                    }
                }
            }

            return result;
        }

        public static bool CompareMaps(List<List<char>> map1, List<List<char>> map2)
        {
            for (int i = 0; i < map1.Count; i++)
            {
                for (int j = 0; j < map1[i].Count; j++)
                {
                    if (map1[i][j] != map2[i][j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        class MapComparer : IEqualityComparer<List<List<char>>>
        {
            public bool Equals(List<List<char>> x, List<List<char>> y)
            {
                for (int i = 0; i < x.Count; i++)
                {
                    for (int j = 0; j < x[i].Count; j++)
                    {
                        if (x[i][j] != y[i][j])
                        {
                            return false;
                        }
                    }
                }
                return true;
            }

            public int GetHashCode(List<List<char>> obj)
            {
                int hashcode = 0;
                for (int i = 0; i < obj.Count; i++)
                {
                    for (int j = 0; j < obj[i].Count; j++)
                    {
                        hashcode ^= obj[i][j].GetHashCode() * i.GetHashCode() * j.GetHashCode();
                    }
                }
                return hashcode;
            }
        }

        public static void Solve2()
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
            var lines = File.ReadAllLines($"{className}/{INPUT}");
            long result = 0;

            long cycles = 1000000000;

            List<List<char>> map = new List<List<char>>();
            foreach (var line in lines)
            {
                map.Add(line.ToList());
            }

            List<int> hashes = new List<int>();
            MapComparer mc = new MapComparer();
            bool foundRepeat = false;

            for (int i = 0; i < cycles; i++)
            {
                var next = Cycle(map);
                map = next;
                if (!foundRepeat)
                {
                    int hash = mc.GetHashCode(next);
                    if (hashes.Contains(hash))
                    {
                        var repeatEveryCycles = i - hashes.IndexOf(hash);
                        var toEnd = cycles - i;
                        //Console.WriteLine($"Found repeat i: {i}, repeat: {repeatEveryCycles}, to end: {toEnd}");
                        i += (int)Math.Floor((double)toEnd / repeatEveryCycles) * repeatEveryCycles;
                        //Console.WriteLine($"Skipped to cycle: {i}");
                        foundRepeat = true;
                        continue;
                    }
                    hashes.Add(hash);
                }
            }

            for (int i = 0; i < map.Count; i++)
            {
                for (int j = 0; j < map[i].Count; j++)
                {
                    //Console.Write(map[i][j]);
                    if (map[i][j] == 'O')
                    {
                        result += map.Count - i;
                    }
                }
                //Console.WriteLine();
            }

            Console.WriteLine($"{className}.2: " + result);
        }
    }
}

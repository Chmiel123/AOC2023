using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AOC2023
{
    internal class Day13
    {
        private const string INPUT = "input.txt";

        public static int FindSymmetry(List<string> map)
        {
            List<int> xSymmetries = new List<int>();
            List<int> ySymmetries = new List<int>();
            for (int i = 1; i < map.Count; i++)
            {
                ySymmetries.Add(i);
            }
            for (int j = 1; j < map[0].Length; j++)
            {
                xSymmetries.Add(j);
            }
            for (int i = 0; i < map.Count; i++)
            {
                for (int j = 0; j < map[i].Length; j++)
                {
                    for (int xsym = xSymmetries.Count - 1; xsym >= 0; xsym--)
                    {
                        int sym = xSymmetries[xsym];
                        var jsym = j + 2 * (sym - j) - 1;
                        if (jsym >= 0 && jsym < map[i].Length && map[i][j] != map[i][jsym])
                        {
                            xSymmetries.RemoveAt(xsym);
                        }
                    }
                    for (int ysym = ySymmetries.Count - 1; ysym >= 0; ysym--)
                    {
                        int sym = ySymmetries[ysym];
                        var isym = i + 2 * (sym - i) - 1;
                        if (isym >= 0 && isym < map.Count && map[i][j] != map[isym][j])
                        {
                            ySymmetries.RemoveAt(ysym);
                        }
                    }
                }
            }
            return xSymmetries.FirstOrDefault(0) + 100 * ySymmetries.FirstOrDefault(0);
        }

        public static void Solve1()
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
            var lines = File.ReadAllLines($"{className}/{INPUT}");
            long result = 0;

            List<string> map = new List<string>();
            foreach ( var line in lines )
            {
                if (string.IsNullOrEmpty(line))
                {
                    result += FindSymmetry(map);

                    map = new List<string>();
                    continue;
                }
                map.Add(line);
            }
            result += FindSymmetry(map);

            Console.WriteLine($"{className}.1: " + result);
        }

        public class Symmetry
        {
            public int Index { get; set; }
            public int Count { get; set; }

            public Symmetry(int index)
            {
                Index = index;
                Count = 0;
            }
        }

        public static int FindSymmetrySmudge(List<string> map)
        {
            List<Symmetry> xSymmetries = new List<Symmetry>();
            List<Symmetry> ySymmetries = new List<Symmetry>();
            for (int i = 1; i < map.Count; i++)
            {
                ySymmetries.Add(new Symmetry(i));
            }
            for (int j = 1; j < map[0].Length; j++)
            {
                xSymmetries.Add(new Symmetry(j));
            }
            for (int i = 0; i < map.Count; i++)
            {
                for (int j = 0; j < map[i].Length; j++)
                {
                    for (int xsym = xSymmetries.Count - 1; xsym >= 0; xsym--)
                    {
                        int sym = xSymmetries[xsym].Index;
                        var jsym = j + 2 * (sym - j) - 1;
                        if (jsym >= 0 && jsym < map[i].Length && map[i][j] != map[i][jsym])
                        {
                            xSymmetries[xsym].Count++;
                        }
                    }
                    for (int ysym = ySymmetries.Count - 1; ysym >= 0; ysym--)
                    {
                        int sym = ySymmetries[ysym].Index;
                        var isym = i + 2 * (sym - i) - 1;
                        if (isym >= 0 && isym < map.Count && map[i][j] != map[isym][j])
                        {
                            ySymmetries[ysym].Count++;
                        }
                    }
                }
            }
            return xSymmetries.Where(x => x.Count == 2).Select(x => x.Index).FirstOrDefault(0)
                + 100 * ySymmetries.Where(x => x.Count == 2).Select(x => x.Index).FirstOrDefault(0);
        }

        public static void Solve2()
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
            var lines = File.ReadAllLines($"{className}/{INPUT}");
            long result = 0;

            List<string> map = new List<string>();
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    result += FindSymmetrySmudge(map);

                    map = new List<string>();
                    continue;
                }
                map.Add(line);
            }
            result += FindSymmetrySmudge(map);

            Console.WriteLine($"{className}.2: " + result);
        }
    }
}

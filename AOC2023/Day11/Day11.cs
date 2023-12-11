using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AOC2023
{
    internal class Day11
    {
        private const string INPUT = "input.txt";

        public class Galaxy
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Galaxy(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        public static void Solve1()
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
            var lines = File.ReadAllLines($"{className}/{INPUT}");
            long result = 0;

            List<Galaxy> galaxies = new List<Galaxy>();

            List<bool> shouldExpandJ = new List<bool>();
            for (int j = 0; j < lines[0].Length; j++)
            {
                shouldExpandJ.Add(true);
            }
            int di = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                bool shouldExpandI = true;
                for (int j = 0; j < line.Length; j++)
                {
                    char c = line[j];
                    if (c == '#')
                    {
                        shouldExpandI = false;
                        shouldExpandJ[j] = false;
                        galaxies.Add(new Galaxy(i + di, j));
                    }
                }
                if (shouldExpandI)
                {
                    di++;
                }
            }
            for (int j = shouldExpandJ.Count - 1; j >= 0; j--)
            {
                if (shouldExpandJ[j])
                {
                    foreach (var galaxy in galaxies)
                    {
                        if (galaxy.Y > j)
                        {
                            galaxy.Y++;
                        }
                    }
                }
            }

            //for (int i = 0; i < galaxies.Count; i++)
            //{
            //    Console.WriteLine($"Galaxy {i}: X = {galaxies[i].X}, Y = {galaxies[i].Y}");
            //}

            for (int i = 0; i < galaxies.Count; i++)
            {
                Galaxy g1 = galaxies[i];
                for (int j = i + 1; j < galaxies.Count; j++)
                {
                    Galaxy g2 = galaxies[j];
                    if (g1.X == g2.X && g1.Y == g2.Y)
                    {
                        continue;
                    }
                    result += Math.Abs(g1.X - g2.X) + Math.Abs(g1.Y - g2.Y);
                }
            }

            Console.WriteLine($"{className}.1: " + result);
        }

        public static void Solve2()
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
            var lines = File.ReadAllLines($"{className}/{INPUT}");
            long result = 0;

            int expansion = 1000000;
            List<Galaxy> galaxies = new List<Galaxy>();

            List<bool> shouldExpandJ = new List<bool>();
            for (int j = 0; j < lines[0].Length; j++)
            {
                shouldExpandJ.Add(true);
            }
            int di = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                bool shouldExpandI = true;
                for (int j = 0; j < line.Length; j++)
                {
                    char c = line[j];
                    if (c == '#')
                    {
                        shouldExpandI = false;
                        shouldExpandJ[j] = false;
                        galaxies.Add(new Galaxy(i + di * (expansion - 1), j));
                    }
                }
                if (shouldExpandI)
                {
                    //Console.WriteLine($"X expansion");
                    di++;
                }
            }
            for (int j = shouldExpandJ.Count - 1; j >= 0; j--)
            {
                if (shouldExpandJ[j])
                {
                    //Console.WriteLine($"Y expansion");
                    foreach (var galaxy in galaxies)
                    {
                        if (galaxy.Y > j)
                        {
                            galaxy.Y += expansion - 1;
                        }
                    }
                }
            }

            //for (int i = 0; i < galaxies.Count; i++)
            //{
            //    Console.WriteLine($"Galaxy {i}: X = {galaxies[i].X}, Y = {galaxies[i].Y}");
            //}

            for (int i = 0; i < galaxies.Count; i++)
            {
                Galaxy g1 = galaxies[i];
                for (int j = i + 1; j < galaxies.Count; j++)
                {
                    Galaxy g2 = galaxies[j];
                    if (g1.X == g2.X && g1.Y == g2.Y)
                    {
                        continue;
                    }
                    result += Math.Abs(g1.X - g2.X) + Math.Abs(g1.Y - g2.Y);
                }
            }

            Console.WriteLine($"{className}.2: " + result);
        }
    }
}

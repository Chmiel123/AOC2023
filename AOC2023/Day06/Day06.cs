using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AOC2023
{
    internal class Day06
    {
        private const string INPUT = "input.txt";

        public static void Solve1()
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
            var lines = File.ReadAllLines($"{className}/{INPUT}");
            long result = 1;

            List<int> times = lines[0].Split(' ').Skip(1).Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToList();
            List<int> distances= lines[1].Split(' ').Skip(1).Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToList();

            for (int i = 0; i < times.Count; i++)
            {
                int waysToRecord = 0;
                for (int j = 1; j < times[i]; j++)
                {
                    int distance = j * (times[i] - j);
                    if (distance > distances[i])
                    {
                        waysToRecord++;
                    }
                }
                //Console.WriteLine($"Race {i}: waysToRecord: {waysToRecord}");
                result *= waysToRecord;
            }

            Console.WriteLine($"{className}.1: " + result);
        }

        public static void Solve2_1()
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
            var lines = File.ReadAllLines($"{className}/{INPUT}");
            long result = 0;

            long time = long.Parse(string.Concat(lines[0].Split(' ').Skip(1).Where(x => !string.IsNullOrEmpty(x))));
            long record = long.Parse(string.Concat(lines[1].Split(' ').Skip(1).Where(x => !string.IsNullOrEmpty(x))));
            
            for (long i = 1; i < time; i++)
            {
                long distance = i * (time - i);
                if (distance > record)
                {
                    result++;
                }
            }

            Console.WriteLine($"{className}.2: " + result);
        }

        private static long Func(long x, long time)
        {
            return x * (time - x);
        }

        private static long Euler(long x, long time, long record, bool ascending)
        {
            long prevX = x;
            while (true)
            {
                long y = Func(x, time);
                long dx = 1;
                long dy = Func(x + dx, time) - y;
                double c = (long)(y - (double)x / dx * dy);
                long newX = (long)((record - c) / dy * dx);
                if (ascending && x >= prevX && newX <= x)
                {
                    long a = Func(newX - 1, time);
                    long b;
                    for (long i = newX; i <= x + 1; i++)
                    {
                        b = Func(i, time);
                        if (b > record && a < record)
                        {
                            return i;
                        }
                        a = b;
                    }
                    throw new Exception("Euler crashed");
                }
                else if (!ascending && x <= prevX && newX >= x)
                {
                    long a = Func(newX + 1, time);
                    long b;
                    for (long i = newX; i >= x - 1; i--)
                    {
                        b = Func(i, time);
                        if (b > record && a < record)
                        {
                            return i;
                        }
                        a = b;
                    }
                    throw new Exception("Euler crashed");
                }
                prevX = x;
                x = newX;
            }
        }

        public static void Solve2_2()
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
            var lines = File.ReadAllLines($"{className}/{INPUT}");
            long result = 0;

            long time = long.Parse(string.Concat(lines[0].Split(' ').Skip(1).Where(x => !string.IsNullOrEmpty(x))));
            long record = long.Parse(string.Concat(lines[1].Split(' ').Skip(1).Where(x => !string.IsNullOrEmpty(x))));

            long a = Euler(1, time, record, ascending: true);
            long b = Euler(time, time, record, ascending: false);
            result = b - a + 1;

            Console.WriteLine($"{className}.2: " + result);
        }
    }
}

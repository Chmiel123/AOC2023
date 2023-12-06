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
        public static void Solve1()
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
            var lines = File.ReadAllLines($"{className}/input.txt");
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

        public static void Solve2()
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
            var lines = File.ReadAllLines($"{className}/input.txt");
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
    }
}

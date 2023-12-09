using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AOC2023
{
    internal class Day09
    {
        private const string INPUT = "input.txt";

        public static IEnumerable<int> Differences(IEnumerable<int> numbers)
        {
            int last = numbers.First();
            var leftNumbers = numbers.Skip(1);
            foreach (int item in leftNumbers)
            {
                yield return item - last;
                last = item;
            }
            yield break;
        }

        public static void Solve1()
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
            var lines = File.ReadAllLines($"{className}/{INPUT}");
            long result = 0;

            foreach (var line in lines)
            {
                var values = line.Split(' ').Select(int.Parse);
                List<List<int>> differences = new List<List<int>>
                {
                    values.ToList()
                };
                while (!differences.Last().All(x => x == 0))
                {
                    differences.Add(Differences(differences.Last()).ToList());
                }
                differences.Last().Add(0);
                for (int i = differences.Count - 2; i >= 0; i--)
                {
                    differences[i].Add(differences[i].Last() + differences[i + 1].Last());
                }
                result += differences.First().Last();
            }

            Console.WriteLine($"{className}.1: " + result);
        }

        public static void Solve2()
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
            var lines = File.ReadAllLines($"{className}/{INPUT}");
            long result = 0;

            foreach (var line in lines)
            {
                var values = line.Split(' ').Select(int.Parse);
                List<List<int>> differences = new List<List<int>>
                {
                    values.ToList()
                };
                while (!differences.Last().All(x => x == 0))
                {
                    differences.Add(Differences(differences.Last()).ToList());
                }
                differences.Last().Insert(0, 0);
                for (int i = differences.Count - 2; i >= 0; i--)
                {
                    differences[i].Insert(0, differences[i].First() - differences[i + 1].First());
                }
                result += differences.First().First();
            }

            Console.WriteLine($"{className}.2: " + result);
        }
    }
}

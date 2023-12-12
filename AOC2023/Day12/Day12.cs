using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AOC2023
{
    internal class Day12
    {
        private const string INPUT = "input.txt";

        class Comparer : IEqualityComparer<(string, List<int>)>
        {
            public bool Equals((string, List<int>) x, (string, List<int>) y)
            {
                if (!x.Item1.Equals(y.Item1))
                    return false;
                return x.Item2.SequenceEqual(y.Item2);
            }

            public int GetHashCode([DisallowNull] (string, List<int>) obj)
            {
                int hashcode = 0;
                hashcode ^= obj.Item1.GetHashCode();
                foreach (int t in obj.Item2)
                {
                    hashcode ^= t.GetHashCode();
                }
                return hashcode;
            }
        }

        public static Dictionary<(string, List<int>), long> Cache = new Dictionary<(string, List<int>), long>(new Comparer());

        public static long Calculate(string input, List<int> seq)
        {
            if (input == "")
            {
                if (seq.Count == 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }

            if (seq.Count == 0)
            {
                if (input.Contains('#'))
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }

            var key = (input, seq);

            if (Cache.ContainsKey(key))
            {
                return Cache[key];
            }

            long res = 0;

            if (input[0] == '.' || input[0] == '?')
            {
                res += Calculate(input.Substring(1), seq);
            }

            if (input[0] == '#' || input[0] == '?')
            {
                if (seq[0] <= input.Length && !input.Substring(0, seq[0]).Contains('.') && (seq[0] == input.Length || input[seq[0]] != '#'))
                {
                    if (seq[0] == input.Length)
                    {
                        res += Calculate("", seq.Skip(1).ToList());
                    }
                    else
                    {
                        res += Calculate(input.Substring(seq[0] + 1), seq.Skip(1).ToList());
                    }
                }
            }

            Cache[key] = res;

            return res;
        }

        public static void Solve1()
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
            var lines = File.ReadAllLines($"{className}/{INPUT}");
            long result = 0;

            foreach (var line in lines)
            {
                var split = line.Split(' ');
                var seq = split[1].Split(',').Select(int.Parse).ToList();
                var ways = Calculate(split[0], seq);
                result += ways;
                Console.WriteLine($"{split[0]} {split[1]} : {ways}");
            }

            Console.WriteLine($"{className}.1: " + result);
        }

        public static void Solve2()
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
            var lines = File.ReadAllLines($"{className}/{INPUT}");
            long result = 0;
            int repeat = 5;

            foreach (var line in lines)
            {
                var split = line.Split(' ');
                var seq = split[1].Split(',').Select(int.Parse).ToList();
                var ways = Calculate(string.Join('?', Enumerable.Repeat(split[0], repeat)), Enumerable.Repeat(seq, repeat).SelectMany(x => x).ToList());
                result += ways;
                Console.WriteLine($"{split[0]} {split[1]} : {ways}");
            }

            Console.WriteLine($"{className}.2: " + result);
        }
    }
}

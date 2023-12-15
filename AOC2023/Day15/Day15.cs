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
    internal class Day15
    {
        private const string INPUT = "input.txt";

        public static int Hash(string input)
        {
            int result = 0;
            foreach (var c in input)
            {
                result += c;
                result *= 17;
                result %= 256;
            }
            return result;
        }

        public static void Solve1()
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
            var lines = File.ReadAllLines($"{className}/{INPUT}");
            long result = 0;

            foreach (var code in lines[0].Split(','))
            {
                result += Hash(code);
            }

            Console.WriteLine($"{className}.1: " + result);
        }

        public class Lens
        {
            public string Label { get; set; }
            public int Focal { get; set; }
        }

        public static void Solve2()
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
            var lines = File.ReadAllLines($"{className}/{INPUT}");
            long result = 0;

            Dictionary<int, List<Lens>> hashMap = new Dictionary<int, List<Lens>>();
            foreach (var code in lines[0].Split(','))
            {
                if (code.Contains('='))
                {
                    var split = code.Split('=');
                    var hash = Hash(split[0]);
                    if (!hashMap.ContainsKey(hash))
                    {
                        hashMap[hash] = new List<Lens>();
                        hashMap[hash].Add(new Lens { Label = split[0], Focal = int.Parse(split[1]) });
                    }
                    else
                    {
                        Lens? found = hashMap[hash].Where(x => x.Label.Equals(split[0])).FirstOrDefault();
                        if (found is not null)
                        {
                            found.Focal = int.Parse(split[1]);
                        }
                        else
                        {
                            hashMap[hash].Add(new Lens { Label = split[0], Focal = int.Parse(split[1]) });
                        }
                    }
                }
                else if (code.Contains('-'))
                {
                    var split = code.Split('-');
                    var hash = Hash(split[0]);
                    if (hashMap.ContainsKey(hash))
                    {
                        hashMap[hash].RemoveAll(x => x.Label.Equals(split[0]));
                    }
                }
            }

            foreach (var kv in hashMap)
            {
                for (int i = 0; i < kv.Value.Count; i++)
                {
                    Lens? lens = kv.Value[i];
                    result += (Hash(lens.Label) + 1) * (i + 1) * lens.Focal;
                }
            }

            Console.WriteLine($"{className}.2: " + result);
        }
    }
}

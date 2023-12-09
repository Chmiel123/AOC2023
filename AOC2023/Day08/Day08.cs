using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace AOC2023
{
    internal class Day08
    {
        private const string INPUT = "example3.txt";

        public class Location
        {
            public string Current { get; set; }
            public string[] Directions { get; set; }

            public Location(string current)
            {
                Current = current;
                Directions = new string[2];
            }

            public override string ToString()
            {
                return $"Location: {Current} = ({Directions[0]}, {Directions[1]})";
            }
        }

        public static void Solve1()
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
            var lines = File.ReadAllLines($"{className}/{INPUT}");
            int steps = 0;

            string instructions = lines[0];
            Dictionary<string, Location> locations = new Dictionary<string, Location>();

            foreach (var line in lines.Skip(2))
            {
                var split = line.Split(new char[] { '=', '(', ',', ')' });
                var loc = new Location(split[0].Trim());
                loc.Directions[0] = split[2].Trim();
                loc.Directions[1] = split[3].Trim();
                locations[loc.Current] = loc;
            }

            var curr = "AAA";
            var goal = "ZZZ";

            while (curr != goal)
            {
                var instruction = instructions[steps % instructions.Length];

                if (!locations.ContainsKey(curr))
                {
                    throw new Exception("Location not found");
                }
                if (instruction == 'L')
                {
                    curr = locations[curr].Directions[0];
                }
                else
                {
                    curr = locations[curr].Directions[1];
                }
                steps++;
            }

            Console.WriteLine($"{className}.1: " + steps);
        }

        public class Path
        {
            public string Start { get; set; }
            public int StepsToCycle { get; set; }
            public int CycleLength { get; set; }
            public int EndLocation { get; set; }
            public List<int> EndLocations { get; set; }
            public long CurrentStep { get; set; }
            public long CurrentZ { get; set; }

            public Path(string start)
            {
                Start = start;
                CurrentStep = 0;
                EndLocations = new List<int>();
            }
        }

        public static void Solve2_1()
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
            var lines = File.ReadAllLines($"{className}/{INPUT}");

            string instructions = lines[0];
            Dictionary<string, Location> locations = new Dictionary<string, Location>();

            foreach (var line in lines.Skip(2))
            {
                var split = line.Split(new char[] { '=', '(', ',', ')' });
                var loc = new Location(split[0].Trim());
                loc.Directions[0] = split[2].Trim();
                loc.Directions[1] = split[3].Trim();
                locations[loc.Current] = loc;
            }

            List<string> starts = new List<string>();
            foreach (var loc in locations)
            {
                if (loc.Key.EndsWith('A'))
                {
                    starts.Add(loc.Key);
                }
            }

            List<Path> paths = new List<Path>();
            foreach (string start in starts)
            {
                List<Tuple<string, int>> steps = new List<Tuple<string, int>>();
                string loc = start;
                int step = 0;
                Path path = new Path(start);
                paths.Add(path);
                while (true)
                {
                    if (loc.EndsWith('Z'))
                    {
                        path.EndLocations.Add(step);
                        path.EndLocation = step;
                        path.CurrentStep = step;
                    }
                    var tuple = new Tuple<string, int>(loc, step % instructions.Length);
                    if (steps.Contains(tuple))
                    {
                        path.StepsToCycle = steps.IndexOf(tuple);
                        path.CycleLength = step - path.StepsToCycle;
                        Console.WriteLine($"finished walk for {start}, {step} steps, cycle length {path.CycleLength}, end point: {path.EndLocation}");
                        break;
                    }
                    steps.Add(tuple);
                    var instruction = instructions[step % instructions.Length];
                    if (instruction == 'L')
                    {
                        loc = locations[loc].Directions[0];
                    }
                    else
                    {
                        loc = locations[loc].Directions[1];
                    }
                    step++;
                }
            }

            long biggestPath = 0;
            while (true)
            {
                biggestPath = paths.Max(x => x.CurrentStep);
                foreach (var path in paths)
                {
                    while (path.CurrentStep < biggestPath)
                    {
                        path.CurrentStep += path.CycleLength;
                    }
                }
                if (paths.All(x => x.CurrentStep == biggestPath))
                {
                    break;
                }
            }

            Console.WriteLine($"{className}.2: " + biggestPath);
        }

        public static Dictionary<int, int> FindPrimeFactors(int n)
        {
            Dictionary<int, int> result = new Dictionary<int, int>();
            int b;
            for (b = 2; n > 1; b++)
            {
                if (n % b == 0)
                {
                    int x = 0;
                    while (n % b == 0)
                    {
                        n /= b;
                        x++;
                    }
                    result[b] = x;
                }
            }
            return result;
        }

        public static long LCM(List<int> num)
        {
            Dictionary<int, int> commonPrimeFactors = new Dictionary<int, int>();
            foreach (var n in num)
            {
                Console.WriteLine($"number: {n}");
                var factors = FindPrimeFactors(n);
                foreach (var factor in factors)
                {
                    Console.Write($"{factor.Key}^{factor.Value}, ");
                    if (commonPrimeFactors.ContainsKey(factor.Key))
                    {
                        if (commonPrimeFactors[factor.Key] < factor.Value)
                        {
                            commonPrimeFactors[factor.Key] = factor.Value;
                        }
                    }
                    else
                    {
                        commonPrimeFactors[factor.Key] = factor.Value;
                    }
                }
                Console.WriteLine();
            }
            long lcm = 1;
            foreach (var factor in commonPrimeFactors)
            {
                lcm *= (long)Math.Round(Math.Pow(factor.Key, factor.Value));
            }
            return lcm;
        }

        // Returns modulo inverse of a  
        // with respect to m using 
        // extended Euclid Algorithm.  
        // Refer below post for details: 
        // https://www.geeksforgeeks.org/ 
        // multiplicative-inverse-under-modulo-m/ 
        public static BigInteger Inv(BigInteger a, BigInteger m)
        {
            BigInteger m0 = m, t, q;
            BigInteger x0 = 0, x1 = 1;

            if (m == 1)
                return 0;

            // Apply extended Euclid Algorithm 
            while (a > 1)
            {
                // q is quotient 
                q = a / m;

                t = m;

                // m is remainder now, process same as 
                // euclid's algo 
                m = a % m;
                a = t;

                t = x0;

                x0 = x1 - q * x0;

                x1 = t;
            }

            // Make x1 positive 
            if (x1 < 0)
                x1 += m0;

            return x1;
        }

        // k is size of num[] and rem[]. Returns the smallest 
        // number x such that: 
        // x % num[0] = rem[0], 
        // x % num[1] = rem[1], 
        // .................. 
        // x % num[k-2] = rem[k-1] 
        // Assumption: Numbers in num[] are pairwise coprime 
        // (gcd for every pair is 1) 
        public static long FindMinX(List<long> num, List<long> rem)
        {
            // Compute product of all numbers

            //BigInteger prod = LCM(num);
            BigInteger prod = 1;
            for (int i = 0; i < num.Count; i++)
            {
                prod *= num[i];
            }
            // Initialize result
            BigInteger result = 0;

            // Apply above formula
            for (int i = 0; i < num.Count; i++)
            {
                BigInteger pp = prod / num[i];
                result += rem[i] * Inv(pp, num[i]) * pp;
            }

            return (long)(result % prod);
        }

        //TODO: Fast solve
        public static void Solve2_2()
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
            var lines = File.ReadAllLines($"{className}/{INPUT}");

            string instructions = lines[0];
            Dictionary<string, Location> locations = new Dictionary<string, Location>();

            foreach (var line in lines.Skip(2))
            {
                var split = line.Split(new char[] { '=', '(', ',', ')' });
                var loc = new Location(split[0].Trim());
                loc.Directions[0] = split[2].Trim();
                loc.Directions[1] = split[3].Trim();
                locations[loc.Current] = loc;
            }

            List<string> starts = new List<string>();
            foreach (var loc in locations)
            {
                if (loc.Key.EndsWith('A'))
                {
                    starts.Add(loc.Key);
                }
            }

            List<Path> paths = new List<Path>();
            foreach (string start in starts)
            {
                List<Tuple<string, int>> steps = new List<Tuple<string, int>>();
                string loc = start;
                int step = 0;
                Path path = new Path(start);
                paths.Add(path);
                while (true)
                {
                    if (loc.EndsWith('Z'))
                    {
                        if (path.EndLocation == 0)
                        {
                            path.EndLocation = step;
                            path.CurrentStep = step;
                        }
                    }
                    var tuple = new Tuple<string, int>(loc, step % instructions.Length);
                    if (steps.Contains(tuple))
                    {
                        path.StepsToCycle = steps.IndexOf(tuple);
                        path.CycleLength = step - path.StepsToCycle;
                        path.CurrentZ = path.EndLocation - path.StepsToCycle;
                        Console.WriteLine($"finished walk for {start}, {step} steps, steps to cycle: {path.StepsToCycle}, cycle length {path.CycleLength}, end point: {path.EndLocation}");
                        break;
                    }
                    steps.Add(tuple);
                    var instruction = instructions[step % instructions.Length];
                    if (instruction == 'L')
                    {
                        loc = locations[loc].Directions[0];
                    }
                    else
                    {
                        loc = locations[loc].Directions[1];
                    }
                    step++;
                }
            }


            long lcm = LCM(paths.Select(x => x.CycleLength).ToList());
            Console.WriteLine($"LCM: {lcm}");

            long maxStepsToCycle = paths.Max(x => x.StepsToCycle);

            long minX = FindMinX(paths.Select(x => lcm).ToList(), paths.Select(x => (long)x.EndLocation - maxStepsToCycle).ToList());
            long result = maxStepsToCycle + minX;

            Console.WriteLine($"{className}.2: " + result);
        }
    }
}

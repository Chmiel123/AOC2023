using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AOC2023
{
    internal class Day08
    {
        private const string INPUT = "input.txt";

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

            public Path(string start)
            {
                Start = start;
                CurrentStep = 0;
                EndLocations = new List<int>();
            }
        }

        public static void Solve2()
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
                    while (path.CurrentStep < biggestPath) {
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
    }
}

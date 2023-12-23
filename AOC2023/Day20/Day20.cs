using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static AOC2023.Day05;
using static AOC2023.Day10;
using static AOC2023.Day16;

namespace AOC2023
{
    public class Day20
    {
        private static long GCD(long a, long b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            return a | b;
        }

        public enum PulseType
        {
            Low,
            High
        }

        public class Pulse
        {
            public PulseType Type { get; set; }
            public string Sender { get; set; }
            public string Destination { get; set; }

            public Pulse(PulseType type, string sender, string destination)
            {
                Type = type;
                Sender = sender;
                Destination = destination;
            }

            public override string ToString()
            {
                return $"{Sender} -{Type}-> {Destination}";
            }
        }

        public abstract class Module
        {
            public string Name { get; set; }
            public List<string> Destinations { get; set; }

            public Module(string name)
            {
                Name = name;
                Destinations = new List<string>();
            }

            public void AddDestination(string name)
            {
                Destinations.Add(name);
            }

            public void ReadDestinations(string line)
            {
                var dests = line.Split(',');
                foreach (var dest in dests)
                {
                    AddDestination(dest.Trim());
                }
            }

            public abstract List<Pulse> Process(Pulse input);
        }

        public class Broadcast : Module
        {
            public Broadcast(string name) : base(name)
            {
            }

            public override List<Pulse> Process(Pulse input)
            {
                List<Pulse> result = new List<Pulse>();
                foreach (var dest in Destinations)
                {
                    result.Add(new Pulse(input.Type, Name, dest));
                }
                return result;
            }

            public override string ToString()
            {
                return $"broadcast -> {string.Join(", ", Destinations)}";
            }
        }

        public class FlipFlop : Module
        {
            public bool State { get; set; }

            public FlipFlop(string name) : base(name)
            {
                State = false;
            }

            public override List<Pulse> Process(Pulse input)
            {
                List<Pulse> result = new List<Pulse>();
                if (input.Type == PulseType.Low)
                {
                    if (State)
                    {
                        State = false;
                        foreach (var dest in Destinations)
                        {
                            result.Add(new Pulse(PulseType.Low, Name, dest));
                        }
                    }
                    else
                    {
                        State = true;
                        foreach (var dest in Destinations)
                        {
                            result.Add(new Pulse(PulseType.High, Name, dest));
                        }
                    }
                }
                return result;
            }

            public override string ToString()
            {
                return $"%{Name} ({State}) -> {string.Join(", ", Destinations)}";
            }
        }

        public class Conjunction : Module
        {
            public Dictionary<string, bool> Inputs { get; set; }

            public Conjunction(string name) : base(name)
            {
                Inputs = new Dictionary<string, bool>();
            }

            public void AddInput(string moduleName)
            {
                Inputs[moduleName] = false;
            }

            public override List<Pulse> Process(Pulse input)
            {
                List<Pulse> result = new List<Pulse>();
                switch (input.Type)
                {
                    case PulseType.Low:
                        Inputs[input.Sender] = false;
                        break;
                    case PulseType.High:
                        Inputs[input.Sender] = true;
                        break;
                }
                if (Inputs.Values.Any(x => x == false))
                {
                    foreach (var dest in Destinations)
                    {
                        result.Add(new Pulse(PulseType.High, Name, dest));
                    }
                }
                else
                {
                    foreach (var dest in Destinations)
                    {
                        result.Add(new Pulse(PulseType.Low, Name, dest));
                    }
                }
                return result;
            }

            public override string ToString()
            {
                return $"&{Name} ({string.Join(", ", Inputs.Select(x => $"{x.Key}, {x.Value}"))})-> {string.Join(", ", Destinations)}";
            }
        }

        public static void Solve1(string input)
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
            var lines = File.ReadAllLines($"{className}/{input}");
            long result = 0;

            Dictionary<string, Module> modules = new Dictionary<string, Module>();
            Dictionary<string, Conjunction> conjunctions = new Dictionary<string, Conjunction>();

            foreach (var line in lines)
            {
                var split = line.Split('-');
                if (split[0].Trim().Equals("broadcaster"))
                {
                    var module = new Broadcast(split[0].Trim());
                    module.ReadDestinations(split[1].Substring(1));
                    modules[module.Name] = module;
                }
                else if (split[0][0] == '%')
                {
                    var module = new FlipFlop(split[0].Substring(1).Trim());
                    module.ReadDestinations(split[1].Substring(1));
                    modules[module.Name] = module;
                }
                else if (split[0][0] == '&')
                {
                    var module = new Conjunction(split[0].Substring(1).Trim());
                    module.ReadDestinations(split[1].Substring(1));
                    modules[module.Name] = module;
                    conjunctions[module.Name] = module;
                }
            }
            foreach (var module in modules.Values)
            {
                foreach (var dest in module.Destinations)
                {
                    if (conjunctions.ContainsKey(dest))
                    {
                        conjunctions[dest].AddInput(module.Name);
                    }
                }
            }

            long highs = 0;
            long lows = 0;
            Queue<Pulse> pulses = new Queue<Pulse>();

            for (int i = 0; i < 1000; i++)
            {
                pulses.Enqueue(new Pulse(PulseType.Low, "button", "broadcaster"));
                while (pulses.Any())
                {
                    var pulse = pulses.Dequeue();
                    if (pulse.Type == PulseType.Low)
                    {
                        lows++;
                    }
                    else
                    {
                        highs++;
                    }
                    if (modules.ContainsKey(pulse.Destination))
                    {
                        var nextPulses = modules[pulse.Destination].Process(pulse);
                        foreach (var nextPulse in nextPulses)
                        {
                            pulses.Enqueue(nextPulse);
                        }
                    }
                }
            }

            result = highs * lows;

            Console.WriteLine($"{className}.1: " + result);
        }

        public static void Solve2(string input)
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
            var lines = File.ReadAllLines($"{className}/{input}");
            long result = 0;

            Dictionary<string, Module> modules = new Dictionary<string, Module>();
            Dictionary<string, Conjunction> conjunctions = new Dictionary<string, Conjunction>();

            foreach (var line in lines)
            {
                var split = line.Split('-');
                if (split[0].Trim().Equals("broadcaster"))
                {
                    var module = new Broadcast(split[0].Trim());
                    module.ReadDestinations(split[1].Substring(1));
                    modules[module.Name] = module;
                }
                else if (split[0][0] == '%')
                {
                    var module = new FlipFlop(split[0].Substring(1).Trim());
                    module.ReadDestinations(split[1].Substring(1));
                    modules[module.Name] = module;
                }
                else if (split[0][0] == '&')
                {
                    var module = new Conjunction(split[0].Substring(1).Trim());
                    module.ReadDestinations(split[1].Substring(1));
                    modules[module.Name] = module;
                    conjunctions[module.Name] = module;
                }
            }

            Dictionary<string, long> cycles = new Dictionary<string, long>();
            Dictionary<string, long> seen = new Dictionary<string, long>();
            string feed = "";

            foreach (var module in modules.Values)
            {
                foreach (var dest in module.Destinations)
                {
                    if (conjunctions.ContainsKey(dest))
                    {
                        conjunctions[dest].AddInput(module.Name);
                    }
                }
            }

            var con = (Conjunction)modules.Where(x => x.Value.Destinations.First().Equals("rx")).First().Value;
            feed = con.Name;
            foreach (var source in con.Inputs.Keys)
            {
                seen[source] = 0;
            }

            long highs = 0;
            long lows = 0;
            long buttonsPresses = 0;
            Queue<Pulse> pulses = new Queue<Pulse>();
            bool finished = false;

            while (!finished)
            {
                buttonsPresses++;
                highs = 0;
                lows = 0;
                pulses.Enqueue(new Pulse(PulseType.Low, "button", "broadcaster"));
                while (pulses.Any())
                {
                    var pulse = pulses.Dequeue();
                    if (pulse.Destination.Equals(feed) && pulse.Type == PulseType.High)
                    {
                        seen[pulse.Sender]++;

                        if (!cycles.ContainsKey(pulse.Sender))
                        {
                            cycles[pulse.Sender] = buttonsPresses;
                        }
                        else if (buttonsPresses != seen[pulse.Sender] * cycles[pulse.Sender])
                        {
                            throw new Exception($"Failed cycle");
                        }
                        if (seen.Values.All(x => x > 0))
                        {
                            long x = 1;
                            foreach (var cycle in cycles.Values)
                            {
                                x = x * cycle / GCD(x, cycle);
                            }
                            result = x;
                            finished = true;
                            break;
                        }
                    }
                    if (modules.ContainsKey(pulse.Destination))
                    {
                        var nextPulses = modules[pulse.Destination].Process(pulse);
                        foreach (var nextPulse in nextPulses)
                        {
                            pulses.Enqueue(nextPulse);
                        }
                    }
                }
            }

            Console.WriteLine($"{className}.2: " + result);
        }
    }
}

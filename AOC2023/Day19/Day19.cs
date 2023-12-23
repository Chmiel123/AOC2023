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
    public class Day19
    {
        public class Part
        {
            public Dictionary<char, int> Properties = new Dictionary<char, int>();

            public int GetScore()
            {
                return Properties['x'] + Properties['m'] + Properties['a'] + Properties['s'];
            }
        }

        public enum Operator
        {
            Less,
            Greater,
            Ignore
        }

        public class Rule
        {
            public char Property { get; set; }
            public Operator Operator { get; set; }
            public int Operand { get; set; }
            public string TargetWorkflow { get; set; }

            public Rule(char property, Operator @operator, int operand, string targetWorkflow)
            {
                Property = property;
                Operator = @operator;
                Operand = operand;
                TargetWorkflow = targetWorkflow;
            }

            internal bool IsSuccess(Part part)
            {
                switch (Operator)
                {
                    case Operator.Less:
                        if (part.Properties[Property] < Operand)
                        {
                            return true;
                        }
                        break;
                    case Operator.Greater:
                        if (part.Properties[Property] > Operand)
                        {
                            return true;
                        }
                        break;
                    case Operator.Ignore:
                        return true;
                }
                return false;
            }
        }

        public class Workflow
        {
            public string Name { get; set; }
            public List<Rule> Rules { get; set; }

            public Workflow(string name)
            {
                Name = name;
                Rules = new List<Rule>();
            }

            public string Process(Part part)
            {
                foreach (var rule in Rules)
                {
                    if (rule.IsSuccess(part))
                    {
                        return rule.TargetWorkflow;
                    }
                }
                throw new Exception($"No rule fired for workflow {Name}");
            }
        }

        public static void Solve1(string input)
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
            var lines = File.ReadAllLines($"{className}/{input}");
            long result = 0;

            Dictionary<string, Workflow> workflows = new Dictionary<string, Workflow>();
            List<Part> parts = new List<Part>();

            bool readWorkflows = true;

            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    readWorkflows = false;
                    continue;
                }
                if (readWorkflows)
                {
                    var split = line.Split(new char[] { '{', '}' });
                    var workflow = new Workflow(split[0]);
                    foreach (var ruleLine in split[1].Split(','))
                    {
                        if (ruleLine.Contains('>'))
                        {
                            var split1 = ruleLine.Split(new char[] { '>', ':' });
                            var rule = new Rule(split1[0][0], Operator.Greater, int.Parse(split1[1]), split1[2]);
                            workflow.Rules.Add(rule);
                        }
                        else if (ruleLine.Contains('<'))
                        {
                            var split1 = ruleLine.Split(new char[] { '<', ':' });
                            var rule = new Rule(split1[0][0], Operator.Less, int.Parse(split1[1]), split1[2]);
                            workflow.Rules.Add(rule);
                        }
                        else
                        {
                            var rule = new Rule(' ', Operator.Ignore, 0, ruleLine);
                            workflow.Rules.Add(rule);
                        }
                    }
                    workflows[workflow.Name] = workflow;
                }
                else
                {
                    // {x=2127,m=1623,a=2188,s=1013}
                    var split = line.Split(new char[] { '{', '}', '=', ',' });
                    var part = new Part();
                    part.Properties['x'] = int.Parse(split[2]);
                    part.Properties['m'] = int.Parse(split[4]);
                    part.Properties['a'] = int.Parse(split[6]);
                    part.Properties['s'] = int.Parse(split[8]);
                    parts.Add(part);
                }
            }

            foreach (var part in parts)
            {
                Workflow wf = workflows["in"];
                bool cont = true;
                while (cont)
                {
                    string next = wf.Process(part);
                    switch (next)
                    {
                        case "A":
                            result += part.GetScore();
                            cont = false;
                            break;
                        case "R":
                            cont = false;
                            break;
                        default:
                            wf = workflows[next];
                            break;
                    }
                }
            }

            Console.WriteLine($"{className}.1: " + result);
        }

        public class Part2
        {
            public Dictionary<char, int> PropertiesStart = new Dictionary<char, int>();
            public Dictionary<char, int> PropertiesEnd = new Dictionary<char, int>();

            public Part2 Clone()
            {
                Part2 result = new Part2();
                result.PropertiesStart = PropertiesStart.ToDictionary(entry => entry.Key, entry => entry.Value);
                result.PropertiesEnd = PropertiesEnd.ToDictionary(entry => entry.Key, entry => entry.Value);
                return result;
            }

            public long GetScore()
            {
                return ((long)PropertiesEnd['x'] - (long)PropertiesStart['x'] + 1)
                    * ((long)PropertiesEnd['m'] - (long)PropertiesStart['m'] + 1)
                    * ((long)PropertiesEnd['a'] - (long)PropertiesStart['a'] + 1)
                    * ((long)PropertiesEnd['s'] - (long)PropertiesStart['s'] + 1);
            }

            public List<Part2> Split(char prop, int delim)
            {
                var result = new List<Part2>();
                if (delim <= PropertiesStart[prop])
                {
                    result.Add(this);
                    return result;
                }
                if (delim > PropertiesEnd[prop])
                {
                    result.Add(this);
                    return result;
                }
                var p1 = Clone();
                var p2 = Clone();
                p1.PropertiesEnd[prop] = delim - 1;
                p2.PropertiesStart[prop] = delim;
                if (p1.PropertiesStart[prop] > p1.PropertiesEnd[prop]
                    || p2.PropertiesStart[prop] > p2.PropertiesEnd[prop])
                {
                    throw new Exception($"Wrong split");
                }
                result.Add(p1);
                result.Add(p2);
                return result;
            }

            public static long IntersectScore(Part2 p1, Part2 p2)
            {
                long result = 1;
                bool any = true;
                char[] chars = new char[] { 'x', 'm', 'a', 's' };
                foreach (var ch in chars)
                {
                    var min = Math.Max(p1.PropertiesStart[ch], p2.PropertiesStart[ch]);
                    var max = Math.Min(p1.PropertiesEnd[ch], p2.PropertiesEnd[ch]);
                    if (min < max)
                    {
                        result *= max - min + 1;
                    }
                    else
                    {
                        any = false;
                    }
                }
                if (any)
                {
                    return result;
                }
                else
                {
                    return 0;
                }
            }

            public override string ToString()
            {
                return $"x: {PropertiesStart['x']}-{PropertiesEnd['x']}, m: {PropertiesStart['m']}-{PropertiesEnd['m']}, a: {PropertiesStart['a']}-{PropertiesEnd['a']}, s: {PropertiesStart['s']}-{PropertiesEnd['s']}, score: {GetScore()}";
            }
        }

        public class Rule2
        {
            public char Property { get; set; }
            public Operator Operator { get; set; }
            public int Operand { get; set; }
            public string TargetWorkflow { get; set; }

            public Rule2(char property, Operator @operator, int operand, string targetWorkflow)
            {
                Property = property;
                Operator = @operator;
                Operand = operand;
                TargetWorkflow = targetWorkflow;
            }

            internal List<(Part2, bool)> Process(Part2 part)
            {
                var result = new List<(Part2, bool)>();
                List<Part2> ps;
                switch (Operator)
                {
                    case Operator.Less:
                        ps = part.Split(Property, Operand);
                        if (ps.Count == 2)
                        {
                            result.Add((ps[0], true));
                            result.Add((ps[1], false));
                        }
                        else if (ps[0].PropertiesStart[Property] < Operand)
                        {
                            result.Add((ps[0], true));
                        }
                        else
                        {
                            result.Add((ps[0], false));
                        }
                        return result;
                    case Operator.Greater:
                        ps = part.Split(Property, Operand + 1);
                        if (ps.Count == 2)
                        {
                            result.Add((ps[0], false));
                            result.Add((ps[1], true));
                        }
                        else if (ps[0].PropertiesStart[Property] > Operand)
                        {
                            result.Add((ps[0], true));
                        }
                        else
                        {
                            result.Add((ps[0], false));
                        }
                        return result;
                    case Operator.Ignore:
                        result.Add((part, true));
                        return result;
                }
                throw new Exception($"Rule missfire");
            }
        }

        public class Workflow2
        {
            public string Name { get; set; }
            public List<Rule2> Rules { get; set; }

            public Workflow2(string name)
            {
                Name = name;
                Rules = new List<Rule2>();
            }

            public List<(Part2, string)> Process(Part2 part)
            {
                List<(Part2, string)> result = new List<(Part2, string)>();
                List<Part2> parts = new List<Part2>();
                List<Part2> nextParts = new List<Part2>();
                parts.Add(part);
                foreach (var rule in Rules)
                {
                    foreach (var p in parts)
                    {
                        var next = rule.Process(p);
                        foreach (var item in next)
                        {
                            if (item.Item2)
                            {
                                result.Add((item.Item1, rule.TargetWorkflow));
                            }
                            else
                            {
                                nextParts.Add(item.Item1);
                            }
                        }
                    }
                    parts = nextParts;
                    nextParts = new List<Part2>();
                }
                return result;
            }
        }

        public static void Solve2(string input)
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
            var lines = File.ReadAllLines($"{className}/{input}");
            long result = 0;

            Dictionary<string, Workflow2> workflows = new Dictionary<string, Workflow2>();

            bool readWorkflows = true;

            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    readWorkflows = false;
                    continue;
                }
                if (readWorkflows)
                {
                    var split = line.Split(new char[] { '{', '}' });
                    var workflow = new Workflow2(split[0]);
                    foreach (var ruleLine in split[1].Split(','))
                    {
                        if (ruleLine.Contains('>'))
                        {
                            var split1 = ruleLine.Split(new char[] { '>', ':' });
                            var rule = new Rule2(split1[0][0], Operator.Greater, int.Parse(split1[1]), split1[2]);
                            workflow.Rules.Add(rule);
                        }
                        else if (ruleLine.Contains('<'))
                        {
                            var split1 = ruleLine.Split(new char[] { '<', ':' });
                            var rule = new Rule2(split1[0][0], Operator.Less, int.Parse(split1[1]), split1[2]);
                            workflow.Rules.Add(rule);
                        }
                        else
                        {
                            var rule = new Rule2(' ', Operator.Ignore, 0, ruleLine);
                            workflow.Rules.Add(rule);
                        }
                    }
                    workflows[workflow.Name] = workflow;
                }
                else
                {
                    break;
                }
            }

            //Add parts2
            Queue<(Part2, string)> parts = new Queue<(Part2, string)>();

            Part2 p = new Part2();
            p.PropertiesStart['x'] = 1;
            p.PropertiesEnd['x'] = 4000;
            p.PropertiesStart['m'] = 1;
            p.PropertiesEnd['m'] = 4000;
            p.PropertiesStart['a'] = 1;
            p.PropertiesEnd['a'] = 4000;
            p.PropertiesStart['s'] = 1;
            p.PropertiesEnd['s'] = 4000;
            parts.Enqueue((p, "in"));

            List<Part2> accepted = new List<Part2>();

            while (parts.Any())
            {
                var part = parts.Dequeue();
                Workflow2 wf = workflows[part.Item2];
                var next = wf.Process(part.Item1);
                foreach (var n in next)
                {
                    switch (n.Item2)
                    {
                        case "A":
                            result += n.Item1.GetScore();
                            accepted.Add(n.Item1);
                            //Console.WriteLine($"{accepted.IndexOf(n.Item1)}: {n.Item1}");
                            break;
                        case "R":
                            break;
                        default:
                            parts.Enqueue((n.Item1, n.Item2));
                            break;
                    }
                }
            }

            Console.WriteLine($"{className}.2: " + result);
        }
    }
}

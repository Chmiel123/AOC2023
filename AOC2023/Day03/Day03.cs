using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AOC2023
{
    internal class Day03
    {
        public static bool IsDigit(char c)
        {
            return (c >= '0' && c <= '9');
        }

        public static void Solve1()
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
            var lines = File.ReadAllLines($"{className}/input.txt");
            int result = 0;

            List<List<char>> chars = new List<List<char>>();

            foreach (var line in lines)
            {
                List<char> row = new List<char>();
                chars.Add(row);
                foreach (var c in line)
                {
                    row.Add(c);
                }
                row.Add('.');
            }
            for (int i = 0; i < chars.Count; i++)
            {
                var row = chars[i];
                int? numberBegin = null;
                for (int j = 0; j < row.Count; j++)
                {
                    if (!IsDigit(row[j]))
                    {
                        if (numberBegin is not null)
                        {
                            string numberString = "";
                            for (int k = numberBegin.Value; k < j; k++)
                            {
                                numberString += row[k];
                            }
                            int number = int.Parse(numberString);
                            bool found = false;
                            for (int k = numberBegin.Value - 1; k < j + 1 && !found; k++)
                            {
                                if (k < 0 || k >= row.Count)
                                {
                                    continue;
                                }
                                for (int l = i - 1; l <= i + 1 && !found; l++)
                                {
                                    if (l < 0 || l >= chars.Count)
                                    {
                                        continue;
                                    }
                                    if (l == i && (k >= numberBegin.Value && k < j))
                                    {
                                        continue;
                                    }
                                    if (chars[l][k] != '.' && !IsDigit(chars[l][k]))
                                    {
                                        //found adjecent part
                                        result += number;
                                        found = true;
                                    }
                                }
                            }
                        }
                        numberBegin = null;
                    }
                    else
                    {
                        if (numberBegin is null)
                        {
                            numberBegin = j;
                        }
                    }
                }
            }

            Console.WriteLine($"{className}.1: " + result);
        }

        public class Gear
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Gear(int x, int y)
            {
                X = x;
                Y = y;
            }

            public override bool Equals(object? obj)
            {
                return obj is Gear gear &&
                       X == gear.X &&
                       Y == gear.Y;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(X, Y);
            }
        }

        public class GearValue
        {
            public int Count { get; set; }
            public int Value { get; set; }

            public GearValue()
            {
                Count = 0;
                Value = 1;
            }
        }

        public static void Solve2()
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
            var lines = File.ReadAllLines($"{className}/input.txt");
            int result = 0;

            List<List<char>> chars = new List<List<char>>();
            Dictionary<Gear, GearValue> gears = new Dictionary<Gear, GearValue>();

            foreach (var line in lines)
            {
                List<char> row = new List<char>();
                chars.Add(row);
                foreach (var c in line)
                {
                    row.Add(c);
                }
                row.Add('.');
            }
            for (int i = 0; i < chars.Count; i++)
            {
                var row = chars[i];
                int? numberBegin = null;
                for (int j = 0; j < row.Count; j++)
                {
                    if (!IsDigit(row[j]))
                    {
                        if (numberBegin is not null)
                        {
                            string numberString = "";
                            for (int k = numberBegin.Value; k < j; k++)
                            {
                                numberString += row[k];
                            }
                            int number = int.Parse(numberString);
                            for (int k = numberBegin.Value - 1; k < j + 1; k++)
                            {
                                if (k < 0 || k >= row.Count)
                                {
                                    continue;
                                }
                                for (int l = i - 1; l <= i + 1; l++)
                                {
                                    if (l < 0 || l >= chars.Count)
                                    {
                                        continue;
                                    }
                                    if (l == i && (k >= numberBegin.Value && k < j))
                                    {
                                        continue;
                                    }
                                    if (chars[l][k] == '*')
                                    {
                                        //found adjecent gear
                                        Gear gear = new Gear(l, k);
                                        if (!gears.ContainsKey(gear))
                                        {
                                            gears.Add(gear, new GearValue());
                                        }
                                        gears[gear].Count++;
                                        gears[gear].Value *= number;
                                    }
                                }
                            }
                        }
                        numberBegin = null;
                    }
                    else
                    {
                        if (numberBegin is null)
                        {
                            numberBegin = j;
                        }
                    }
                }
            }
            foreach (var gear in gears)
            {
                if (gear.Value.Count == 2)
                {
                    result += gear.Value.Value;
                }
            }
            Console.WriteLine($"{className}.2: " + result);
        }
    }
}

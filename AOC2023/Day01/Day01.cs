using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AOC2023
{
    internal class Day01
    {
        public static void Solve1()
        {
            var lines = File.ReadAllLines("Day01/input.txt");
            int result = 0;

            foreach (var line in lines)
            {
                int? first = null, last = null;
                foreach (var c in line)
                {
                    if (c >= '0' && c <= '9')
                    {
                        if (first is null)
                        {
                            first = int.Parse(c.ToString());
                        }
                        last = int.Parse(c.ToString());
                    }
                }
                if (first is not null && last is not null)
                {
                    //Console.WriteLine($"{line}: first: {first}, last: {last}");
                    result += 10 * first.Value + last.Value;
                }
            }

            Console.WriteLine("Day01.1: " + result);
        }

        internal class OccurencePointer
        {
            public string Text { get; private set; }
            public int Value { get; private set; }
            public List<int> Pointers { get; set; }

            public OccurencePointer(string text, int value)
            {
                Text = text;
                Value = value;
            }
        }

        public static void Solve2()
        {
            var lines = File.ReadAllLines("Day01/input.txt");
            List<OccurencePointer> pointers = new List<OccurencePointer>
            {
                new OccurencePointer("one", 1),
                new OccurencePointer("two", 2),
                new OccurencePointer("three", 3),
                new OccurencePointer("four", 4),
                new OccurencePointer("five", 5),
                new OccurencePointer("six", 6),
                new OccurencePointer("seven", 7),
                new OccurencePointer("eight", 8),
                new OccurencePointer("nine", 9)
            };
            int result = 0;

            foreach (var line in lines)
            {
                int? first = null, last = null;
                foreach (var pointer in pointers)
                {
                    pointer.Pointers = new List<int>();
                }
                foreach (var c in line)
                {
                    char cc = char.ToLower(c);
                    if (cc >= '0' && cc <= '9')
                    {
                        if (first is null)
                        {
                            first = int.Parse(cc.ToString());
                        }
                        last = int.Parse(cc.ToString());
                    }
                    if (cc >= 'a' && cc <= 'z')
                    {
                        foreach (var pointer in pointers)
                        {
                            for (int i = pointer.Pointers.Count - 1; i >= 0; i--)
                            {
                                if (cc == pointer.Text[pointer.Pointers[i]])
                                {
                                    pointer.Pointers[i]++;
                                    if (pointer.Pointers[i] == pointer.Text.Length)
                                    {
                                        if (first is null)
                                        {
                                            first = pointer.Value;
                                        }
                                        last = pointer.Value;
                                        pointer.Pointers.RemoveAt(i);
                                    }
                                }
                                else
                                {
                                    pointer.Pointers.RemoveAt(i);
                                }
                            }
                            if (cc == pointer.Text[0])
                            {
                                pointer.Pointers.Add(1);
                            }
                        }
                    }
                }
                if (first is not null && last is not null)
                {
                    //Console.WriteLine($"{line}: first: {first}, last: {last}");
                    result += 10 * first.Value + last.Value;
                }
            }

            Console.WriteLine("Day01.2: " + result);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AOC2023
{
    internal class Day02
    {
        public static void Solve1()
        {
            var lines = File.ReadAllLines("Day02/input.txt");
            int result = 0;

            Dictionary<string, int> total = new Dictionary<string, int>
            {
                { "red", 12 },
                { "green", 13 },
                { "blue", 14 }
            };
            Dictionary<string, int> current;

            foreach (var line in lines)
            {
                var headerData = line.Split(':');
                int id = int.Parse(headerData[0].Split(' ')[1]);
                var games = headerData[1].Split(';');
                bool possible = true;
                foreach (var game in games)
                {
                    current = new Dictionary<string, int>
                    {
                        { "red", 0 },
                        { "green", 0 },
                        { "blue", 0 }
                    };
                    var cubes = game.Split(',');
                    foreach (var cube in cubes)
                    {
                        var split = cube.Trim().Split(' ');
                        int number = int.Parse(split[0]);
                        current[split[1]] += number;
                    }
                    foreach (var key in current.Keys)
                    {
                        if (current[key] > total[key])
                        {
                            possible = false;
                        }
                    }
                }
                if (possible)
                {
                    result += id;
                }
            }

            Console.WriteLine("Day02.1: " + result);
        }

        public static void Solve2()
        {
            var lines = File.ReadAllLines("Day02/input.txt");
            int result = 0;

            Dictionary<string, int> current;

            foreach (var line in lines)
            {
                current = new Dictionary<string, int>
                {
                    { "red", 0 },
                    { "green", 0 },
                    { "blue", 0 }
                };
                var headerData = line.Split(':');
                int id = int.Parse(headerData[0].Split(' ')[1]);
                var games = headerData[1].Split(';');
                foreach (var game in games)
                {
                    var cubes = game.Split(',');
                    foreach (var cube in cubes)
                    {
                        var split = cube.Trim().Split(' ');
                        int number = int.Parse(split[0]);
                        if (current[split[1]] < number)
                        {
                            current[split[1]] = number;
                        }
                    }
                }
                int gameResult = 1;
                //Console.WriteLine($"game {id}: red {current["red"]}, green {current["green"]}, blue {current["blue"]}");
                foreach (var key in current.Keys)
                {
                    gameResult *= current[key];
                }
                //Console.WriteLine($"game {id}: {gameResult}");
                result += gameResult;
            }

            Console.WriteLine("Day02.2: " + result);
        }
    }
}

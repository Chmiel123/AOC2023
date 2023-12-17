using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static AOC2023.Day05;
using static AOC2023.Day16;

namespace AOC2023
{
    internal class Day16
    {
        public enum Direction
        {
            Up = 0b0001,
            Down = 0b0010,
            Right = 0b0100,
            Left = 0b1000
        }

        public class Map
        {
            public List<List<char>> Chars { get; set; }
            public List<List<int>> Beams { get; set; }

            public Map()
            {
                Chars = new List<List<char>>();
                Beams = new List<List<int>>();
            }

            public void AddLine(string line)
            {
                Chars.Add(line.ToList());
                Beams.Add(line.Select(x => 0).ToList());
            }

            public void ResetBeams()
            {
                for (int i = 0; i < Beams.Count; i++)
                {
                    for (int j = 0; j < Beams[i].Count; j++)
                    {
                        Beams[i][j] = 0; ;
                    }
                }
            }

            public bool BeamExists(int x, int y, Direction direction)
            {
                return (Beams[x][y] & (int)direction) > 0;
            }

            public void AddBeam(int x, int y, Direction direction)
            {
                Beams[x][y] |= (int)direction;
            }

            public static (int, int) NextTile(int x, int y, Direction direction)
            {
                return direction switch
                {
                    Direction.Up => (x - 1, y),
                    Direction.Down => (x + 1, y),
                    Direction.Right => (x, y + 1),
                    Direction.Left => (x, y - 1),
                    _ => throw new Exception($"Unknown direction: {direction}"),
                };
            }

            public void PropagateBeam(int x, int y, Direction direction)
            {
                PropagateBeam(new List<(int, int, Direction)>() { (x, y, direction) });
            }

            public void AddNextTile(int x, int y, Direction direction, List<(int, int, Direction)> list)
            {
                AddBeam(x, y, direction);
                var nextTile = NextTile(x, y, direction);
                list.Add((nextTile.Item1, nextTile.Item2, direction));
            }

            public void PropagateBeam(List<(int, int, Direction)> list)
            {
                while (list.Any())
                {
                    var current = list.First();
                    list = list.Skip(1).ToList();
                    int x = current.Item1;
                    int y = current.Item2;
                    Direction direction = current.Item3;

                    if (x < 0 || x >= Chars.Count || y < 0 || y >= Chars[0].Count)
                    {
                        continue;
                    }
                    Direction nextDir;
                    switch (Chars[x][y])
                    {
                        case '.':
                            if (BeamExists(x, y, direction))
                            {
                                continue;
                            }
                            AddNextTile(x, y, direction, list);
                            break;
                        case '/':
                            switch (direction)
                            {
                                case Direction.Up:
                                    nextDir = Direction.Right;
                                    break;
                                case Direction.Down:
                                    nextDir = Direction.Left;
                                    break;
                                case Direction.Right:
                                    nextDir = Direction.Up;
                                    break;
                                case Direction.Left:
                                    nextDir = Direction.Down;
                                    break;
                                default:
                                    throw new Exception($"Unknown direction: {direction}");
                            }
                            if (BeamExists(x, y, nextDir))
                            {
                                continue;
                            }
                            AddNextTile(x, y, nextDir, list);
                            break;
                        case '\\':
                            switch (direction)
                            {
                                case Direction.Up:
                                    nextDir = Direction.Left;
                                    break;
                                case Direction.Down:
                                    nextDir = Direction.Right;
                                    break;
                                case Direction.Right:
                                    nextDir = Direction.Down;
                                    break;
                                case Direction.Left:
                                    nextDir = Direction.Up;
                                    break;
                                default:
                                    throw new Exception($"Unknown direction: {direction}");
                            }
                            if (BeamExists(x, y, nextDir))
                            {
                                continue;
                            }
                            AddNextTile(x, y, nextDir, list);
                            break;
                        case '|':
                            switch (direction)
                            {
                                case Direction.Up:
                                case Direction.Down:
                                    if (BeamExists(x, y, direction))
                                    {
                                        continue;
                                    }
                                    AddNextTile(x, y, direction, list);
                                    break;
                                case Direction.Right:
                                case Direction.Left:
                                    if (BeamExists(x, y, Direction.Up) && BeamExists(x, y, Direction.Down))
                                        continue;
                                    AddNextTile(x, y, Direction.Up, list);
                                    AddNextTile(x, y, Direction.Down, list);
                                    break;
                                default:
                                    throw new Exception($"Unknown direction: {direction}");
                            }
                            break;
                        case '-':
                            switch (direction)
                            {
                                case Direction.Up:
                                case Direction.Down:
                                    if (BeamExists(x, y, Direction.Right) && BeamExists(x, y, Direction.Left))
                                        continue;
                                    AddNextTile(x, y, Direction.Right, list);
                                    AddNextTile(x, y, Direction.Left, list);
                                    break;
                                case Direction.Right:
                                case Direction.Left:
                                    if (BeamExists(x, y, direction))
                                    {
                                        continue;
                                    }
                                    AddNextTile(x, y, direction, list);
                                    break;
                                default:
                                    throw new Exception($"Unknown direction: {direction}");
                            }
                            break;
                    }
                }
            }

            public int CountEnergized()
            {
                int result = 0;
                foreach (var line in Beams)
                {
                    foreach (var tile in line)
                    {
                        if (tile > 0)
                        {
                            result++;
                        }
                    }
                }
                return result;
            }
        }

        public static void Solve1(string input)
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
            var lines = File.ReadAllLines($"{className}/{input}");
            long result = 0;

            Map map = new Map();

            foreach (var line in lines)
            {
                map.AddLine(line);
            }

            map.PropagateBeam(0, 0, Direction.Right);

            /*
            for (int i = 0; i < map.Beams.Count; i++)
            {
                List<int>? line = map.Beams[i];
                for (int j = 0; j < line.Count; j++)
                {
                    int b = line[j];
                    Console.Write($"({i},{j})");
                    if (map.Chars[i][j] == '.')
                    {
                        switch (b)
                        {
                            case (int)Direction.Up:
                                Console.Write('^');
                                break;
                            case (int)Direction.Down:
                                Console.Write('v');
                                break;
                            case (int)Direction.Right:
                                Console.Write('>');
                                break;
                            case (int)Direction.Left:
                                Console.Write('<');
                                break;
                            default:
                                if (b > 0)
                                {
                                    Console.Write('#');
                                }
                                else
                                {
                                    Console.Write(map.Chars[i][j]);
                                }
                                break;
                        }
                    }
                    else
                    {
                        Console.Write(map.Chars[i][j]);
                    }
                }
                Console.WriteLine();
            }
            */

            result = map.CountEnergized();

            Console.WriteLine($"{className}.1: " + result);
        }

        public static void Solve2(string input)
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
            var lines = File.ReadAllLines($"{className}/{input}");
            long result = 0;

            Map map = new Map();

            foreach (var line in lines)
            {
                map.AddLine(line);
            }

            for (int i = 0; i < map.Chars.Count; i++)
            {
                map.ResetBeams();
                map.PropagateBeam(i, 0, Direction.Right);
                result = Math.Max(result, map.CountEnergized());
                map.ResetBeams();
                map.PropagateBeam(i, map.Chars[i].Count - 1, Direction.Left);
                result = Math.Max(result, map.CountEnergized());
            }
            for (int j = 0; j < map.Chars[0].Count; j++)
            {
                map.ResetBeams();
                map.PropagateBeam(0, j, Direction.Down);
                result = Math.Max(result, map.CountEnergized());
                map.ResetBeams();
                map.PropagateBeam(map.Chars.Count - 1, j, Direction.Up);
                result = Math.Max(result, map.CountEnergized());
            }

            Console.WriteLine($"{className}.2: " + result);
        }
    }
}

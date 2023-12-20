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
using static AOC2023.Day16;

namespace AOC2023
{
    internal class Day17
    {
        public const int MAX_LENGTH = 1;

        public enum Dir
        {
            Up,
            Down,
            Left,
            Right,
            None
        }

        public static bool IsReversed(Dir dir1, Dir dir2)
        {
            return dir1 == Dir.Up && dir2 == Dir.Down
                || dir1 == Dir.Down && dir2 == Dir.Up
                || dir1 == Dir.Left && dir2 == Dir.Right
                || dir1 == Dir.Right && dir2 == Dir.Left;
        }

        public class Coord
        {
            public int R { get; set; }
            public int C { get; set; }

            public Coord(int r, int c)
            {
                R = r;
                C = c;
            }

            public override bool Equals(object? obj)
            {
                if (obj == null) return false;
                if (obj == this) return true;
                Coord other = (Coord)obj;
                return R == other.R && C == other.C;
            }

            public override int GetHashCode()
            {
                return R << 8 ^ C;
            }

            public override string ToString()
            {
                return $"({R},{C})";
            }
        }

        public class CoordDir
        {
            public Coord Coord { get; set; }
            public Dir Dir { get; set; }
            public int Length { get; set; }

            public CoordDir(Coord coord, Dir dir, int length)
            {
                Coord = coord;
                Dir = dir;
                Length = length;
            }

            public override bool Equals(object? obj)
            {
                if (obj == null) return false;
                if (obj == this) return true;
                CoordDir other = (CoordDir)obj;
                return Coord.Equals(other.Coord) && Dir == other.Dir && Length == other.Length;
            }

            public override int GetHashCode()
            {
                return Coord.GetHashCode() ^ (int)Dir << 16 ^ Length << 18;
            }

            public override string ToString()
            {
                return $"({Coord},{Dir},{Length})";
            }
        }

        public class CoordDirHeat
        {
            public CoordDir CoordDir { get; set; }
            public int Heat { get; set; }

            public CoordDirHeat(CoordDir coordDir, int heat)
            {
                CoordDir = coordDir;
                Heat = heat;
            }

            public override bool Equals(object? obj)
            {
                if (obj == null) return false;
                if (obj == this) return true;
                CoordDirHeat other = (CoordDirHeat)obj;
                return CoordDir.Equals(other.CoordDir) && Heat == other.Heat;
            }

            public override int GetHashCode()
            {
                return CoordDir.GetHashCode() ^ Heat << 24;
            }

            public override string ToString()
            {
                return $"({CoordDir},{Heat})";
            }
        }

        public class Map
        {
            public readonly List<(Dir, Coord)> DIRECTIONS = new()
            {
                (Dir.Up, new Coord(-1, 0)),
                (Dir.Down, new Coord(1, 0)),
                (Dir.Left, new Coord(0, -1)),
                (Dir.Right, new Coord(0, 1))
            };

            public List<List<int>> Heats { get; set; }

            public Map()
            {
                Heats = new List<List<int>>();
            }

            public int GetHeat(Coord coord)
            {
                return Heats[coord.R][coord.C];
            }

            public static int H(Coord n, Coord end)
            {
                return Math.Abs(end.R - n.R) + Math.Abs(end.C - n.C);
            }

            public int RouteCost(List<Coord> path)
            {
                int result = 0;
                foreach (var coord in path)
                {
                    if (!(coord.R == 0 && coord.C == 0))
                    {
                        result += Heats[coord.R][coord.C];
                    }
                }
                return result;
            }

            public int FindRoute(Coord start, Coord end, int min, int max)
            {
                var startCoordDir = new CoordDir(start, Dir.None, 0);
                var startCoordDirHeat = new CoordDirHeat(startCoordDir, 0);
                PriorityQueue<CoordDirHeat, int> openSet = new();
                openSet.Enqueue(startCoordDirHeat, 0);
                HashSet<CoordDir> seen = new HashSet<CoordDir>();

                CoordDirHeat current;
                int priority;

                while (openSet.TryDequeue(out current, out priority))
                {
                    if (current.CoordDir.Coord.Equals(end) && current.CoordDir.Length >= min - 1)
                    {
                        Console.WriteLine($"seen count: {seen.Count}");
                        Console.WriteLine($"queue count: {openSet.Count}");
                        return priority;
                    }

                    if (seen.Contains(current.CoordDir))
                    {
                        continue;
                    }
                    seen.Add(current.CoordDir);

                    foreach (var dir in DIRECTIONS)
                    {
                        if (IsReversed(dir.Item1, current.CoordDir.Dir))
                        {
                            continue;
                        }
                        if (dir.Item1 == current.CoordDir.Dir && current.CoordDir.Length >= max - 1)
                        {
                            continue;
                        }
                        if (dir.Item1 != current.CoordDir.Dir && current.CoordDir.Length < min - 1 && current.CoordDir.Dir != Dir.None)
                        {
                            continue;
                        }
                        int l = 0;
                        if (dir.Item1 == current.CoordDir.Dir)
                        {
                            l = current.CoordDir.Length + 1;
                        }
                        var nextCoord = new Coord(current.CoordDir.Coord.R + dir.Item2.R, current.CoordDir.Coord.C + dir.Item2.C);
                        if (nextCoord.R < 0 || nextCoord.R >= Heats.Count || nextCoord.C < 0 || nextCoord.C >= Heats[0].Count)
                        {
                            continue;
                        }
                        var neighbor = new CoordDirHeat(new CoordDir(nextCoord, dir.Item1, l), priority + GetHeat(nextCoord));
                        openSet.Enqueue(neighbor, priority + GetHeat(nextCoord));
                    }
                }
                throw new Exception($"No route found");
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
                map.Heats.Add(line.ToList().Select(x => int.Parse(new string(x, 1))).ToList());
            }

            result = map.FindRoute(new Coord(0, 0), new Coord(map.Heats.Count - 1, map.Heats[0].Count - 1), 0, 3);

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
                map.Heats.Add(line.ToList().Select(x => int.Parse(new string(x, 1))).ToList());
            }

            result = map.FindRoute(new Coord(0, 0), new Coord(map.Heats.Count - 1, map.Heats[0].Count - 1), 4, 10);
            
            Console.WriteLine($"{className}.2: " + result);
        }
    }
}

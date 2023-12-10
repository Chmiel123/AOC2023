using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AOC2023
{
    internal class Day10
    {
        private const string INPUT = "input.txt";

        public enum Pipe
        {
            Vertical,
            Horizontal,
            NorthEast,
            NorthWest,
            SouthEast,
            SouthWest,
            Ground,
            Start
        }

        public static class PipeExtensions
        {
            public static Pipe GetPipe(char c)
            {
                switch (c)
                {
                    case '|':
                        return Pipe.Vertical;
                    case '-':
                        return Pipe.Horizontal;
                    case 'L':
                        return Pipe.NorthEast;
                    case 'J':
                        return Pipe.NorthWest;
                    case '7':
                        return Pipe.SouthWest;
                    case 'F':
                        return Pipe.SouthEast;
                    case '.':
                        return Pipe.Ground;
                    case 'S':
                        return Pipe.Start;
                    default:
                        throw new Exception($"Unknown pipe: {c}");
                }
            }
        }

        public static void Solve1()
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
            var lines = File.ReadAllLines($"{className}/{INPUT}");
            long result = 0;

            Pipe[,] pipes = new Pipe[lines.Length, lines[0].Length];
            int startX = 0, startY = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                for (int j = 0; j < line.Length; j++)
                {
                    char c = line[j];
                    pipes[i, j] = PipeExtensions.GetPipe(c);
                    if (c == 'S')
                    {
                        startX = i;
                        startY = j;
                    }
                }
            }

            bool north = false, south = false, east = false, west = false;
            int x = -1, y = -1;
            if (startX > 0)
            {
                Pipe up = pipes[startX - 1, startY];
                north = (up == Pipe.Vertical || up == Pipe.SouthEast || up == Pipe.SouthWest);
                if (north)
                {
                    x = startX - 1;
                    y = startY;
                }
            }
            if (startX < pipes.GetLength(0) - 1)
            {
                Pipe down = pipes[startX + 1, startY];
                south = (down == Pipe.Vertical || down == Pipe.NorthWest || down == Pipe.NorthEast);
                if (south)
                {
                    x = startX + 1;
                    y = startY;
                    north = false;
                }
            }
            if (startY > 0)
            {
                Pipe left = pipes[startX, startY - 1];
                west = (left == Pipe.Horizontal || left == Pipe.NorthEast || left == Pipe.SouthEast);
                if (west)
                {
                    x = startX;
                    y = startY - 1;
                    north = false;
                    south = false;
                }
            }
            if (startY < pipes.GetLength(1) - 1)
            {
                Pipe right = pipes[startX, startY + 1];
                east = (right == Pipe.Horizontal || right == Pipe.NorthWest || right == Pipe.SouthWest);
                if (east)
                {
                    x = startX;
                    y = startY + 1;
                    west = false;
                    north = false;
                    south = false;
                }
            }

            int distance = 0;
            while (x != startX || y != startY)
            {
                distance++;
                Pipe p = pipes[x, y];
                switch (p)
                {
                    case Pipe.Vertical:
                        if (north)
                        {
                            x--;
                        }
                        else
                        {
                            x++;
                        }
                        break;
                    case Pipe.Horizontal:
                        if (west)
                        {
                            y--;
                        }
                        else
                        {
                            y++;
                        }
                        break;
                    case Pipe.NorthEast:
                        if (south)
                        {
                            y++;
                            south = false;
                            east = true;
                        }
                        else
                        {
                            x--;
                            west = false;
                            north = true;
                        }
                        break;
                    case Pipe.NorthWest:
                        if (south)
                        {
                            y--;
                            south = false;
                            west = true;
                        }
                        else
                        {
                            x--;
                            east = false;
                            north = true;
                        }
                        break;
                    case Pipe.SouthEast:
                        if (north)
                        {
                            y++;
                            north = false;
                            east = true;
                        }
                        else
                        {
                            x++;
                            west = false;
                            south = true;
                        }
                        break;
                    case Pipe.SouthWest:
                        if (north)
                        {
                            y--;
                            north = false;
                            west = true;
                        }
                        else
                        {
                            x++;
                            east = false;
                            south = true;
                        }
                        break;
                    case Pipe.Ground:
                        throw new Exception($"Walk on ground x: {x}, y: {y}");
                        break;
                    case Pipe.Start:
                        throw new Exception($"Walk on start x: {x}, y: {y}");
                        break;
                    default:
                        break;
                }
            }

            result = (distance + 1) / 2;

            Console.WriteLine($"{className}.1: " + result);
        }

        public static void Solve2()
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
            var lines = File.ReadAllLines($"{className}/{INPUT}");
            long result = 0;

            Pipe[,] pipes = new Pipe[lines.Length, lines[0].Length];
            int startX = 0, startY = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                for (int j = 0; j < line.Length; j++)
                {
                    char c = line[j];
                    pipes[i, j] = PipeExtensions.GetPipe(c);
                    if (c == 'S')
                    {
                        startX = i;
                        startY = j;
                    }
                }
            }

            bool north = false, south = false, east = false, west = false;
            int x = -1, y = -1;
            if (startX > 0)
            {
                Pipe up = pipes[startX - 1, startY];
                north = (up == Pipe.Vertical || up == Pipe.SouthEast || up == Pipe.SouthWest);
                if (north)
                {
                    x = startX - 1;
                    y = startY;
                }
            }
            if (startX < pipes.GetLength(0) - 1)
            {
                Pipe down = pipes[startX + 1, startY];
                south = (down == Pipe.Vertical || down == Pipe.NorthWest || down == Pipe.NorthEast);
                if (south)
                {
                    x = startX + 1;
                    y = startY;
                    north = false;
                }
            }
            if (startY > 0)
            {
                Pipe left = pipes[startX, startY - 1];
                west = (left == Pipe.Horizontal || left == Pipe.NorthEast || left == Pipe.SouthEast);
                if (west)
                {
                    x = startX;
                    y = startY - 1;
                    north = false;
                    south = false;
                }
            }
            if (startY < pipes.GetLength(1) - 1)
            {
                Pipe right = pipes[startX, startY + 1];
                east = (right == Pipe.Horizontal || right == Pipe.NorthWest || right == Pipe.SouthWest);
                if (east)
                {
                    x = startX;
                    y = startY + 1;
                    west = false;
                    north = false;
                    south = false;
                }
            }

            bool[,] belongToLoop = new bool[pipes.GetLength(0), pipes.GetLength(1)];
            belongToLoop[startX, startY] = true;

            int distance = 0;
            while (x != startX || y != startY)
            {
                distance++;
                Pipe p = pipes[x, y];
                belongToLoop[x, y] = true;
                switch (p)
                {
                    case Pipe.Vertical:
                        if (north)
                        {
                            x--;
                        }
                        else
                        {
                            x++;
                        }
                        break;
                    case Pipe.Horizontal:
                        if (west)
                        {
                            y--;
                        }
                        else
                        {
                            y++;
                        }
                        break;
                    case Pipe.NorthEast:
                        if (south)
                        {
                            y++;
                            south = false;
                            east = true;
                        }
                        else
                        {
                            x--;
                            west = false;
                            north = true;
                        }
                        break;
                    case Pipe.NorthWest:
                        if (south)
                        {
                            y--;
                            south = false;
                            west = true;
                        }
                        else
                        {
                            x--;
                            east = false;
                            north = true;
                        }
                        break;
                    case Pipe.SouthEast:
                        if (north)
                        {
                            y++;
                            north = false;
                            east = true;
                        }
                        else
                        {
                            x++;
                            west = false;
                            south = true;
                        }
                        break;
                    case Pipe.SouthWest:
                        if (north)
                        {
                            y--;
                            north = false;
                            west = true;
                        }
                        else
                        {
                            x++;
                            east = false;
                            south = true;
                        }
                        break;
                    case Pipe.Ground:
                        throw new Exception($"Walk on ground x: {x}, y: {y}");
                        break;
                    case Pipe.Start:
                        throw new Exception($"Walk on start x: {x}, y: {y}");
                        break;
                    default:
                        break;
                }
            }

            for (int i = 0; i < pipes.GetLength(0); i++)
            {
                for (int j = 0; j < pipes.GetLength(1); j++)
                {
                    if (belongToLoop[i, j])
                    {
                        continue;
                    }
                    int crosses = 0;
                    for (int k = j - 1; k >= 0; k--)
                    {
                        if (belongToLoop[i, k])
                        {
                            Pipe startPipe = pipes[i, k];
                            Pipe p = startPipe;
                            crosses++;
                            if (p == Pipe.SouthWest || p == Pipe.NorthWest)
                            {
                                do
                                {
                                    k--;
                                    p = pipes[i, k];
                                } while (p == Pipe.Horizontal);
                                if (startPipe == Pipe.SouthWest && p == Pipe.SouthEast)
                                {
                                    crosses++;
                                }
                                if (startPipe == Pipe.NorthWest && p == Pipe.NorthEast)
                                {
                                    crosses++;
                                }
                            }
                        }
                    }
                    if (crosses % 2 == 1)
                    {
                        result++;
                    }
                }
            }

            Console.WriteLine($"{className}.2: " + result);
        }
    }
}

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
    public class Day18
    {
        public static void Solve1(string input)
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
            var lines = File.ReadAllLines($"{className}/{input}");
            long result = 0;

            int size = 1000;

            List<List<uint>> map = new List<List<uint>>();
            List<List<char>> mapC = new List<List<char>>();

            for (int i = 0; i < size; i++)
            {
                map.Add(new List<uint>());
                mapC.Add(new List<char>());
                for (int j = 0; j < size; j++)
                {
                    map[i].Add(0);
                    mapC[i].Add('.');
                }
            }

            int r = size / 2;
            int c = size / 2;
            map[r][c] = Convert.ToUInt32("FFFFFF", 16);

            foreach (var line in lines)
            {
                var split = line.Split(' ');
                int length = int.Parse(split[1]);
                uint color = Convert.ToUInt32(split[2].Substring(2, 6), 16);
                switch (split[0])
                {
                    case "U":
                        for (int i = 1; i <= length; i++)
                        {
                            r++;
                            map[r][c] = color;
                            mapC[r][c] = 'U';
                        }
                        break;
                    case "D":
                        for (int i = 1; i <= length; i++)
                        {
                            r--;
                            map[r][c] = color;
                            mapC[r][c] = 'D';
                        }
                        break;
                    case "R":
                        for (int i = 1; i <= length; i++)
                        {
                            c++;
                            map[r][c] = color;
                            mapC[r][c] = 'R';
                        }
                        break;
                    case "L":
                        for (int i = 1; i <= length; i++)
                        {
                            c--;
                            map[r][c] = color;
                            mapC[r][c] = 'L';
                        }
                        break;
                    default:
                        break;
                }
            }

            //count
            for (int i = 0; i < map.Count; i++)
            {
                for (int j = 0; j < map[i].Count; j++)
                {
                    if (map[i][j] > 0)
                    {
                        //Console.Write(mapC[i][j]);
                        result++;
                    }
                    else
                    {
                        int crosses = 0;
                        for (int k = j - 1; k >= 0; k--)
                        {
                            if (map[i][k] > 0)
                            {
                                char startChar = mapC[i][k];
                                char ch = startChar;
                                crosses++;
                                if (ch == 'R')
                                {
                                    if (mapC[i - 1][k] == 'D')
                                    {
                                        startChar = 'D';
                                    }
                                    if (mapC[i + 1][k] == 'U')
                                    {
                                        startChar = 'U';
                                    }
                                    do
                                    {
                                        k--;
                                        ch = mapC[i][k];
                                    } while (ch == 'R');
                                    if (startChar == 'U' && ch == 'D'
                                        || startChar == 'D' && ch == 'U')
                                    {
                                        crosses++;
                                    }
                                }
                                if (ch == 'U' || ch == 'D')
                                {
                                    startChar = ch;
                                    do
                                    {
                                        k--;
                                        ch = mapC[i][k];
                                    } while (ch == 'L');
                                    if (mapC[i - 1][k + 1] == 'D')
                                    {
                                        ch = 'D';
                                    }
                                    if (mapC[i + 1][k + 1] == 'U')
                                    {
                                        ch = 'U';
                                    }
                                    if (startChar == 'U' && ch == 'D'
                                        || startChar == 'D' && ch == 'U')
                                    {
                                        crosses++;
                                    }
                                }
                            }
                        }
                        if (crosses % 2 == 1)
                        {
                            //Console.Write("X");
                            result++;
                        }
                        else
                        {
                            //Console.Write(".");
                        }
                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine($"{className}.1: " + result);
        }

        public class Point
        {
            public long X { get; set; }
            public long Y { get; set; }

            public Point(long x, long y)
            {
                X = x;
                Y = y;
            }

            public override string? ToString()
            {
                return $"({X}, {Y})";
            }
        }

        public static void Solve2(string input)
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
            var lines = File.ReadAllLines($"{className}/{input}");
            long result = 0;

            int size = 600;

            List<List<char>> mapC = new List<List<char>>();
            Dictionary<long, int> rowsIdx = new Dictionary<long, int>();
            Dictionary<long, int> colsIdx = new Dictionary<long, int>();
            List<Point> path = new List<Point>();

            for (int i = 0; i < size; i++)
            {
                mapC.Add(new List<char>());
                for (int j = 0; j < size; j++)
                {
                    mapC[i].Add('.');
                }
            }

            long x = 0;
            long y = 0;
            path.Add(new Point(x, y));

            foreach (var line in lines)
            {
                var split = line.Split(' ');
                string hex = split[2].Substring(2, 6);
                uint length = Convert.ToUInt32(hex.Substring(0, 5), 16);
                switch (hex[5])
                {
                    case '3': //U
                        y -= length;
                        path.Add(new Point(x, y));
                        break;
                    case '1': //D
                        y += length;
                        path.Add(new Point(x, y));
                        break;
                    case '0': //R
                        x -= length;
                        path.Add(new Point(x, y));
                        break;
                    case '2': //L
                        x += length;
                        path.Add(new Point(x, y));
                        break;
                    default:
                        break;
                }
            }
            //Console.WriteLine($"Finished path at x={x} y={y}");

            List<long> xs = path.OrderBy(x => x.X).Select(x => x.X).Distinct().ToList();
            List<long> xss = new List<long> { 0, 0 };
            List<long> ys = path.OrderBy(x => x.Y).Select(x => x.Y).Distinct().ToList();
            List<long> yss = new List<long> { 0, 0 };
            for (int i = 0; i < xs.Count; i++)
            {
                colsIdx[xs[i]] = 2 * i + 2;
                colsIdx[xs[i] + 1] = 2 * i + 3;
                xss.Add(xs[i]);
                xss.Add(xs[i] + 1);
            }
            for (int i = 0; i < ys.Count; i++)
            {
                rowsIdx[ys[i]] = 2 * i + 2;
                rowsIdx[ys[i] + 1] = 2 * i + 3;
                yss.Add(ys[i]);
                yss.Add(ys[i] + 1);
            }
            for (int i = 1; i < path.Count; i++)
            {
                Point point = path[i];
                Point prev = path[i - 1];
                var dx = Math.Sign(point.X - prev.X);
                var dy = Math.Sign(point.Y - prev.Y);
                if (dx == 0)
                {
                    int x1 = colsIdx[point.X];
                    //Console.WriteLine($"y: {rowsIdx[prev.Y]} -> {rowsIdx[point.Y]}");
                    if (dy > 0)
                    {
                        for (int j = rowsIdx[prev.Y] + 1; j <= rowsIdx[point.Y]; j++)
                        {
                            mapC[j][x1] = 'D';
                        }
                    }
                    else
                    {
                        for (int j = rowsIdx[prev.Y] - 1; j >= rowsIdx[point.Y]; j--)
                        {
                            mapC[j][x1] = 'U';
                        }
                    }
                }
                else if (dy == 0)
                {
                    int y1 = rowsIdx[point.Y];
                    //Console.WriteLine($"x: {colsIdx[prev.X]} -> {colsIdx[point.X]}");
                    if (dx > 0)
                    {
                        for (int j = colsIdx[prev.X] + 1; j <= colsIdx[point.X]; j++)
                        {
                            mapC[y1][j] = 'R';
                        }
                    }
                    else
                    {
                        for (int j = colsIdx[prev.X] - 1; j >= colsIdx[point.X]; j--)
                        {
                            mapC[y1][j] = 'L';
                        }
                    }
                }
                else
                {
                    throw new Exception($"Point not found: {point}");
                }
            }

            //count
            for (int i = 0; i < mapC.Count - 1; i++)
            {
                for (int j = 0; j < mapC[i].Count - 1; j++)
                {
                    if (mapC[i][j] != '.')
                    {
                        //Console.Write(mapC[i][j]);
                        //Console.WriteLine($"({xs[j]},{ys[i]}) -> ({xs[j+1]},{ys[i+1]})");
                        //Console.WriteLine($" Edge: ({xss[j]},{yss[i]}) -> ({xss[j + 1]},{yss[i + 1]}) = {(xss[j + 1] - xss[j]) * (yss[i + 1] - yss[i])}");
                        result += (xss[j + 1] - xss[j]) * (yss[i + 1] - yss[i]);
                    }
                    else
                    {
                        int crosses = 0;
                        for (int k = j - 1; k >= 0; k--)
                        {
                            if (mapC[i][k] != '.')
                            {
                                char startChar = mapC[i][k];
                                char ch = startChar;
                                crosses++;
                                if (ch == 'R')
                                {
                                    if (mapC[i - 1][k] == 'U')
                                    {
                                        startChar = 'U';
                                    }
                                    if (mapC[i + 1][k] == 'D')
                                    {
                                        startChar = 'D';
                                    }
                                    do
                                    {
                                        k--;
                                        ch = mapC[i][k];
                                    } while (ch == 'R');
                                    if (startChar == 'U' && ch == 'D'
                                        || startChar == 'D' && ch == 'U')
                                    {
                                        crosses++;
                                    }
                                }
                                else if (ch == 'U' || ch == 'D')
                                {
                                    startChar = ch;
                                    do
                                    {
                                        k--;
                                        ch = mapC[i][k];
                                    } while (ch == 'L');
                                    k++;  
                                    if (mapC[i - 1][k] == 'U')
                                    {
                                        ch = 'U';
                                    }
                                    if (mapC[i + 1][k] == 'D')
                                    {
                                        ch = 'D';
                                    }
                                    if (startChar == 'U' && ch == 'D'
                                        || startChar == 'D' && ch == 'U')
                                    {
                                        crosses++;
                                    }
                                }
                            }
                        }
                        if (crosses % 2 == 1)
                        {
                            //Console.Write("X");
                            //Console.WriteLine($" Full: ({xss[j]},{yss[i]}) -> ({xss[j + 1]},{yss[i + 1]}) = {(xss[j + 1] - xss[j]) * (yss[i + 1] - yss[i])}");
                            result += (xss[j + 1] - xss[j]) * (yss[i + 1] - yss[i]);
                        }
                        else
                        {
                            //Console.Write(".");
                        }
                    }
                }
                //Console.WriteLine();
            }

            Console.WriteLine($"{className}.2: " + result);
        }
    }
}

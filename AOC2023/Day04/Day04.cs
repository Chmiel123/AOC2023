using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AOC2023
{
    internal class Day04
    {
        public static void Solve1()
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
            var lines = File.ReadAllLines($"{className}/input.txt");
            int result = 0;

            foreach (var line in lines)
            {
                var data = line.Split(new char[] { ':', '|' });
                var winningNumbers = data[1].Trim().Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => int.Parse(x));
                var currentNumbers = data[2].Trim().Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => int.Parse(x));
                var points = 0;
                foreach (var current in currentNumbers)
                {
                    if (winningNumbers.Contains(current))
                    {
                        if (points == 0)
                        {
                            points = 1;
                        }
                        else
                        {
                            points *= 2;
                        }
                    }
                }
                result += points;
            }

            Console.WriteLine($"{className}.1: " + result);
        }

        public class ScratchCard
        {
            public List<int>? WinningNumbers { get; set; }
            public List<int>? CurrentNumbers { get; set; }
            public int Count { get; set; }

            private int points = -1;
            public int Points {
                get
                {
                    if (points == -1)
                    {
                        if (CurrentNumbers == null || WinningNumbers == null)
                        {
                            return -1;
                        }
                        points = (CurrentNumbers.Where(current => WinningNumbers.Contains(current))).Count();
                    }
                    return points;
                }
            }

            public ScratchCard()
            {
                WinningNumbers = null;
                CurrentNumbers = null;
                Count = 1;
            }
        }

        public static void Solve2()
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
            var lines = File.ReadAllLines($"{className}/input.txt");
            int result = 0;

            var scratchCards = new List<ScratchCard>();

            for (int i = 0; i < lines.Length; i++)
            {
                string? line = lines[i];
                var data = line.Split(new char[] { ':', '|' });
                var winningNumbers = data[1].Trim().Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => int.Parse(x)).ToList();
                var currentNumbers = data[2].Trim().Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => int.Parse(x)).ToList();
                ScratchCard currentSC;
                if (scratchCards.Count <= i)
                {
                    currentSC = new ScratchCard();
                    scratchCards.Add(currentSC);
                }
                else
                {
                    currentSC = scratchCards[i];
                }
                currentSC.WinningNumbers = winningNumbers;
                currentSC.CurrentNumbers = currentNumbers;
                for (int j = 1; j <= currentSC.Points; j++)
                {
                    var l = i + j;
                    ScratchCard nextSC;
                    if (scratchCards.Count <= l)
                    {
                        nextSC = new ScratchCard();
                        scratchCards.Add(nextSC);
                    }
                    else
                    {
                        nextSC = scratchCards[l];
                    }
                    nextSC.Count += currentSC.Count;
                }
                //Console.WriteLine($"Card {i + 1}: points: {currentSC.Points}, count: {currentSC.Count}");
                result += currentSC.Count;
            }

            Console.WriteLine($"{className}.2: " + result);
        }
    }
}

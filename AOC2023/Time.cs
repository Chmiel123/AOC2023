using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2023
{
    internal static class Time
    {
        public static long TimeMethod(Action action)
        {
            Console.WriteLine($"Start {action.Method.Name}");
            var watch = System.Diagnostics.Stopwatch.StartNew();
            action.Invoke();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine($"End {action.Method.Name}");
            Console.WriteLine($"    {elapsedMs}ms");
            return elapsedMs;
        }

        public static long TimeMethod(Action<string> action, string input)
        {
            Console.WriteLine($"Start {action.Method.Name}");
            var watch = System.Diagnostics.Stopwatch.StartNew();
            action.Invoke(input);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine($"End {action.Method.Name}");
            Console.WriteLine($"    {elapsedMs}ms");
            return elapsedMs;
        }
    }
}

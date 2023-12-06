using AdventOfCode;

using System.Diagnostics;

internal class Program
{
    private static async Task Main(string[] args)
    {
        //var s = new Stopwatch();
        //var input = File.ReadAllText(@"C:\Users\mnor\source\repos\aoc2023\AdventOfCode\Inputs\06.txt");
        //s.Start();
        //var a = new Day06();
        //a.SetInput(input);
        //await a.Solve_2();
        //s.Stop();
        //Console.WriteLine(s.Elapsed.TotalMilliseconds.ToString());
        //return;

        if (args.Length == 0)
        {
            await Solver.SolveLast(opt => opt.ClearConsole = false);
        }
        else if (args.Length == 1 && args[0].Contains("all", StringComparison.CurrentCultureIgnoreCase))
        {
            await Solver.SolveAll(opt =>
            {
                opt.ShowConstructorElapsedTime = true;
                opt.ShowTotalElapsedTimePerDay = false;
                opt.ElapsedTimeFormatSpecifier = "F3";
            });
        }
        else if (args.Length == 1 && args[0].Contains("today", StringComparison.CurrentCultureIgnoreCase))
        {
            await Solver.Solve([(uint)DateTime.Today.Day], opt =>
            {
                opt.ShowConstructorElapsedTime = true;
                opt.ShowTotalElapsedTimePerDay = false;
                opt.ElapsedTimeFormatSpecifier = "F3";
            });
        }
        else
        {
            var indexes = args.Select(arg => uint.TryParse(arg, out var index) ? index : uint.MaxValue);

            await Solver.Solve(indexes.Where(i => i < uint.MaxValue));
        }
    }
}
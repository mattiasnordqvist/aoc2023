using Spectre.Console;

namespace AdventOfCode;

public class Day02 : MyBaseDay
{
    public override async ValueTask<string> Solve_1()
    {
        return Input.Zplit()
            .Where(x => IsPossible(x))
            .Sum(x => Id(x)).ToString();

    }

    private bool IsPossible(string line)
    {
        // 12 red cubes, 13 green cubes, and 14 blue
        return !line[(line.IndexOf(":")+1)..].Replace(";", ",")
            .Zplit(",")
            .Any(x => ((x.Zplit(" ")[0].i() > 12 && x.Zplit(" ")[1] == "red")
            || (x.Zplit(" ")[0].i() > 13 && x.Zplit(" ")[1] == "green")
            || (x.Zplit(" ")[0].i() > 14 && x.Zplit(" ")[1] == "blue")));
    }

    private int Power(string line)
    {
        // 12 red cubes, 13 green cubes, and 14 blue
        return line[(line.IndexOf(":") + 1)..].Replace(";", ",")
            .Zplit(",")
            .Select(x => (amount: x.Zplit(" ")[0].i(), color: x.Zplit(" ")[1]))
            .GroupBy(x => x.color)
            .Select(x => x.Max(m => m.amount))
            .Aggregate(1, (a, b) => a * b);

    }

    private static int Id(string line)
    {
        return line[5..(line.IndexOf(":"))].i();
    }

    public override async ValueTask<string> Solve_2()
    {
        return Input.Zplit()
           .Sum(x => Power(x))
           .ToString();
    }
}

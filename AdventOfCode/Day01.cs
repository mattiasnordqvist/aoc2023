using System.Diagnostics;

namespace AdventOfCode;
public class Day01 : MyBaseDay
{
    public override async ValueTask<string> Solve_1()
    {
        var lines = _input.Split('\n');
        var sum = 0;
        foreach (var line in lines)
        {
            bool firstFound = false;
            char? last = null;
            foreach (var c in line)
            {

                if (char.IsDigit(c))
                {
                    last = c;
                    if (!firstFound)
                    {
                        sum += (c - 48) * 10;
                        firstFound = true;
                    }
                }
            }
            sum += (last.Value - 48);

        }
        Debug.Assert(sum == 54601);
        return sum.ToString();
    }

    public override ValueTask<string> Solve_2()
    {
        Dictionary<string, int> textual = new()
        {
            {"one", 1 },
            {"two", 2 },
            {"three", 3 },
            {"four", 4},
            {"five", 5 },
            {"six", 6 },
            {"seven", 7 },
            {"eight", 8 },
            {"nine", 9 }
        };
        var lines = _input.Split('\n');
        var sum = 0;
        foreach (var line in lines)
        {
            ReadOnlySpan<char> span = line.AsSpan();
            bool firstFound = false;
            int? last = null;
            for (int i = 0; i < line.Length; i++)
            {
                var c = span[i];
                if (char.IsDigit(c))
                {
                    last = c - 48;
                    if (!firstFound)
                    {
                        sum += (c - 48) * 10;
                        firstFound = true;
                    }
                }
                else
                {
                    foreach (var key in textual.Keys)
                    {
                        if (span[i..].StartsWith(key))
                        {
                            last = textual[key];
                            if (!firstFound)
                            {
                                sum += textual[key] * 10;
                                firstFound = true;
                            }
                        }
                    }
                }
            }

            sum += (last.Value);

        }
        Debug.Assert(sum == 54078);
        return ValueTask.FromResult(sum.ToString());
    }
}

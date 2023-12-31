﻿using System.Diagnostics;

namespace AdventOfCode;
public class Day01 : MyBaseDay
{
    public override ValueTask<string> Solve_1()
    {
        var lines = Input.SplitLines();//Split('\n');
        var sum = 0;
        foreach (var line in lines)
        {
            bool firstFound = false;
            char? last = null;
            foreach (var c in line.Line)
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
            if(last == null)
            {
                throw new Exception("last expected to not be null at this time");
            }
            sum += (last.Value - 48);

        }
        Debug.Assert(sum == 54601);
        return ValueTask.FromResult(sum.ToString());
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
        var lines = Input.SplitLines();
        var sum = 0;
        foreach (var line in lines)
        {
            ReadOnlySpan<char> span = line;
            bool firstFound = false;
            int? last = null;
            for (int i = 0; i < line.Line.Length; i++)
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

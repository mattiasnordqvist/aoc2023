


namespace AdventOfCode;
public class Day12 : MyBaseDay
{
    public static int _completed = 0;

    public override async ValueTask<string> Solve_1()
    {
        var lines = Input.Zplit();
        var s = lines.Select(x => new Line(x).CountArrangements()).Sum();
        return s.ToString();
    }


    public override async ValueTask<string> Solve_2()
    {
        Console.WriteLine("Part 2");
        var lines = Input.Zplit().Select(x =>
        {
            return x.Split(" ")[0] + "?" + x.Split(" ")[0] + "?" + x.Split(" ")[0] + "?" + x.Split(" ")[0] + "?" + x.Split(" ")[0]
            + " " + x.Split(" ")[1] + "," + x.Split(" ")[1] + "," + x.Split(" ")[1] + "," + x.Split(" ")[1] + "," + x.Split(" ")[1];

        }).ToList();

        var s = lines.Select(x => new Line(x).CountArrangements()).Sum();
        return s.ToString();
    }

    public static (int[] remainingGroups, string remainingText) Eat(int[] groups, string text)
    {
        if(groups.Count() == 0)
        {
            return (groups, string.Join("",text.SkipWhile(c => c == '.')));
        }
        var nextMatch = new string('#', groups[0]);
        var span = text.AsSpan();
        if (span.Length == 0)
        {
            return (groups, text);
        }
        for (int i = 0; i < span.Length;)
        {
            if (span[i] == '.') { i++; continue; }
            if (span[i] == '?')
            {
                return (groups, span[i..].ToString());
            }
            if (i + groups[0] > span.Length)
            {
                return (groups, span[i..].ToString());
            }
            if ((i + groups[0]) == span.Length && span[i..(i + groups[0])].ToString() == nextMatch)
            {
                groups = groups[1..];

                i += nextMatch.Length;
                if (groups.Length == 0)
                {
                    return (groups, span[i..].ToString());
                }
                nextMatch = new string('#', groups[0]);
            }
            else if (i + groups[0] + 1 > span.Length)
            {
                return (groups, span[i..].ToString());
            }
            else if (span[i..(i + groups[0] + 1)].ToString() == nextMatch + ".")
            {
                groups = groups[1..];
                i += nextMatch.Length + 1;
                if (groups.Length == 0)
                {
                    return (groups, span[i..].ToString());
                }
                nextMatch = new string('#', groups[0]);
            }
            else
            {
                return (groups, span[i..].ToString());
            }
        }
        return (groups, "");
    }
}

public class Line
{
    private string _x;
    private string _text;
    private int[] _groups;

    public Line(string x)
    {
        _x = x;
        _text = x.Zplit(" ")[0];
        _groups = x.Zplit(" ")[1].Zplit(",").ia();
    }
    public long CountArrangements()
    {
        return CountArrangements(_groups, _text);
    }
    public long CountArrangements(int[] groups, string text)
    {
        var (rG, rT) = Day12.Eat(groups, text);
        if (rT.Length == 0 && rG.Length > 0) return 0;
        if (rG.Length == 0 && !rT.Contains('#') && !rT.Contains('?'))
        {
            return 1;
        }
        else if (rT.Contains('?'))
        {
            var i = rT.IndexOf('?');

            return CountArrangements(rG, rT[0..i] + '.' + rT[(i+1)..]) + CountArrangements(rG, rT[0..i] + '#' + rT[(i + 1)..]);
        }
        return 0;
    }
}
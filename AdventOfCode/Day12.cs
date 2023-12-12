


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

        var tasks = lines.Chunk(100)
            .Select(x => Task.WhenAll(x.Select(y => Task.Run(() => new Line(y).CountArrangements()))));
        var ss = await Task.WhenAll(tasks);
        return ss.SelectMany(x => x).Sum().ToString();
    }
}

public class Line
{
    private string _x;
    private string _springs;
    private int[] _groupSizes;

    public Dictionary<(string remainingDesiredGroups, string remainingSprings), long> _cache = [];

    public Line(string x)
    {
        _x = x;
        _springs = x.Zplit(" ")[0];
        _groupSizes = x.Zplit(" ")[1].Zplit(",").ia();
    }
    public long CountArrangements()
    {
        return CountArrangements(_groupSizes, _springs, "");
        
    }

    private long CountArrangements(int[] groupSizes, string remainingSprings, string accumulated)
    {
        if (_cache.ContainsKey((string.Join(",", groupSizes), remainingSprings)))
        {
            return _cache[(string.Join(",", groupSizes), remainingSprings)];
        }
        else
        {
            long count = AllAlts(groupSizes, groupSizes, remainingSprings, accumulated);
            return count;
        }
    }

    private long AllAlts(int[] groupSizes, int[] remainingGroupSizes, string remainingSprings, string accumulated)
    {
        
        var (isCandidate, newRemainingGroupSizes) = IsCandidate(groupSizes, accumulated);
        
        if(isCandidate && newRemainingGroupSizes != null)
        {
            remainingGroupSizes = newRemainingGroupSizes;
        }
        if (newRemainingGroupSizes != null && _cache.ContainsKey((string.Join(",", remainingGroupSizes), remainingSprings)))
        {
            return _cache[(string.Join(",", remainingGroupSizes), remainingSprings)];
        }
        if (!isCandidate)
        {
            _cache[(string.Join(",", remainingGroupSizes), remainingSprings)] = 0;
            return 0;
        }
        if (remainingSprings == "")
        {
            return IsMatch(accumulated, groupSizes) ? 1 : 0;
        }
        if (remainingSprings[0] == '.') { 
            var a = AllAlts(groupSizes, remainingGroupSizes, remainingSprings[1..], accumulated + ".");
            //_cache[(string.Join(",", remainingGroupSizes), remainingSprings)] = a;
            return a;
        }
        if (remainingSprings[0] == '#') {

            var a = AllAlts(groupSizes, remainingGroupSizes, remainingSprings[1..], accumulated + "#");
            return a;
        }
        if (remainingSprings[0] == '?') { 
            var a = AllAlts(groupSizes, remainingGroupSizes, remainingSprings[1..], accumulated + ".") + AllAlts(groupSizes, remainingGroupSizes, remainingSprings[1..], accumulated + "#");
            return a;
        }
        throw new Exception();
    }

    private (bool, int[]? remainingGroupSizes) IsCandidate(int[] groupSizes, string accumulated)
    {
        var groups = accumulated.Zplit(".");
        if (groups.Length == 0) return (true, groupSizes);
        if (groups.Count() > groupSizes.Length) return (false, []);
        var i = 0;
        for (; i < groups.Count() - 1; i++)
        {
            if (groups[i].Length != groupSizes[i]) return (false, []);
        }
        if (accumulated.EndsWith("#"))
        {
            return (groups[i].Length <= groupSizes[i], null);
        }
        else
        {
            return (groups[i].Length == groupSizes[i], groupSizes.Skip(i+1).ToArray());
        }

    }

    public static bool IsMatch(string x, int[] groupSizes)
    {
        var groups = x.Zplit(".");
        if (groups.Count() != groupSizes.Length) return false;
        for (var i = 0; i < groups.Count(); i++)
        {
            if (groups[i].Length != groupSizes[i]) return false;
        }
        return true;
    }
}
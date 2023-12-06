
using System.Diagnostics;

namespace AdventOfCode;

public class Day05 : MyBaseDay
{
    public override ValueTask<string> Solve_1()
    {
        var lines = _input.Zplit();
        var seeds = lines[0].Zplit(":")[1].Zplit(" ").la();
        ListDictionary<string, (string to, (long dest, long source, long length) map)> maps = [];
        for (int l = 2; l < lines.Length;)
        {
            var splut = lines[l].Zplit("-");
            var from = splut[0];
            var to = splut[2].Zplit(" ")[0];
            int ll = l + 1;
            for (; lines[ll] != string.Empty; ll++)
            {
                var r = lines[ll].Zplit(" ");
                maps.Add(from, ((to, map: (dest: r.l(0), source: r.l(1), length: r.l(2)))));
            }
            l = ll + 1;
        }

        var min = long.MaxValue;
        foreach (var seed in seeds)
        {
            var value = seed;
            var current = "seed";
            while (current != "location")
            {
                var ms = maps[current];
                for (int i = 0; i < ms.Count; i++)
                {
                    if (value >= ms[i].map.source && value < ms[i].map.source + ms[i].map.length)
                    {
                        value = ms[i].map.dest + (value - ms[i].map.source);
                        break;
                    }
                }
                current = ms[0].to;
            }
            if (value < min)
            {
                min = value;
            }
        }
        Debug.Assert(min == 84470622);
        return ValueTask.FromResult(min.ToString());

    }

    public record Range(long i, long l)
    {
        public long e = i + l - 1;

        public bool Valid() => l > 0;

        public static Range From(long i, long e) => new(i, e - i + 1);

        internal Range ReMap(long source, long dest)
        {
            return new Range(i + dest - source, l);
        }
    }

    public override async ValueTask<string> Solve_2()
    {
        var lines = _input.Zplit();
        var seedsArray = lines[0].Zplit(":")[1].Zplit(" ").la();

        var seeds = new Range[seedsArray.Length / 2];
        for (int i = 0; i < seedsArray.Length / 2; i++)
        {
            seeds[i] = new(seedsArray[i * 2], seedsArray[i * 2 + 1]);
        }

        ListDictionary<string, (string to, (long dest, Range range) map)> maps = [];
        for (int l = 2; l < lines.Length;)
        {
            var splut = lines[l].Zplit("-");
            var from = splut[0];
            var to = splut[2].Zplit(" ")[0];
            int ll = l + 1;
            for (; lines[ll] != string.Empty; ll++)
            {
                var r = lines[ll].Zplit(" ");
                maps.Add(from, ((to, map: (dest: r.l(0), range: new(r.l(1), r.l(2))))));
            }
            l = ll + 1;
        }

        var min = long.MaxValue;

        foreach (var seedRange in seeds)
        {
            var smallest = FindSmallestInSeedRange(seedRange);
            if (smallest < min)
            {
                min = smallest;
            }
        }

        Debug.Assert(min == 26714516);
        return min.ToString();

        (string to, (long dest, Range range) map)[] FindMapsOrdered(string from)
        {
            var mapRanges = maps[from].OrderBy(x => x.map.range.i).ToArray();
            return mapRanges;
        }

        long FindSmallestInSeedRange(Range seedRange)
        {
            List<Range> seedGroups = [seedRange];
            var current = "seed";
            while (current != "location")
            {
                var orderedMaps = FindMapsOrdered(current);
                var newGroups = seedGroups.SelectMany(x => Filter(x, orderedMaps)).ToList();
                seedGroups = newGroups.ToList();
                current = orderedMaps.First().to;
            }
            return seedGroups.Select(x => x.i).Min();
        }
    }

    private IEnumerable<Range> Filter(Range rangeToFilter, (string to, (long dest, Range range) map)[] orderedMaps)
    {
        Range[] currentGroups = [rangeToFilter];
        var newGroups = new List<Range>();

        foreach (var range in orderedMaps)
        {
            foreach(var currentGroup in currentGroups) { 
                var before = Range.From(currentGroup.i, Math.Min(currentGroup.e, range.map.range.i-1));
                var overlap = Range.From(Math.Max(currentGroup.i, range.map.range.i), Math.Min(currentGroup.e, range.map.range.e));
                var after = Range.From(Math.Max(currentGroup.i,range.map.range.e), currentGroup.e);
                
                if (before.Valid() && after.Valid())
                {
                    currentGroups = [before, after];
                }
                else if (before.Valid())
                {
                    currentGroups = [before];
                }
                else if (after.Valid())
                {
                    currentGroups = [after];
                }
                else
                {
                    currentGroups = [];
                }
                if(overlap.Valid())
                {
                    newGroups.Add(overlap.ReMap(range.map.range.i, range.map.dest));
                }
            }
        }
        return [..newGroups, ..currentGroups];
    }
}


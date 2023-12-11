

using System.Collections.Generic;
using System.Diagnostics;


namespace AdventOfCode;
public class Day11 : MyBaseDay
{
    public override async ValueTask<string> Solve_1()
    {
        var map = Input.ToMap();
        var maxx = map.Max(x => x.Key.x);
        var maxy = map.Max(x => x.Key.y);
        var ysToExpand = new HashSet<int>();
        for (var y = 0; y <= maxy; y++)
        {
            if (map.Where(m => m.Key.y == y).All(m => m.Value == '.')) { ysToExpand.Add(y); }
        }
        var xsToExpand = new HashSet<int>();
        for (var x = 0; x <= maxx; x++)
        {
            if (map.Where(m => m.Key.x == x).All(m => m.Value == '.')) { xsToExpand.Add(x); }
        }

        var galaxies = map.Where(x => x.Value == '#').ToList();

        var sum = 0l;
        for (int i = 0; i < galaxies.Count - 1; i++)
        {
            for (int j = i + 1; j < galaxies.Count; j++)
            {
                var a = galaxies[i].Key;
                var b = galaxies[j].Key;

                var lowy = Math.Min(a.y, b.y);
                var highy = Math.Max(a.y, b.y);
                var lowx = Math.Min(a.x, b.x);
                var highx = Math.Max(a.x, b.x);
                var extraYs = ysToExpand.Where(y => y > lowy && y < highy).Count();
                var extraXs = xsToExpand.Where(x => x > lowx && x < highx).Count();

                sum += (ManhattanDistance(a, b) + extraXs + extraYs);
            }
        }

        return sum.ToString();
    }

    public int ManhattanDistance(P2D p1, P2D p2)
    {
        return Math.Abs(p1.x - p2.x)+Math.Abs(p1.y-p2.y);
    }

    public override async ValueTask<string> Solve_2()
    {
        var map = Input.ToMap();
        var maxx = map.Max(x => x.Key.x);
        var maxy = map.Max(x => x.Key.y);
        var ysToExpand = new HashSet<int>();
        for (var y = 0; y <= maxy; y++)
        {
            if (map.Where(m => m.Key.y == y).All(m => m.Value == '.')) { ysToExpand.Add(y); }
        }
        var xsToExpand = new HashSet<int>();
        for (var x = 0; x <= maxx; x++)
        {
            if (map.Where(m => m.Key.x == x).All(m => m.Value == '.')) { xsToExpand.Add(x); }
        }

        var galaxies = map.Where(x => x.Value == '#').ToList();

        var sum = 0l;
        for (int i = 0; i < galaxies.Count - 1; i++)
        {
            for (int j = i + 1; j < galaxies.Count; j++)
            {
                var a = galaxies[i].Key;
                var b = galaxies[j].Key;

                var lowy = Math.Min(a.y, b.y);
                var highy = Math.Max(a.y, b.y);
                var lowx = Math.Min(a.x, b.x);
                var highx = Math.Max(a.x, b.x);
                var extraYs = ysToExpand.Where(y => y > lowy && y < highy).Count();
                var extraXs = xsToExpand.Where(x => x > lowx && x < highx).Count();

                sum += (ManhattanDistance(a,b)+ extraXs* (1000000-1) + extraYs* (1000000-1));
            }
        }

        return sum.ToString();
    }
}

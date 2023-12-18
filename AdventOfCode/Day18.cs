
namespace AdventOfCode;
public class Day18 : MyBaseDay
{
    private List<(char d, int s, string c)> _instructions = new List<(char, int, string)>();
    private HashSet<(int x, int y)> map;
    private int minx;
    private int maxx;
    private int miny;
    private int maxy;

    public Day18()
    {
        var lines = Input.Zplit();
        foreach (var line in lines)
        {
            var splut = line.Zplit(" ");
            _instructions.Add((splut[0][0], splut[1].i(), splut[2][2..8]));

        }
    }

    public override async ValueTask<string> Solve_1()
    {
        map = [];
        var p = (x: 0, y: 0);
        map.Add(p);
        foreach (var instr in _instructions)
        {
            var (dx, dy) = instr.d switch
            {
                'R' => (1, 0),
                'L' => (-1, 0),
                'U' => (0, -1),
                'D' => (0, 1)
            };

            for (int s = 0; s < instr.s; s++)
            {
                p.x += dx;
                p.y += dy;
                map.Add(p);
            }
        }

        minx = map.Min(c => c.x);
        maxx = map.Max(c => c.x);
        miny = map.Min(c => c.y);
        maxy = map.Max(c => c.y);
        for (int y = miny; y <= maxy; y++)
        {
            for (int x = minx; x <= maxx; x++)
            {
                if (map.Contains((x, y))) continue;
                else
                {
                    if (IsContained((x, y), out var filled))
                    {
                        foreach (var f in filled)
                        {
                            map.Add(f);
                        }
                    }
                }
            }
        }
        return map.Count().ToString();
    }

    private bool IsContained((int x, int y) start, out HashSet<(int x, int y)> filled)
    {
        filled = [];
        Stack<(int x, int y)> s = [];
        s.Push(start);
        while (s.Count != 0)
        {
            var popped = s.Pop();
            if (popped.x < minx || popped.x > maxx || popped.y < miny || popped.y > maxy) return false;
            if (!filled.Contains(popped) && !map.Contains(popped))
            {
                filled.Add(popped);
                (int x, int y)[] s4 = [(popped.x - 1, popped.y), (popped.x + 1, popped.y), (popped.x, popped.y + 1), (popped.x, popped.y - 1)];
                foreach (var surr in s4)
                {
                    s.Push(surr);
                }
            }
        }
        return true;

    }

    public override async ValueTask<string> Solve_2()
    {
        var map2 = new ListDictionary<long, long>();
        var p = (x: 0, y: 0);
        map2.Add(0, 0);
        foreach (var instr in _instructions)
        {
            var (dx, dy) = instr.c[5] switch
            {
                '0' => (1, 0),
                '2' => (-1, 0),
                '3' => (0, -1),
                '1' => (0, 1)
            };

            for (int s = 0; s < FromHex(instr.c[0..5]); s++)
            {
                p.x += dx;
                p.y += dy;
                map2.Add(p.y, p.x);
            }
        }

        var miny2 = map2.Min(c => c.Key);
        var maxy2 = map2.Max(c => c.Key);
        var l = 0l;
        for (int y = miny; y <= maxy; y++)
        {
            var ranges = GetRanges(map2[y]);
            if(ranges.Count() == 1)
            {
                l += ranges[0].last - ranges[0].start;
            }
            else
            {
                for(int r = 0; r < ranges.Count() / 2; r++)
                {
                    var from = ranges[r*2];
                    var to = ranges[r*2+1];
                    l += (to.last - from.start);
                }
            }
        }
        return l.ToString();
    }

    private (long start, long last)[] GetRanges(List<long> list)
    {
        List<(long start, long last)> ranges = [];

        long? start = null;
        long? last = null;
        foreach(var x in list)
        {
            if(start == null) { start = x; last = x; }
            else if(x == last + 1) { last = x; }
            else { ranges.Add((start.Value, last.Value));  start = x; last = x; }
        }
        ranges.Add((start.Value, last.Value));
        return [.. ranges];
    }

    private long FromHex(string hex)
    {
        return hex.Reverse().Aggregate((place: 0, acc: 0), (a, b) => (place: a.place + 1, acc: a.acc + (b switch
        {
            >= '0' and <= '9' => b - 48,
            >= 'a' => b - 87,

        }) * (int)Math.Pow(16,a.place))).acc;
    }
}
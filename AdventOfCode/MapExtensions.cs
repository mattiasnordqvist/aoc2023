namespace AdventOfCode;

public static class MapExtensions
{
    public static void Print<T>(Dictionary<P2D, T> map)
    {
        var minx = map.Keys.MinBy(x => x.x).x;
        var miny = map.Keys.MinBy(x => x.y).y;
        var maxx = map.Keys.MaxBy(x => x.x).x;
        var maxy = map.Keys.MaxBy(x => x.y).y;
        for (int y = miny; y <= maxy; y++)
        {
            for (int x = minx; x <= maxx; x++)
            {
                var k = new P2D(x, y);
                if (map.ContainsKey(k))
                {
                    Console.Write(map[k].ToString()[0]);
                }
                else
                {
                    Console.Write(' ');
                }
            }
            Console.WriteLine();

        }
    }

    public static Dictionary<P2D, char> ToMap(this string input)
    {
        var lines = input.Zplit();
        var map = new Dictionary<P2D, char>();
        var y = 0;
        foreach (var line in lines)
        {
            var x = 0;
            foreach (var c in line)
            {
                map.Add(new(x, y), c);
                x++;
            }
            y++;
        }
        return map;
    }

    public static Dictionary<(int x, int y), char> ToMap2(this string input)
    {
        var lines = input.Zplit();
        var map = new Dictionary<(int x, int y), char>();
        var y = 0;
        foreach (var line in lines)
        {
            var x = 0;
            foreach (var c in line)
            {
                map.Add((x, y), c);
                x++;
            }
            y++;
        }
        return map;
    }
}


namespace AdventOfCode;
public class Day13 : MyBaseDay
{
    public override async ValueTask<string> Solve_1()
    {
        var lines = Input.Zplit();
        var maps = CreateMaps(lines).ToList();
        return maps.Sum(x => Evaluate(x)!.Value).ToString();
    }

    private long? Evaluate(Dictionary<P2D, char> map, long? ignore = null)
    {
        for (int x = 1; x <= map.MaxBy(p => p.Key.x).Key.x - 1; x++)
        {
            if (AreColumnsEqual(x, x + 1, map))
            {
                if (ColumnsSurroundingAreEqual(x, x + 1, map))
                {
                    if(x == ignore)
                    {
                        continue;
                    }
                    return x;
                }
            }
        }

        for (int y = 1; y <= map.MaxBy(p => p.Key.y).Key.y - 1; y++)
        {
            if (AreRowsEqual(y, y + 1, map))
            {
                if (RowsSurroundingAreEqual(y, y + 1, map))
                {
                    if (y*100 == ignore)
                    {
                        continue;
                    }
                    return y * 100;
                }
            }
        }

        return null;
    }

    private bool ColumnsSurroundingAreEqual(int x, int x2, Dictionary<P2D, char> map)
    {
        while (true)
        {
            if (AreColumnsEqual(x, x2, map))
            {
                x--;
                x2++;
                if (!map.Keys.Any(k => k.x == x) || !map.Keys.Any(k => k.x == x2))
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
    }

    private bool RowsSurroundingAreEqual(int y, int y2, Dictionary<P2D, char> map)
    {
        while (true)
        {
            if (AreRowsEqual(y, y2, map))
            {
                y--;
                y2++;
                if (!map.Keys.Any(k => k.y == y) || !map.Keys.Any(k => k.y == y2))
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
    }

    private bool AreRowsEqual(int y, int y2, Dictionary<P2D, char> map)
    {
        for (int x = 1; x <= map.MaxBy(p => p.Key.x).Key.x; x++)
        {
            if (map[new(x, y)] != map[new(x, y2)])
            {
                return false;
            }
        }
        return true;
    }

    private bool AreColumnsEqual(int x, int x2, Dictionary<P2D, char> map)
    {
        for (int y = 1; y <= map.MaxBy(p => p.Key.y).Key.y; y++)
        {
            if (map[new(x, y)] != map[new(x2, y)])
            {
                return false;
            }
        }
        return true;
    }

    private IEnumerable<Dictionary<P2D, char>> CreateMaps(string[] lines)
    {
        Dictionary<P2D, char> keyValuePairs = new Dictionary<P2D, char>();
        int y = 0;
        foreach (var line in lines)
        {
            for (int x = 0; x < line.Length; x++)
            {
                keyValuePairs[new(x + 1, y + 1)] = line[x];
            }

            y++;
            if (line.Length == 0)
            {
                yield return keyValuePairs;
                keyValuePairs = new Dictionary<P2D, char>();
                y = 0;
            }
        }
        yield return keyValuePairs;

    }

    public override async ValueTask<string> Solve_2()
    {
        var lines = Input.Zplit();
        var maps = CreateMaps(lines).ToList();
        return maps.Sum(Evaluate2).ToString();
    }

    private long Evaluate2(Dictionary<P2D, char> map)
    {
        var original = Evaluate(map)!.Value;

        foreach (var p in map)
        {
            var newDictionary = map.ToDictionary(x => x.Key, x => x.Key == p.Key ? x.Value == '.' ? '#' : '.' : x.Value);
            var newReflection = Evaluate(newDictionary, ignore: original);
            if (newReflection != null && newReflection.Value != original)
            {
                return newReflection.Value;
            }
        }
        return original;
    }
}
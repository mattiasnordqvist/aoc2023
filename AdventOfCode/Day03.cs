using System.Diagnostics;

namespace AdventOfCode;

public class Day03 : MyBaseDay
{
    public override async ValueTask<string> Solve_1()
    {
        var lines = Input.Zplit();
        var nrs = new Dictionary<(int x, int y), (int nr, int length)>();
        var symbols = new HashSet<(int x, int y)>();
        var gears = new HashSet<(int x, int y)>();
        for (int y = 0; y < lines.Length; y++)
        {
            var position = (-1, -1);
            var length = 0;
            var nr = 0;
            for (int x = 0; x < lines[y].Length; x++)
            {
                var c = lines[y][x];
                if (char.IsDigit(c))
                {
                    if (position == (-1, -1))
                    {
                        position = (x, y);
                    }
                    length++;
                    nr = nr * 10 + int.Parse(c + "");
                }
                else
                {
                    if (position != (-1, -1))
                    {
                        nrs[position] = (nr, length);
                        position = (-1, -1);
                        length = 0;
                        nr = 0;
                    }
                }
                if (c != '.' && !char.IsDigit(c))
                {
                    symbols.Add((x, y));
                    if (c == '*')
                    {
                        gears.Add((x, y));
                    }
                }
            }
            if (position != (-1, -1))
            {
                nrs[position] = (nr, length);
                position = (-1, -1);
                length = 0;
                nr = 0;
            }
        }
        var sum = 0;
        foreach (var candidate in nrs)
        {
            if (IsCloseToSymbol(candidate))
            {
                sum += candidate.Value.nr;
            }
        }

        Debug.Assert(sum == 559667);
        return sum.ToString();

        bool IsCloseToSymbol(KeyValuePair<(int x, int y), (int nr, int length)> candidate)
        {
            for (int x = -1; x < candidate.Value.length + 1; x++)
            {
                if (symbols.Contains((candidate.Key.x + x, candidate.Key.y))
                    || symbols.Contains((candidate.Key.x + x, candidate.Key.y - 1))
                    || symbols.Contains((candidate.Key.x + x, candidate.Key.y + 1)))
                {
                    return true;
                }
            }
            return false;
        }
    }

    public override async ValueTask<string> Solve_2()
    {
        var lines = Input.Zplit();
        var nrs = new Dictionary<(int x, int y), (int nr, int length)>();
        var symbols = new HashSet<(int x, int y)>();
        var gears = new HashSet<(int x, int y)>();
        for (int y = 0; y < lines.Length; y++)
        {
            var position = (-1, -1);
            var length = 0;
            var nr = 0;
            for (int x = 0; x < lines[y].Length; x++)
            {
                var c = lines[y][x];
                if (char.IsDigit(c))
                {
                    if (position == (-1, -1))
                    {
                        position = (x, y);
                    }
                    length++;
                    nr = nr * 10 + int.Parse(c + "");
                }
                else
                {
                    if (position != (-1, -1))
                    {
                        nrs[position] = (nr, length);
                        position = (-1, -1);
                        length = 0;
                        nr = 0;
                    }
                }
                if (c != '.' && !char.IsDigit(c))
                {
                    symbols.Add((x, y));
                    if (c == '*')
                    {
                        gears.Add((x, y));
                    }
                }
            }
            if (position != (-1, -1))
            {
                nrs[position] = (nr, length);
                position = (-1, -1);
                length = 0;
                nr = 0;
            }
        }
        var sumG = 0;

        foreach (var candidateGear in gears)
        {
            var surroundingNrs = SurroundingNrs(candidateGear);
            if (surroundingNrs.Count == 2)
            {
                sumG += surroundingNrs[0] * surroundingNrs[1];
            }
        }

        Debug.Assert(sumG == 86841457);
        return sumG.ToString();

        List<int> SurroundingNrs((int x, int y) gear)
        {
            var surroundingNrs = new List<int>();
            var nrc = nrs.Where(n =>
            (n.Key.y == gear.y - 1
            || n.Key.y == gear.y
            || n.Key.y == gear.y + 1)).ToList();
            foreach (var n in nrc)
            {
                for (int x = -1; x < n.Value.length + 1; x++)
                {
                    if (gear.x == n.Key.x + x)
                    {
                        surroundingNrs.Add(n.Value.nr);
                        break;
                    }
                }
            }
            return surroundingNrs;
        }
    }
}

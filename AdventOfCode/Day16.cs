using System.Numerics;
using System.Reflection.Emit;
using System.Xml.Linq;

namespace AdventOfCode;
public class Day16 : MyBaseDay
{
    private Dictionary<(int x, int y), char> map;

    public Day16()
    {
        map = Input.ToMap2();
    }
    public override async ValueTask<string> Solve_1()
    {
        return GetEnergy((-1, 0, 1, 0)).ToString();
    }

    private int GetEnergy((int x, int y, int dx , int dy) start)
    {
        var visited = new HashSet<(int x, int y, int dx, int dy)>();

        Stack<(int x, int y, int dx, int dy)> s = new();
        s.Push(start);

        while (s.Any())
        {
            var c = s.Pop();
            while (true)
            {

                if (visited.Contains(c)) break;
                visited.Add(c);
                c.x += c.dx;
                c.y += c.dy;
                if (!map.ContainsKey((c.x, c.y))) break;
                if (map[(c.x, c.y)] == '/' && c.dx != 0)
                {
                    c.dy = -c.dx;
                    c.dx = 0;
                }
                else if (map[(c.x, c.y)] == '\\' && c.dx != 0)
                {
                    c.dy = c.dx;
                    c.dx = 0;
                }
                else if (map[(c.x, c.y)] == '/' && c.dy != 0)
                {
                    c.dx = -c.dy;
                    c.dy = 0;
                }
                else if (map[(c.x, c.y)] == '\\' && c.dy != 0)
                {
                    c.dx = c.dy;
                    c.dy = 0;
                }
                else if (map[(c.x, c.y)] == '|' && c.dx != 0)
                {
                    s.Push((c.x, c.y, 0, -1));
                    s.Push((c.x, c.y, 0, 1));
                    break;
                }
                else if (map[(c.x, c.y)] == '-' && c.dy != 0)
                {
                    s.Push((c.x, c.y, -1, 0));
                    s.Push((c.x, c.y, 1, 0));
                    break;
                }
            }
        }

        return (visited.Select(x => (x.x, x.y)).Distinct().Count() - 1);
    }

    public override async ValueTask<string> Solve_2()
    {
        var fromleft = Enumerable.Range(0, map.Keys.Max(x => x.y) + 1).Select(y => (-1, y, 1, 0)).ToList();
        var fromright = Enumerable.Range(0, map.Keys.Max(x => x.y) + 1).Select(y => (map.Keys.Max(x => x.y) + 1, y, -1, 0)).ToList();

        var fromtop = Enumerable.Range(0, map.Keys.Max(x => x.x) + 1).Select(x => (x, -1, 0, 1)).ToList();
        var frombottom = Enumerable.Range(0, map.Keys.Max(x => x.x) + 1).Select(x => (x, map.Keys.Max(x => x.y) + 1, 0, -1)).ToList();

        return fromleft.Concat(fromright).Concat(fromtop).Concat(frombottom)
            .Select(GetEnergy).Max().ToString();
    }
}


using System.Collections.Generic;
using System.Diagnostics;


namespace AdventOfCode;
public class Day10 : MyBaseDay
{
    public override async ValueTask<string> Solve_1()
    {
        var map = Input.ToMap();

        var start = map.Where(x => x.Value == 'S').Single().Key;
        var positionsSurroundingStart = start.S4();
        var (pipesConnectingToStart, sChar) = FindConnectingPipes(positionsSurroundingStart, start, map);
        Debug.Assert(pipesConnectingToStart.Count() == 2);
        var (currentPos, nextPos) = (start, pipesConnectingToStart.First());
        var mainLoop = new HashSet<P2D>() { currentPos };
        while (true)
        {
            (currentPos, nextPos) = Move(map, currentPos, nextPos);
            mainLoop.Add(currentPos);
            if (nextPos == start)
            {
                break;
            }
        }

        return (mainLoop.Count() / 2).ToString();
    }

    private (P2D to, P2D next) Move(Dictionary<P2D, char> map, P2D from, P2D through)
    {

        if (from.IsDirectlyToTheLeftOf(through))
        {
            if (map[through] == '-') return (through, new(from.x + 2, from.y));
            if (map[through] == '7') return (through, new(from.x + 1, from.y + 1));
            if (map[through] == 'J') return (through, new(from.x + 1, from.y - 1));
        }
        if (from.IsDirectlyToTheRightOf(through))
        {
            if (map[through] == '-') return (through, new(from.x - 2, from.y));
            if (map[through] == 'F') return (through, new(from.x - 1, from.y + 1));
            if (map[through] == 'L') return (through, new(from.x - 1, from.y - 1));
        }
        if (from.IsDirectlyAbove(through))
        {
            if (map[through] == '|') return (through, new(from.x, from.y + 2));
            if (map[through] == 'J') return (through, new(from.x - 1, from.y + 1));
            if (map[through] == 'L') return (through, new(from.x + 1, from.y + 1));
        }
        if (from.IsDirectlyBelow(through))
        {
            if (map[through] == '|') return (through, new(from.x, from.y - 2));
            if (map[through] == '7') return (through, new(from.x - 1, from.y - 1));
            if (map[through] == 'F') return (through, new(from.x + 1, from.y - 1));
        }
        throw new Exception();
    }

    public bool AreConnected(P2D p1, P2D p2, Dictionary<P2D, char> map)
    {
        if (!p1.S4().Contains(p2)) return false;

        if (p2.IsDirectlyAbove(p1))
        {
            return (map[p1] == 'L' || map[p1] == 'J' || map[p1] == '|')
                && (map[p2] == 'F' || map[p2] == '7' || map[p2] == '|');
        }
        if (p2.IsDirectlyBelow(p1))
        {
            return (map[p2] == 'L' || map[p2] == 'J' || map[p2] == '|')
                && (map[p1] == 'F' || map[p1] == '7' || map[p1] == '|');
        }
        if (p2.IsDirectlyToTheLeftOf(p1))
        {
            return (map[p1] == '7' || map[p1] == 'J' || map[p1] == '-')
                && (map[p2] == 'F' || map[p2] == 'L' || map[p2] == '-');
        }
        if (p2.IsDirectlyToTheRightOf(p1))
        {
            return (map[p2] == '7' || map[p2] == 'J' || map[p2] == '-')
                && (map[p1] == 'F' || map[p1] == 'L' || map[p1] == '-');
        }
        return false;

    }
    private (List<P2D>, char) FindConnectingPipes(P2D[] positionsSurroundingStart, P2D p, Dictionary<P2D, char> map)
    {
        var possibleS = new HashSet<char> { 'F', '7', 'J', 'L', '|', '-' };
        var positions = new List<P2D>();
        foreach (var positionSurroundingStart in positionsSurroundingStart)
        {
            if (!map.ContainsKey(positionSurroundingStart)) continue;
            var pipe = map[positionSurroundingStart];
            if (positionSurroundingStart.IsDirectlyAbove(p))
            {
                if (pipe == 'F' || pipe == '|' || pipe == '7')
                {
                    possibleS.Remove('-');
                    possibleS.Remove('F');
                    possibleS.Remove('7');
                    positions.Add(positionSurroundingStart);
                }
            }
            if (positionSurroundingStart.IsDirectlyBelow(p))
            {
                if (pipe == 'J' || pipe == '|' || pipe == 'L')
                {
                    possibleS.Remove('-');
                    possibleS.Remove('J');
                    possibleS.Remove('L');
                    positions.Add(positionSurroundingStart);
                }
            }
            if (positionSurroundingStart.IsDirectlyToTheRightOf(p))
            {
                if (pipe == 'J' || pipe == '-' || pipe == '7')
                {
                    possibleS.Remove('|');
                    possibleS.Remove('J');
                    possibleS.Remove('7');
                    positions.Add(positionSurroundingStart);
                }
            }
            if (positionSurroundingStart.IsDirectlyToTheLeftOf(p))
            {
                if (pipe == 'F' || pipe == '-' || pipe == 'L')
                {
                    possibleS.Remove('|');
                    possibleS.Remove('F');
                    possibleS.Remove('L');
                    positions.Add(positionSurroundingStart);
                }
            }
        }
        return (positions, possibleS.FirstOrDefault());
    }

    public override async ValueTask<string> Solve_2()
    {
        var map = Input.ToMap();
        var start = map.Where(x => x.Value == 'S').Single().Key;
        var positionsSurroundingStart = start.S4();
        var (pipesConnectingToStart, sChar) = FindConnectingPipes(positionsSurroundingStart, start, map);
        map[start] = sChar;
        Debug.Assert(pipesConnectingToStart.Count() == 2);

        var (currentPos, nextPos) = (start, pipesConnectingToStart.First());


        var mainLoop = new Dictionary<P2D, char>() { { currentPos, 'S' } };
        var outside = new HashSet<P2D>();
        while (true)
        {
            var dir = nextPos.IsDirectlyAbove(currentPos) ? (0, -1)
            : nextPos.IsDirectlyBelow(currentPos) ? (0, 1)
            : nextPos.IsDirectlyToTheLeftOf(currentPos) ? (-1, 0)
            : (1, 0);
            outside.Add(currentPos.PoVLeft(currentPos, dir));
            outside.Add(currentPos.PoVLeft(nextPos, dir));

            (currentPos, nextPos) = Move(map, currentPos, nextPos);
            mainLoop.Add(currentPos, map[currentPos]);
            if (nextPos == start)
            {
                break;
            }
        }
        foreach (var p in map)
        {
            if (!mainLoop.ContainsKey(p.Key))
            {
                map[p.Key] = outside.Contains(p.Key) ? '*' : '.';
            }
        }
        
        var considered = new HashSet<P2D>();
        while (true)
        {
            var foundOne = false;
            foreach (var p in map.Where(x => x.Value == '*' && !considered.Contains(x.Key)))
            {
                considered.Add(p.Key);
                foreach (var s in p.Key.S8())
                {
                    if (map.ContainsKey(s) && map[s] == '.') { map[s] = '*'; foundOne = true; }
                }
            }
            if (!foundOne) break;
        }

        return map.Where(x => x.Value == '.').Count().ToString();
    }
}

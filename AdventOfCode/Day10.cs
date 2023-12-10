

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
        HashSet<(decimal x, decimal y)> nonSquezablePositions = [];
        

        HashSet<(decimal x, decimal y)> shortCuts = [];
        var map = Input.ToMap();
        var start = map.Where(x => x.Value == 'S').Single().Key;
        var positionsSurroundingStart = start.S4();
        var (pipesConnectingToStart, sChar) = FindConnectingPipes(positionsSurroundingStart, start, map);
        map[start] = sChar;
        Debug.Assert(pipesConnectingToStart.Count() == 2);
        var (currentPos, nextPos) = (start, pipesConnectingToStart.First());
        nonSquezablePositions.Add(Between(currentPos, nextPos));
        var mainLoop = new Dictionary<P2D, char>() { { currentPos, 'S' } };
        while (true)
        {
            (currentPos, nextPos) = Move(map, currentPos, nextPos);
            nonSquezablePositions.Add(Between(currentPos, nextPos));
            mainLoop.Add(currentPos, map[currentPos]);
            if (nextPos == start)
            {
                break;
            }
        }
        var maxx = map.Keys.Select(x => x.x).Max();
        var maxy = map.Keys.Select(x => x.y).Max();

        var emptyCount = map.Values.Where(x => x == ' ').Count();
        var squeezeEnabled = false;
        var considerUs = map.ToDictionary();
        var reconsiderUs = considerUs.ToDictionary();
        var deemedOutThisRound = new HashSet<P2D>();
        var deemedOutThisRound2 = new HashSet<(decimal x, decimal y)>();
        while (true)
        {
            deemedOutThisRound = new HashSet<P2D>();
            deemedOutThisRound2 = new HashSet<(decimal x, decimal y)>();
            foreach (var p in considerUs)
            {
                if (deemedOutThisRound.Contains(p.Key))
                {
                    continue;
                }
                if (mainLoop.Keys.Contains(p.Key))
                    if (mainLoop.Keys.Contains(p.Key))
                    {
                        considerUs.Remove(p.Key); continue;
                    };
                if (map[p.Key] == ' ')
                {
                    considerUs.Remove(p.Key); continue;
                }
                if (p.Key.x == 0) { map[p.Key] = ' '; considerUs.Remove(p.Key); continue; }
                if (p.Key.y == 0) { map[p.Key] = ' '; considerUs.Remove(p.Key); continue; }
                if (p.Key.x == maxx) { map[p.Key] = ' '; considerUs.Remove(p.Key); continue; }
                if (p.Key.y == maxy) { map[p.Key] = ' '; considerUs.Remove(p.Key); continue; }
                if (p.Key.S8().Any(p => map.ContainsKey(p) && map[p] == ' ')) { map[p.Key] = ' '; considerUs.Remove(p.Key); continue; }
                if (squeezeEnabled)
                {
                    var shouldReconsider = false;
                    var reachedOut = CanReachOut(p.Key, ref shouldReconsider);
                    if (reachedOut) { map[p.Key] = ' '; considerUs.Remove(p.Key); }
                    else if (!shouldReconsider) { considerUs.Remove(p.Key); }
                }
            }
            var newEmptyCount = map.Values.Where(x => x == ' ').Count();
            if (newEmptyCount == emptyCount && squeezeEnabled) { break; }
            if (newEmptyCount == emptyCount && !squeezeEnabled) { squeezeEnabled = true; }
            else
            {
                emptyCount = newEmptyCount;
            }
        }
        foreach (var candidate in map.Where(x => x.Value != ' ' && !mainLoop.ContainsKey(x.Key)))
        {
            map[candidate.Key] = '.';
        }

        return map.Values.Where(x => x == '.').Count().ToString();

        bool CanReachOut(P2D p, ref bool shouldReconsider)
        {
            var visitedSqueezes = new HashSet<(decimal x, decimal y)>();
            var surroundingSqueezes = SurroundingSqueezes(p).Except(nonSquezablePositions).ToHashSet();
            if (surroundingSqueezes.Count == 0) return false;
            else
            {
                foreach (var surroundingSqueeze in surroundingSqueezes)
                {
                    if (CanReachOutFromSqueeze(surroundingSqueeze, visitedSqueezes, ref shouldReconsider))
                    {
                        return true;
                    }
                }
                deemedOutThisRound.Add(p);
                return false;
            }
        }
        bool CanReachOutFromSqueeze((decimal x, decimal y) p, HashSet<(decimal x, decimal y)> visited, ref bool shouldReconsider)
        {
            if (deemedOutThisRound2.Contains(p))
            {
                shouldReconsider = true;
                return false;
            }
            if (shortCuts.Contains(p))
            {
                return true;
            }
            visited.Add(p);
            if (SurroundingNormal(p).Any(u => !map.ContainsKey(u) || map[u] == ' '))
            {
                shortCuts.Add(p);
                return true;
            }

            if (!shouldReconsider && SurroundingNormal(p).Any(u => map.ContainsKey(u) && map[u] == '.'))
            {
                shouldReconsider = true;
            }

            var canReachOutFromSqueeze = false;
            var a = SurroundingSqueezesFromSqueeze(p)
                .Except(visited)
                .Except(nonSquezablePositions).ToList();
            foreach (var s in a)
            {
                if (CanReachOutFromSqueeze(s, visited, ref shouldReconsider))
                {
                    canReachOutFromSqueeze = true; break;
                }
            }
            if (canReachOutFromSqueeze)
            {
                shortCuts.Add(p);
                return true;
            }
            else
            {
                deemedOutThisRound2.Add(p);
                return false;
            }

        }
    }

    

    private HashSet<P2D> SurroundingNormal((decimal x, decimal y) p)
    {
        var h = new HashSet<P2D>();

        if (p.x == (int)p.x)
        {
            h.Add(new((int)p.x - 1, (int)(p.y + 0.5m)));
            h.Add(new((int)p.x + 1, (int)(p.y + 0.5m)));
            h.Add(new((int)p.x - 1, (int)(p.y - 0.5m)));
            h.Add(new((int)p.x + 1, (int)(p.y - 0.5m)));
        }
        else
        {
            h.Add(new((int)(p.x + 0.5m), (int)p.y - 1));
            h.Add(new((int)(p.x + 0.5m), (int)p.y + 1));
            h.Add(new((int)(p.x - 0.5m), (int)p.y - 1));
            h.Add(new((int)(p.x - 0.5m), (int)p.y + 1));
        }
        return h;
    }

    private HashSet<(decimal x, decimal y)> SurroundingSqueezesFromSqueeze((decimal x, decimal y) p)
    {
        if (p.x == (int)p.x)
        {
            return new HashSet<(decimal x, decimal y)>
            {
                { (p.x-1, p.y) },
                { (p.x+1, p.y) },
                { (p.x-0.5m, p.y-0.5m) },
                { (p.x-0.5m, p.y+0.5m) },
                { (p.x+0.5m, p.y-0.5m) },
                { (p.x+0.5m, p.y+0.5m) }
            };
        }
        else
        {
            return new HashSet<(decimal x, decimal y)>
            {
                { (p.x, p.y-1) },
                { (p.x, p.y+1) },
                { (p.x-0.5m, p.y-0.5m) },
                { (p.x-0.5m, p.y+0.5m) },
                { (p.x+0.5m, p.y-0.5m) },
                { (p.x+0.5m, p.y+0.5m) }
            };
        }
    }

    private HashSet<(decimal x, decimal y)> SurroundingSqueezes(P2D p)
    {
        return new HashSet<(decimal x, decimal y)>
        {
            { (p.x-0.5m, p.y-1) },
            { (p.x+0.5m, p.y-1) },
            { (p.x-0.5m, p.y+1) },
            { (p.x+0.5m, p.y+1) },
            { (p.x-1, p.y-0.5m) },
            { (p.x-1, p.y+0.5m) },
            { (p.x+1, p.y-0.5m) },
            { (p.x+1, p.y+0.5m) },
        };
    }

    private (decimal x, decimal y) Between(P2D currentPos, P2D nextPos)
    {
        if (currentPos.IsDirectlyAbove(nextPos))
        {
            return (nextPos.x, nextPos.y - 0.5m);
        }
        if (currentPos.IsDirectlyBelow(nextPos))
        {
            return (nextPos.x, nextPos.y + 0.5m);
        }
        if (currentPos.IsDirectlyToTheLeftOf(nextPos))
        {
            return (nextPos.x - 0.5m, nextPos.y);
        }
        if (currentPos.IsDirectlyToTheRightOf(nextPos))
        {
            return (nextPos.x + 0.5m, nextPos.y);
        }
        throw new Exception();
    }
}

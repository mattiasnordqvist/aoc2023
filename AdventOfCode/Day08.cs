
using System.Diagnostics;

namespace AdventOfCode;

public class Day08 : MyBaseDay
{
    public override async ValueTask<string> Solve_1()
    {
        var lines = Input.Zplit();
        var instructions = lines[0];
        var map = lines[2..].ToDictionary(x => x.Zplit("=")[0], x => x.Zplit("=")[1].Zplit(ss => (l: ss[0][1..], r: ss[1][0..^1]), ","));

        var pos = "AAA";
        var steps = 0;
        while (pos != "ZZZ")
        {
            pos = instructions[steps % instructions.Length] == 'R' 
                ? map[pos].r 
                : map[pos].l;
            steps++;
        }
        return steps.ToString();

    }


    public override async ValueTask<string> Solve_2()
    {
        var lines = Input.Zplit();
        var instructions = lines[0];

        var map = lines[2..].ToDictionary(x => x.Zplit("=")[0], x => (l: x.Zplit("=")[1].Zplit(",")[0].Replace("(", ""), r: x.Zplit("=")[1].Zplit(",")[1].Replace(")", "")));
        var positions = map.Keys.Where(x => x.EndsWith("A")).ToList();
        var paths = positions.Select(x => (x, BuildPath(x))).ToDictionary(x => x.x, x=> x.Item2);
        List<(string p, int steps)> BuildPath(string start)
        {
            var stepsSoFar = 0;
            var pos = start;
            string? firstZ = null;
            List<(string p, int steps)> path = [];
            while (true)
            {
                var (newpos, steps) = FindNextZ(pos, stepsSoFar);
                
                path.Add((newpos, steps));
                stepsSoFar += steps;
                pos = newpos;
                if(firstZ == null)
                {
                    firstZ = newpos;
                }
                else if (newpos == firstZ)
                {
                    path = path[0..(path.Count - 1)];
                    return path;
                }
            }
        }

        (string p, int steps) FindNextZ(string start, int stepsSoFar)
        {
            var pos = start;
            var steps = 0;
            var index = (stepsSoFar ) % instructions.Length;
            while (true)
            {
                if (instructions[index] == 'R')
                {
                    pos = map[pos].r;
                }
                else
                {
                    pos = map[pos].l;
                }

                index += 1;
                index = index % instructions.Length;
                steps++;
                if (pos.EndsWith("Z"))
                {
                    return (pos, steps);
                }
            }
        }
        Debug.Assert(paths.All(x => x.Value.Count == 1));
        return MathHelpers.LeastCommonMultiple<long>(paths.Select(x => (long)x.Value[0].steps)).ToString();
    }
}
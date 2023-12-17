namespace AdventOfCode;
public class Day01 : MyBaseDay
{
    public override async ValueTask<string> Solve_1()
    {
        var endingPos = Input
             .Zplit(",")
             .Aggregate((x: 0, y: 0, dir: 0), (pos, move) =>
             {
                 var (x, y, dir) = pos;
                 var (turn, steps) = (move[0], int.Parse(move[1..]));
                 dir = (dir + (turn == 'R' ? 1 : 3)) % 4;
                 return dir switch
                 {
                     0 => (x, y + steps, dir),
                     1 => (x + steps, y, dir),
                     2 => (x, y - steps, dir),
                     3 => (x - steps, y, dir),
                     _ => throw new Exception()
                 };
             });
        return (Math.Abs(endingPos.x) + Math.Abs(endingPos.y)).ToString();
    }

    public override async ValueTask<string> Solve_2()
    {
        var moves = Input
            .Zplit(",");
        var (x, y, dir) = (0,0,0);
        var visited = new HashSet<(int x, int y)>();
        visited.Add((0, 0));
        foreach (var move in moves)
        {
            var (turn, steps) = (move[0], int.Parse(move[1..]));
            dir = (dir + (turn == 'R' ? 1 : 3)) % 4;
            for(int step = 0; step<steps; step++)
            {
                (x, y) = dir switch
                {
                    0 => (x, y + 1),
                    1 => (x + 1, y),
                    2 => (x, y - 1),
                    3 => (x - 1, y),
                    _ => throw new Exception()
                };
                if (visited.Contains((x, y)))
                {
                    return (Math.Abs(x) + Math.Abs(y)).ToString();
                }
                else
                {
                    visited.Add((x, y));
                }
            }
        }
        throw new Exception();
    }
}

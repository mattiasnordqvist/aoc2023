namespace AdventOfCode;
public class Day17 : MyBaseDay
{
    private Dictionary<(int x, int y), char> map;
    private int maxx;
    private int maxy;

    public Day17()
    {
        map = Input.ToMap2();
        maxx = map.Keys.Max(x => x.x);
        maxy = map.Keys.Max(x => x.y);
    }

    public override async ValueTask<string> Solve_1()
    {
        var visited = new Dictionary<(int x, int y, int dir, int steps), int>();
        PriorityQueue<(int x, int y, int dir, int steps), int> queue = new();
        queue.Enqueue((0, 0, 0, 3), 0);
        queue.Enqueue((0, 0, 3, 3), 0);
        visited.Add((0, 0, 0, 3), 0);
        visited.Add((0, 0, 3, 3), 0);
        while (true)
        {
            var (x, y, d, s) = queue.Dequeue();
            var accHeatLoss = visited[(x, y, d, s)];
            if (x == maxx && y == maxy)
            {
                return visited.Where(v => v.Key.x == x && v.Key.y == y).Min(m => m.Value).ToString();
            }

            int[] newDirs = [(d + 1) % 4, (d + 3) % 4];
            foreach (var newDir in newDirs)
            {
                var (dx, dy) = newDir switch
                {
                    0 => (1, 0),
                    1 => (0, -1),
                    2 => (-1, 0),
                    3 => (0, 1),
                };
                var addedHeatLoss = 0;
                for (int i = 1; i <= 3; i++)
                {
                    var newNode = (x: x + dx * i, y: y + dy * i, d: newDir, s: i);
                    if (newNode.x < 0 || newNode.x > maxx || newNode.y < 0 || newNode.y > maxy) { continue; }
                    addedHeatLoss += map[(newNode.x, newNode.y)] - 48;
                    var newTotalHeatloss = accHeatLoss + addedHeatLoss;
                    if (!visited.ContainsKey(newNode))
                    {
                        queue.Enqueue(newNode, newTotalHeatloss);
                        visited.Add(newNode, newTotalHeatloss);
                    }
                    else if (newTotalHeatloss < visited[newNode])
                    {
                        queue.Enqueue(newNode, newTotalHeatloss);
                        visited[newNode] = newTotalHeatloss;
                    }
                }
            }
        }

        throw new Exception();
    }


    public override async ValueTask<string> Solve_2()
    {
        var visited = new Dictionary<(int x, int y, int dir, int steps), int>();
        PriorityQueue<(int x, int y, int dir, int steps), int> queue = new();
        queue.Enqueue((0, 0, 0, 10), 0);
        queue.Enqueue((0, 0, 3, 10), 0);
        visited.Add((0, 0, 0, 10), 0);
        visited.Add((0, 0, 3, 10), 0);
        while (true)
        {
            var (x, y, d, s) = queue.Dequeue();
            var accHeatLoss = visited[(x, y, d, s)];
            if (x == maxx && y == maxy)
            {
                return visited.Where(v => v.Key.x == x && v.Key.y == y).Min(m => m.Value).ToString();
            }

            int[] newDirs = [(d + 1) % 4, (d + 3) % 4];
            foreach (var newDir in newDirs)
            {
                var (dx, dy) = newDir switch
                {
                    0 => (1, 0),
                    1 => (0, -1),
                    2 => (-1, 0),
                    3 => (0, 1),
                };
                var addedHeatLoss = 0;
                for (int i = 1; i <= 10; i++)
                {
                    var newNode = (x: x + dx * i, y: y + dy * i, d: newDir, s: i);
                    if (newNode.x < 0 || newNode.x > maxx || newNode.y < 0 || newNode.y > maxy) { continue; }
                    addedHeatLoss += map[(newNode.x, newNode.y)] - 48;
                    if (i >= 4)
                    {
                        var newTotalHeatloss = accHeatLoss + addedHeatLoss;
                        if (!visited.ContainsKey(newNode))
                        {
                            queue.Enqueue(newNode, newTotalHeatloss);
                            visited.Add(newNode, newTotalHeatloss);
                        }
                        else if (newTotalHeatloss < visited[newNode])
                        {
                            queue.Enqueue(newNode, newTotalHeatloss);
                            visited[newNode] = newTotalHeatloss;
                        }
                    }
                }
            }
        }
    }
}


namespace AdventOfCode;
public class Day14 : MyBaseDay
{
    public override async ValueTask<string> Solve_1()
    {
        var map = Input.ToMap();
        var maxy = map.Keys.MaxBy(x => x.y).y;
        var maxx = map.Keys.MaxBy(x => x.x).x;
        TiltNorth();
        var load = CalculateLoad(map);
        
        return load.ToString();
        void TiltNorth()
        {
            for (int y = 1; y <= maxy; y++)
            {
                for (int x = 0; x <= maxx; x++)
                {
                    if (map[(new(x, y))] == 'O')
                    {
                        var i = 0;
                        while (y - (i + 1) >= 0 && map[new(x, y - (i + 1))] == '.')
                        {
                            i++;
                        }
                        if (i != 0)
                        {
                            map[(new(x, y))] = '.';
                            map[(new(x, y - (i)))] = 'O';

                        }
                    }
                }
            }
        }

        void TiltSouth()
        {
            for (int y = maxy-1; y >= 0; y--)
            {
                for (int x = 0; x <= maxx; x++)
                {
                    if (map[(new(x, y))] == 'O')
                    {
                        var i = 0;
                        while (y + (i + 1) >= 0 && map[new(x, y + (i + 1))] == '.')
                        {
                            i++;
                        }
                        if (i != 0)
                        {
                            map[(new(x, y))] = '.';
                            map[(new(x, y + (i)))] = 'O';

                        }
                    }
                }
            }
        }
        int CalculateLoad(Dictionary<P2D, char> map)
        {
            return map.Where(x => x.Value == 'O').Sum(x => maxy - x.Key.y + 1);
        }
    }

    public override async ValueTask<string> Solve_2()
    {
        var map = Input.ToMap();
        var maxy = map.Keys.MaxBy(x => x.y).y;
        var maxx = map.Keys.MaxBy(x => x.x).x;
        var totalCycles = 1000000000;
        var warmup = 500;
        List<int> sequence = new List<int>();
        for (int a = 0;a < warmup; a++)
        {
            Cycle();
            sequence.Add(CalculateLoad(map));
        }
        var (cycleStart, cycleLength) = DetectCycle(sequence);
        totalCycles -= warmup;
        totalCycles = totalCycles % cycleLength;
        for(int a = 0; a <totalCycles; a++)
        {
            Cycle();
        }

        var load = CalculateLoad(map);
        void Cycle()
        {
            TiltNorth();
            TiltWest();
            TiltSouth();
            TiltEast();
        }
        return load.ToString();
        void TiltNorth()
        {
            for (int y = 1; y <= maxy; y++)
            {
                for (int x = 0; x <= maxx; x++)
                {
                    if (map[(new(x, y))] == 'O')
                    {
                        var i = 0;
                        while (y - (i + 1) >= 0 && map[new(x, y - (i + 1))] == '.')
                        {
                            i++;
                        }
                        if (i != 0)
                        {
                            map[(new(x, y))] = '.';
                            map[(new(x, y - (i)))] = 'O';

                        }
                    }
                }
            }
        }

        void TiltWest()
        {
            for (int x = 1; x <= maxx; x++)
            {
                for (int y = 0; y <= maxy; y++)
                {
                    if (map[(new(x, y))] == 'O')
                    {
                        var i = 0;
                        while (x - (i + 1) >= 0 && map[new(x - (i + 1), y)] == '.')
                        {
                            i++;
                        }
                        if (i != 0)
                        {
                            map[(new(x, y))] = '.';
                            map[(new(x - (i), y))] = 'O';

                        }
                    }
                }
            }
        }

        void TiltSouth()
        {
            for (int y = maxy - 1; y >= 0; y--)
            {
                for (int x = 0; x <= maxx; x++)
                {
                    if (map[(new(x, y))] == 'O')
                    {
                        var i = 0;
                        while (y + (i + 1) <= maxy && map[new(x, y + (i + 1))] == '.')
                        {
                            i++;
                        }
                        if (i != 0)
                        {
                            map[(new(x, y))] = '.';
                            map[(new(x, y + (i)))] = 'O';

                        }
                    }
                }
            }
        }

        void TiltEast()
        {
            for (int x = maxx - 1; x >= 0; x--)
            {
                for (int y = 0; y <= maxy; y++)
                {
                    if (map[(new(x, y))] == 'O')
                    {
                        var i = 0;
                        while (x + (i + 1) <= maxx && map[new(x + (i + 1), y)] == '.')
                        {
                            i++;
                        }
                        if (i != 0)
                        {
                            map[(new(x, y))] = '.';
                            map[(new(x+i, y))] = 'O';

                        }
                    }
                }
            }
        }
        int CalculateLoad(Dictionary<P2D, char> map)
        {
            return map.Where(x => x.Value == 'O').Sum(x => maxy - x.Key.y + 1);
        }
    }

    /// <summary>
    /// Using the tortoise and hare algorithm to detect a cycle in the sequence
    /// </summary>
    public (int cycleStart, int cycleLength) DetectCycle(List<int> sequence)
    {
        var hare = 0;
        var tortoise = 0;
        while (true)
        {
            hare += 2;
            tortoise += 1;
            if (sequence[hare] == sequence[tortoise])
            {
                tortoise = 0; break;
            }
        }
        var cycleLength = 0;
        while (true)
        {
            cycleLength += 2;
            hare += 1;
            tortoise += 1;
            if(sequence[hare] == sequence[tortoise])
            {
                return (tortoise, cycleLength);
            }
        }
    }
}
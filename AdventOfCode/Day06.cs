
using System.Diagnostics;

namespace AdventOfCode;

public class Day06 : MyBaseDay
{
    public override async ValueTask<string> Solve_1()
    {
        var lines = _input.Zplit("\n");
        var times = lines[0].Zplit(":")[1].Zplit(" ").ia();
        var distances = lines[1].Zplit(":")[1].Zplit(" ").ia();

        var winnings = new List<int>();
        for (int i = 0; i < times.Length; i++)
        {
            var time = times[i];
            var distance = distances[i];

            var goodTimes = new List<int>();
            for (int j = 0; j < time; j++)
            {
                var myDistance = MyDistance(j, time);
                if (myDistance > distance)
                {
                    goodTimes.Add(j);
                }
            }
            winnings.Add(goodTimes.Count);

        }
        var result = winnings.Aggregate(1, (a, b) => a * b);
        Debug.Assert(result == 32076);
        return result.ToString();
    }

    private long MyDistance(long hold, long time)
    {
        long distance = 0;
        long speed = hold;
        distance = speed * (time - hold);
        return distance;
    }

    public override async ValueTask<string> Solve_2()
    {
        var lines = _input.Zplit("\n");
        var time = string.Join("", lines[0].Zplit(":")[1].Zplit(" ")).l();
        var distance = string.Join("", lines[1].Zplit(":")[1].Zplit(" ")).l();

        long? firstGoodTime = null;
        long? lastGoodTime = null;
        for (long j = 0; j < time; j++)
        {
            var myDistance = MyDistance(j, time);
            if (myDistance > distance)
            {
                firstGoodTime = j;
                break;
            }
        }
        for (long j = time - 1; j >=0; j--)
        {
            var myDistance = MyDistance(j, time);
            if (myDistance > distance)
            {
                lastGoodTime = j;
                break;
            }
        }

        var goodTimes = (lastGoodTime - firstGoodTime) + 1;
        Debug.Assert(goodTimes == 34278221);
        return goodTimes.ToString();
    }
}

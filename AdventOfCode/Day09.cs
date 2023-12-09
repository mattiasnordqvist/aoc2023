namespace AdventOfCode;

public class Day09 : MyBaseDay
{
    public override async ValueTask<string> Solve_1()
    {
        var lines = Input.Zplit();
        var sum = lines
            .Select(line => line.Split(" ").la())
            .Select(FindNext)
            .Sum();
        return sum.ToString();
    }

    private long FindNext(long[] history)
    {
        var sequences = FindSequences(history);
        long next = 0;
        foreach (var sequence in sequences.Reverse<long[]>().Skip(1))
        {
            next = next + sequence.Last();
        }
        return next;

    }

    private List<long[]> FindSequences(long[] history)
    {
        var sequences = new List<long[]>();
        sequences.Add(history);
        while (!sequences.Last().All(x => x == 0))
        {
            var difference = Difference(sequences.Last());
            sequences.Add(difference);
        }
        return sequences;
    }

    private long[] Difference(long[] longs)
    {
        var result = new long[longs.Length - 1];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = longs[i + 1] - longs[i];
        }
        return result;
    }

    public override async ValueTask<string> Solve_2()
    {
        var lines = Input.Zplit();
        var sum = lines
            .Select(line => line.Split(" ").la())
            .Select(FindNextBackwards)
            .Sum();
        return sum.ToString();
    }

    private long FindNextBackwards(long[] history)
    {
        var sequences = FindSequences(history);
        long next = 0;
        var reversed = sequences.Reverse<long[]>().ToArray();
        for(int i=0;i<reversed.Count()-1; i++)
        {
            next = (reversed[i+1].First() - next);

        }
        return next;

    }
}

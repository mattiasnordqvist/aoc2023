using System.Diagnostics;

namespace AdventOfCode;

public class Day04 : MyBaseDay
{
    public record Card(int nr, int matching);

    public override ValueTask<string> Solve_1()
    {
        throw new NotImplementedException();
    }

    public override ValueTask<string> Solve_2()
    {
        var lines = _input.Zplit();
        var cards = lines.Select(CreateCard).ToList();
        for (int i = 0; i < cards.Count; i++)
        {
            var nr = cards[i].nr;
            var matching = cards[i].matching;
            for (int m = 0; m < matching; m++)
            {
                cards.Add(cards[nr + m]);
            }
        }
        var result = cards.Count();
        Debug.Assert(result == 15455663);
        return new ValueTask<string>(result.ToString());
        
    }

    private static Card CreateCard(string line)
    {
        var nr = int.Parse(line.Split(":")[0].Substring(5));

        var a = line.Split(":")[1].Split("|");
        var w = a[0];
        var c = a[1];

        var wc = w.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x.Trim())).ToArray();
        var my = c.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x.Trim())).ToArray();

        var m = (my.Intersect(wc).Count());
        return new Card(nr, m);
    }
}

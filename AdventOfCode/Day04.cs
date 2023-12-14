using System.Diagnostics;

namespace AdventOfCode;

public class Day04 : MyBaseDay
{
    public record Card(int nr, int matching);

    public override async ValueTask<string> Solve_1()
    {
        return Input.Zplit()
            .Select(x => x.Zplit(":")[1])
            .Select(x => (winning: x.Zplit("|")[0].Zplit(" ").ToHashSet(), mycards: x.Zplit("|")[1].Zplit(" ").ToHashSet()))
            .Select(x => x.winning.Intersect(x.mycards).Count())
            .Sum(x => x == 1 ? 1 : (int)Math.Pow(2, x-1))
            .ToString();
    }

    public override ValueTask<string> Solve_2()
    {
        var lines = Input.Zplit();
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




using System.Reflection.Emit;
using System.Xml.Linq;

namespace AdventOfCode;
public class Day15 : MyBaseDay
{
    public override async ValueTask<string> Solve_1()
    {
        return Input
            .Zplit(",")
            .Select(Hash)
            .Sum()
            .ToString();
    }

    private int Hash(string x) => x.Aggregate(0, (a, b) => (a + b) * 17 % 256);

    public override async ValueTask<string> Solve_2()
    {
        var d = new Dictionary<int, List<string>>();
        foreach (var letters in Input.Zplit(","))
        {
            var label = letters.Split(['=', '-'])[0];
            var hash = Hash(label);

            if (letters.Contains('='))
            {
                if (!d.ContainsKey(hash))
                {
                    d.Add(hash, []);
                }
                var value = d[hash];
                var match = value.FirstOrDefault(x => x.StartsWith(label));
                if(match != null)
                {
                    var index = value.IndexOf(match);
                    value.RemoveAt(index);
                    value.Insert(index, letters);
                }
                else
                {
                    value.Add(letters);
                }
            }
            else
            {
                if (d.TryGetValue(hash, out List<string>? value))
                {
                    var match = value.FirstOrDefault(x => x.StartsWith(label));
                    if(match != null)
                    {
                        value.Remove(match);
                    }
                }
            }
        }

        var sum = d.Sum(b => (b.Key + 1) * b.Value.Aggregate((slot: 1, s: 0), (a, b) => (slot: a.slot+1, s: a.s + (a.slot * b.Split("=")[1].i()))).s);
        return sum.ToString();
    }
}
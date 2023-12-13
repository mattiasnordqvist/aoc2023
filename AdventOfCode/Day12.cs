namespace AdventOfCode;
public class Day12 : MyBaseDay
{
    public override async ValueTask<string> Solve_1()
    {
        var lines = Input.Zplit();
        var s = lines.Select(x => new Line(x).CountArrangements()).Sum();
        return s.ToString();
    }

    public override async ValueTask<string> Solve_2()
    {
        Console.WriteLine("Part 2");
        var lines = Input.Zplit().Select(x =>
        {
            return x.Split(" ")[0] + "?" + x.Split(" ")[0] + "?" + x.Split(" ")[0] + "?" + x.Split(" ")[0] + "?" + x.Split(" ")[0]
            + " " + x.Split(" ")[1] + "," + x.Split(" ")[1] + "," + x.Split(" ")[1] + "," + x.Split(" ")[1] + "," + x.Split(" ")[1];

        }).ToList();

        var s = lines.Select(x => new Line(x).CountArrangements()).Sum();
        return s.ToString();
    }

    public static Dictionary<string, long> EatHashtags(int amountToEat, string textToEatFrom)
    {
        textToEatFrom = textToEatFrom.TrimEnd('.').TrimStart('.');
        if (textToEatFrom.Length == amountToEat)
        {
            if (textToEatFrom.Replace(".", "").Length == amountToEat)
            {
                return new Dictionary<string, long>()
                {
                    { "", 1 }
                };
            }
            else
            {
                return new Dictionary<string, long>() { { "", 0 } };
            }
        }
        else if (textToEatFrom.Length < amountToEat)
        {
            return new Dictionary<string, long>() { { "", 0 } };
        }
        else if (textToEatFrom.Length > amountToEat)
        {
            var allVariants = new CountDictionary<string>();
            while (textToEatFrom[0] is '#' or '?')
            {
                var candidate = textToEatFrom[0..amountToEat];
                var afterCandidate = textToEatFrom[amountToEat];
                if (candidate.Contains('.'))
                {
                    if (candidate[0] == '?')
                    {
                        return EatHashtags(amountToEat, textToEatFrom[1..]);
                    }
                    else { 
                        return new Dictionary<string, long>() { { textToEatFrom, 0 } };
                    }
                }
                else if (textToEatFrom[0] is '#' && afterCandidate == '#')
                {
                    return new Dictionary<string, long>() { { string.Join("", textToEatFrom.SkipWhile(x => x == '#')), 0 } };
                }
                else
                {
                    if (afterCandidate == '?' || afterCandidate == '.')
                    {
                        allVariants.AddCount(textToEatFrom[(amountToEat + 1)..].TrimEnd('.').TrimStart('.'), 1);
                        if (textToEatFrom[0] is '?')
                        {
                            var nextEat = EatHashtags(amountToEat, textToEatFrom[1..]);
                            foreach (var n in nextEat)
                            {
                                allVariants.AddCount(n.Key, n.Value);
                            }
                        }
                    }
                    if (textToEatFrom[0] is '?' && afterCandidate == '#')
                    {
                        var nextEat = EatHashtags(amountToEat, textToEatFrom[1..]);
                        foreach (var n in nextEat)
                        {
                            allVariants.AddCount(n.Key, n.Value);
                        }
                    }
                    return allVariants;
                }
            }
            return allVariants;
        }
        throw new Exception();
    }
}

public class Line
{
    private string _x;
    private string _text;
    private int[] _groups;

    public Line(string x)
    {
        _x = x;
        _text = x.Zplit(" ")[0];
        _groups = x.Zplit(" ")[1].Zplit(",").ia();
    }
    public long CountArrangements()
    {
        var a = CountArrangements(_groups, _text);
        Console.WriteLine(_x + ": " + a);
        return a;
    }

    public long CountArrangements(int[] groups, string text)
    {
        if(groups.Count() == 0 && text == "")
        {
            return 1;
        }
        var d = Day12.EatHashtags(groups[0], text).Where(x => x.Value != 0).ToList();
        var sum = 0l;
        foreach(var kv in d)
        {
            var worth = groups.Length == 1 && kv.Key.Contains('#') ? 0
                : (groups.Length > 1 ? CountArrangements(groups[1..], kv.Key) : 1);

            sum += kv.Value * worth;
        }
        return sum;

        //var x = Day12.Eat(groups, text);
        //var rT = x.RemainingText;
        //var rG = x.RemainingGroups;
        //if (rT.Length == 0 && rG.Length > 0) return 0;
        //if (rG.Length == 0 && -1 == rT.IndexOfAny(['#', '?']))
        //{
        //    return 1;
        //}
        //else if (rT.Contains('?'))
        //{
        //    var i = rT.IndexOf('?');

        //    return CountArrangements(rG, rT[0..i].ToString() + "." + rT[(i + 1)..].ToString()) + CountArrangements(rG, rT[0..i].ToString() + '#' + rT[(i + 1)..].ToString());
        //}
        //return 0;
    }
}
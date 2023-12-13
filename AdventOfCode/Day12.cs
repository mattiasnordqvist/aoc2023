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

    //public ref struct EatResult(int[] remainingGroups, ReadOnlySpan<char> remainingText)
    //{
    //    public int[] RemainingGroups { get; } = remainingGroups;
    //    public ReadOnlySpan<char> RemainingText { get; } = remainingText;
    //}


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
                    return new Dictionary<string, long>() { { textToEatFrom, 0 } };
                }
                else if (textToEatFrom[0] is '#' && afterCandidate == '#')
                {
                    return new Dictionary<string, long>() { { string.Join("", textToEatFrom.SkipWhile(x => x == '#')), 0 } };
                }
                else if (afterCandidate == '?' || afterCandidate == '.')
                {
                    allVariants.AddCount(textToEatFrom[(amountToEat)..].TrimEnd('.').TrimStart('.'),1);
                    if (textToEatFrom[0] is '?')
                    {
                        var nextEat = EatHashtags(amountToEat, textToEatFrom[1..]);
                        foreach (var n in nextEat)
                        {
                            allVariants.AddCount(n.Key, n.Value);
                        }
                    }
                }
                return allVariants;
            }
            return allVariants;
        }
        throw new Exception();
    }

    //public static EatResult Eat(int[] groups, ReadOnlySpan<char> text)
    //{
    //    if (groups.Count() == 0)
    //    {
    //        var i = text.IndexOfAnyExcept('.');
    //        if (i == -1)
    //        {
    //            return new(groups, "");
    //        }
    //        return new(groups, text[i..]);
    //    }
    //    var nextMatch = new string('#', groups[0]);
    //    if (text.Length == 0)
    //    {
    //        return new(groups, text);
    //    }
    //    for (int i = 0; i < text.Length;)
    //    {
    //        if (text[i] == '.') { i++; continue; }
    //        if (text[i] == '?')
    //        {
    //            return new(groups, text[i..]);
    //        }
    //        if (i + groups[0] > text.Length)
    //        {
    //            return new(groups, text[i..]);
    //        }
    //        if ((i + groups[0]) == text.Length && text[i..(i + groups[0])].ToString() == nextMatch)
    //        {
    //            groups = groups[1..];

    //            i += nextMatch.Length;
    //            if (groups.Length == 0)
    //            {
    //                return new(groups, text[i..]);
    //            }
    //            nextMatch = new string('#', groups[0]);
    //        }
    //        else if (i + groups[0] + 1 > text.Length)
    //        {
    //            return new(groups, text[i..]);
    //        }
    //        else if (text[i..(i + groups[0] + 1)].ToString() == nextMatch + ".")
    //        {
    //            groups = groups[1..];
    //            i += nextMatch.Length + 1;
    //            if (groups.Length == 0)
    //            {
    //                return new(groups, text[i..]);
    //            }
    //            nextMatch = new string('#', groups[0]);
    //        }
    //        else
    //        {
    //            return new(groups, text[i..]);
    //        }
    //    }
    //    return new(groups, "");
    //}
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
        var x = Day12.Eat(groups, text);
        var rT = x.RemainingText;
        var rG = x.RemainingGroups;
        if (rT.Length == 0 && rG.Length > 0) return 0;
        if (rG.Length == 0 && -1 == rT.IndexOfAny(['#', '?']))
        {
            return 1;
        }
        else if (rT.Contains('?'))
        {
            var i = rT.IndexOf('?');

            return CountArrangements(rG, rT[0..i].ToString() + "." + rT[(i + 1)..].ToString()) + CountArrangements(rG, rT[0..i].ToString() + '#' + rT[(i + 1)..].ToString());
        }
        return 0;
    }
}
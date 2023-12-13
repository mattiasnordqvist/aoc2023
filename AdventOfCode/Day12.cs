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
        var lines = Input.Zplit().Select(x =>
        {
            return x.Split(" ")[0] + "?" + x.Split(" ")[0] + "?" + x.Split(" ")[0] + "?" + x.Split(" ")[0] + "?" + x.Split(" ")[0]
            + " " + x.Split(" ")[1] + "," + x.Split(" ")[1] + "," + x.Split(" ")[1] + "," + x.Split(" ")[1] + "," + x.Split(" ")[1];

        }).ToList();

        var s = lines.Select(x => new Line(x).CountArrangements()).Sum();
        return s.ToString();
    }

    private static Dictionary<string, Dictionary<string, long>> _eatCache = new();
    public static Dictionary<string, long> EatHashtagsWithCache(int amountToEat, string textToEatFrom)
    {
        textToEatFrom = textToEatFrom.TrimStart('.').TrimEnd('.');
        var key = amountToEat+":"+ textToEatFrom;
        if (!_eatCache.ContainsKey(key))
        {
            _eatCache[key] = EatHashtags(amountToEat, textToEatFrom);
        }
        return _eatCache[key];
    }

    public static Dictionary<string, long> EatHashtags(int amountToEat, string textToEatFrom)
    {
        textToEatFrom = textToEatFrom.TrimEnd('.').TrimStart('.');
        if (textToEatFrom.Length == amountToEat)
        {
            if (!textToEatFrom.Contains('.'))
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
                        return EatHashtagsWithCache(amountToEat, textToEatFrom[1..]);
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
                            var nextEat = EatHashtagsWithCache(amountToEat, textToEatFrom[1..]);
                            foreach (var n in nextEat)
                            {
                                allVariants.AddCount(n.Key, n.Value);
                            }
                        }
                    }
                    if (textToEatFrom[0] is '?' && afterCandidate == '#')
                    {
                        var nextEat = EatHashtagsWithCache(amountToEat, textToEatFrom[1..]);
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
        var a = CountArrangementsWithCache(_groups, _text);
        return a;
    }

    private static Dictionary<(string, string), long> _cache = new Dictionary<(string, string), long>();
    public long CountArrangementsWithCache(int[] groups, string text)
    {
        text = text.TrimStart('.').TrimEnd('.');
        var key = (string.Join(",", groups), text);
        if (!_cache.ContainsKey(key))
        {
            _cache[key] = CountArrangements(groups, text);
        }
        return _cache[key];
    }
    public long CountArrangements(int[] groups, string text)
    {
        if(groups.Count() == 0 && text == "")
        {
            return 1;
        }
        var d = Day12.EatHashtagsWithCache(groups[0], text).Where(x => x.Value != 0).ToList();
        var sum = 0l;
        foreach(var kv in d)
        {
            var worth = groups.Length == 1 && kv.Key.Contains('#') ? 0
                : (groups.Length > 1 ? CountArrangementsWithCache(groups[1..], kv.Key) : 1);

            sum += kv.Value * worth;
        }
        return sum;
    }
}
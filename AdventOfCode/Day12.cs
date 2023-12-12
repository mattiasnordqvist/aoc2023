


namespace AdventOfCode;
public class Day12 : MyBaseDay
{
    public override async ValueTask<string> Solve_1()
    {
        var lines = Input.Zplit();

        var s = lines.Select(x => new Line(x).Arrangements()).Sum();

        return s.ToString();
    }



    public override async ValueTask<string> Solve_2()
    {
        return "";
        var lines = Input.Zplit().Select(x =>
        {
            return x.Split(" ")[0] + "?" + x.Split(" ")[0] + "?" + x.Split(" ")[0] + "?" + x.Split(" ")[0] + "?" + x.Split(" ")[0]
            + " " + x.Split(" ")[1] + "," + x.Split(" ")[1] + "," + x.Split(" ")[1] + "," + x.Split(" ")[1] + "," + x.Split(" ")[1];

        }).ToList();

        var s = lines.Select(x => new Line(x).Arrangements()).Sum();
        //var ss = await Task.WhenAll(tasks);
        return s.ToString();
    }
}

public class Line
{
    private string _x;
    private string _springs;
    private int[] _groupSizes;

    public Line(string x)
    {
        _x = x;
        _springs = x.Zplit(" ")[0];
        _groupSizes = x.Zplit(" ")[1].Zplit(",").ia();
    }
    public long Arrangements()
    {
        var s = AllAlts(_springs, "").Count(x => Matches(x.Item2, _groupSizes));
        //Console.WriteLine(_x+": "+s);
        return s;
    }

    private List<(string, string)> AllAlts(string springs, string sNew)
    {
        if(!IsCandidate(sNew)) { return []; }
        if (springs == "") return [("", sNew)];
        if (springs[0] == '.') return AllAlts(springs[1..], sNew + ".");
        if (springs[0] == '#') return AllAlts(springs[1..], sNew + "#");
        if (springs[0] == '?') return [.. AllAlts(springs[1..], sNew + "."), .. AllAlts(springs[1..], sNew + "#")];
        throw new Exception();
    }

    private bool IsCandidate(string sNew)
    {
        
        var groups = sNew.Zplit(".");
        if(groups.Length == 0) return true;
        if (groups.Count() > _groupSizes.Length) return false;
        var i = 0;
        for (; i < groups.Count()-1; i++)
        {
            if (groups[i].Length != _groupSizes[i]) return false;
        }
        if (sNew.EndsWith("#")) { 
            return groups[i].Length <= _groupSizes[i];
        }
        else
        {
            return groups[i].Length == _groupSizes[i];
        }

    }

    public static bool Matches(string x, int[] groupSizes)
    {
        var groups = x.Zplit(".");
        if(groups.Count() != groupSizes.Length) return false;
        for (var i = 0; i < groups.Count(); i++)
        {
            if (groups[i].Length != groupSizes[i]) return false;
        }
        return true;
    }

    //public static List<(string, int[], string)> Alts(string s, int[] groups, string sNew)
    //{
    //    if (s == "" && groups.Length == 0) return new List<(string, int[], string)> { ("", [], sNew) };
    //    var alts = new List<(string, int[], string)>();
    //    //for (int i = 0; i < s.Length; i++)
    //    //{
    //    var broken = s[0..].TakeWhile(x => x == '#').Count();
    //    if (s[0] == '#')
    //    {
    //        if (broken > 0 && broken == groups.First())
    //        {
    //            if (broken == s.Length)
    //            {
    //                alts.Add(("", [], sNew + new string('#', broken)));
    //            }
    //            else
    //            {
    //                alts.AddRange(Alts(s[(broken + 1)..], groups[1..], sNew + new string('#', broken)));
    //            }
    //        }
    //        else { return alts; }
    //    }
    //    else if (s[0] != '?')
    //    {
    //        alts.AddRange(Alts(s[1..], groups, sNew + s[0]));
    //    }
    //    else
    //    {
    //        var l = groups[0];
    //        alts.AddRange(Alts(s[(0 + 1)..], groups, sNew + "."));
    //        if (new string('?', l) == s[0..(0 + l)])
    //        {
    //            if ((s[0 + l] != '#'))
    //            {
    //                alts.AddRange(Alts(s[(0 + l + 1)..], groups[1..], sNew + new string('#', l) + "."));
    //            }
    //        }
    //        else {
    //            alts.AddRange(Alts("#" + s[1..],groups,sNew));
    //        }
    //        //break;
    //        //}
    //    }
    //    return alts;
    //}

}
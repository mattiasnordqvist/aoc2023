
using System.Diagnostics;

namespace AdventOfCode;

public class Day07 : MyBaseDay
{
    public override ValueTask<string> Solve_1()
    {
        var lines = Input.SplitLines();
        var hands = new List<Hand>();
        foreach (var line in lines)
        {
            hands.Add(new Hand(line.Line[0..5].ToString(), line.Line[5..].ToString().l()));
        }
        var orderedHands = hands.GroupBy(x => x.HandType)
            .OrderByDescending(x => x.Key)
            .ToList();
        long totalWinnings = 0;
        var rank = 1;
        foreach(var strengthGroup in orderedHands)
        {
            var orderedGroup = strengthGroup.OrderByDescending(x => x.Cards, new CardsComparer());
            foreach(var card in orderedGroup)
            {
                totalWinnings += card.Bid * rank;
                rank++;
            }
        }
        Debug.Assert(totalWinnings == 246912307);
        return ValueTask.FromResult(totalWinnings.ToString());

        
    }
    

    public override ValueTask<string> Solve_2()
    {
        CamelCards = CamelCards2;
        CardValues = CardValues2;
        JokerEnabled = true;
        var lines = Input.SplitLines();
        var hands = new List<Hand>();
        foreach (var line in lines)
        {
            hands.Add(new Hand(line.Line[0..5].ToString(), line.Line[5..].ToString().l()));
        }
        var orderedHands = hands.GroupBy(x => x.HandType)
            .OrderByDescending(x => x.Key)
            .ToList();
        long totalWinnings = 0;
        var rank = 1;
        foreach (var strengthGroup in orderedHands)
        {
            var orderedGroup = strengthGroup.OrderByDescending(x => x.Cards, new CardsComparer());
            foreach (var card in orderedGroup)
            {
                totalWinnings += card.Bid * rank;
                rank++;
            }
        }
        Debug.Assert(totalWinnings == 246894760);
        return ValueTask.FromResult(totalWinnings.ToString());
    }


    public enum HandType
    {
        Five,
        Four,
        Full,
        Three,
        Two,
        One,
        High
    }

    public static string CamelCards1 = "AKQJT98765432";
    public static string CamelCards2 = "AKQT98765432J";
    public static string CamelCards = CamelCards1;
    public static bool JokerEnabled = false;
    public static Dictionary<char, int> CardValues1 = new Dictionary<char, int>()
    {
        {'A', 13},
        {'K', 12},
        {'Q', 11},
        {'J', 10},
        {'T', 9},
        {'9', 8},
        {'8', 7},
        {'7', 6},
        {'6', 5},
        {'5', 4},
        {'4', 3},
        {'3', 2},
        {'2', 1},
    };
    public static Dictionary<char, int> CardValues2 = new Dictionary<char, int>()
    {
        {'A', 13},
        {'K', 12},
        {'Q', 11},
        {'T', 10},
        {'9', 9},
        {'8', 8},
        {'7', 7},
        {'6', 6},
        {'5', 5},
        {'4', 4},
        {'3', 3},
        {'2', 2},
        {'J', 1},
    };
    public static Dictionary<char, int> CardValues = CardValues1;

    internal class CardsComparer : IComparer<string>
    {
        public int Compare(string? x, string? y)
        {
            if (x == null || y == null) throw new Exception();
            if (x == y) return 0;
            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] == y[i]) continue;
                return CardValues[x[i]] > CardValues[y[i]] ? -1 : 1;
            }
            throw new Exception();
        }
    }
    public class Hand
    {
        public Hand(string cards, long bid)
        {
            Cards = cards;
            Bid = bid;
            HandType = JokerEnabled ? CreateBestHand(cards) : CalcHand(cards);

        }

        private HandType CreateBestHand(string cards)
        {
            var bestHand = CalcHand(cards);

            if (!cards.Contains('J')) return bestHand;
            foreach (char c in CamelCards[0..^1])
            {
                var newHand = cards.Replace('J', c);
                var hand = CalcHand(newHand);
                if (hand < bestHand)
                {
                    bestHand = hand;
                }
                if (bestHand == HandType.Five)
                {
                    return bestHand;
                }
            }
            return bestHand;

        }

        private HandType CalcHand(string cards)
        {
            var groups = cards.GroupBy(x => x).ToArray();
            if (groups.Length == 1)
            {
                return HandType.Five;
            }
            else if (groups.Length == 2 && groups.Any(x => x.Count() == 4))
            {
                return HandType.Four;
            }
            else if (groups.Length == 2 && groups.Any(x => x.Count() == 3))
            {
                return HandType.Full;
            }
            else if (groups.Length == 3 && groups.Any(x => x.Count() == 3))
            {
                return HandType.Three;
            }
            else if (groups.Length == 3)
            {
                return HandType.Two;
            }
            else if (groups.Count() == 4)
            {
                return HandType.One;
            }
            else
            {
                return HandType.High;
            }

        }
        public HandType HandType { get; private set; }
        public string Cards { get; }
        public long Bid { get; }
    }
}

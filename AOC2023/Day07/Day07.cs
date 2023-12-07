using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AOC2023
{
    internal class Day07
    {
        private const string INPUT = "input.txt";

        private static List<char> Cards = new() { 'A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2' };

        public enum HandType
        {
            FiveOfAKind = 6,
            FourOfAKind = 5,
            FullHouse = 4,
            ThreeOfAKind = 3,
            TwoPair = 2,
            OnePair = 1,
            HighCard = 0
        }

        public class HandBid : IComparable<HandBid>
        {
            public string Hand { get; private set; }
            public int Bid { get; private set; }
            private HandType? handType;
            public HandType HandType
            {
                get
                {
                    if (handType == null)
                    {
                        handType = GetHandType();
                    }
                    return handType.Value;
                }
            }

            public HandBid(string hand, int bid)
            {
                Hand = hand;
                Bid = bid;
                handType = null;
            }

            public HandType GetHandType()
            {
                Dictionary<char, int> cards = new Dictionary<char, int>();
                foreach (var c in Hand)
                {
                    if (cards.ContainsKey(c))
                    {
                        cards[c]++;
                    }
                    else
                    {
                        cards[c] = 1;
                    }
                }
                List<int> counts = cards.Values.ToList();
                counts.Sort((a, b) => b.CompareTo(a));
                bool three = false;
                bool pair = false;
                foreach (var count in counts)
                {
                    switch (count)
                    {
                        case 5:
                            return HandType.FiveOfAKind;
                        case 4:
                            return HandType.FourOfAKind;
                        case 3:
                            three = true;
                            break;
                        case 2:
                            if (three)
                                return HandType.FullHouse;
                            if (pair)
                                return HandType.TwoPair;
                            pair = true;
                            break;
                        default:
                            break;
                    }
                }
                if (three && !pair)
                    return HandType.ThreeOfAKind;
                if (!three && pair)
                    return HandType.OnePair;
                return HandType.HighCard;
            }

            int IComparable<HandBid>.CompareTo(HandBid? other)
            {
                if (other == null)
                    throw new Exception("Other is null");
                var compareType = ((int)HandType).CompareTo((int)other.HandType);
                if (compareType != 0)
                    return compareType;
                for (int i = 0; i < Hand.Length; i++)
                {
                    int i1 = Cards.FindIndex(x => x == Hand[i]);
                    int i2 = Cards.FindIndex(x => x == other.Hand[i]);
                    int compare = i2.CompareTo(i1);
                    if (compare != 0)
                        return compare;
                }
                return 0;
            }

            public override string ToString()
            {
                return $"HandBid: {Hand}, {Bid}";
            }
        }

        public static void Solve1()
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
            var lines = File.ReadAllLines($"{className}/{INPUT}");
            long result = 0;

            List<HandBid> handBids = new List<HandBid>();

            foreach (var line in lines)
            {
                var split = line.Split(' ');
                handBids.Add(new HandBid(split[0], int.Parse(split[1])));
            }
            handBids.Sort();
            for (int i = 0; i < handBids.Count; i++)
            {
                result += (i + 1) * handBids[i].Bid;
            }

            Console.WriteLine($"{className}.1: " + result);
        }

        private static List<char> Cards2 = new() { 'A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J' };

        public class HandBid2 : IComparable<HandBid2>
        {
            public string Hand { get; private set; }
            public int Bid { get; private set; }
            private HandType? handType;
            public HandType HandType
            {
                get
                {
                    if (handType == null)
                    {
                        handType = GetHandTypeWithJoker();
                    }
                    return handType.Value;
                }
            }

            public HandBid2(string hand, int bid)
            {
                Hand = hand;
                Bid = bid;
                handType = null;
            }

            public HandType GetHandType(List<int> counts)
            {
                counts.Sort((a, b) => b.CompareTo(a));
                bool three = false;
                bool pair = false;
                foreach (var count in counts)
                {
                    switch (count)
                    {
                        case 5:
                            return HandType.FiveOfAKind;
                        case 4:
                            return HandType.FourOfAKind;
                        case 3:
                            three = true;
                            break;
                        case 2:
                            if (three)
                                return HandType.FullHouse;
                            if (pair)
                                return HandType.TwoPair;
                            pair = true;
                            break;
                        default:
                            break;
                    }
                }
                if (three && !pair)
                    return HandType.ThreeOfAKind;
                if (!three && pair)
                    return HandType.OnePair;
                return HandType.HighCard;
            }

            public HandType GetHandTypeWithJoker()
            {
                Dictionary<char, int> cards = new Dictionary<char, int>();
                foreach (var c in Hand)
                {
                    if (cards.ContainsKey(c))
                    {
                        cards[c]++;
                    }
                    else
                    {
                        cards[c] = 1;
                    }
                }
                if (cards.ContainsKey('J'))
                {
                    if (cards['J'] == 5)
                        return HandType.FiveOfAKind;
                    List<HandType> types = new List<HandType>();
                    foreach (var key in cards.Keys)
                    {
                        if (key == 'J')
                            continue;
                        Dictionary<char, int> cards2 = cards.ToDictionary(entry => entry.Key, entry => entry.Value);
                        cards2[key] += cards2['J'];
                        cards2['J'] = 0;
                        types.Add(GetHandType(cards2.Values.ToList()));
                    }
                    return types.Max();
                }
                return GetHandType(cards.Values.ToList());
            }

            int IComparable<HandBid2>.CompareTo(HandBid2? other)
            {
                if (other == null)
                    throw new Exception("Other is null");
                var compareType = ((int)HandType).CompareTo((int)other.HandType);
                if (compareType != 0)
                    return compareType;
                for (int i = 0; i < Hand.Length; i++)
                {
                    int i1 = Cards2.FindIndex(x => x == Hand[i]);
                    int i2 = Cards2.FindIndex(x => x == other.Hand[i]);
                    int compare = i2.CompareTo(i1);
                    if (compare != 0)
                        return compare;
                }
                return 0;
            }

            public override string ToString()
            {
                return $"HandBid2: {Hand}, {Bid}, {HandType}";
            }
        }
        public static void Solve2()
        {
            var className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name;
            var lines = File.ReadAllLines($"{className}/{INPUT}");
            long result = 0;

            List<HandBid2> handBids = new List<HandBid2>();

            foreach (var line in lines)
            {
                var split = line.Split(' ');
                handBids.Add(new HandBid2(split[0], int.Parse(split[1])));
            }
            handBids.Sort();
            for (int i = 0; i < handBids.Count; i++)
            {
                result += (i + 1) * handBids[i].Bid;
            }

            Console.WriteLine($"{className}.2: " + result);
        }
    }
}

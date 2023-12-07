<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

#region Preamble

#load "..\Lib\Utils"
#load "..\Lib\BFS"

//#define TEST
//#define CHECKED

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"".GetLines();
#endif

#if CHECKED
checked{
#endif

#endregion

var hands = lines.Extract<(string, int)>(@"([0-9TJQKA]+) (\d+)");
hands.OrderBy(x => x.Item1, new HandComparer(jokers: false)).Select((x, i) => x.Item2 * (i + 1)).Sum().Dump("Part 1");
hands.OrderBy(x => x.Item1, new HandComparer(jokers: true)).Select((x, i) => x.Item2 * (i + 1)).Sum().Dump("Part 2");


class HandComparer : IComparer<string>
{
    bool jokers;

    public HandComparer(bool jokers)
    {
        this.jokers = jokers;
    }

    int Cardval(char ch)
    {
        return ch switch
        {
            'A' => 14,
            'K' => 13,
            'Q' => 12,
            'J' => jokers ? 0 : 11,
            'T' => 10,
            _ => ch - '0'
        };
    }

    int IComparer<string>.Compare(string? h1, string? h2)
    {
        var s1 = h1.GroupBy(ch => ch).OrderByDescending(g => g.Count()).Select(g => (g.Key, g.Count())).ToList();
        var s2 = h2.GroupBy(ch => ch).OrderByDescending(g => g.Count()).Select(g => (g.Key, g.Count())).ToList();
        
        if (jokers)
        {
            if (h1 != "JJJJJ")
            {
                var ji1 = s1.FindIndex(x => x.Key == 'J');            
                if (ji1 != -1)
                {
                var j1 = s1[ji1].Item2;
                s1.RemoveAt(ji1);
                s1[0] = (s1[0].Key, s1[0].Item2 + j1);
                }
            }

            if (h2 != "JJJJJ")
            {
                var ji2 = s2.FindIndex(x => x.Key == 'J');
                if (ji2 != -1)
                {
                    var j2 = s2[ji2].Item2;
                    s2.RemoveAt(ji2);
                    s2[0] = (s2[0].Key, s2[0].Item2 + j2);
                }
            }
        }

        var p1 = s1 switch
        {
            [(_, 5)] => 10,
            [(_, 4), ..] => 9,
            [(_, 3), (_, 2)] => 8,
            [(_, 3), ..] => 7,
            [(_, 2), (_, 2), ..] => 6,
            [(_, 2), ..] => 5,
            _ => 4
        };

        var p2 = s2 switch
        {
            [(_, 5)] => 10,
            [(_, 4), ..] => 9,
            [(_, 3), (_, 2)] => 8,
            [(_, 3), ..] => 7,
            [(_, 2), (_, 2), ..] => 6,
            [(_, 2), ..] => 5,
            _ => 4
        };

        if (p1 == p2)
        {
            int t = 0;
            int i = 0;
            while (t == 0 && i < 5)
            {
                t += Cardval(h1[i]) - Cardval(h2[i]);
                i++;
            }
            return t;
        }
        else
        {
            return p1 - p2;
        }
    }
}

#if CHECKED
}
#endif

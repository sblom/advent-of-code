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
#define CHECKED

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11".GetLines();
#endif

#if CHECKED
checked{
#endif

#endregion

long tot = 0;

int i = 1;
List<(HashSet<int> winners, HashSet<int> mine)> cards = new();
cards.Add((new HashSet<int>(), new HashSet<int>()));
Dictionary<int,long> memoize = new();

var cardlists = lines.Extract<(int num, List<int> winners, List<int> mine)>(@"Card +(\d+): +(?:(\d+) *)+\| +(?:(\d+) *)+");

foreach (var card in cardlists)
{
    cards.Add((card.winners.ToHashSet(),card.mine.ToHashSet()));
    var N = card.mine.Where(n => card.winners.Contains(n)).Count();    
    if (N > 0) tot += (1L << (N - 1));
}

tot.Dump("Part 1");

long GetCards(int cardno)
{
    if (memoize.ContainsKey(cardno)) return memoize[cardno];
    
    var result = 1L + Enumerable.Range(cardno + 1, cards[cardno].mine.Where(n => cards[cardno].winners.Contains(n)).Count()).Select(x => GetCards(x)).Sum();
    memoize[cardno] = result;
    return result;
}

Enumerable.Range(1, cards.Count - 1).Sum(n => GetCards(n)).Dump("Part 2");


#if CHECKED
}
#endif

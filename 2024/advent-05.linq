<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

#load "..\Lib\Utils"
#load "..\Lib\BFS"

//#define TEST

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"".GetLines();
#endif

var parts = lines.GroupLines();
var rules = parts.First().Extract<(int,int)>(@"(\d+)\|(\d+)").Dump();
var books = parts.Skip(1).First().Extract<List<int>>(@"((\d+),?)+").Dump();

int t = 0;
int t2 = 0;

foreach (var book in books)
{
    foreach (var rule in rules)
    {
        var m = book.IndexOf(rule.Item1);
        var n = book.IndexOf(rule.Item2);
        if (m != -1 && n != -1 && m > n) goto nope;
    }
    t += book[book.Count / 2];
    continue;
nope:;
    List<int> order = new();
    foreach (var page in book)
    {
        var before = rules.Where(r => r.Item2 == page).Select(r => r.Item1);
        var after = rules.Where(r => r.Item1 == page).Select(r => r.Item2);

        var split1 = order.Where(x => before.Contains(x));
        var split2 = order.Where(x => after.Contains(x));
        order = split1.Concat(new[] { page }).Concat(split2).ToList();
    }
    t2 += order[order.Count / 2];
    t2.Dump();
}
t.Dump();
t2.Dump();
//
//t = 0;
//foreach (var book in books)
//{
//    List<int> order = new();
//    foreach (var page in book)
//    {
//        var before = rules.Where(r => r.Item2 == page).Select(r => r.Item1);
//        var after = rules.Where(r => r.Item1 == page).Select(r => r.Item2);
//        
//        var split1 = order.Where(x => before.Contains(x));
//        var split2 = order.Where(x => after.Contains(x));
//        order = split1.Concat(new[]{page}).Concat(split2).ToList();
//    }
//    t2 += order[order.Count / 2];
//}
//t.Dump();
//
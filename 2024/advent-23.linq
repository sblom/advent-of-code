<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

#LINQPad checked+

#load "..\Lib\Utils"
#load "..\Lib\BFS"

//#define TEST

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = $@"1
2
3
2024".Replace("\\", "").GetLines();
    //   2,4,1,4,7,5,4,1,1,4,5,5,0,3,3,0

#endif

var edges = lines.Extract<(string, string)>(@"(..)-(..)").Select(x => string.Compare(x.Item1, x.Item2) < 0 ? (x.Item1,x.Item2) : (x.Item2,x.Item1)).ToHashSet();

var nodes = edges.Select(x => x.Item1).Concat(edges.Select(x => x.Item2)).Distinct().ToHashSet();

var dict = nodes.Select(n => (n, edges.Where(e => e.Item1 == n).Select(e => e.Item2).Concat(edges.Where(e => e.Item2 == n).Select(e => e.Item1)).ToImmutableHashSet())).ToDictionary(x => x.n, x=> x.Item2);

var trios = (from e in edges let b = e.Item1 let c = e.Item2 from n in nodes where n != b && n != c && (edges.Contains((n, b)) || edges.Contains((b, n))) && (edges.Contains((c, n)) || edges.Contains((n, c))) select new[] { n, b, c }.OrderBy(x => x).ToList()).Select(x => (x[0],x[1],x[2])).Distinct().Where(x => x.Item1.StartsWith("t") || x.Item2.StartsWith("t") || x.Item3.StartsWith("t"));

trios.Count().Dump();

foreach (var kv in dict)
{
    string.Join(",",BiggestCollection(ImmutableHashSet<string>.Empty.Add(kv.Key)).OrderBy(x => x)).Dump();
}

List<string> BiggestCollection(ImmutableHashSet<string> set)
{
    var possible = dict[set.First()];
    
    foreach (var s in set)
    {
        possible = possible.Intersect(dict[s]).Except(set);
    }

    List<string> max = new();

    foreach (var p in possible)
    {
        var result = BiggestCollection(set.Add(p));
        if (result.Count > max.Count) max = result;
    }
    
    return max;
}

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
var lines = $@"Register A: {k}
Register B: 0
Register C: 0

Program: 2,4,1,3,7,5,4,0,1,3,0,3,5,5,3,0".Replace("\\", "").GetLines();
    //   2,4,1,4,7,5,4,1,1,4,5,5,0,3,3,0

#endif

var groups = lines.GroupLines();

var towels = groups.First().SelectMany(x => x.Replace(" ","").Split(",")).ToHashSet().Dump();

var goals = groups.Skip(1).First();

int c = 0;
Dictionary<string,bool> cache = new();
Dictionary<string,long> countcache = new();

foreach (var goal in goals)
{
    goal.Dump();
    if (MatchTowels(goal)) c++;
}

c.Dump();

long t = 0;

foreach (var goal in goals)
{
    goal.Dump();
    t += CountMatchTowels(goal);
}

t.Dump();

bool MatchTowels(ReadOnlySpan<char> remainder)
{
    var str = remainder.ToString();
    if (string.IsNullOrEmpty(str)) return true;
    if (cache.ContainsKey(str)) return cache[str];
    
    long tot = 0;
    
    for (int i = 1; i < remainder.Length + 1; i++)
    {
        var pre = remainder[0..i].ToString();
        var rem = remainder[i..];
        if (towels.Contains(pre) && MatchTowels(rem))
            return cache[str] = true;
    }
    return cache[str] = false;
}

long CountMatchTowels(ReadOnlySpan<char> remainder)
{
    var str = remainder.ToString();
    if (string.IsNullOrEmpty(str)) return 1;
    if (countcache.ContainsKey(str)) return countcache[str];

    long tot = 0;

    for (int i = 1; i < remainder.Length + 1; i++)
    {
        var pre = remainder[0..i].ToString();
        var rem = remainder[i..];
        if (towels.Contains(pre)) tot += CountMatchTowels(rem);
    }
    return countcache[str] = tot;
}
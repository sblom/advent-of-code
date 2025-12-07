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

var towels = groups.First().SelectMany(x => x.Replace(" ","").Split(",")).ToHashSet();

var goals = groups.Skip(1).First();

goals.Count(x => MatchTowels(x)).Dump("Part 1");

bool MatchTowels(ReadOnlySpan<char> remainder)
{
    if (remainder.Length == 0) return true;
    
    for (int i = 1; i < remainder.Length; i++)
    {
        var pre = remainder[0..i].ToString();
        var rem = remainder[i..];
        if (towels.Contains(pre) && MatchTowels(rem)) return true;
    }
    return false;
}
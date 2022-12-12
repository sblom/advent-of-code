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

var tot = 0;

foreach (var line in lines)
{
    var num = line.Length / 2;
    
    var set1 = new HashSet<char>(line.Take(num));
    var set2 = new HashSet<char>(line.Skip(num));
    
    var ch = set1.Intersect(set2).Single();
    int val = 0;
    if (ch >= 'a' && ch <= 'z') val = ch - 'a' + 1;
    else if (ch >= 'A' && ch <= 'Z') val = ch - 'A' + 27;
    
    tot += val;
}

tot.Dump();

var groups = lines.Chunk(3);

groups.Select(x => x.Select(y => new HashSet<char>(y)).Aggregate((z, w) => z.Intersect(w).ToHashSet())).Select(c =>
{
    var ch = c.Single();
    if (ch >= 'a' && ch <= 'z') { return ch - 'a' + 1; }
    else { return ch - 'A' + 27; }
}).Sum().Dump();
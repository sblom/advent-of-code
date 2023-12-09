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
var lines = @"0 3 6 9 12 15
1 3 6 10 15 21
10 13 16 21 30 45".GetLines();
#endif

#if CHECKED
checked{
#endif

#endregion

var seqs = lines.Select(x => x.Split(" ").Select(int.Parse));
var rseqs = lines.Select(x => x.Split(" ").Select(int.Parse).Reverse());

int Extend(IEnumerable<int> seq)
{
    IEnumerable<int> seqi = seq;
    
    int tot = 0;
    
    while (seqi.Any(x => x != 0))
    {
        var next = seqi.Skip(1).Zip(seqi).Select(x => x.First - x.Second).ToList();
        tot += seqi.Last();
        seqi = next;
    }
    return tot;
}

seqs.Sum(x => Extend(x)).Dump("Part 1");
rseqs.Sum(x => Extend(x)).Dump("Part 2");

#if CHECKED
}
#endif

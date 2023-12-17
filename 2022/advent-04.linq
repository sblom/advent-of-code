<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference  Prerelease="true">RegExtract</NuGetReference>
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
var lines = @"2-4,6-8
2-3,4-5
5-7,7-9
2-8,3-7
6-6,4-6
2-6,4-8".GetLines();
#endif

var pairs = lines.Extract<(int lo1,int hi1,int lo2,int hi2)>(@"^(\d+)-(\d+),(\d+)-(\d+)$");
pairs.Dump();

pairs.Select(x => (x.lo1 <= x.lo2 && x.hi1 >= x.hi2) || (x.lo1 >= x.lo2 && x.hi1 <= x.hi2)).Count(x => x).Dump();

pairs.Count(x =>(x.hi1 >= x.lo2 && x.lo1 <= x.lo2) || (x.hi1 >= x.hi2 && x.lo1 <= x.hi2) || (x.hi2 >= x.lo1 && x.lo2 <= x.lo1) || (x.hi2 >= x.hi1 && x.lo2 <= x.hi1)).Dump();
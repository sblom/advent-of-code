<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference Prerelease="true">RegExtract</NuGetReference>
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
var lines = @"two1nine
eightwothree
abcone2threexyz
xtwone3four
4nineeightseven2
zoneight234
7pqrstsixteen".GetLines();
#endif

//lines.Extract<List<(int?,string)>>(@"(?=([0-9]|one|two|three|four|five|six|seven|eight|nine).*?)+").Dump();

var splits = lines.Select(x => x.Split("  "));
var list1 = splits.Select(x => int.Parse(x[0])).ToList(); list1.Sort();
var list2 = splits.Select(x => int.Parse(x[1])).ToList(); list2.Sort();

int t = 0;

foreach (var x in Enumerable.Range(0,list1.Count))
{
    t += Math.Abs(list1[x] - list2[x]);
}

t.Dump("Part 1");
list1.Select(x => list2.Where(y => y == x).Count() * x).Sum().Dump("Part 2");

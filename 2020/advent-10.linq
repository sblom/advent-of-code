<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference Prerelease="true">RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

#load "..\Lib\Utils"
#load "..\Lib\BFS"

var lines = AoC.GetLines().Select(long.Parse);
var max = lines.Max();

var ordered = lines.Append(0).Append(max + 3).OrderBy(x => x).ToArray();

var diffs = ordered.Zip(ordered.Skip(1)).Select(x => x.Second - x.First).GroupBy(x => x);
(diffs.First().Count() * diffs.Skip(1).First().Count()).Dump("Part 1");

long[] ways = new long[ordered.Count()];

ways[0] = 1;
ways[1] = (ordered[0] >= ordered[1] - 3 ? ways[0] : 0);
ways[2] = (ordered[0] >= ordered[2] - 3 ? ways[0] : 0) + (ordered[1] >= ordered[2] - 3 ? ways[1] : 0);

for (int i = 3; i < ordered.Length; i++)
{
	ways[i] = (ordered[i - 3] >= ordered[i] - 3 ? ways[i - 3] : 0) + (ordered[i - 2] >= ordered[i] - 3 ? ways[i - 2] : 0) + (ordered[i - 1] >= ordered[i] - 3 ? ways[i - 1] : 0);
}

ways[^1].Dump("Part 2");
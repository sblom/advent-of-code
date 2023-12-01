<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

#load "..\Lib\Utils"
#load "..\Lib\BFS"

var lines = AoC.GetLines().Select(long.Parse).ToArray();

long part1 = 0;

for (int i = 25; i < lines.Length; i++)
{
	var window = lines[(i - 25)..i];

	var result = from x in (0..25) from y in (0..25) where x != y && window[x] + window[y] == lines[i] select i;
	if (!result.Any())
	{
		lines[i].Dump("Part 1");
		part1 = lines[i];
		break;
	}
}

for (int i = 0; i < lines.Length; i++)
{
	long c = 0;
	int j = 0;
	for (j = i; c < part1; j++)
	{
		c += lines[j];
	}
	if (c == part1)
	{
		(lines[i..(j + 1)].Min() + lines[i..(j + 1)].Max()).Dump("Part 2");
		break;
	}
}
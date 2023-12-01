<Query Kind="Statements">
  <NuGetReference>RegExtract</NuGetReference>
  <Namespace>RegExtract</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

#load "..\Lib\Utils"

var groups = AoC.GetLines().GroupLines();

int part1 = 0, part2 = 0;

foreach (var group in groups)
{
	var qs = group.Aggregate(ImmutableHashSet<char>.Empty, (set, str) => set.Union(str));

	part1 += qs.Count;	
	part2 += qs.Count(q => group.All(str => str.Contains(q)));
}

part1.Dump("Part 1");
part2.Dump("Part 2");
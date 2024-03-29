<Query Kind="Statements">
  <NuGetReference>System.Collections.Immutable</NuGetReference>
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>System.Linq</Namespace>
</Query>

var lines = (await AoC.GetLinesWeb()).ToList();
lines.Count.Dump();

IImmutableList<(int,int)> edges = ImmutableList<(int,int)>.Empty;

foreach (var line in lines)
{
	var nums = line.Split('/');
	edges = edges.Add((int.Parse(nums[0]), int.Parse(nums[1])));
}

int Search(IImmutableList<(int,int)> e, int cur = 0, int strength = 0)
{
	return e.Where(x => x.Item1 == cur || x.Item2 == cur).Select(x => Search(e.Remove(x), x.Item1 == cur ? x.Item2 : x.Item1, strength + x.Item1 + x.Item2)).Concat(Enumerable.Repeat(strength,1)).Max();
}

(int,int) Search2(IImmutableList<(int, int)> e, int cur = 0, int strength = 0, int length = 0)
{
	return e.Where(x => x.Item1 == cur || x.Item2 == cur).Select(x => Search2(e.Remove(x), x.Item1 == cur ? x.Item2 : x.Item1, strength + x.Item1 + x.Item2, length + 1)).Concat(Enumerable.Repeat((strength,length),1)).OrderByDescending(x => x.Item2).ThenByDescending(x => x.Item1).First();
}


Search(edges).Dump("part 1");
Search2(edges).Item1.Dump("part 2");
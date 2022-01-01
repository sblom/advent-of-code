<Query Kind="Statements">
  <NuGetReference>System.Collections.Immutable</NuGetReference>
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>System.Linq</Namespace>
</Query>

var lines = AoC.GetLines();

IImmutableSet<string> locs = ImmutableHashSet<string>.Empty;
var adj = new List<(string from,string to,int dist)>();

foreach (var line in lines)
{
	var ts = line.Split(' ');
	adj.Add((ts[0],ts[2],int.Parse(ts[4])));
	adj.Add((ts[2],ts[0],int.Parse(ts[4])));
}

locs = adj.Select(x=>x.from).ToImmutableHashSet();

int FindShortest(IImmutableSet<string> next, Func<IEnumerable<int>,int> pick = null, string cur = null, int dist = 0)
{
	if (pick == null) pick = Enumerable.Min<int>;
	
	if (!next.Any())
	{
		return dist;
	}
	else
	{
		return pick(next.Select(x => FindShortest(next.Remove(x), pick, x, dist + adj.Where(y => y.from == cur && y.to == x).FirstOrDefault().dist)));
	}
}

FindShortest(locs).Dump("part 1");
FindShortest(locs,Enumerable.Max<int>).Dump("part 2");
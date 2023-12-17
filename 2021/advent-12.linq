<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference  Prerelease="true">RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>static UserQuery</Namespace>
</Query>

//#define TEST

#region preamble
#load "..\Lib\Utils"
#load "..\Lib\BFS"
#endregion

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"dc-end
HN-start
start-kj
dc-start
dc-HN
LN-dc
HN-end
kj-sa
kj-HN
kj-dc".GetLines();
#endif

Dictionary<string, List<string>> graph = new();

foreach (var line in lines)
{
	var nodes = line.Split("-");
	AddEdge(nodes);
}

int path = 0;

var bfs = new BFS<(string loc,IImmutableSet<string> prev)>(
	(loc: "start", prev: ImmutableHashSet<string>.Empty),
	NextLocations,
	_ => false,
	state =>
	{
		if (state.loc == "end")
		{
			path++;
		}
		return false;
	}
);

foreach (var L in bfs.Search())
{
	L.Dump();
}

path.Dump();

IEnumerable<(string, IImmutableSet<string>)> NextLocations((string loc, IImmutableSet<string> prev) state)
{
	var (loc, prev) = state;
	
	foreach (var adj in graph[loc])
	{
		if (adj[0] is >= 'A' and <= 'Z' || !prev.Contains(adj))
		{
			yield return (adj, prev.Add(loc));
		}
	}
}

int path2 = 0;

var bfs2 = new BFS<(string loc, IImmutableList<string> prev)>(
	(loc: "start", prev: ImmutableList<string>.Empty),
	NextLocations2,
	_ => false,
	state =>
	{
		if (state.loc == "end")
		{
			path2++;
			//(string.Join(",",state.prev) + ",end").Dump();
		}
		return false;
	}
);

IEnumerable<(string, IImmutableList<string>)> NextLocations2((string loc, IImmutableList<string> prev) state)
{
	var (loc, prev) = state;
	
	if (loc == "end") yield break;

	prev = prev.Add(loc);

	foreach (var adj in graph[loc])
	{
		if (adj == "start") continue;
		
		switch (adj)
		{
			case string s when s[0] is >= 'A' and <= 'Z':
				yield return (adj, prev);
				break;
			case string s:
				var groups = prev.GroupBy(p => p);
				var howMany = 				prev.Where(p => p == s).Count();
			
				if (howMany < 1 || (!groups.Where(g => g.Key[0] is >= 'a' and <= 'z' && g.Count() >= 2).Any() && howMany < 2))
				{
					yield return (adj, prev);
				}
				break;
		}
	}
}

foreach (var L in bfs2.Search())
{
	L.Dump();
}

path2.Dump();

void AddEdge(string[] nodes)
{
	if (!graph.ContainsKey(nodes[0]))
	{
		graph[nodes[0]] = new();
	}
	if (!graph.ContainsKey(nodes[1]))
	{
		graph[nodes[1]] = new();
	}

	graph[nodes[0]].Add(nodes[1]);
	graph[nodes[1]].Add(nodes[0]);
}
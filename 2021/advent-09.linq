<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

//#define TEST

#region preamble
#load "..\Lib\Utils"
#load "..\Lib\BFS"
#endregion

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"2199943210
3987894921
9856789892
8767896789
9899965678".GetLines();
#endif

//var dirs = from dx in new[] { -1, 0, 1 } from dy in new[] {-1,0,1} where dx != 0 || dy != 0 select (dx,dy);
var dirs = new[]{ (0,1), (0,-1), (1,0), (-1,0) };

var data = lines.Select(line => line.Select(ch => ch - '0').ToArray()).ToArray();

int c = 0;

HashSet<(int,int)> mins = new();

for (int y0 = 0; y0 < data.Length; y0++)
{
	for (int x0 = 0; x0 < data[0].Length; x0++)
	{
		if (dirs.Select(d => (x0 + d.Item1, y0 + d.Item2) switch {
			var (x,y) when (x >= 0 && x < data[0].Length && y >= 0 && y < data.Length) => data[y][x],
			_ => 100
		}).All(n => n > data[y0][x0])){
			c += (data[y0][x0] + 1);
			
			mins.Add((x0,y0));
		}		
	}
}

c.Dump();

List<int> results = new();

foreach (var min in mins)
{
	int r = 0;

	var bfs = new BFS<(int, int)>(min,
	next: x => GetHigherNeighbors(x),
	isTerminal: _ => false,
	isInteresting: x => {r++; return false;});
	
	foreach (var s in bfs.Search())
	{
		continue;
	}
	
	results.Add(r);
}

results.OrderByDescending(r => r).Take(3).Aggregate(1, (x,y) => x*y).Dump();

IEnumerable<(int,int)> GetNeighbors ((int x,int y) loc)
{
	return dirs.SelectMany(d => (loc.x + d.Item1, loc.y + d.Item2) switch
	{
		var (x, y) when (x >= 0 && x < data[0].Length && y >= 0 && y < data.Length) => new[] { (x, y)},
		_ => Array.Empty<(int,int)>()
	});
}

IEnumerable<(int,int)> GetHigherNeighbors ((int x,int y) loc)
{
	foreach (var neighbor in GetNeighbors((loc.x,loc.y)))
	{
		var d = data[neighbor.Item2][neighbor.Item1];
		if (d != 9 && d > data[loc.y][loc.x])
			yield return neighbor;
	}
}
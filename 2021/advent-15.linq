<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>RegExtract</NuGetReference>
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
var lines = @"1163751742
1381373672
2136511328
3694931569
7463417111
1319128137
1359912421
3125421639
1293138521
2311944581".GetLines();
#endif

var risks = lines.Select(line => line.Select(ch => ch - '0').ToArray()).ToArray();
var cumRisks = Enumerable.Range(0, risks.Length).Select(_ => Enumerable.Repeat(int.MaxValue, risks[0].Length).ToArray()).ToArray();

var frontier = new Dictionary<(int,int),int>();
frontier.Add((0, 0), 0);

while (frontier.Any())
{
	var head = frontier.OrderBy(x => x.Value).First();
	frontier.Remove(head.Key);

	foreach (var (dx, dy) in new[] { (0, 1), (1, 0), (0, -1), (-1, 0) })
	{
		var (x,y) = (head.Key.Item1 + dx, head.Key.Item2 + dy);
		if (x < risks[0].Length && x >= 0 && y < risks.Length && y >= 0)
		{
			if (head.Value + risks[y][x] < cumRisks[y][x])
			{
				cumRisks[y][x] = head.Value + risks[y][x];
				frontier[(x,y)] = head.Value + risks[y][x];
			}
		}
	}
}

cumRisks.Last().Last().Dump();

cumRisks = Enumerable.Range(0, risks.Length * 5).Select(_ => Enumerable.Repeat(int.MaxValue, risks[0].Length * 5).ToArray()).ToArray();
frontier = new Dictionary<(int, int), int>();
frontier.Add((0, 0), 0);

while (frontier.Any())
{
	var head = frontier.OrderBy(x => x.Value).First();
	frontier.Remove(head.Key);

	foreach (var (dx, dy) in new[] { (0, 1), (1, 0), (0, -1), (-1, 0) })
	{
		var (x, y) = (head.Key.Item1 + dx, head.Key.Item2 + dy);
		if (x < risks[0].Length * 5 && x >= 0 && y < risks.Length * 5 && y >= 0)
		{
			if (head.Value + GetRisk(x,y) < cumRisks[y][x])
			{
				cumRisks[y][x] = head.Value + GetRisk(x,y);
				frontier[(x, y)] = head.Value + GetRisk(x,y);
			}
		}
	}
}

int GetRisk(int x, int y) => ((risks[y % risks.Length][x % risks[0].Length] + (y / risks.Length) + (x / risks[0].Length) - 1) % 9) + 1;

cumRisks.Last().Last().Dump();
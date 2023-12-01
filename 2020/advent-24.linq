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
var lines = @"".GetLines();
#endif

var blackTiles = new HashSet<(int x, int y)>();

foreach (var line in lines)
{
	var coord = line.Extract<List<string>>(@"((?:w|e)|(?:n|s)(?:w|e))+").Aggregate((x: 0, y: 0), (loc, dir) =>
	{
		var (dx, dy) = dir switch
		{
			"w" => (-1, 0),
			"e" => (1, 0),
			"ne" => (1, -1),
			"sw" => (-1, 1),
			"nw" => (0, -1),
			"se" => (0, 1)
		};

		return (loc.x + dx, loc.y + dy);
	});
	
	if (blackTiles.Contains(coord)) blackTiles.Remove(coord);
	else blackTiles.Add(coord);
}

blackTiles.Count().Dump("Part 1");

var neighbors = new[] {(-1,0), (1,0), (1,-1), (-1, 1), (0,-1), (0,1)};

for (int c = 0; c < 100; c++)
{
	var newTiles = new HashSet<(int x, int y)>();
	
	var (x, X, y, Y) = (blackTiles.Min(x => x.x), blackTiles.Max(x => x.x), blackTiles.Min(x => x.y), blackTiles.Max(x => x.y));
	
	for (int xi = x - 1; xi <= X + 1; xi++)
	{
		for (int yi = y - 1; yi <= Y + 1; yi++)
		{
			var n = neighbors.Count(ng => blackTiles.Contains((ng.Item1 + xi, ng.Item2 + yi)));

			if (blackTiles.Contains((xi, yi)))
			{
				if (n == 1 || n == 2) newTiles.Add((xi, yi));
			}
			else
			{
				if (n == 2) newTiles.Add((xi,yi));
			}
		}
	}
	blackTiles = newTiles;
}

blackTiles.Count().Dump("Part 2");

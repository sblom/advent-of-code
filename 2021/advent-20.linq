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
var lines = @"..#.#..#####.#.#.#.###.##.....###.##.#..###.####..#####..#....#..#..##..###..######.###...####..#..#####..##..#.#####...##.#.#..#.##..#.#......#.###.######.###.####...#.##.##..#..#..#####.....#.#....###..#.##......#.....#..#..#..##..#...##.######.####.####.#.#...#.......#..#.#.#...####.##.#......#..#...##.#.##..#...##.#.##..###.#......#.#.......#.#.#.####.###.##...#.....####.#..#..#.##.#....##..#.####....##...##..#...#......#.#.......#.......##..####..#...#.#.#...##..#.#..###..#####........#..####......#..#

^#..#.
^#....
^##..#
..#..
..###".Replace("^","").GetLines();
#endif

var points = new HashSet<(int,int)>();

var groups = lines.GroupLines();
var rule = groups.First().First().Select(ch => ch switch { '#' => true, '.' => false }).ToArray();
//rule.Length.Dump();

var image = groups.Skip(1);

groups.Skip(1).First().Select((line, y) => line.Select((ch, x) => { if (ch == '#') points.Add((x, y)); return 0; }).ToArray()).ToArray();

//points.Dump();

var neighbors = new (int dx, int dy)[] { (-1, -1), (0, -1), (1, -1), (-1, 0), (0, 0), (1, 0), (-1, 1), (0, 1), (1, 1) };

(int x, int X, int y, int Y) bounds = points.Aggregate<(int x,int y),(int x, int X, int y, int Y)>((int.MaxValue, int.MinValue, int.MaxValue, int.MinValue), (a,x) => (Math.Min(a.x, x.x), Math.Max(a.X, x.x), Math.Min(a.y, x.y), Math.Max(a.Y, x.y)));;

bool beyond = false;

for (int c = 0; c < 50; c++)
{	
	var newpoints = new HashSet<(int,int)>();
	
	for (int y = bounds.y - 1; y <= bounds.Y + 1; y++)
	{
		for (int x = bounds.x - 1; x <= bounds.X + 1; x++)
		{
			if ((rule[neighbors.Select(n => (beyond && (x + n.dx < bounds.x || x + n.dx > bounds.X || y + n.dy < bounds.y || y + n.dy > bounds.Y)) || points.Contains((x + n.dx, y + n.dy)) ? 1 : 0).Aggregate(0, (a, b) => a * 2 + b)])) newpoints.Add((x,y));
		}
	}
	
	//newpoints.Dump();
	
	beyond = rule[beyond ? 511 : 0];
	bounds = (bounds.x - 1, bounds.X + 1, bounds.y - 1, bounds.Y + 1);
	points = newpoints;
	//DisplayPoints();
	
	if (c == 1) points.Count().Dump("Part 1");
}

//points.Dump();

points.Count().Dump("Part 2");

DisplayPoints();

void DisplayPoints()
{
	var sb = new StringBuilder();
	for (int y = bounds.y; y <= bounds.Y; y++)
	{
		for (int x = bounds.x; x <= bounds.X; x++)
		{
			sb.Append(points.Contains((x, y)) ? '#' : '.');
		}
		sb.AppendLine();
	}
	
	sb.ToString().DumpFixed();
}
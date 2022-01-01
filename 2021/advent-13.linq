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

List<(int x, int y)> dots = new();

var groups = lines.GroupLines().ToArray();

foreach (var dot in groups[0])
{
	var coords = dot.Split(",");
	dots.Add((int.Parse(coords[0]), int.Parse(coords[1])));
}

dots.Select(dot =>
dot switch
{
	(int x, int y) when x > 655 => (2 * 655 - x, y),
	(int x, int y) => (x, y)
}).Distinct().Count().Dump("Part 1");

var identity = ((int x, int y) p) => (p.x, p.y);
var transpose = ((int x, int y) p) => (p.y, p.x);

foreach (var fold in groups[1].Extract<(char axis, int val)>(@"fold along (.)=(\d+)"))
{
	var transform = fold.axis switch {
		'x' => identity,
		'y' => transpose
	};

	dots = dots.Select(dot =>
		transform(
			transform(dot) switch
			{
				(int x, int y) when x > fold.val => (2 * fold.val - x, y),
				(int x, int y) => (x, y)
			}
		)
	).Distinct().ToList();
}

StringBuilder sb = new();

for (int y = 0; y <= dots.Max(x => x.y); y++)
{
for (int x = 0; x <= dots.Max(x => x.x); x++)
{
	sb.Append(dots.Any(dot => dot == (x,y)) ? "##" : "  ");
}
	sb.AppendLine();
}

sb.ToString().DumpFixed("Part 2");
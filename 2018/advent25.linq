<Query Kind="Statements" />

var lines = await AoC.GetLinesWeb();

var points = new List<(int x, int y, int z, int w)>();

foreach (var line in lines)
{
	var coords = line.Split(',');
	var coordnums = coords.Select(x => int.Parse(x)).ToArray();
	var point = (coordnums[0], coordnums[1], coordnums[2], coordnums[3]);
	points.Add(point);
}

var edges = (from p1 in points from p2 in points where p1 != p2 && dist(p1,p2) <= 3 select (p1,p2)).ToList();

int dist((int,int,int,int) p1, (int,int,int,int) p2)
{
	return Math.Abs(p1.Item1 - p2.Item1) + Math.Abs(p1.Item2 - p2.Item2) + Math.Abs(p1.Item3 - p2.Item3) + Math.Abs(p1.Item4 - p2.Item4);
}

var constellations = new List<List<(int,int,int,int)>>();

while (points.Count > 0)
{
	var constellation = new List<(int,int,int,int)>();
	var frontier = new List<(int,int,int,int)>();
	var visited = new HashSet<(int,int,int,int)>();
	
	var p = points.First();
	points.RemoveAt(0);
	frontier.Add(p);
	visited.Add(p);
	
	while (frontier.Count > 0)
	{
		var f = frontier.First();
		frontier.RemoveAt(0);
		var candidates = (from edge in edges where edge.p1 == f && !visited.Contains(edge.p2) select edge.p2).ToList();
		frontier.AddRange(candidates);
		visited.UnionWith(candidates);
		points.RemoveAll(x => candidates.Contains(x));
	}	
	
	constellations.Add(constellation);
}

constellations.Count.Dump("Part 1");
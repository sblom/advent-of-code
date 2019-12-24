<Query Kind="Statements">
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

var lines = await AoC.GetLinesWeb();
var map = lines.Select(x => x.Trim().ToArray()).ToArray();

(int dx, int dy)[] dirs = new[] { (-1, 0), (1, 0), (0, -1), (0, 1) };

var frontier = new Queue<((int x, int y) loc, IImmutableSet<char> keys, int steps)>();
var visited = new HashSet<((int x, int y) loc, IImmutableSet<char> keys)>();

var entrance = (from y in Enumerable.Range(0,map.Length)
from x in Enumerable.Range(0,map[0].Length)
where map[y][x] == '@' select (x,y)).First();

frontier.Enqueue((entrance, ImmutableHashSet<char>.Empty, 0));
visited.Add((entrance, ImmutableHashSet<char>.Empty));

var max = 0;

while (frontier.Count > 0)
{
	var (loc, keys, steps) = frontier.Dequeue();
	
	if (char.IsLower(map[loc.y][loc.x]))
	{
		keys = keys.Add(map[loc.y][loc.x]);
	}
	
	var count = keys.Count;

	if (count > max)
	{
		max = count;
		count.Dump();
	}
	
	if (count == 26)
		steps.Dump("Part 1");
	
	foreach (var dir in dirs)
	{
		var nextch = map[loc.y + dir.dy][loc.x + dir.dx];

		if (nextch == '#')
			continue;

		if (visited.Contains(((loc.x + dir.dx, loc.y + dir.dy),keys)))
			continue;
		
		if (char.IsUpper(nextch) && !keys.Contains(char.ToLower(nextch)))
			continue;

		frontier.Enqueue(((loc.x + dir.dx, loc.y + dir.dy), keys, steps + 1));
		visited.Add(((loc.x + dir.dx, loc.y + dir.dy), keys));
		//visited.Dump();
	}
}
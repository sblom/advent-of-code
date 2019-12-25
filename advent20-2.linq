<Query Kind="Statements">
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

var lines = await AoC.GetLinesWeb();

var map = lines.Select(x => x.ToArray()).ToArray();

var portals = new List<((char, char), (int x, int y))>();

for (int i = 0; i < map.Length; i++)
{
	for (int j = 0; j < map[0].Length; j++)
	{
		if (char.IsLetter(map[i][j]))
		{
			if (i < map.Length - 2 && map[i + 2][j] == '.' && char.IsLetter(map[i + 1][j]))
			{
				portals.Add(((map[i][j], map[i + 1][j]), (j, i + 1)));
				map[i][j] = ' ';
			}
			else if (j < map[0].Length - 2 && map[i][j + 2] == '.' && char.IsLetter(map[i][j + 1]))
			{
				portals.Add(((map[i][j], map[i][j + 1]), (j + 1, i)));
				map[i][j] = ' ';
			}
			else if (i > 0 && map[i - 1][j] == '.' && char.IsLetter(map[i + 1][j]))
			{
				portals.Add(((map[i][j], map[i + 1][j]), (j, i)));
				map[i + 1][j] = ' ';
			}
			else if (j > 0 && map[i][j - 1] == '.' && char.IsLetter(map[i][j + 1]))
			{
				portals.Add(((map[i][j], map[i][j + 1]), (j, i)));
				map[i][j + 1] = ' ';
			}
		}
	}
}

//map.Select(x => string.Join("",x)).DumpFixed();

var warps = from p in portals from q in portals where p.Item1 == q.Item1 && p.Item2 != q.Item2 select (p.Item2,q.Item2);

var warpDict = warps.ToDictionary(x => x.Item1, x => x.Item2);

var start = portals.Where(kv => kv.Item1 == ('A','A')).Select(kv => kv.Item2).Single();

var frontier = ImmutableQueue<((int x, int y, int z), int steps)>.Empty.Enqueue(((start.x, start.y + 1, 0), 0));
var visited = new HashSet<(int x, int y, int z)> { (start.x, start.y, 0) };

var dirs = new (int x, int y)[] {
	(-1,0),(1,0),(0,-1),(0,1)
};

while (!frontier.IsEmpty)
{
	var (loc, steps) = frontier.Peek();
	frontier = frontier.Dequeue();
	
	if (visited.Contains(loc)) continue;	
	visited.Add(loc);
	
	foreach (var dir in dirs)
	{
		var next = (x: loc.x + dir.x, y: loc.y + dir.y, z: loc.z);

		if (map[next.y][next.x] == '.')
		{
			frontier = frontier.Enqueue((next, steps + 1));
		}
		else if (char.IsLetter(map[next.y][next.x]))
		{
			if (loc.z == 0 && portals.Where(x => x.Item1 == ('Z', 'Z')).First().Item2 == (next.x, next.y))
			{
				// 630 is too high.
				// 620 is still too high.
				// 610 is too low.
				// 619 is wrong.
				(steps).Dump("Part 1");
				goto done_part1;
			}
			
			if (portals.Where(x => x.Item1 == ('A', 'A')).First().Item2 == (next.x, next.y) || portals.Where(x => x.Item1 == ('Z', 'Z')).First().Item2 == (next.x, next.y))
				continue;
			
			var depth = next.z;
			
			if (next.x < 10 || next.y < 10 || next.x > 100 || next.y > 100)
			{
				if (depth == 0) continue;
				else depth = depth - 1;
			}
			else
			{
				depth = depth + 1;
			}
			
			var warp = warpDict[(next.x, next.y)];
			
			frontier = frontier.Enqueue(((warp.x, warp.y, depth), steps));
		}
	}
}

done_part1: ;
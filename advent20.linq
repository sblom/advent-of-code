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

var frontier = ImmutableQueue<((int x,int y), int steps, IImmutableSet<(int,int)>)>.Empty.Enqueue(((start.x, start.y + 1), 0, ImmutableHashSet<(int,int)>.Empty.Add((start.x, start.y)).Add((start.x, start.y + 1))));

var dirs = new (int x, int y)[] {
	(-1,0),(1,0),(0,-1),(0,1)
};

while (!frontier.IsEmpty)
{
	var (loc, steps, visited) = frontier.Peek();
	frontier = frontier.Dequeue();
	
	foreach (var dir in dirs)
	{
		var next = (x: loc.x + dir.x, y: loc.y + dir.y);
		if (visited.Contains(next)) continue;

		if (map[next.y][next.x] == '.')
		{
			frontier = frontier.Enqueue((next, steps + 1, visited.Add(next)));
		}
		else if (char.IsLetter(map[next.y][next.x]))
		{
			if (portals.Where(x => x.Item1 == ('Z', 'Z')).First().Item2 == next)
			{
				// 630 is too high.
				// 620 is still too high.
				// 610 is too low.
				// 619 is wrong.
				(steps).Dump("Part 1");
				goto done_part1;
			}
			
			frontier = frontier.Enqueue((warpDict[next], steps, visited.Add(warpDict[next])));
		}
	}
}

done_part1: ;
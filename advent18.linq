<Query Kind="Statements">
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

var lines = await AoC.GetLinesWeb();

var map = lines.Select(x => x.Trim().ToArray()).ToArray();

var start = (from y in Enumerable.Range(0,map.Length) from x in Enumerable.Range(0,map[0].Length) where map[y][x] == '@' select (x, y)).First();

(int dx, int dy)[] dirs = new[] { (-1, 0), (1, 0), (0, -1), (0, 1)};

(IImmutableSet<char> keys, (int x, int y) loc, int steps) state;

IImmutableDictionary<char, (int steps, (int x,int y) loc)> ReachableKeys(IImmutableSet<char> keys, (int x, int y) loc)
{
	var reachable = ImmutableDictionary<char, (int, (int, int))>.Empty;

	var frontier = new Queue<(IImmutableSet<char> keys, (int x, int y) loc, int steps, IImmutableSet<(int,int)> visited)>(new[] {(keys, loc, 0, ImmutableHashSet<(int,int)>.Empty.Add(loc) as IImmutableSet<(int,int)>) });
	
	while (frontier.Count > 0)
	{
		IImmutableSet<(int x, int y)> visited; int steps;
		(keys, loc, steps, visited) = frontier.Dequeue();
		var cell = map[loc.y][loc.x];
		switch (cell)
		{
			case char c when c >= 'A' && c <= 'Z':
				if (!keys.Contains(char.ToLower(c)))
				{
					// We don't have the right key, treat this as a wall.
					continue;
				}
				// Otherwise, treat it as a path.
				break;
			case char c when c >= 'a' && c <= 'z':
				// If it's a new key, stop here.
				if (!keys.Contains(c))
				{
					keys = keys.Add(c);
					if (!reachable.ContainsKey(c))
						reachable = reachable.Add(c,(steps,loc));
					continue;
				}
				// Otherwise, treat it like it's a path.
				break;
			case '#':
				continue;
		}
		
		foreach (var dir in dirs)
		{
			(int x, int y) next = (loc.x + dir.dx, loc.y + dir.dy);
			if (!visited.Contains(next) && map[next.y][next.x] != '#')
			{
				frontier.Enqueue((keys, next, steps + 1, visited.Add(next)));
			}
		}
	}
	
	return reachable;
}

var frontier = new Queue<(IImmutableSet<char> keys, (int x, int y) loc, int steps)>(new[] { (ImmutableHashSet<char>.Empty as IImmutableSet<char>, start, 0) });

while (frontier.Count > 0)
{
	var front = frontier.Dequeue();
	
	if (front.keys.Count == 26) front.steps.Dump("Part 1");
	
	var reachableKeys = ReachableKeys(front.keys, front.loc).Take(1);
	
	foreach (var key in reachableKeys)
	{
		frontier.Enqueue((front.keys.Add(key.Key), key.Value.loc, front.steps + key.Value.steps));
	}
}
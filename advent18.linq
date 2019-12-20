<Query Kind="Statements">
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

var lines = await AoC.GetLinesWeb();
var map = lines.Select(x => x.Trim().ToArray()).ToArray();

(int dx, int dy)[] dirs = new[] { (-1, 0), (1, 0), (0, -1), (0, 1) };

Func<char,(int x, int y)> findkey = (char ch) => (from y in Enumerable.Range(0, map.Length) from x in Enumerable.Range(0, map[0].Length) where map[y][x] == ch select (x, y)).First();

Dictionary<char,Dictionary<char,(int dist,IImmutableSet<char> doors,IImmutableSet<char> keys)>> distances = new Dictionary<char, System.Collections.Generic.Dictionary<char, (int dist, IImmutableSet<char> doors, IImmutableSet<char> keys)>>();

distances['@'] = FindDistances('@').ToDictionary(x => x.Key, x => x.Value);
for (char ch = 'a'; ch <= 'z'; ch++)
{
	distances[ch] = FindDistances(ch).ToDictionary(x => x.Key, x => x.Value);
}

//distances.Dump();

IImmutableDictionary<char,(int dist,IImmutableSet<char> doors,IImmutableSet<char> keys)> FindDistances(char ch)
{
	var loc = (from y in Enumerable.Range(0, map.Length) from x in Enumerable.Range(0, map[0].Length) where map[y][x] == ch select (x, y)).First();

	var distances = ImmutableDictionary<char,(int dist,IImmutableSet<char> doors, IImmutableSet<char> keys)>.Empty;

	var frontier = new Queue<(IImmutableSet<char> doors, IImmutableSet<char> keys, (int x, int y) loc, int steps, IImmutableSet<(int,int)> visited)>(new[] {(ImmutableHashSet<char>.Empty as IImmutableSet<char>, ImmutableHashSet<char>.Empty as IImmutableSet<char>, loc, 0, ImmutableHashSet<(int,int)>.Empty.Add(loc) as IImmutableSet<(int,int)>) });
	
	while (frontier.Count > 0)
	{
		IImmutableSet<char> doors; IImmutableSet<char> keys; IImmutableSet<(int x, int y)> visited; int steps;
		(doors, keys, loc, steps, visited) = frontier.Dequeue();
		var cell = map[loc.y][loc.x];
		switch (cell)
		{
			case char c when c >= 'A' && c <= 'Z':
				doors = doors.Add(c);
				// Otherwise, treat it as a path.
				break;
			case char c when c >= 'a' && c <= 'z':
				if (!distances.ContainsKey(c))
				{
					distances = distances.Add(c,(steps,doors,keys));
				}
				keys = keys.Add(c);				
				break;
			case '#':
				continue;
		}
		
		foreach (var dir in dirs)
		{
			(int x, int y) next = (loc.x + dir.dx, loc.y + dir.dy);
			if (!visited.Contains(next) && map[next.y][next.x] != '#')
			{
				frontier.Enqueue((doors, keys, next, steps + 1, visited.Add(next)));
			}
		}
	}
	
	return distances;
}

var frontier = new Queue<(IImmutableSet<char> keys, char currentKey, int steps)>(new[] { (ImmutableHashSet<char>.Empty as IImmutableSet<char>, '@', 0) });

var maxkeys = 0;

while (frontier.Count > 0)
{
	var states = new Dictionary<(string, char), int>();
	
	var front = frontier.Dequeue();
	
	if (front.keys.Count > maxkeys)
	{
		maxkeys = front.keys.Count.Dump();
	}
	
	if (front.keys.Count == 26) front.steps.Dump("Part 1");
	
	var reachableKeys = distances[front.currentKey].Where(kv => !front.keys.Contains(kv.Key) && kv.Value.doors.All(key => front.keys.Contains(char.ToLower(key))) && kv.Value.keys.All(key => front.keys.Contains(key))).OrderBy(kv => kv.Value.dist);
	
	foreach (var key in reachableKeys)
	{
		var next = front.keys.Add(key.Key);
		//var hashkey = (string.Join("",next.OrderBy(ch => ch)), key.Key);
		//if (states.ContainsKey(hashkey) && front.steps + key.Value.dist >= states[hashkey])
		//{
		//		continue;
		//}
		//else
		//{
		//	states[hashkey] = front.steps + key.Value.dist;
		//}
		
		frontier.Enqueue((front.keys.Add(key.Key), key.Key, front.steps + key.Value.dist));
	}
}
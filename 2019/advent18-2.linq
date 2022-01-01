<Query Kind="Statements">
  <Connection>
    <ID>b7440089-72f5-46f7-8c5f-8419bdb81277</ID>
    <Persist>true</Persist>
    <Driver Assembly="IQDriver" PublicKeyToken="5b59726538a49684">IQDriver.IQDriver</Driver>
    <Provider>System.Data.SQLite</Provider>
    <CustomCxString>Data Source=C:\Users\sblom\AppData\Local\Packages\Fitbit.Fitbit_6mqt6hf9g46tw\LocalState\fitbit.232TPT.db;FailIfMissing=True</CustomCxString>
    <AttachFileName>&lt;LocalApplicationData&gt;\Packages\Fitbit.Fitbit_6mqt6hf9g46tw\LocalState\fitbit.232TPT.db</AttachFileName>
    <DisplayName>fitbit</DisplayName>
    <DriverData>
      <StripUnderscores>false</StripUnderscores>
      <QuietenAllCaps>false</QuietenAllCaps>
    </DriverData>
  </Connection>
  <NuGetReference>RawScape.Wintellect.PowerCollections</NuGetReference>
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>Wintellect.PowerCollections</Namespace>
</Query>

var lines = await AoC.GetLinesWeb();

//lines = @"#################
//#i.G..c...e..H.p#
//########.########
//#j.A..b...f..D.o#
//########@########
//#k.E..a...g..B.n#
//########.########
//#l.F..d...h..C.m#
//#################".Split('\n');

var map = lines.Select(x => x.Trim().ToArray()).ToArray();

var replacement = new[] {"1#2","###","3#4"};

for (int i = -1; i <= 1; i++)
{
	for (int j = -1; j <= 1; j++)	
	{
		map[40+i][40+j] = replacement[1 + i][1 + j];
	}
}

map.Select(x => string.Join("",x)).DumpFixed();

(int dx, int dy)[] dirs = new[] { (-1, 0), (1, 0), (0, -1), (0, 1) };

Func<char, (int x, int y)> findkey = (char ch) => (from y in Enumerable.Range(0, map.Length) from x in Enumerable.Range(0, map[0].Length) where map[y][x] == ch select (x, y)).First();

Dictionary<char, Dictionary<char, (int dist, IImmutableSet<char> doors, IImmutableSet<char> keys)>> distances = new Dictionary<char, System.Collections.Generic.Dictionary<char, (int dist, IImmutableSet<char> doors, IImmutableSet<char> keys)>>();

for (char ch = '1'; ch <= '4'; ch++)
{
	distances[ch] = FindDistances(ch).ToDictionary(x => x.Key, x => x.Value);
}

int keycount = 0;

for (char ch = 'a'; ch <= 'z'; ch++)
{
	var dists = FindDistances(ch);
	if (dists == null)
		break;
	distances[ch] = dists.ToDictionary(x => x.Key, x => x.Value);
	keycount++;
}

distances.Dump();

IImmutableDictionary<char,(int dist,IImmutableSet<char> doors,IImmutableSet<char> keys)> FindDistances(char ch)
{
	var loc = (from y in Enumerable.Range(0, map.Length) from x in Enumerable.Range(0, map[0].Length) where map[y][x] == ch select (x, y)).FirstOrDefault();
	
	if (loc == (0,0)) return null;

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

var frontier = new OrderedBag<(IImmutableList<char> keys, char[] currentKeys, int steps)>(
	Comparer<(IImmutableList<char> keys, char[] currentKeys, int steps)>.Create((x,y) => x.steps - y.steps)
) { (ImmutableList<char>.Empty as IImmutableList<char>, "1234".ToCharArray(), 0) };

var maxkeys = 0;

var states = new HashSet<(string keys, string positions)>();

while (frontier.Count > 0)
{
	var front = frontier.RemoveFirst();
	
	//(string.Join("",front.keys),front.currentKey,front.steps).ToString().Dump();

	var hashkey = (string.Join("", front.keys.OrderBy(ch => ch)), string.Join("",front.currentKeys));
	if (states.Contains(hashkey))
	{
		continue;
	}

	states.Add(hashkey);

	if (front.keys.Count > maxkeys)
	{
		maxkeys = front.keys.Count.Dump();
	}

	// 5532 is too high.
	if (front.keys.Count == keycount)
	{
		front.steps.Dump("Part 1");
		front.keys.Dump();
		break;
	}
	
	for (int i = 0; i < 4; i++)
	{
		var reachableKeys = distances[front.currentKeys[i]].Where(kv => !front.keys.Contains(kv.Key) && kv.Value.doors.All(key => front.keys.Contains(char.ToLower(key))) && kv.Value.keys.All(key => front.keys.Contains(key))).OrderBy(kv => kv.Value.dist);
		
		foreach (var key in reachableKeys)
		{
			var next = front.keys.Add(key.Key);
			var nextpositions = front.currentKeys.ToArray();
			nextpositions[i] = key.Key;
	
			frontier.Add((next, nextpositions, front.steps + key.Value.dist));
		}
	}
}
<Query Kind="Statements">
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var lines = await AoC.GetLinesWeb();

var weights = lines.Reverse().Select(int.Parse);

var quarter = weights.Sum() / 4;

var ways = new Dictionary<int, List<IImmutableSet<int>>> { { 0, new List<IImmutableSet<int>> { ImmutableHashSet<int>.Empty } } };

foreach (var weight in weights)
{
	//weight.Dump();
	foreach (var way in ways.ToDictionary(kv => kv.Key, kv => kv.Value.ToList()))
	{
		var newweight = way.Key + weight;
		
		if (newweight > quarter) continue;
		
		if (!ways.ContainsKey(newweight))
		{
			ways[newweight] = way.Value.Select(x => x.Add(weight)).ToList();
			//ways[newweight].Dump();
		}
		else
		{
			if (ways[newweight][0].Count > way.Value[0].Count + 1)
			{
				ways[newweight] = way.Value.Select(x => x.Add(weight)).ToList();
				// (newweight,ways[newweight]).Dump();				
			}
			else if (ways[newweight][0].Count == way.Value[0].Count + 1)
			{
				ways[newweight].AddRange(way.Value.Select(x => x.Add(weight)));
				// (newweight,ways[newweight]).Dump();
			}
		}
	}
}

//ways[quarter].Dump();

var shortest = ways[quarter].ToList();

var success = new List<IImmutableSet<int>>();

foreach (var candidate in shortest)
{
	ways = new Dictionary<int, List<IImmutableSet<int>>> { { 0, new List<IImmutableSet<int>> { ImmutableHashSet<int>.Empty } } };
	
	foreach (var weight in weights.Where(x => !candidate.Contains(x)))
	{
		//weight.Dump();
		//weight.Dump();
		foreach (var prev in ways.ToDictionary(kv => kv.Key, kv => kv.Value.ToList()))
		{
			var newweight = prev.Key + weight;
			if (newweight > quarter) continue;
			
			if (!ways.ContainsKey(newweight))
			{
				ways[newweight] = new List<IImmutableSet<int>>();
			}
			ways[newweight].AddRange(prev.Value.Select(x => x.Add(weight)));
		}
	}

	if ((from x in ways[quarter] from y in ways[quarter] where !x.Any(v => y.Contains(v)) select true).Any())
	{
		success.Add(candidate);
	}
	else
	{
		candidate.Dump("Impossible");
	}
}

success.OrderBy(x => x.Select(x=>(long)x).Aggregate((a,b) => a * b)).First().Select(x => (long)x).Aggregate((a,b) => a * b).Dump();
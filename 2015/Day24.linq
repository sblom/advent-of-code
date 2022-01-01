<Query Kind="Statements">
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var lines = await AoC.GetLinesWeb();

var weights = lines.Reverse().Select(int.Parse);

var third = weights.Sum() / 3;

var ways = new Dictionary<int, List<IImmutableSet<int>>> { { 0, new List<IImmutableSet<int>> { ImmutableHashSet<int>.Empty } } };

foreach (var weight in weights)
{
	//weight.Dump();
	foreach (var way in ways.ToDictionary(kv => kv.Key, kv => kv.Value.ToList()))
	{
		var newweight = way.Key + weight;
		
		if (newweight > third) continue;
		
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

var shortest = ways[third].ToList();

var prevs = new HashSet<int> { 0 };

var success = new List<IImmutableSet<int>>();

foreach (var candidate in shortest)
{
	foreach (var weight in weights.Where(x => !candidate.Contains(x)))
	{
		//weight.Dump();
		foreach (var prev in prevs.ToList())
		{
			var newweight = prev + weight;

			if (newweight == third)
			{
				//candidate.Dump();
				// 778231 is too low.
				// 118135586371‬ is wrong.
				success.Add(candidate);
				//candidate.Select(x => (long)x).Aggregate((a,b) => a * b).Dump("QE");
				goto next_candidate;
			}
	
			if (newweight > third) continue;
	
			prevs.Add(newweight);
		}
	}
	next_candidate:;
}

success.OrderBy(x => x.Select(x=>(long)x).Aggregate((a,b) => a * b)).First().Select(x => (long)x).Aggregate((a,b) => a * b).Dump();
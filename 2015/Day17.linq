<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async void Main()
{
	var lines = (await AoC.GetLinesWeb()).Select(x => int.Parse(x));

	var ways = new Dictionary<int, Dictionary<int, int>> { { 0, new Dictionary<int, int> { { 0, 1 } } } };
	var ways2 = new List<(int,IEnumerable<int>)>();
	
	foreach (var size in lines)
	{
		foreach (var kvp in ways.ToList())
		{
			if (!ways.ContainsKey(kvp.Key + size))
			{
				ways[kvp.Key + size] = ways[kvp.Key].ToDictionary(a => a.Key + 1, a => a.Value);
			}
			else
			{
				foreach (var kvp2 in ways[kvp.Key].ToList())
				{
					if (ways[kvp.Key + size].ContainsKey(kvp2.Key + 1))
					{
						ways[kvp.Key + size][kvp2.Key + 1] += ways[kvp.Key][kvp2.Key];
					}
					else
					{
						ways[kvp.Key + size][kvp2.Key + 1] = ways[kvp.Key][kvp2.Key];
					}
				}
			}
		}
	}
	
	ways[150].Dump("Part 1");
	
	for (int i = 0; i < 1 << lines.Count(); i++)
	{
		if (i % 10000 == 0) i.Dump();
		
		var cups = lines.Zip(FromBits(i)).Where(x => x.Second).Select(x => x.First);
		
		if (cups.Sum() == 150)
		{
			ways2.Add((cups.Count(), cups));
		}
	}
	
	ways2.OrderBy(x => x.Item2.Count()).Dump();
}

IEnumerable<bool> FromBits(int i)
{
	while (true)
	{
		yield return (i % 2) == 1;
		i = i / 2;
	}
}

// Define other methods, classes and namespaces here

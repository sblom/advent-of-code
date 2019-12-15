<Query Kind="Statements">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var lines = (await AoC.GetLinesWeb()).Select(x => int.Parse(x));

var ways = new Dictionary<int, int> { { 0, 1 } };

foreach (var size in lines)
{
	foreach (var kvp in ways.ToList())
	{
		var count = ways.GetValueOrDefault(kvp.Key + size);
		ways[kvp.Key + size] = count + kvp.Value;
	}
}

ways.OrderBy(x => x.Key).Dump();
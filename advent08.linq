<Query Kind="Statements">
  <NuGetReference>BenchmarkDotNet</NuGetReference>
  <NuGetReference>System.Collections.Immutable</NuGetReference>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>BenchmarkDotNet.Running</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var lines = await AoC.GetLinesWeb();
var line = lines.First().Trim();

var list = new List<(int zeros, int product)>();

for (int i = 0; i < line.Length; i += 25 * 6)
{
	var group = line[i..(i + 25 * 6)].GroupBy(ch => ch);
	var d = group.ToDictionary(g => g.Key, g => g.Count());
	list.Add((zeros: d['0'], product: d['1'] * d['2']));
}

var min = list.Min(v => v.zeros);
list.Where(v => v.zeros == min).Select(v => v.product).First().Dump("Part 1");

var layers = new List<int[,]>();

for (int i = 0; i < line.Length; i += 25 * 6)
{
	var layer = new int[6,25];
	for (int y = 0; y < 6; ++y)
	{
		for (int x = 0; x < 25; ++x)
		{
			layer[y,x] = line[i + y * 25 + x] - '0';
		}
	}
	
	layers.Add(layer);
}

var result = new int[6,25];

for (int y = 0; y < 6; ++y)
{
	for (int x = 0; x < 25; ++x)
	{
		for (int i = 0; ; ++i)
		{
			if (layers[i][y,x] != 2)
			{
				result[y,x] = layers[i][y,x];
				break;
			}
		}
	}
}

for (int y = 0; y < 6; ++y)
{
	var ln = "";
	for (int x = 0; x < 25; ++x)
	{
		ln += result[y,x] == 1 ? "XX" : "  ";
	}
	ln.DumpFixed();
}

var res = from y in Enumerable.Range(0, 6)
		  select from x in Enumerable.Range(0, 25)
		  select (from i in Enumerable.Range(0, line.Length / 25 / 6)
		  select line[(i * 25 * 6 + y * 25 + x)]).Aggregate((a, b) => a == '2' ? b : a);

res.Select(line => string.Join("", line.Select(ch => ch == '1' ? "XX" : "  "))).DumpFixed("Part 2 LINQWhack");
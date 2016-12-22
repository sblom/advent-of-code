<Query Kind="Statements" />

Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
var lines = File.ReadLines("advent20.txt");

var rx = new Regex(@"(\d+)-(\d+)");

var ranges = new List<(uint, uint)>();

foreach (var line in lines)
{
	var match = rx.Match(line);
	ranges.Add((uint.Parse(match.Groups[1].Value),uint.Parse(match.Groups[2].Value)));
}

ranges = ranges.OrderBy(r => r.Item1).ToList();

//ranges.Dump();

for (int i = 1; i < ranges.Count; i++)
{
	while (i < ranges.Count && (ranges[i].Item1 <= ranges[i - 1].Item2 + 1 || ranges[i - 1].Item2 == uint.MaxValue))
	{
		ranges[i - 1] = (ranges[i-1].Item1, Math.Max(ranges[i].Item2,ranges[i-1].Item2));
		ranges.RemoveAt(i);
	}
}

uint allowed = 0;

(ranges[0].Item2 + 1).Dump("Part 1");

for (int i = 1; i < ranges.Count; i++)
{
	var diff = ranges[i].Item1 - ranges[i - 1].Item2 - 1;
	allowed += diff;
}

allowed += 4294967295 - ranges[ranges.Count - 1].Item2;

allowed.Dump("Part 2");
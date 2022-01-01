<Query Kind="Statements" />

var lines = AoC.GetLines().ToList();

var part1 = 0;
var part2 = 0;

var twohashset = new HashSet<string>();
var threehashset = new HashSet<string>();

foreach (var line in lines)
{
	var groups = line.GroupBy(ch => ch);
	
	foreach (var group in groups)
	{
		if (group.Count() == 2)
		  twohashset.Add(line);
		if (group.Count() == 3)
			threehashset.Add(line);
	}
}

foreach (var line in lines)
{
	foreach (var other in lines)
	{
		if (line == other) continue;
		
		int c = 0;
		
		for (int i = 0; i < line.Length; i++)
		{
			if (line[i] != other[i]) c++;
		}
		
		if (c == 1)
		{
			line.Dump();
			other.Dump();
			break;
		}
	}
}

(twohashset.Count * threehashset.Count).Dump("Part 1");
part2.Dump("Part 2");
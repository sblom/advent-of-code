<Query Kind="Statements" />

var lines = await AoC.GetLinesWeb();

lines.Dump();

var dict = lines.Select(line => line.Split(':').Select(x => int.Parse(x.Trim())).ToArray()).ToDictionary(a => a[0], b => b[1]);

int sev = 0;

for (int i = 0; ; i++)
{
	foreach (var kv in dict)
	{
		if (0 == (kv.Key + i) % ((kv.Value - 1) * 2))
		{
			goto next_i;
		}
	}
	
	sev = i;
	break;

next_i: continue;
}


sev.Dump();
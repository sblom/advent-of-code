<Query Kind="Statements" />

var lines = await AoC.GetLinesWeb();

var vals = new List<((int,int),(int,int))>();

foreach (var line in lines)
{
	var split = line.Split('<','>');
	var poses = split[1];
	var vels = split[3];
	
	var posnums = poses.Split(',').Select(x => int.Parse(x)).ToList();
	var velnums = vels.Split(',').Select(x => int.Parse(x)).ToList();
	
	vals.Add(((posnums[0],posnums[1]),(velnums[0],velnums[1])));
}

IEnumerable<(int,int)> prev = null;
int prevwidth = int.MaxValue;
int prevheight = int.MaxValue;

for (int i = 0; ; i++)
{
	var tr = vals.Select(x => ((x.Item1.Item1 + x.Item2.Item1 * i), (x.Item1.Item2 + x.Item2.Item2 * i)));
	
	var width = Math.Abs(tr.Max(x=>x.Item1) - tr.Min(x=>x.Item1));
	var height = Math.Abs(tr.Max(x=>x.Item2) - tr.Min(x=>x.Item2));
	
	// I figure we'll grow once we've found the message.
	if (width > prevwidth && height > prevheight)
	{
		"".Dump("Part 1");
		i = i - 1;
		tr = vals.Select(x => ((x.Item1.Item1 + x.Item2.Item1 * i), (x.Item1.Item2 + x.Item2.Item2 * i)));
		for (var y = tr.Min(X => X.Item2); y <= tr.Max(X => X.Item2); y++)
		{
			var linesb = new StringBuilder();
			for (var x = tr.Min(X => X.Item1); x <= tr.Max(X => X.Item1); x++)
			{
				if (tr.Any(z => z.Item1 == x && z.Item2 == y))
				{
					linesb.Append("#");
				}
				else linesb.Append(" ");
			}
			linesb.ToString().DumpFixed();
		}
		i.Dump("Part 2");
		break;
	}

	prevwidth = width;
	prevheight = height;

//	$"{i}: {width}x{height}".Dump();
}


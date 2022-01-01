<Query Kind="Statements" />

var lines = AoC.GetLines().ToList();

var size = lines.Count();
var nodes = new Dictionary<(int,int),char>();

var dir = (0, -1);
var loc = (0, 0);
var infected = 0;

void Initialize(){
	dir = (0, -1);
	loc = (0, 0);
	infected = 0;
	nodes.Clear();
	for (int x = 0; x < size; x++)
	{
		for (int y = 0; y < size; y++)
		{
			if (lines[y][x] == '#')
			{
				nodes.Add((x - 12, y - 12),'I');
			}
		}
	}
}


Initialize();

for (int burst = 0; burst < 10_000; burst++)
{
	if (!nodes.ContainsKey(loc))
	{
		// left
		dir = (dir.Item2, -dir.Item1);
		nodes.Add(loc,'I');
		infected++;
	}
	else if (nodes[loc] == 'I')
	{
		//right
		dir = (-dir.Item2, dir.Item1);
		nodes.Remove(loc);
	}

	loc = (loc.Item1 + dir.Item1, loc.Item2 + dir.Item2);
}

infected.Dump("part 1");

Initialize();

for (int burst = 0; burst < 10_000_000; burst++)
{
	if (!nodes.ContainsKey(loc))
	{
		// left
		dir = (dir.Item2, -dir.Item1);
		nodes[loc] = 'W';
	}
	else if (nodes[loc] == 'W')
	{
		nodes[loc] = 'I';
		infected++;
	}
	else if (nodes[loc] == 'I')
	{		
		//right
		dir = (-dir.Item2, dir.Item1);
		nodes[loc] = 'F';
	}
	else if (nodes[loc] == 'F')
	{
		dir = (-dir.Item1, -dir.Item2);
		nodes.Remove(loc);
	}

	loc = (loc.Item1 + dir.Item1, loc.Item2 + dir.Item2);
}

infected.Dump("part 2");
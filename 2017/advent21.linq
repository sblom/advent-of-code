<Query Kind="Statements" />

var lines = AoC.GetLines();

IEnumerable<(string,string)> Rotations((string,string) rule)
{
	var block = rule.Item1.Split('/');
	
	var result = new StringBuilder();
	
	foreach (var transform in new Func<int,int,char>[]{
		(x,y)=>block[y][x],
		(x,y)=>block[block.Length - y - 1][x],
		(x,y)=>block[y][block.Length - x - 1],		
		(x,y)=>block[block.Length - 1 - x][block.Length - 1 - y],
		(x,y)=>block[x][y],
		(x,y)=>block[block.Length - 1 - x][y],
		(x,y)=>block[x][block.Length - 1 - y],
		(x,y)=>block[block.Length - 1 - x][block.Length - 1 - y]
	})
	{
		for (int y = 0; y < block.Length; y++)
		{
			if (y != 0) result.Append("/");
			for (int x = 0; x < block.Length; x++)
			{
				result.Append(transform(x,y));
			}
		}
		yield return (result.ToString(), rule.Item2);
		result.Clear();
	}
}

var rules = lines.Select(x => x.Split(' ')).Select(y => (y[0], y[2])).SelectMany(x => Rotations(x)).GroupBy(x=>x.Item1).Select(x => (x.Key, x.First().Item2)).ToDictionary(x => x.Item1, y=>y.Item2);

rules.Dump();

var grid = new[]{
".#.".ToCharArray(),
"..#".ToCharArray(),
"###".ToCharArray()};
for (int n = 0; n < 18; n++)
{
	int b = 0;
	if (grid.Length % 2 == 0)
	{
		b = 2;
	}
	else if (grid.Length % 3 == 0)
	{
		b = 3;
	}

	var newgrid = new char[grid.Length / b * (b + 1)][];
	for (int i = 0; i < newgrid.Length; i++)
	{
		newgrid[i] = new char[grid.Length / b * (b + 1)];
	}
	
	for (int y = 0; y < grid.Length / b; y++)
	{
		for (int x = 0; x < grid.Length / b; x++)
		{
			var block = new StringBuilder();
			for (int dy = 0; dy < b; dy++)
			{
				if (dy != 0) block.Append("/");
				for (int dx = 0; dx < b; dx++)
				{
					block.Append(grid[b*y+dy][b*x+dx]);
				}
			}
			
			var outblock = rules[block.ToString()].Split('/');

			for (int dy = 0; dy < b + 1; dy++)
			{
				for (int dx = 0; dx < b + 1; dx++)
				{
					newgrid[(b + 1) * y + dy][(b + 1) * x + dx] = outblock[dy][dx];
				}
			}

			block.Clear();
		}
	}
	grid = newgrid;
}
//grid.Select(x => string.Join("",x)).Dump();

grid.Select(x => x.Count(y => y == '#')).Sum().Dump();
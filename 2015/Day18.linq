<Query Kind="Statements" />

var lines = (await AoC.GetLinesWeb()).ToList();

var grid = new bool[102][];
var grid2 = new bool[102][];
for (int i = 0; i < 102; i++)
{
	grid[i] = new bool[102];
	grid2[i] = new bool[102];	
}

for (int i = 1; i <= 100; i++)
{
	for (int j = 1; j <= 100; j++)
	{
		grid[i][j] = lines[i - 1][j - 1] == '#';
		grid2[i][j] = lines[i - 1][j - 1] == '#';		
	}
}

grid.Select(x => x.Where(x => x).Count()).Sum().Dump("before");

for (int n = 0; n < 100; n++)
{
	var newgrid = new bool[102][];
	newgrid[0] = new bool[102];
	newgrid[101] = new bool[102];
	
	for (int i = 1; i <= 100; i++)
	{
		newgrid[i] = new bool[102];
		for (int j = 1; j <= 100; j++)
		{
			var neighbors = (from dx in Enumerable.Range(-1, 3)
			from dy in Enumerable.Range(-1, 3)
			where dx != 0 || dy != 0
			where grid[i + dy][j + dx] select true).Count();
			if (grid[i][j])
			{
				if (neighbors == 2 || neighbors == 3)
				{
					newgrid[i][j] = true;
				}
				else
				{
					newgrid[i][j] = false;
				}
			}
			else
			{
				if (neighbors == 3)
				{
					newgrid[i][j] = true;
				}
				else
				{
					newgrid[i][j] = false;
				}
			}
		}
	}
	grid = newgrid;
}

grid.Select(x => x.Where(x => x).Count()).Sum().Dump("after");

grid = grid2;

for (int n = 0; n < 100; n++)
{
	var newgrid = new bool[102][];
	newgrid[0] = new bool[102];
	newgrid[101] = new bool[102];

	for (int i = 1; i <= 100; i++)
	{
		grid[1][1] = true;
		grid[1][100] = true;
		grid[100][1] = true;
		grid[100][100] = true;
		
		newgrid[i] = new bool[102];
		for (int j = 1; j <= 100; j++)
		{
			var neighbors = (from dx in Enumerable.Range(-1, 3)
							 from dy in Enumerable.Range(-1, 3)
							 where dx != 0 || dy != 0
							 where grid[i + dy][j + dx]
							 select true).Count();
			if (grid[i][j])
			{
				if (neighbors == 2 || neighbors == 3)
				{
					newgrid[i][j] = true;
				}
				else
				{
					newgrid[i][j] = false;
				}
			}
			else
			{
				if (neighbors == 3)
				{
					newgrid[i][j] = true;
				}
				else
				{
					newgrid[i][j] = false;
				}
			}
		}
	}
	grid = newgrid;
}

grid[1][1] = true;
grid[1][100] = true;
grid[100][1] = true;
grid[100][100] = true;

// 1005 is too low.
grid.Select(x => x.Where(x => x).Count()).Sum().Dump("part2");
<Query Kind="Statements" />

var lines = await AoC.GetLinesWeb();
//var lines = @".#.#...|#.
//.....#|##|
//.|..|...#.
//..|#.....#
//#.#|||#|#|
//...#.||...
//.|....|...
//||...#|.#|
//|.||||..|.
//...#.|..|.".Split('\n');

var gridlist = lines.Select(x => { var list = x.ToList(); list.Add(' '); list.Insert(0, ' '); return list.ToArray(); }).ToList();
gridlist.Add(new char[gridlist[0].Length]);
gridlist.Insert(0,new char[gridlist[0].Length]);

var grid = gridlist.ToArray();

char[][] newgrid()
{
	var ng = new char[grid.Length][];
	for (int i = 0; i < grid.Length; i++)
	{
		ng[i] = new char[grid[0].Length];
	}
	
	return ng;
}

var offsets = from x in Enumerable.Range(-1,3) from y in Enumerable.Range(-1,3) where !(x==0 && y==0) select (x,y);

int trees = 0, yards = 0;

for (int t = 0; t < 10; t++)
{
	var ng = newgrid();
	for (int i = 1; i < grid.Length - 1; i++)
	{
		for (int j = 1; j < grid[0].Length - 1; j++)
		{
			switch (grid[i][j])
			{
				case '.':
					if (offsets.Select((o) => grid[i+o.y][j+o.x]).Where(c => c == '|').Count() >= 3)
					{
						ng[i][j] = '|';
					}
					else
					{
						ng[i][j] = '.';
					}
				break;
				case '|':
					if (offsets.Select((o) => grid[i + o.y][j + o.x]).Where(c => c == '#').Count() >= 3)
					{
						ng[i][j] = '#';
					}
					else
					{
						ng[i][j] = '|';
					}
					break;
				case '#':
					if (offsets.Select((o) => grid[i + o.y][j + o.x]).Where(c => c == '#').Count() > 0 && offsets.Select((o) => grid[i + o.y][j + o.x]).Where(c => c == '|').Count() > 0)
					{
						ng[i][j] = '#';
					}
					else
					{
						ng[i][j] = '.';
					}
					break;
			}
		}
	}
	grid = ng;
	//string.Join("\n",ng.Select(line => string.Join("", line))).DumpFixed();
	trees = grid.Select(line => line.Count(ch => ch == '|')).Sum();
	yards = grid.Select(line => line.Count(ch => ch == '#')).Sum();

	//(trees * yards).Dump(t.ToString());
}

(trees*yards).Dump("Part 1");

//605 -> 633


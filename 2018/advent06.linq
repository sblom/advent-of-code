<Query Kind="Statements" />

var lines = await AoC.GetLinesWeb();

bool PART1 = true;
bool PART2 = true;

//lines = @"1, 1
//1, 6
//8, 3
//3, 4
//5, 5
//8, 9".Split('\n');

List<(int, int)> frontier = new List<(int,int)>();
List<(int,int)> newfrontier = new List<(int,int)>();
Dictionary<(int,int),char> decoder = new Dictionary<(int,int),char>();

((int,int),int)[,] grid = new ((int,int),int)[2000,2000];

(int,int)[] dirs = new (int,int)[]{ (-1,0), (1,0), (0,-1), (0,1) };

Dictionary<(int,int),int> areas = new Dictionary<(int, int), int>();

char ch = 'A';

void DumpGrid()
{
	var sb = new StringBuilder("<pre>");
	for (var i = 0; i < grid.GetLength(0); i++)
	{
		for (var j = 0; j < grid.GetLength(1); j++)
		{
			var ((x,y),d) = grid[i, j];
			
			if ((x,y) == (0,0))
			{
				sb.Append(" ");
			}
			else if ((x, y) == (i, j))
			{
				try
				{
					sb.Append(decoder[(x,y)]);
				}
				catch (Exception ex)
				{
					sb.Append(".");
				}
			}
			else
			{
				sb.Append(char.ToLower(decoder[(x,y)]));
			}
		}
		sb.AppendLine();
	}
	sb.AppendLine("</pre>");
	Util.RawHtml(sb.ToString());
}

foreach (var line in lines)
{
	var coords = line.Split(',');
	var (x, y) = (int.Parse(coords[0])+700, int.Parse(coords[1])+700);
	decoder[(x,y)] = ch++;
	
	frontier.Add((x,y));
	areas[(x,y)] = 1;
	grid[x,y] = ((x,y),0);
}

//DumpGrid();

if (PART1)
{
	for (int i = 0; i < 500; i++)
	{
		while (frontier.Count > 0)
		{
			var (x, y) = frontier[0]; frontier.RemoveAt(0);

			var ((x0, y0), d) = grid[x, y];

			foreach (var (dx, dy) in dirs)
			{
				var (x1, y1) = (x + dx, y + dy);

				if (grid[x1, y1].Item1 == (0, 0))
				{
					areas[(x0, y0)]++;
					grid[x1, y1] = ((x0, y0), d + 1);
					newfrontier.Add((x1, y1));
				}
				else if (grid[x1, y1].Item2 == d + 1 && grid[x1, y1].Item1 != (x0, y0))
				{
					areas[grid[x1, y1].Item1]--;
					newfrontier.Remove((x1, y1));
					grid[x1, y1] = ((x1, y1), 0);
				}
			}
		}

		frontier = newfrontier;
		newfrontier = new List<(int, int)>();
		//DumpGrid();
	}

	foreach (var (x, y) in frontier)
	{
		areas.Remove(grid[x, y].Item1);
	}
	areas.OrderBy(item => item.Value).Last().Value.Dump("Part 1");
}

if (PART2)
{
	var count = 0;
	for (int i = -1000; i < 2000; i++)
	{
		for (int j = -1000; j < 2000; j++)
		{
			var dist = 0;
			foreach (var (x,y) in decoder.Keys)
			{
				dist += Math.Abs(x-i) + Math.Abs(y-j);
			}
			if (dist <= 10000)
			{
				count++;
			}
		}
	}
	
	count.Dump("Part 2");
}
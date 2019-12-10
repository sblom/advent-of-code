<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

char[,] grid;
int xmin, xmax, ymin, ymax;
List<(int x, int y)> frontier;

async Task Main()
{
	int sample = 0;
	
	var lines = await AoC.GetLinesWeb();

//	lines = @"x=495, y=2..7
//y=7, x=495..501
//x=501, y=3..7
//x=498, y=2..4
//x=506, y=1..2
//x=498, y=10..13
//x=504, y=10..13
//y=13, x=498..504".Split('\n');
	
	var rx = new Regex(@"(\w)=(\d+), (\w)=(\d+)\.\.(\d+)");	
	
	(xmin, xmax, ymin, ymax) = (int.MaxValue, int.MinValue, int.MaxValue, int.MinValue);
	frontier = new List<(int,int)>();
	
	foreach (var line in lines)
	{
		var match = rx.Match(line);
		
		var dim = match.Groups[1].Value;
		var s0 = int.Parse(match.Groups[2].Value);
		var r0 = int.Parse(match.Groups[4].Value);
		var r1 = int.Parse(match.Groups[5].Value);
		
		if (dim == "x")
		{
			xmin = Math.Min(xmin, s0);
			xmax = Math.Max(xmax, s0);
			ymin = Math.Min(ymin, r0);
			ymax = Math.Max(ymax, r1);
		}
		else
		{
			ymin = Math.Min(ymin, s0);
			ymax = Math.Max(ymax, s0);
			xmin = Math.Min(xmin, r0);
			xmax = Math.Max(xmax, r1);
		}
	}
	
	grid = new char[ymax+1,xmax+11];
	
	for (int i = 0; i <= ymax; i++)
	{
		for (int j = 0; j <= xmax; j++)
		{
			grid[i,j] = '.';
		}
	}
	
	(xmin, xmax, ymin, ymax).ToString().Dump();
	
	foreach (var line in lines)
	{
		var match = rx.Match(line);
	
		var dim = match.Groups[1].Value;
		var s0 = int.Parse(match.Groups[2].Value);
		var r0 = int.Parse(match.Groups[4].Value);
		var r1 = int.Parse(match.Groups[5].Value);
	
		if (dim == "x")
		{
			for (int i = r0; i <= r1; i++)
			{
				grid[i,s0] = '#';
			}
		}
		else
		{
			for (int i = r0; i <= r1; i++)
			{
				grid[s0, i] = '#';
			}
		}
	}
	
	frontier.Add((500, ymin));
	
	grid[0,500] = '+';
	grid[ymin, 500] = '|';
	
	while (frontier.Count > 0)
	{
		sample++;
		var (x,y) = frontier[0];
		frontier.RemoveAt(0);
		if (y >= ymax) continue;

		var below = grid[y+1,x];

		if (below == '.')
		{
			grid[y+1,x] = '|';
			frontier.Add((x,y+1));
		}
		else if (below == '#' || below == '~')
		{
			int left = -1, right = -1;
			
			for (int li = x; /*li >= xmin*/; li--)
			{
				if (grid[y+1,li] == '#' || grid[y+1,li] == '~')
				{
					grid[y, li] = '|';
					if (/* li != xmin && */ grid[y, li - 1] == '#')
					{
						if (li < xmin) xmin = li;
						left = li;
						break;
					}
				}
				else
				{
					if (li < xmin) xmin = li;					
					grid[y,li] = '|';
					frontier.Add((li,y));
					break;
				}
			}
			for (int i = x; /*i <= xmax*/; i++)
			{
				if (grid[y + 1, i] == '#' || grid[y + 1, i] == '~')
				{
					grid[y, i] = '|';
					if (/* i != xmin &&*/ grid[y, i + 1] == '#')
					{
						if (i > xmax) xmax = i;
						right = i;
						break;
					}
				}
				else
				{
					if (i > xmax) xmax = i;
					grid[y, i] = '|';
					frontier.Add((i, y));
					break;
				}
			}
			if (left != -1 && right != -1)
			{
				for (int i = left; i <= right; i++)
				{
					grid[y, i] = '~';
				}
				frontier.Add((x,y-1));
			}
		}
//		if (sample > 8000)
//		{
//			try
//			{
//				DumpGrid();
//				sample.DumpFixed();
//			}
//			catch {}
//		}
	}
	DumpGrid(full: true);
	int tot = 0;
	for (int i = ymin; i <= ymax; i++)
	{
		for (int j = xmin; j <= xmax; j++)
		{
			if (grid[i,j] == '~' /* || grid[i,j] == '|'*/) tot++;
		}
	}
	tot.Dump("Part 1");
}

// Define other methods and classes here
void DumpGrid(bool full = false)
{
	int yn, yx;
	if (!full)
	{
		yn = frontier.Select(i => i.y).Min();
		yx = frontier.Select(i => i.y).Max();
	}
	else
	{
		yn = ymin;
		yx = ymax;
	}
	
	for (int j = Math.Max(0,yn - 5); j <= Math.Min(ymax,yx + 5); j++)
	{
		var sb = new StringBuilder();
		for (int i = xmin; i <= xmax; i++)
		{
			sb.Append(grid[j,i]);
		}
		sb.ToString().DumpFixed();
	}
}
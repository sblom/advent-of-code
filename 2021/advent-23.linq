<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference  Prerelease="true">RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>static UserQuery</Namespace>
</Query>

#define TEST

#region preamble
#load "..\Lib\Utils"
#load "..\Lib\BFS"
#endregion

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"#############
^#...........#
^###A#C#B#C###
^..#D#C#B#A#
^..#D#B#A#C#
^..#D#A#D#B#
^..#########".Replace("^", "").GetLines();
#endif

var height = lines.Count();

char[,] grid = new char[height,13];

foreach (var (line, i) in lines.Select((line,i) => (line,i)))
{
	foreach (var (ch, j) in line.Select((ch,j) => (ch,j)))
	{
		grid[i,j] = ch;
	}
}

grid.Dump();

int HomeCol(char ch)
{
	return ch switch {
		'A' => 3,
		'B' => 5,
		'C' => 7,
		'D' => 9
	};
}

char HomeChar(int col)
{
	return col switch {
		3 => 'A',
		5 => 'B',
		7 => 'C',
		9 => 'D'
	};
}

int Cost(char ch) {
	return ch switch {
		'A' => 1,
		'B' => 10,
		'C' => 100,
		'D' => 1000
	};
}

//GetMoves(GetMoves(grid,0).Take(10..11).Single().grid,0).Dump();
//var bfs = new BFS<(char[,] grid, int cost)>((grid,0), x => GetMoves(x.grid,x.cost), x => false, x => false, x => FlatString(x.grid), x => x.cost);

PriorityQueue<(char[,] grid, int cost), int> pq = new();

pq.Enqueue((grid, 0), 0);

int c = 0;

int min = 999999999;

while (pq.Count > 0)
{
	var current = pq.Dequeue();

	//if (c++ % 10000 == 0) pq.Count.Dump();

	if (IsSolved(current.grid))
	{
		if (current.cost < min)
		{
			min = current.cost;
			min.Dump("Part 1");
		}
	}

	var next = GetMoves(current.grid,current.cost);

	foreach (var nx in next)
	{
		pq.Enqueue(nx, nx.cost + DistanceFromHome(nx.grid));
	}
}

int DistanceFromHome(char[,] grid)
{
	int tot = 0;
	for (int i = 3; i <= 9; i += 2)
	{
		for (int j = 2; j < height - 1; j++)
		{
			char ch = grid[j, i];
			if (ch != '.') tot += i != HomeCol(ch) ? (j - 1 + 2 + Math.Abs(i - HomeCol(ch))) * Cost(ch) : 0;
		}
	}
	for (int i = 1; i <= 11; i++)
	{
		char ch = grid[1, i];
		if (ch != '.') tot += (2 + Math.Abs(i - HomeCol(ch))) * Cost(ch);
	}

	return tot;
}

bool IsSolved(char[,] grid)
{
	for (int i = 2; i < height - 1; i++)
	{
		for (int j = 3; j <= 9; j += 2)
		{
			if (grid[i,j] != HomeChar(j)) return false;
		}
	}
	
	return true;
}

IEnumerable<(char[,] grid, int cost)> GetMoves(char[,] grid, int cost)
{
	// Can anyone move home?
	for (int i = 1; i <= 11; i++)
	{
		char ch = grid[1,i];
		if (ch is >= 'A' and <= 'D')
		{
			var col = HomeCol(ch);

			int top = 0;
			
			for (int j = height - 2; j >= 2; j--)
			{
				top = j;
				if (grid[j,col] == '.') break;
				if (grid[j,col] != ch) goto blocked;
			}
			
			for (int ii = Math.Min(i + 1,col); ii <= Math.Max(i - 1,col); ii++)
			{
				if (grid[1,ii] != '.') goto blocked;
			}
			
			char[,] newgrid = (char[,])grid.Clone();
			newgrid[1,i] = '.';
			newgrid[top,col] = ch;
			yield return (newgrid, cost + Cost(ch) * (top - 1 + Math.Abs(i - col)));
		}
		blocked:;
	}
	
	// Start new characters into hall
	for (int i = 3; i <= 9; i += 2)
	{
		int top;
		for (top = 2; top < height - 1; top++)
		{
			if (grid[top,i] != '.') break;
		}
		if (top == height - 1) continue;
		
		int j;
		for (j = top; j < height - 1; j++)
		{
			if (grid[j,i] != HomeChar(i)) break;
		}
		if (j == height - 1) continue;

		char ch = grid[top,i];
		for (int ii = i; ii <= 11; ii++)
		{
			if (ii is 3 or 5 or 7 or 9) continue;
			if (grid[1, ii] != '.') break;

			char[,] newgrid = (char[,])grid.Clone();
			newgrid[top, i] = '.';
			newgrid[1, ii] = ch;
			yield return (newgrid, cost + Cost(ch) * (top - 1 + Math.Abs(i - ii)));
		}
		for (int ii = i; ii >= 1; ii--)
		{
			if (ii is 3 or 5 or 7 or 9) continue;
			if (grid[1, ii] != '.') break;

			char[,] newgrid = (char[,])grid.Clone();
			newgrid[top, i] = '.';
			newgrid[1, ii] = ch;
			yield return (newgrid, cost + Cost(ch) * (top - 1 + Math.Abs(i - ii)));
		}
	}
}

// 22116 too high
// 17272 too high
// 16330 too high

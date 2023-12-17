<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference  Prerelease="true">RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

//#define TEST

#region preamble
#load "..\Lib\Utils"
#load "..\Lib\BFS"
#endregion

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"00100
11110
10110
10111
10101
01111
00111
11100
10000
11001
00010
01010".GetLines();
#endif

var groups = lines.GroupLines();

var calls = groups.First().First().Split(",").Select(int.Parse);

var boards = groups.Skip(1).Select(board => board.Select(row => row.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray()).ToArray()).ToList();

bool[] isDone = new bool[boards.Count];

foreach (var call in calls)
{
	foreach (var (board, idx) in boards.Select((x,i) => (x,i)))
	{
		for (int x = 0; x < 5; x++)
		{
			for (int y = 0; y < 5; y++)
			{
				if (board[x][y] == call)
				{
					board[x][y] = -1;
					goto check_card;
				}
			}
		}
	check_card:
		if (!isDone[idx])
		{
			for (int x = 0; x < 5; x++)
			{
				if (board[x].All(n => n == -1))
				{
					isDone[idx] = true;
					"Bingo".Dump();
					(call * board.Sum(row => row.Where(n => n != -1).Sum())).Dump1();
					(call * board.Sum(row => row.Where(n => n != -1).Sum())).Dump2(true);
				}
			}

			for (int y = 0; y < 5; y++)
			{
				if (Enumerable.Range(0, 5).All(x => board[x][y] == -1))
				{
					isDone[idx] = true;					
					"Bingo".Dump();
					(call * board.Sum(row => row.Where(n => n != -1).Sum())).Dump1();
					(call * board.Sum(row => row.Where(n => n != -1).Sum())).Dump2(true);
				}
			}
		}
	}
}
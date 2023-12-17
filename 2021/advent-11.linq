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
var lines = @"5483143223
2745854711
5264556173
6141336146
6357385478
4167524645
2176841721
6882881134
4846848554
5283751526".GetLines();
#endif

var octopod = lines.Select(line => line.Select(ch => (int?)(ch - '0')).ToArray()).ToArray();

var dirs = from dx in new[] { -1, 0, 1 } from dy in new[] { -1, 0, 1 } where dx != 0 || dy != 0 select (dx, dy);

int flashes = 0;

for (int c = 1; ; c++)
{
	for (int y = 0; y < octopod.Length; y++)
	{
		for (int x = 0; x < octopod[0].Length; x++)
		{
			octopod[y][x]++;
		}	
	}

	bool flash = false;

	do
	{
		flash = false;
		for (int y = 0; y < octopod.Length; y++)
		{
			for (int x = 0; x < octopod[0].Length; x++)
			{
				if (octopod[y][x] > 9)
				{
					flashes++;
					DoFlash(x,y);
					flash = true;
				}
			}
		}
	} while (flash);


	int flashedNow = 0;
	for (int y = 0; y < octopod.Length; y++)
	{
		for (int x = 0; x < octopod[0].Length; x++)
		{
			if (octopod[y][x] == null)
			{
				octopod[y][x] = 0;
				flashedNow++;
			}
		}
	}
	if (flashedNow == octopod.Length * octopod[0].Length)
	{
		c.Dump("Part 2");
		break;
	}
	
	if (c == 100) flashes.Dump("Part 1");
}

//flashes.Dump();

void DoFlash(int x, int y)
{
	octopod[y][x] = null;
	foreach (var dir in dirs)
	{
		try {
			octopod[y + dir.dy][x + dir.dx]++;
		}
		catch { continue; }
	}
}

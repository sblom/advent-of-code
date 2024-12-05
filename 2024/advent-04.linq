<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

#load "..\Lib\Utils"
#load "..\Lib\BFS"

//#define TEST

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"MMMSXXMASM
MSAMXMSMSA
AMXSXMAAMM
MSAMASMSMX
XMASAMXAMM
XXAMMXXAMA
SMSMSASXSS
SAXAMASAAA
MAMMMXMMMM
MXMXAXMASX".GetLines();
#endif

int c = 0;

var word = "XMAS";

var grid = lines.Select(x => x.ToArray()).ToArray();

var dirs = from dx in new[]{-1,0,1} from dy in new[]{-1,0,1} where dx != 0 || dy != 0 select (dx,dy);

for (int y = 0; y < grid.Length; y++)
{
    for (int x = 0; x < grid[0].Length; x++)
    {
        foreach (var (dx, dy) in dirs)
        {
            for (int n = 0; n < 4; n++)
            {
                try
                {
                    if (grid[y + n * dy][x + n * dx] == word[n])
                        continue;
                    else goto nope;
                }
                catch
                {
                    goto nope;
                }
            }
            c++;
            nope:;
        }
    }
}

c.Dump();

var dirs2 = new[]{((1,0),(0,1)),((0,1),(1,0)),((-1,0),(0,1)),((0,-1),(1,0))};
c = 0;

for (int y = 1; y < grid.Length - 1; y++)
{
    for (int x = 1; x < grid[0].Length - 1; x++)
    {
        foreach (var ((mx, my),(sx,sy)) in dirs2)
        {            
            if (grid[y][x] == 'A' &&
                grid[y + my + sy][x + mx + sx] == 'M' &&
                grid[y + my - sy][x + mx - sx] == 'M' &&
                grid[y - my + sy][x - mx + sx] == 'S' &&
                grid[y - my - sy][x - mx - sx] == 'S')
                goto yep;
        }
        goto nope;
    yep:;
        c++;
    nope:;
    }
}

c.Dump();
<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference  Prerelease="true">RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

#region Preamble

#load "..\Lib\Utils"
#load "..\Lib\BFS"

//#define TEST
//#define CHECKED

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = """
//#.##..##.
//..#.##.#.
//##......#
//##......#
//..#.##.#.
//..##..##.
//#.#.##.#.
//
//#...##..#
//#....#..#
//..##..###
//#####.##.
//#####.##.
//..##..###
//#....#..#
""".GetLines();
#endif

AoC._outputs = new DumpContainer[] { new(), new() };
Util.HorizontalRun("Part 1,Part 2", AoC._outputs).Dump();

#if CHECKED
checked{
#endif

#endregion

var grids = string.Join("\n", lines).Split("\n\n").Select(x => x.Split("\n"));

int total = 0;
int total2 = 0;

foreach (var grid in grids)
{
    for (int i = 0; i < grid[0].Length - 1; i++)
    {
        int smudge = 0;
        int m = Math.Min(grid[0].Length - i - 1, i + 1);
        for (int k = 0; k < m; k++)
        {
            for (int j = 0; j < grid.Length; j++)
            {
                if (grid[j][i - k] != grid[j][i + k + 1])
                {
                    smudge++;
                    if (smudge > 1) goto no_mirror;
                }
            }
        }
        
        if (smudge == 0) total += i + 1;
        if (smudge == 1) total2 += i + 1;
    no_mirror:;
    }

    for (int i = 0; i < grid.Length - 1; i++)
    {
        int smudge = 0;
        int m = Math.Min(grid.Length - i - 1, i + 1);
        for (int k = 0; k < m; k++)
        {
            for (int j = 0; j < grid[0].Length; j++)
            {
                if (grid[i - k][j] != grid[i + k + 1][j])
                {
                    smudge++;
                    if (smudge > 1) goto no_mirror;
                }
            }
        }

        if (smudge == 0) total += 100 * (i + 1);
        if (smudge == 1) total2 += 100 * (i + 1);
    no_mirror:;
    }
    next_pattern:;
}

total.Dump1();
total2.Dump2();

#if CHECKED
}
#endif

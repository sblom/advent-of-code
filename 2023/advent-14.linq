<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>RegExtract</NuGetReference>
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
var lines = @"O....#....
O.OO#....#
.....##...
OO.#O....O
.O.....O#.
O.#..O.#.#
..O..#O..O
.......O..
\#....###..
\#OO..#....".GetLines();
#endif

#if CHECKED
checked{
#endif

AoC._outputs = new DumpContainer[] {new(), new()};
Util.HorizontalRun("Part 1,Part 2",AoC._outputs).Dump();

#endregion

var grid1 = lines.Select(x => x.ToCharArray()).ToArray();
var grid2 = lines.Select(x => x.ToCharArray()).ToArray();

bool moved = false;

do
{
    moved = false;
    for (int x = 0; x < grid1[0].Length; x++)
    {
        for (int y = 1; y < grid1.Length; y++)
        {
            if (grid1[y][x] == 'O' && grid1[y - 1][x] == '.')
            {
                grid1[y - 1][x] = 'O';
                grid1[y][x] = '.';
                moved = true;
            }
        }
    }
} while (moved);

int load = 0;

for (int x = 0; x < grid1[0].Length; x++)
{
    for (int y = 0; y < grid1.Length; y++)
    {
        if (grid1[y][x] == 'O')
        {
            load += (grid1.Length - y);
        }
    }
}

load.Dump1();

List<int> loads = new(250);

for (int i = 0; i < 1_000; i++)
{
// North
do
{
    moved = false;
    for (int x = 0; x < grid2[0].Length; x++)
    {
        for (int y = 1; y < grid2.Length; y++)
        {
            if (grid2[y][x] == 'O' && grid2[y - 1][x] == '.')
            {
                grid2[y - 1][x] = 'O';
                grid2[y][x] = '.';
                moved = true;
            }
        }
    }
} while (moved);

// West
do
{
    moved = false;
    for (int x = 1; x < grid2[0].Length; x++)
    {
        for (int y = 0; y < grid2.Length; y++)
        {
            if (grid2[y][x] == 'O' && grid2[y][x - 1] == '.')
            {
                grid2[y][x - 1] = 'O';
                grid2[y][x] = '.';
                moved = true;
            }
        }
    }
} while (moved);

// South
do
{
    moved = false;
    for (int x = 0; x < grid2[0].Length; x++)
    {
        for (int y = grid2.Length - 2; y >= 0; y--)
        {
            if (grid2[y][x] == 'O' && grid2[y + 1][x] == '.')
            {
                grid2[y + 1][x] = 'O';
                grid2[y][x] = '.';
                moved = true;
            }
        }
    }
} while (moved);

// East
do
{
    moved = false;
    for (int x = grid2[0].Length - 2; x >= 0; x--)
    {
        for (int y = 0; y < grid2.Length; y++)
        {
            if (grid2[y][x] == 'O' && grid2[y][x + 1] == '.')
            {
                grid2[y][x + 1] = 'O';
                grid2[y][x] = '.';
                moved = true;
            }
        }
    }
} while (moved);

    //for (int y = 0; y < grid2.Length; y++)
    //{
    //    for (int x = 0; x < grid2[0].Length; x++)
    //    {
    //        Console.Write(grid2[y][x]);
    //    }
    //    Console.WriteLine();
    //}
    //Console.WriteLine();
load = 0;
    for (int x = 0; x < grid2[0].Length; x++)
    {
        for (int y = 0; y < grid2.Length; y++)
        {
            if (grid2[y][x] == 'O')
            {
                load += (grid2.Length - y);
            }
        }
    }
    
    var prev = loads.LastIndexOf(load);
    loads.Add(load);

    if (prev != -1)
    {
        var cycle = loads.Count - prev - 1;
        
        if (cycle < 3) goto next_cycle;
        
        for (int ii = loads.Count - 1; ii > prev; ii--)
        {
            if (ii - cycle < 0 || loads[ii] != loads[ii - cycle]) goto next_cycle;
        }
        
        loads[((1_000_000_000 - loads.Count - 1) % cycle) + loads.Count - cycle].Dump2();
        return;
    }
    next_cycle:;
}

// 124 first occurrence
// 150 first RE-occurrence
// Cycle == 26
// (1_000_000_000 - 124) % 26 == 18


// 3 first occurrence
// 10 first RE-occurrence
// Cycle = 7
// (1_000_000_000 - 3) % 7

#if CHECKED
}
#endif

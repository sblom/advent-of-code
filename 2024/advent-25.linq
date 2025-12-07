<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

#LINQPad checked+

#load "..\Lib\Utils"
#load "..\Lib\BFS"

//#define TEST

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = $@"#####
.####
.####
.####
.#.#.
.#...
.....

\#####
\##.##
.#.##
...##
...#.
...#.
.....

.....
\#....
\#....
\#...#
\#.#.#
\#.###
\#####

.....
.....
\#.#..
\###..
\###.#
\###.#
\#####

.....
.....
.....
\#....
\#.#..
\#.#.#
\#####".Replace("\\", "").GetLines();
    //   2,4,1,4,7,5,4,1,1,4,5,5,0,3,3,0

#endif

var graphs = lines.GroupLines().Select(g => g.Select(l => l.ToCharArray()).ToArray()).ToArray();

int pair = 0;

foreach (var g1 in graphs)
{
    foreach (var g2 in graphs)
    {
        for (int i = 0; i < g1.Length; i++)
        {
            for (int j = 0; j < g1[0].Length; j++)
            {
                if (g1[i][j] == '#' && g2[i][j] == '#') goto nope;
            }
        }
        pair++;
        nope:;
    }
}

(pair/2).Dump();

// After solving, I played with AI to solve it other ways:
// parse each graph in graphs into a set numbers indicating how far down the # bars go from the top (locks) or how far up the # bars go from the bottom (keys)
// for example,
//.####
//.####
//.####
//.#.#.
//.#...
//.....
// is a lock with heights 0,5,3,4,3
var locks = graphs.Select(g => g[0].Select((_, i) => g.TakeWhile(row => row[i] == '#').Count()).ToArray()).ToArray();
// now do keys
var keys = graphs.Select(g => g[0].Select((_, i) => g.Reverse().TakeWhile(row => row[i] == '#').Count()).ToArray()).ToArray();

// count the number of pairs of char grids in graphs that don't both have # symbols at the same location
var nonOverlappingPairs = graphs.SelectMany((g1, i) => graphs.Skip(i + 1), (g1, g2) => (g1, g2))
    .Count(pair => !pair.g1.Zip(pair.g2, (row1, row2) => row1.Zip(row2, (c1, c2) => c1 == '#' && c2 == '#').Any(bothHash => bothHash)).Any(bothHash => bothHash)).Dump();
<Query Kind="Statements">
  <NuGetReference>RegExtract</NuGetReference>
  <Namespace>RegExtract</Namespace>
</Query>

#load "..\Lib\Utils"

var lines = AoC.GetLines().ToArray();

//var lines = @"..##.......
//#...#...#..
//.#....#..#.
//..#.#...#.#
//.#...##..#.
//..#.##.....
//.#.#.#....#
//.#........#
//#.##...#...
//#...##....#
//.#..#...#.#".GetLines().ToArray();

var width = lines[0].Length;

var (dx, dy) = (3, 1);

long CountTrees(int dx, int dy)
{
	var (y, x) = (0, 0);

	long trees = 0;

	do
	{
		if (lines[y][x % width] == '#') trees++;
		(y, x) = (y + dy, x + dx);
	} while (y < lines.Length);

	return trees;
}

CountTrees(3,1).Dump("Part 1");

long acc = 1;

foreach (var (edx, edy) in new[] {(1,1), (3,1), (5,1), (7,1), (1,2)})
{
	acc *= CountTrees(edx, edy);
}

acc.Dump("Part 2");
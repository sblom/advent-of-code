<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference  Prerelease="true">RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

#load "..\Lib\Utils"
#load "..\Lib\BFS"

//#define TEST

#if !TEST
var lines = (await AoC.GetLinesWeb()).ToArray();
#else
var lines = @"addx 15
addx -11
addx 6
addx -3
addx 5
addx -1
addx -8
addx 13
addx 4
noop
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx -35
addx 1
addx 24
addx -19
addx 1
addx 16
addx -11
noop
noop
addx 21
addx -15
noop
noop
addx -3
addx 9
addx 1
addx -3
addx 8
addx 1
addx 5
noop
noop
noop
noop
noop
addx -36
noop
addx 1
addx 7
noop
noop
noop
addx 2
addx 6
noop
noop
noop
noop
noop
addx 1
noop
noop
addx 7
addx 1
noop
addx -13
addx 13
addx 7
noop
addx 1
addx -33
noop
noop
noop
addx 2
noop
noop
noop
addx 8
noop
addx -1
addx 2
addx 1
noop
addx 17
addx -9
addx 1
addx 1
addx -3
addx 11
noop
noop
addx 1
noop
addx 1
noop
noop
addx -13
addx -19
addx 1
addx 3
addx 26
addx -30
addx 12
addx -1
addx 3
addx 1
noop
noop
noop
addx -9
addx 18
addx 1
addx 2
noop
noop
addx 9
noop
noop
noop
addx -1
addx 2
addx -37
addx 1
addx 3
noop
addx 15
addx -21
addx 22
addx -6
addx 1
noop
addx 2
addx 1
noop
addx -10
noop
noop
addx 20
addx 1
addx 2
addx 2
addx -6
addx -11
noop
noop
noop".GetLines().ToArray();
#endif

int cycle = 1;

int X = 1;
int tot = 0;

int loc = 0;

for (int c = 1; c <= lines.Length; c++)
{
    //lines[c - 1].Dump();
    var inst = lines[c - 1].Split(" ");

    if (inst[0] == "noop")
    {
        cycle++;
        Console.Write(loc >= X - 1 && loc <= X + 1 ? "#" : ".");
        loc++;
        if (cycle % 40 == 0) { Console.WriteLine(); loc = 0; }

        if ((cycle - 20) % 40 == 0)
        {
            //(X, cycle).ToString().Dump();
            tot += X * cycle;
        }
    }
    else
    {
        cycle++;
        Console.Write(loc >= X - 1 && loc <= X + 1 ? "#" : ".");
        loc++;
        if (cycle % 40 == 0) { Console.WriteLine(); loc = 0; }

        if ((cycle - 20) % 40 == 0)
        {
            //(X, cycle).ToString().Dump();
            tot += X * cycle;
        }
        cycle++;

        X += int.Parse(inst[1]);

        Console.Write(loc >= X - 1 && loc <= X + 1 ? "#" : ".");
        loc++;
        if (cycle % 40 == 0) { Console.WriteLine(); loc = 0; }

        if ((cycle - 20) % 40 == 0)
        {
            //(X, cycle).ToString().Dump();
            tot += X * cycle;
        }
    }
}

tot.Dump();

//##...#.#..##....##....#...#####......##
//#..#....#.#..#.#..#....#.#....#....#.#..
//###....#..#..#.#..#....#.###..#....##...
//#..#..#...###..####....#.#....#....#.#..
//#..#.#....#....#..#.#..#.#....#....#.#..
//###..####.#....#..#..##..####.####.#..#.
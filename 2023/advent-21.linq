<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>RawScape.Wintellect.PowerCollections</NuGetReference>
  <NuGetReference Prerelease="true">RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>Wintellect.PowerCollections</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Collections.ObjectModel</Namespace>
</Query>

#region Preamble

#load "..\Lib\Utils"
//#load "..\Lib\BFS"

//#define TEST
//#define CHECKED
//#define TRACE

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"...........
.....###.#.
.###.##..#.
..#.#...#..
....#.#....
.##..S####.
.##..#...#.
.......##..
.##.#.####.
.##..##.##.
...........".GetLines();
#endif

#if CHECKED
checked{
#endif

AoC._outputs = new DumpContainer[] {new(), new()};
Util.HorizontalRun("Part 1,Part 2",AoC._outputs).Dump();

#endregion

var grid = lines.Select(x => x.ToCharArray()).ToArray();

grid.Sum(row => row.Count(ch => ch == '#')).Dump();

var Y = grid.Length.Dump();
var X = grid[0].Length;

var y0 = grid.Select((row,i) => (row, i)).Where(x => x.row.Contains('S')).Single().i;
var x0 = grid[y0].Select((ch,i) => (ch, i)).Where(ch => ch.ch == 'S').Single().i;

var tiles = new HashSet<(int x, int y)> { (x0, y0) };

List<(int dx, int dy)> dirs = [(0,1),(0,-1),(1,0),(-1,0)];

var steps = 26501365;

long R = steps / X;
long Rm = (2 + R % 2);

(R,Rm).Dump();

for (int r = 1; r <= Rm * X + steps % X; r++)
{
    tiles = tiles.SelectMany(loc => Next(loc.x, loc.y)).ToHashSet();
}

long origin = tiles.Where(t => t.x >= 0 && t.x < X && t.y >= 0 && t.y < Y).Count();
long neighbor = tiles.Where(t => t.x >= X && t.x < 2 * X && t.y >= 0 && t.y < Y).Count();

long W = tiles.Where(t => t.x >= -Rm * X && t.x < (-Rm + 1) * X && t.y >= 0 && t.y < Y).Count();
long N = tiles.Where(t => t.y >= -Rm * X && t.y < (-Rm + 1) * X && t.x >= 0 && t.x < Y).Count();
long E = tiles.Where(t => t.x >= Rm * X && t.x < (Rm + 1) * X && t.y >= 0 && t.y < Y).Count();
long S = tiles.Where(t => t.y >= Rm * X && t.y < (Rm + 1) * X && t.x >= 0 && t.x < Y).Count();

long NE1 = tiles.Where(t => t.x >= (Rm - 1) * X && t.x < Rm * X && t.y >= Y && t.y < 2 * Y).Count();
long NE2 = tiles.Where(t => t.x >= (Rm - 1) * X && t.x < Rm * X && t.y >= 2 * Y && t.y < 3 * Y).Count();

long SE1 = tiles.Where(t => t.x >= (Rm - 1) * X && t.x < Rm * X && t.y >= -Y && t.y < 0).Count();
long SE2 = tiles.Where(t => t.x >= (Rm - 1) * X && t.x < Rm * X && t.y >= -2 * Y && t.y < -Y).Count();

long NW1 = tiles.Where(t => t.x >= (-Rm + 1) * X && t.x < (-Rm + 2) * X && t.y >= Y && t.y < 2 * Y).Count();
long NW2 = tiles.Where(t => t.x >= (-Rm + 1) * X && t.x < (-Rm + 2) * X && t.y >= 2 * Y && t.y < 3 * Y).Count();

long SW1 = tiles.Where(t => t.x >= (-Rm + 1) * X && t.x < (-Rm + 2) * X && t.y >= -Y && t.y < 0).Count();
long SW2 = tiles.Where(t => t.x >= (-Rm + 1) * X && t.x < (-Rm + 2) * X && t.y >= -2 * Y && t.y < -Y).Count();

(origin, neighbor, N, E, S, W).Dump();
(NE1, NE2, SE1, SE2, NW1, NW2, SW1, SW2).Dump();

((R - 1) * (R - 1) * origin + R * R * neighbor + N + E + S + W + R * (NE2 + NW2 + SE2 + SW2) + (R - 1) * (NE1 + NW1 + SW1 + SE1)).Dump2();

//DrawGrid(tiles);

void DrawGrid(HashSet<(int x, int y)> tiles)
{
    StringBuilder sb = new();

    int xl = tiles.Min(x => x.x);
    int xh = tiles.Max(x => x.x);
    int yl = tiles.Min(x => x.y);
    int yh = tiles.Max(x => x.y);
    
    for (int y = yl; y <= yh; y++)
    {
        for (int x = xl; x <= xh; x++)
        {
            if (tiles.Contains((x,y)))
            {
                sb.Append("O");
            }
            else
            {
                sb.Append(grid[((y % Y + Y) % Y)][((x % X + X) % X)]);
            }
            if (((x + 1) % X + X) % X == 0)
            {
                sb.Append("|");
            }
        }
        sb.AppendLine();

        if (((y + 1) % Y + Y) % Y == 0)
        {
            for (int x = xl; x <= xh; x++)
            {
                if (((x) % X + X) % X == 0)
                {
                    sb.Append("+");
                }
                sb.Append("-");
            }
            sb.AppendLine();
        }
    }

    sb.ToString().DumpFixed();
}

IEnumerable<(int x,int y)> Next(int x, int y)
{
    foreach (var (dx, dy) in dirs)
    {
        var (xn, yn) = (x + dx, y + dy);
        
        if (
#if PART1
        xn >= 0 && xn < X && yn >= 0 && yn < Y &&
#endif
        grid[((yn % Y + Y) % Y)][((xn % X + X) % X)] is '.' or 'S')
            yield return (xn,yn);
    }
}

tiles.Count().Dump1();

#if CHECKED
}
#endif

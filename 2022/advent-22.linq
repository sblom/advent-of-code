<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <NuGetReference  Prerelease="true">RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>System.Text.Json.Nodes</Namespace>
  <Namespace>System.Text.Json</Namespace>
</Query>

#load "..\Lib\Utils"
#load "..\Lib\BFS"

//#define TEST
//#define PART2

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"        ...#
        .#..
        \#...
        ....
...#.......#
........#...
..#....#....
..........#.
        ...#....
        .....#..
        .#......
        ......#.

10R5L5R10L4R5L5".Replace("\t","    ").Replace("\r","").Split("\n");
#endif

const int RIGHT = 0;
const int DOWN = 1;
const int LEFT = 2;
const int UP = 3;

var width = lines.ToArray()[0..^2].Max(x => x.Length);
var grid = lines.ToArray()[0..^2].Select(x => x.Concat(Enumerable.Range(0,width - x.Count()).Select(x => ' ')).ToArray()).ToArray();
var tilegrid = Enumerable.Range(0,grid.Length).Select(x => new tile[grid[0].Length]).ToArray();
tile start = default;

for (int i = 0; i < grid.Length; i++)
{
    for (int j = 0; j < grid[0].Length; j++)
    {
        tilegrid[i][j] = new tile(grid[i][j], (i,j), new (tile, int)[4]);
    }
}

for (int j = 0; j < grid[0].Length; j++)
{
    if (tilegrid[0][j].type != ' ')
    {
        start = tilegrid[0][j];
        break;
    }
}

for (int i = 0; i < grid.Length; i++)
{
    for (int j = 0; j < grid[0].Length; j++)
    {
        if (i > 0 && tilegrid[i][j].type != ' ' && tilegrid[i - 1][j].type != ' ') tilegrid[i][j].next[3] = (tilegrid[i - 1][j], 0);
        if (i < grid.Length - 1 && tilegrid[i][j].type != ' ' && tilegrid[i + 1][j].type != ' ') tilegrid[i][j].next[1] = (tilegrid[i + 1][j], 0);
        if (j > 0 && tilegrid[i][j].type != ' ' && tilegrid[i][j - 1].type != ' ') tilegrid[i][j].next[2] = (tilegrid[i][j - 1], 0);
        if (j < grid[0].Length - 1 && tilegrid[i][j].type != ' ' && tilegrid[i][j + 1].type != ' ') tilegrid[i][j].next[0] = (tilegrid[i][j + 1], 0);
    }
}

#if !PART2
for (int i = 0; i < grid.Length; i++)
{
    var left = default(tile);
    var right = default(tile);
    for (int j = 0; j < grid[0].Length; j++)
    {
        if (left == default && tilegrid[i][j].type != ' ') left = tilegrid[i][j];
        if (tilegrid[i][j].type != ' ') right = tilegrid[i][j];        
    }
    left.next[2] = (right, 0);
    right.next[0] = (left, 0);
}

for (int j = 0; j < grid[0].Length; j++)
{
    var top = default(tile);
    var bottom = default(tile);

    for (int i = 0; i < grid.Length; i++)
    {
        if (top == default && tilegrid[i][j].type != ' ') top = tilegrid[i][j];
        if (tilegrid[i][j].type != ' ') bottom = tilegrid[i][j];
    }
    
    top.next[3] = (bottom, 0);
    bottom.next[1] = (top, 0);
}
#else
for (int i = 0; i < 50; i++)
{
    tilegrid[0][100 + i].next[UP] = (tilegrid[199][i], 0); // A top
    tilegrid[199][i].next[DOWN] = (tilegrid[0][100 + i], 0); // F bottom
    
    tilegrid[i][149].next[RIGHT] = (tilegrid[149 - i][99], 2); // A right
    tilegrid[149 - i][99].next[RIGHT] = (tilegrid[i][149], 2); // D right

    tilegrid[49][100 + i].next[DOWN] = (tilegrid[50 +  i][99], 1); // A bottom
    tilegrid[50 +  i][99].next[RIGHT] = (tilegrid[49][100 + i], -1); // C right
    
    tilegrid[0][50 + i].next[UP] = (tilegrid[150 + i][0], 1); // B top
    tilegrid[150 + i][0].next[LEFT] = (tilegrid[0][50 + i], -1); // F left
    
    tilegrid[i][50].next[LEFT] = (tilegrid[149 - i][0], 2); // B left
    tilegrid[149 - i][0].next[LEFT] = (tilegrid[i][50], 2); // E left
    
    tilegrid[50 + i][50].next[LEFT] = (tilegrid[100][i], -1); // C left
    tilegrid[100][i].next[UP] = (tilegrid[50 + i][50], 1); // E top
    
    tilegrid[149][50 + i].next[DOWN] = (tilegrid[150 + i][49], 1); // D bottom
    tilegrid[150 + i][49].next[RIGHT] = (tilegrid[149][50 + i], -1); // F right
}
#endif

var directions = lines.Last() + "S";

var steps = directions.Extract<List<string>>(@"(\d+[RLS])+").Select(x => (int.Parse(x[..^1]), x[^1]));

int dir = 0;
var loc = start;
foreach (var step in steps)
{
    for (int i = 0; i < step.Item1; i++)
    {
        if (loc.next[dir].tile.type == '.')
        {
            var tmploc = loc;
            loc = loc.next[dir].tile;
            dir = (dir + 4 + tmploc.next[dir].ddir) % 4;
        }
    }
    dir = (dir + 4 + step.Item2 switch {
        'L' => -1, 'R' => 1, 'S' => 0
    }) % 4;
}

var (y, x) = (loc.coords.x + 1, loc.coords.y + 1);

(1000 * y + 4 * x + dir).Dump(
#if !PART2
"Part 1"
#else
"Part 2"
#endif
);

record tile (char type, (int x, int y) coords, (tile tile, int ddir)[] next);
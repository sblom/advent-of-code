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

#define TEST

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"##########
\#..O..O.O#
\#......O.#
\#.OO..O.O#
\#..O@..O.#
\#O#..O...#
\#O..O..O.#
\#.OO.O.OO#
\#....O...#
\##########

<vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^
vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v
><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<
<<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^
^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><
^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^
>^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^
<><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>
^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>
v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^".Replace("\\","").GetLines();
#endif

var parts = lines.GroupLines();

var grid = parts.First().Select(x => x.ToCharArray()).ToArray();
var moves = parts.Last();

var row = grid.Index().Where(x => x.Item.Contains('@')).Single().Index;
var col = grid[row].Index().Where(x => x.Item == '@').Single().Index;

foreach (var line in moves)
{
    foreach (var move in line)
    {
        int n, x, y;
        var (dy,dx) = GetDir(move);
        bool freespace = false;
        for (n = 0, (y,x) = (row + n * dy, col + n * dx); grid[y][x] != '#'; n++, (y,x) = (row + n * dy, col + n * dx))
        {
            if (grid[y][x] == '.')
            {
                freespace = true;
                break;
            }
        }
        if (freespace)
        {
            for ((y,x) = (row + n * dy, col + n * dx); n > 0; n--, (y,x) = (row + n * dy, col + n * dx))
            {
                grid[y][x] = grid[y - dy][x - dx];
            }
            grid[y][x] = '.';
            (row,col) = (row + dy, col + dx);
        }
    }
}

int c = 0;

for (int i = 0; i < grid.Length; i++)
{
    for (int j = 0; j < grid[0].Length; j++)
    {
        if (grid[i][j] == 'O')
        {
            c += i * 100 + j;
        }
    }
}

c.Dump("Part 1");

//grid.Select(x => string.Join("", x)).DumpFixed();

grid = parts.First().Select(x => x.ToCharArray()).ToArray();
var widegrid = grid.Select(x => string.Join("", x.Select(y => y switch { '#' => "##", 'O' => "[]", '.' => "..", '@' => "@." })).ToCharArray()).ToArray();

row = widegrid.Index().Where(x => x.Item.Contains('@')).Single().Index;
col = widegrid[row].Index().Where(x => x.Item == '@').Single().Index;

foreach (var line in moves)
{
    foreach (var move in line)
    {
        int n, x, y;
        var (dy, dx) = GetDir(move);
        
        if (CanMove(row, col, dy, dx))
        {
            Move(row, col, dy, dx);
            (row, col) = (row + dy, col + dx);
        }
    }
}

widegrid.Select(x => string.Join("", x)).DumpFixed();

c = 0;

for (int i = 0; i < widegrid.Length; i++)
{
    for (int j = 0; j < widegrid[0].Length; j++)
    {
        if (widegrid[i][j] == '[')
        {
            c += i * 100 + j;
        }
    }
}

c.Dump("Part 2");

void Move(int row, int col, int dy, int dx)
{
    if (widegrid[row][col] == '.') return;
    if (widegrid[row][col] == '#') throw new Exception();

    if (widegrid[row][col] == '@')
    {
        Move(row + dy, col + dx, dy, dx);
        widegrid[row + dy][col + dx] = '@';
        widegrid[row][col] = '.';
        return;
    }    
    if (dy == 0  && widegrid[row][col] is '[' or ']')
    {
        Move(row + dy, col + dx, dy, dx);
        widegrid[row + dy][col + dx] = widegrid[row][col];
        widegrid[row][col] = '.';
        return;
    }
    if (dx == 0 && widegrid[row][col] is '[')
    {        
        Move(row + dy, col, dy, dx);
        Move(row + dy, col + 1, dy, dx);
        widegrid[row + dy][col] = widegrid[row][col];
        widegrid[row + dy][col + 1] = widegrid[row][col + 1];
        widegrid[row][col] = '.';
        widegrid[row][col + 1] = '.';
        return;
    }
    if (dx == 0 && widegrid[row][col] is ']')
    {
        Move(row + dy, col, dy, dx);
        Move(row + dy, col - 1, dy, dx);
        widegrid[row + dy][col] = widegrid[row][col];
        widegrid[row + dy][col - 1] = widegrid[row][col - 1];
        widegrid[row][col] = '.';
        widegrid[row][col - 1] = '.';
        return;
    }
    
    throw new Exception();
}

bool CanMove(int row, int col, int dy, int dx)
{
    if (widegrid[row][col] == '.') return true;
    if (widegrid[row][col] == '#') return false;
    if (widegrid[row][col] == '@') return CanMove(row + dy, col + dx, dy, dx);
    // horizontal
    if (dy == 0 && widegrid[row][col] is '[' or ']')
    {
        return CanMove(row, col + 2 * dx, dy, dx);
    }
    if (dx == 0 && widegrid[row][col] is '[')
    {
        return CanMove(row + dy, col, dy, dx) && CanMove(row + dy, col + 1, dy, dx);
    }
    if (dx == 0 && widegrid[row][col] is ']')
    {
        return CanMove(row + dy, col, dy, dx) && CanMove(row + dy, col - 1, dy, dx);
    }
    else throw new Exception();
}

(int,int) GetDir(char dir) =>
    dir switch
    {
        '^' => (-1,0),
        '>' => (0,1),
        '<' => (0,-1),
        'v' => (1,0)
    };
    
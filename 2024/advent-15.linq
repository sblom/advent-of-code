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

var dc = new DumpContainer().DumpFixed();

var parts = lines.GroupLines();

var grid = parts.First().Select(x => x.ToCharArray()).ToArray();
var moves = parts.Last();

var row = grid.Index().Where(x => x.Item.Contains('@')).Single().Index;
var col = grid[row].Index().Where(x => x.Item == '@').Single().Index;

#if TRACE
dc.Content = grid.Select(x => string.Join("", x));
#endif

foreach (var line in moves)
{
    foreach (var move in line)
    {
        int n, x, y;
        var (dy, dx) = GetDir(move);

        if (CanMove(grid, row, col, dy, dx))
        {
            Move(grid, row, col, dy, dx);
            (row, col) = (row + dy, col + dx);
#if TRACE            
            dc.Content = grid.Select(x => string.Join("", x));
#endif
        }
    }
}

GetGpsSum(grid).Dump("Part 1");

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
        
        if (CanMove(widegrid, row, col, dy, dx))
        {
            Move(widegrid, row, col, dy, dx);
            (row, col) = (row + dy, col + dx);
#if TRACE            
            dc.Content = widegrid.Select(x => string.Join("", x));
#endif
        }
    }
}

GetGpsSum(widegrid).Dump("Part 2");

dc.ClearContent();

static int GetGpsSum(char[][] grid)
{
    int c = 0;
    for (int i = 0; i < grid.Length; i++)
    {
        for (int j = 0; j < grid[0].Length; j++)
        {
            if (grid[i][j] is '[' or 'O')
            {
                c += i * 100 + j;
            }
        }
    }
    return c;
}

static void Move(char[][] grid, int row, int col, int dy, int dx)
{
    if (grid[row][col] == '.') return;
    if (grid[row][col] == '#') throw new Exception();

    if (grid[row][col] == '@')
    {
        Move(grid, row + dy, col + dx, dy, dx);
        grid[row + dy][col + dx] = '@';
        grid[row][col] = '.';
        return;
    }
    if (grid[row][col] == 'O')
    {
        Move(grid, row + dy, col + dx, dy, dx);
        grid[row + dy][col + dx] = 'O';
        grid[row][col] = '.';
        return;
    }
    if (dy == 0  && grid[row][col] is '[' or ']')
    {
        Move(grid, row + dy, col + dx, dy, dx);
        grid[row + dy][col + dx] = grid[row][col];
        grid[row][col] = '.';
        return;
    }
    if (dx == 0 && grid[row][col] is '[')
    {        
        Move(grid, row + dy, col, dy, dx);
        Move(grid, row + dy, col + 1, dy, dx);
        grid[row + dy][col] = grid[row][col];
        grid[row + dy][col + 1] = grid[row][col + 1];
        grid[row][col] = '.';
        grid[row][col + 1] = '.';
        return;
    }
    if (dx == 0 && grid[row][col] is ']')
    {
        Move(grid, row + dy, col, dy, dx);
        Move(grid, row + dy, col - 1, dy, dx);
        grid[row + dy][col] = grid[row][col];
        grid[row + dy][col - 1] = grid[row][col - 1];
        grid[row][col] = '.';
        grid[row][col - 1] = '.';
        return;
    }
    
    throw new Exception();
}

static bool CanMove(char[][] grid, int row, int col, int dy, int dx)
{
    if (grid[row][col] == '.') return true;
    if (grid[row][col] == '#') return false;
    if (grid[row][col] == '@') return CanMove(grid, row + dy, col + dx, dy, dx);
    if (grid[row][col] == 'O')
    {
        return CanMove(grid, row + dy, col + dx, dy, dx);
    }
    // horizontal
    if (dy == 0 && grid[row][col] is '[' or ']')
    {
        return CanMove(grid, row, col + 2 * dx, dy, dx);
    }
    if (dx == 0 && grid[row][col] is '[')
    {
        return CanMove(grid, row + dy, col, dy, dx) && CanMove(grid, row + dy, col + 1, dy, dx);
    }
    if (dx == 0 && grid[row][col] is ']')
    {
        return CanMove(grid, row + dy, col, dy, dx) && CanMove(grid, row + dy, col - 1, dy, dx);
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
    
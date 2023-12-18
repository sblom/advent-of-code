<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference Prerelease="true">RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

#region Preamble

#load "..\Lib\Utils"
#load "..\Lib\BFS"

//#define TEST
#define CHECKED

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @".|...\....
|.-.\.....
.....|-...
........|.
..........
.........\
..../.\\..
.-.-/..|..
.|....-|.\
..//.|....".GetLines();
#endif

#if CHECKED
checked{
#endif

AoC._outputs = new DumpContainer[] {new(), new()};
Util.HorizontalRun("Part 1,Part 2",AoC._outputs).Dump();

#endregion

var grid = lines.Select(line => line.ToCharArray()).ToArray();

    var bfs = new BFS<(int x,int y,int dx,int dy)>((x: 0, y: 0, dx: 1, dy: 0),Next,(x) => false,(x) => true);
    var locs = bfs.Search().Select(d => (d.x, d.y)).Where(d => d.x >= 0 && d.x < grid[0].Length && d.y >= 0 && d.y < grid.Length).ToHashSet();
    
    locs.Count().Dump1();

IEnumerable<(int, int, int, int)> Next((int x, int y, int dx, int dy) dir)
{
    var (x, y, dx, dy) = dir;
    if (x < 0 || x >= grid[0].Length || y < 0 || y >= grid.Length) yield break;

    switch (grid[y][x])
    {
        case '/':
            (dx, dy) = (-dy, -dx); yield return (x + dx, y + dy, dx, dy);
            break;
        case '\\':
            (dx, dy) = (dy, dx); yield return (x + dx, y + dy, dx, dy);
            break;
        case '|':
            if (dx != 0)
            {
                yield return (x, y - 1, 0, -1);
                yield return (x, y + 1, 0, 1);
            }
            else
            {
                yield return (x + dx, y + dy, dx, dy);
            }
            break;
        case '-':
            if (dy != 0)
            {
                yield return (x - 1, y, -1, 0);
                yield return (x + 1, y, 1, 0);
            }
            else
            {
                yield return (x + dx, y + dy, dx, dy);
            }
            break;
        default: yield return (x + dx,y + dy,dx,dy);
            break;
    };
}

var energized = new List<int>();

for (int i = 0; i < grid.Length; i++)
{
    bfs = new BFS<(int x, int y, int dx, int dy)>((x: 0, y: i, dx: 1, dy: 0), Next, (x) => false, (x) => true);
    energized.Add(bfs.Search().Select(d => (d.x, d.y)).Where(d => d.x >= 0 && d.x < grid[0].Length && d.y >= 0 && d.y < grid.Length).ToHashSet().Count());

    bfs = new BFS<(int x, int y, int dx, int dy)>((x: grid[0].Length - 1, y: i, dx: -1, dy: 0), Next, (x) => false, (x) => true);
    energized.Add(bfs.Search().Select(d => (d.x, d.y)).Where(d => d.x >= 0 && d.x < grid[0].Length && d.y >= 0 && d.y < grid.Length).ToHashSet().Count());
}

for (int i = 0; i < grid[0].Length; i++)
{
    bfs = new BFS<(int x, int y, int dx, int dy)>((x: i, y: 0, dx: 0, dy: 1), Next, (x) => false, (x) => true);
    energized.Add(bfs.Search().Select(d => (d.x, d.y)).Where(d => d.x >= 0 && d.x < grid[0].Length && d.y >= 0 && d.y < grid.Length).ToHashSet().Count());

    bfs = new BFS<(int x, int y, int dx, int dy)>((x: i, y: grid.Length - 1, dx: 0, dy: -1), Next, (x) => false, (x) => true);
    energized.Add(bfs.Search().Select(d => (d.x, d.y)).Where(d => d.x >= 0 && d.x < grid[0].Length && d.y >= 0 && d.y < grid.Length).ToHashSet().Count());
}

//energized.Dump();

energized.Max().Dump2();

#if CHECKED
}
#endif

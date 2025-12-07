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
var lines = @"###############
\#.......#....E#
\#.#.###.#.###.#
\#.....#.#...#.#
\#.###.#####.#.#
\#.#.#.......#.#
\#.#.#####.###.#
\#...........#.#
\###.#.#####.#.#
\#...#.....#.#.#
\#.#.#.###.#.#.#
\#.....#...#.#.#
\#.###.#.#.#.#.#
\#S..#.....#...#
\###############".Replace("\\","").GetLines();
#endif

var grid = lines.Select(x => x.ToCharArray()).ToArray();

var startRow = grid.Index().Where(x => x.Item.Contains('S')).Single().Index;
var startCol = grid[startRow].Index().Where(x => x.Item == 'S').Single().Index;

var endRow = grid.Index().Where(x => x.Item.Contains('E')).Single().Index;
var endCol = grid[endRow].Index().Where(x => x.Item == 'E').Single().Index;

(endRow,endCol).Dump();

int minscore = 0;

var bfs = new BFS<(int y,int x,int dy,int dx,int score,ImmutableList<(int,int)> path)>((startRow,startCol,0,1,0,ImmutableList<(int,int)>.Empty.Add((startRow,startCol))),GetPossibleMoves,
s => minscore != 0 && s.score != minscore,
s => s.x == endCol && s.y == endRow && (minscore == 0 || s.score == minscore),
loc: s => (s.path,s.dy,s.dx), weight: s => s.score);

var set = new HashSet<(int,int)>();

foreach (var result in bfs.Search())
{
    minscore = result.score;
    foreach (var point in result.path)
    {
        set.Add(point);
    }
    result.Dump();
}

set.Count().Dump("Part 2");

IEnumerable<(int y, int x, int dy, int dx, int score,ImmutableList<(int,int)> path)> GetPossibleMoves((int y, int x, int dy, int dx, int score,ImmutableList<(int,int)> path) tuple)
{
    var (y,x,dy,dx,score,path) = tuple;
    
    if (grid[y + dy][x + dx] is '.' or 'E') yield return (y+dy, x+dx, dy, dx, score + 1,path.Add((y+dy,x+dx)));
    yield return (y, x, dx, dy, score + 1000,path);
    yield return (y, x, -dx, -dy, score + 1000,path);
}

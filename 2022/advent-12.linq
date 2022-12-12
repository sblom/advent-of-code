<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>RegExtract</NuGetReference>
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
var lines = @"".GetLines().ToArray();
#endif

var grid = lines.Select(x => x.ToArray()).ToArray();
grid[20].Select((x,y) => (x,y)).Dump();
var Sloc = (0,20);
var Eloc = (136,20);

int GetHeight (char ch)
{
    return ch switch {
        'S' => 0,
        'E' => 25,
        _ => ch - 'a'
    };
}

var dirs = new(int dx, int dy)[] { (-1, 0), (1, 0), (0, -1), (0, 1)};

var bfs = new BFS<(ImmutableStack<(int,int)>,(int,int))>((ImmutableStack<(int,int)>.Empty, Sloc), ((ImmutableStack<(int,int)> hist, (int x, int y) loc) args) =>
{
    var x = args.loc.x;
    var y = args.loc.y;
    var curheight = GetHeight(grid[y][x]);
    if (curheight == 25) args.Dump();
    var neighbors = from dir in dirs where x + dir.dx < grid[0].Length && x + dir.dx >= 0 && y + dir.dy < grid.Length && y + dir.dy >= 0 select (x: x + dir.dx, y: y + dir.dy);
    return from neighbor in neighbors where GetHeight(grid[neighbor.y][neighbor.x]) <= curheight + 1 select (args.hist.Push((x,y)), (neighbor.x, neighbor.y));
},
(args) => { 
//args.Item2.Dump();
if (args.Item2 == Eloc) { 
"Done".Dump();
args.Dump();
throw new Exception();
    }
    return args.Item2 == Eloc;
},
(args) => { return args.Item2 == Eloc;},
(args) => args.Item2,
null);

var search = bfs.Search();
search.Dump();
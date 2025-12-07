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
var lines = $@"Register A: {k}
Register B: 0
Register C: 0

Program: 2,4,1,3,7,5,4,0,1,3,0,3,5,5,3,0".Replace("\\", "").GetLines();
    //   2,4,1,4,7,5,4,1,1,4,5,5,0,3,3,0

#endif

var mem = new HashSet<(int,int)>();

var cells = lines.Extract<(int,int)>(@"(\d+),(\d+)").Dump();

var dirs = new (int x, int y)[] { (1, 0), (-1, 0), (0, 1), (0, -1) };


for (int i = 1025; ; i++)
{
    mem = cells.Take(i).ToHashSet();
    var bfs = new BFS<(int, int, int)>((0, 0, 0), GetNeighbors, x => x.Item1 == 70 && x.Item2 == 70, x => x.Item1 == 70 && x.Item2 == 70, loc: x => (x.Item1, x.Item2));
    if (bfs.Search().Count() == 0)
    {
        cells.Take(i).Last().Dump();
        return;
    }
}

IEnumerable<(int,int,int)> GetNeighbors((int x,int y,int n) loc)
{
    foreach (var dir in dirs)
    {
        var (x, y) = (loc.x + dir.x, loc.y + dir.y);
        if (x >= 0 && x <= 70 && y >= 0 && y <= 70)
        {
            if (!mem.Contains((x, y))) {
                yield return (x, y, loc.n+1);
            }
        }
    }
}

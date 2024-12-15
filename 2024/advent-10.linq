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
var lines = await AoC.GetLinesWeb();
#else
var lines = @"2333133121414131402".GetLines();
#endif

var grid = lines.Select(x => x.Select(y => y - '0').ToArray()).ToArray();

var griddict = new Dictionary<(int,int),int>();

for (int i = 0; i < grid.Length; i++)
{
    for (int j = 0; j < grid[0].Length; j++)
    {
        griddict[(j,i)] = grid[i][j];
    }
}

var directions = new[] { (0, 1), (1, 0), (0, -1), (-1, 0) };

int t = 0;

foreach (var (k,v) in griddict)
{
    if (v != 0) continue;
    
    var BFS = new BFS<(int,int)>(k, GetNeighbors, p => false, p => griddict[p] == 9);
    
    t += BFS.Search().Count();
}

t.Dump();

t = 0;

foreach (var (k, v) in griddict)
{
    if (v != 0) continue;

    var BFS = new BFS<ImmutableList<(int,int)>>(ImmutableList<(int,int)>.Empty.Add((k)), GetTrailNeighbors, p => false, p => griddict[p.Last()] == 9);

    t += BFS.Search().Count();
}

t.Dump();

IEnumerable<(int,int)> GetNeighbors((int,int) p)
{    
    var (x,y) = p;

    foreach (var (dx, dy) in directions)
    {
        var (nx, ny) = (x + dx, y + dy);
        if (griddict.ContainsKey((nx,ny)) && griddict[(nx,ny)] == griddict[(x,y)] + 1) yield return (nx,ny);
    }
}

IEnumerable<ImmutableList<(int,int)>> GetTrailNeighbors(ImmutableList<(int,int)> p)
{
    var (x, y) = p.Last();

    foreach (var (dx, dy) in directions)
    {
        var (nx, ny) = (x + dx, y + dy);
        if (griddict.ContainsKey((nx, ny)) && griddict[(nx, ny)] == griddict[(x, y)] + 1) yield return p.Add((nx, ny));
    }
}
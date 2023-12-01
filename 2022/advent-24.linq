<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <NuGetReference>RegExtract</NuGetReference>
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
var lines = @"#.######
\#>>.<^<#
\#.<..<<#
\#>v.><>#
\#<^v^^>#
\######.#".Replace("\t", "    ").Replace("\r", "").Replace("\\","").Split("\n");
#endif

var grid = lines.Select(x => x.ToArray()).ToArray();

HashSet<int>[] left = Enumerable.Range(0,grid.Length).Select(x => new HashSet<int>()).ToArray();
HashSet<int>[] right = Enumerable.Range(0,grid.Length).Select(x => new HashSet<int>()).ToArray();
HashSet<int>[] down = Enumerable.Range(0,grid[0].Length).Select(x => new HashSet<int>()).ToArray();
HashSet<int>[] up = Enumerable.Range(0,grid[0].Length).Select(x => new HashSet<int>()).ToArray();

for (int i = 1; i < grid.Length - 1; i++)
{    
    for (int j = 1; j < grid[0].Length - 1; j++)
    {
        switch (grid[i][j]) {
            case '<': left[i].Add(j); break;
            case '>': right[i].Add(j); break;
            case '^': up[j].Add(i); break;
            case 'v': down[j].Add(i); break;
        }
    }
}

var dirs = new (int dx, int dy)[] {(-1,0),(1,0),(0,-1),(0,1),(0,0)};

IEnumerable<(int, int, int)> Neighbors((int t, int x, int y) loc)
{
    var t = loc.t + 1;
    foreach (var dir in dirs)
    {
        (int x, int y) = (loc.x + dir.dx, loc.y + dir.dy);
        if ((x == grid[0].Length - 2 && y == grid.Length - 1) || (x == 1 && y == 0)) {
            yield return (t, x, y); yield break;
        }
        if (x <= 0 || x >= grid[0].Length - 1 || y <= 0 || y >= grid.Length - 1) continue;
        if (right[y].Contains(((x - (t % (grid[0].Length - 2)) + (grid[0].Length - 2) - 1) % (grid[0].Length - 2) + 1))) continue;
        if (left[y].Contains((x + t - 1) % (grid[0].Length - 2) + 1)) continue;
        if (down[x].Contains(((y - (t % (grid.Length - 2)) + (grid.Length - 2) - 1) % (grid.Length - 2) + 1))) continue;
        if (up[x].Contains((y + t - 1) % (grid.Length - 2) + 1)) continue;
        
        yield return (t, x, y);
    }
}

var bfs = new BFS<(int t, int x, int y)>(
    (0,1,0), Neighbors, ((int t, int x, int y) loc) => (loc.x, loc.y) == (grid[0].Length - 2, grid.Length - 1), ((int t, int x, int y) loc) => (loc.x, loc.y) == (grid[0].Length - 2, grid.Length - 1), null, ((int t, int x, int y) loc) => loc.t
);

var there = bfs.Search().First().Dump().t;

var bfsback = new BFS<(int t, int x, int y)>(
    (there, grid[0].Length - 2, grid.Length - 1), Neighbors, ((int t, int x, int y) loc) => (loc.x, loc.y) == (1,0), ((int t, int x, int y) loc) => (loc.x, loc.y) == (1,0), null, ((int t, int x, int y) loc) => loc.t
);

var back = bfsback.Search().First().Dump().t;

var bfsagain = new BFS<(int t, int x, int y)>(
    (back,1,0), Neighbors, ((int t, int x, int y) loc) => (loc.x, loc.y) == (grid[0].Length - 2, grid.Length - 1), ((int t, int x, int y) loc) => (loc.x, loc.y) == (grid[0].Length - 2, grid.Length - 1), null, ((int t, int x, int y) loc) => loc.t
);

var again = bfsagain.Search().First().Dump().t;

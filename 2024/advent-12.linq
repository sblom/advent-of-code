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
var lines = @"RRRRIICCFF
RRRRIICCCF
VVRRRCCFFF
VVRCCCJFFF
VVVVCJJCFE
VVIVCCJJEE
VVIIICJJEE
MIIIIIJJEE
MIIISIJEEE
MMMISSJEEE".GetLines();
#endif

var gridText = lines.Select(x => x.ToCharArray()).ToArray();
var gridDict = new Dictionary<(int,int),char>();
var regions = new List<(int,int)>();
var dirs = new (int, int)[] { (0, 1), (1, 0), (0, -1), (-1, 0) };

for (int i = 0; i < gridText.Length; i++)
{
    for (int j = 0; j < gridText[0].Length; j++)
    {
        gridDict[(j,i)] = gridText[i][j];
    }
}

long t = 0, t2 = 0;

while (gridDict.Any())
{
    var (start,ch) = gridDict.First();
    var frontier = new HashSet<(int,int)>();
    var visited = new HashSet<(int,int)>();
    var perimeter = 0;
    var area = 0;
    var sections = 0;
    frontier.Add(start);
    
    while (frontier.Any())
    {        
        var next = frontier.First();
        if (visited.Contains(next)) continue;
        frontier.Remove(next);
        visited.Add(next);
        var neighbors = GetNeighbors(next);
        area += 1;
        perimeter += 4 - neighbors.Count();
        foreach (var n in neighbors) if (!visited.Contains(n)) frontier.Add(n);
    }

// Tricky cases tonight: Since I'm looking between rows and columns, having it switch from bottom to top or vice versa requires an extra state.

    for (int i2 = visited.Min(x => x.Item1) - 1; i2 <= visited.Max(x => x.Item1); i2++)
    {
        bool top = false;
        bool bottom = false;
        for (int j2 = visited.Min(x => x.Item2); j2 <= visited.Max(x => x.Item2); j2++)
        {            
            var cur = i2 != visited.Min(x => x.Item1) - 1 && visited.Contains((i2,j2));
            var next = i2 != visited.Max(x => x.Item1) && visited.Contains((i2 + 1, j2));
            if (cur == next) { top = false; bottom = false; }
            else if (!top && !cur && next) { top = true; bottom = false; sections++; }
            else if (!bottom && cur && !next) { top = false; bottom = true; sections++; }
        }
    }

    for (int j3 = visited.Min(x => x.Item2) - 1; j3 <= visited.Max(x => x.Item2); j3++)
    {
        bool top = false;
        bool bottom = false;
        for (int i3 = visited.Min(x => x.Item1); i3 <= visited.Max(x => x.Item1); i3++)
        {
            var cur = j3 != visited.Min(x => x.Item2) - 1 && visited.Contains((i3, j3));
            var next = j3 != visited.Max(x => x.Item2) && visited.Contains((i3, j3 + 1));
            if (cur == next) { top = false; bottom = false; }
            else if (!top && !cur && next) { top = true; bottom = false; sections++; }
            else if (!bottom && cur && !next) { top = false; bottom = true; sections++; }
        }
    }

    foreach (var v in visited) gridDict.Remove(v);
    t += area * perimeter;
    t2 += area * sections;
}

t.Dump();
t2.Dump();

IEnumerable<(int, int)> GetNeighbors((int, int) next)
{
    foreach (var (dx, dy) in dirs)
    {
        var neighbor = (next.Item1 + dx, next.Item2 + dy);
        if (gridDict.ContainsKey(neighbor) && gridDict[neighbor] == gridDict[next]) yield return neighbor;
    }
}

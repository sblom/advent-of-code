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
var lines = @"............
........0...
.....0......
.......0....
....0.......
......A.....
............
............
........A...
.........A..
............
............".GetLines();
#endif

var grid = lines.Select(x => x.ToCharArray()).ToArray();

Dictionary<char, List<(int x, int y)>> antennas = new();

for (int y = 0; y < grid.Length; y++)
{
    for (int x = 0; x < grid[0].Length; x++)
    {
        if (grid[y][x] != '.')
        {
            if (!antennas.ContainsKey(grid[y][x]))
            {
                antennas[grid[y][x]] = new();                
            }
            
            antennas[grid[y][x]].Add((x,y));
        }
    }
}

var antinodes = new HashSet<(int x,int y)>();

foreach (var (_,v) in antennas)
{
    foreach (var a in v)
    {
        foreach (var b in v)
        {
            if (b == a) continue;
            
            var (dx, dy) = (a.x - b.x, a.y - b.y);
            antinodes.Add((a.x + dx, a.y + dy));
            antinodes.Add((b.x - dx, b.y - dy));
        }
    }
}

antinodes.Where(z => z.x >= 0 && z.x < grid[0].Length && z.y >= 0 && z.y < grid.Length).Count().Dump();

antinodes.Clear();

foreach (var (_, v) in antennas)
{
    foreach (var a in v)
    {
        foreach (var b in v)
        {
            if (b == a) continue;

            var (dx, dy) = (a.x - b.x, a.y - b.y);
            var g = gcd(dx, dy);
            (dx, dy) = (dx / g, dy / g);
            
            bool updated = true;
            for (int n = 0; updated; n++)
            {
                updated = false;
                var (x1, y1) = (a.x + n * dx, a.y + n * dy);
                var (x2, y2) = (a.x - n * dx, a.y - n * dy);
                
                if (x1 >= 0 && x1 < grid[0].Length && y1 >= 0 && y1 < grid.Length)
                {
                    antinodes.Add((x1,y1));
                    updated = true;
                }
                if (x2 >= 0 && x2 < grid[0].Length && y2 >= 0 && y2 < grid.Length)
                {
                    antinodes.Add((x2, y2));
                    updated = true;
                }
            }
        }
    }
}

antinodes.Count().Dump();

int gcd(int a, int b)
{
    while (b != 0)
    {
        (a, b) = (b, a % b);
    }
    return Abs(a);
}
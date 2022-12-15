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

#if !TEST
var lines = (await AoC.GetLinesWeb()).ToArray();
#else
var lines = @"498,4 -> 498,6 -> 496,6
503,4 -> 502,4 -> 502,9 -> 494,9".GetLines().ToArray();
#endif

HashSet<(int x,int y)> grid = new();

foreach (var line in lines)
{
    var coords = line.Split(" -> ").Select(x => x.Extract<(int,int)>(@"(\d+),(\d+)")).ToArray();
    for (int i = 0; i < coords.Length - 1; i++)
    {
        switch ((coords[i], coords[i+1]))
        {
            case var ((x1,y1),(x2,y2)) when x1 == x2:
                int y = 0;
                for (y = y1; y != y2; y += Math.Sign(y2 - y1))
                {
                    grid.Add((x1,y));
                }
                grid.Add((x1,y));
                
                break;
            case var ((x1, y1), (x2, y2)) when y1 == y2:
                int x = 0;
                for (x = x1; x != x2; x += Math.Sign(x2 - x1))
                {
                    grid.Add((x,y1));
                }
                grid.Add((x,y1));

                break;
        }
    }
}

grid.Dump();

var bottom = grid.Max(x => x.Item2);
var floor = bottom + 2;
bool bottomDumped = false;

var loc = (x: 500, y: 0);

int sand = 0;

while (true)
{
    if (loc.y + 1 == floor)
    {
        sand++; grid.Add(loc); loc = (500,0);
    }
    else if (!grid.Contains((loc.x, loc.y + 1)))
    {
        loc = (loc.x, loc.y + 1);
    }
    else if (!grid.Contains((loc.x - 1, loc.y + 1)))
    {
        loc = (loc.x - 1, loc.y + 1);
    }
    else if (!grid.Contains((loc.x + 1, loc.y + 1)))
    {
        loc = (loc.x + 1, loc.y + 1);
    }
    else
    {
        sand++;
        grid.Add(loc);
        if (loc == (500,0)) { sand.Dump("Part 2"); break; }
        loc = (500,0);
    }
    
    if (!bottomDumped && loc.y > bottom) { bottomDumped = true; sand.Dump("Part 1"); }
}


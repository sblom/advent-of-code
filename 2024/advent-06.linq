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
var lines = @"".GetLines();
#endif

var grid = lines.Select(x => x.ToArray()).ToArray();

var guardlinepair = grid.Select((x,i) => (x,i)).Where(x => x.x.Contains('^')).First();
var guardline = guardlinepair.i;
var guardcol = guardlinepair.x.Select((x,i) => (x,i)).Where(x => x.x == '^').First().i;

grid[guardline][guardcol].Dump();

var y = guardline;
var x = guardcol;
var (dx, dy) = (0,-1);

var locs = new HashSet<(int,int)>();
var turns = new HashSet<(int,int)>();

while (x >=0 && x < grid[0].Length && y >= 0 && y < grid.Length)
{
    locs.Add((x,y));
    if (x + dx < 0 || x + dx >= grid[0].Length || y + dy < 0 || y + dy >= grid.Length)
        break;
    
    if (grid[y + dy][x + dx] == '#')
    {
        turns.Add((x,y));
        (dx, dy) = (-dy, dx);
    }
    else
    {
        (x, y) = (x + dx, y + dy);
    }
}

int c = 0;

foreach (var loc in locs)
{
    var states = new HashSet<(int, int, int, int)>();
    y = guardline;
    x = guardcol;
    (dx, dy) = (0, -1);

    while (x >= 0 && x < grid[0].Length && y >= 0 && y < grid.Length)
    {
        var state = (x, y, dx, dy);
        
        if (states.Contains(state))
        {
            c++;
            break;
        }
        
        states.Add(state);
        if (x + dx < 0 || x + dx >= grid[0].Length || y + dy < 0 || y + dy >= grid.Length)
            break;

        if (grid[y + dy][x + dx] == '#' || (y + dy == loc.Item2 && x + dx == loc.Item1))
        {
            (dx, dy) = (-dy, dx);
        }
        else
        {
            (x, y) = (x + dx, y + dy);
        }
    }
}

locs.Count.Dump();
c.Dump();
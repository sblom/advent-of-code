<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>RawScape.Wintellect.PowerCollections</NuGetReference>
  <NuGetReference Prerelease="true">RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>System.Collections.ObjectModel</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>Wintellect.PowerCollections</Namespace>
</Query>

#region Preamble

#load "..\Lib\Utils"
//#load "..\Lib\BFS"

//#define TEST
//#define CHECKED
//#define TRACE

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"#.#####################
\#.......#########...###
\#######.#########.#.###
\###.....#.>.>.###.#.###
\###v#####.#v#.###.#.###
\###.>...#.#.#.....#...#
\###v###.#.#.#########.#
\###...#.#.#.......#...#
\#####.#.#.#######.#.###
\#.....#.#.#.......#...#
\#.#####.#.#.#########v#
\#.#...#...#...###...>.#
\#.#.#v#######v###.###v#
\#...#.>.#...>.>.#.###.#
\#####v#.#.###v#.#.###.#
\#.....#...#...#.#.#...#
\#.#########.###.#.#.###
\#...###...#...#...#.###
\###.###.#.###v#####v###
\#...#...#.#.>.>.#.>.###
\#.###.###.#.###.#.#v###
\#.....###...###...#...#
\#####################.#".GetLines();
#endif

#if CHECKED
checked{
#endif

AoC._outputs = new DumpContainer[] { new(), new() };
Util.HorizontalRun("Part 1,Part 2", AoC._outputs).Dump();

#endregion

var grid = lines.Select(x => x.ToCharArray()).ToArray();
//lines.DumpFixed();

List<((int dr, int dc) dir, char gate)> dirs = [((0,1),'>'), ((0,-1),'<'), ((1,0),'v'), ((-1,0),'^')];

Queue<((int,int),(int,int))> frontier = new();
frontier.Enqueue(((0,1), (1,0)));
List<((int r0, int c0) start, (int r1, int c1) end, int len)> segments = new();

while (frontier.TryDequeue(out ((int,int) loc, (int,int) dir) c))
{
    var segment = Walk(c.loc, c.dir);
    segments.Add((c.loc, segment.end, segment.len));
    if (segment.end.r != grid.Length - 1)    
        foreach (var (direction, gate) in dirs)
        {
            if (grid[segment.end.r + direction.dr][segment.end.c + direction.dc] == gate)
            {
                frontier.Enqueue((segment.end, direction));
            }
        }
}

((int r, int c) end, int len) Walk((int r, int c) loc, (int dr, int dc) dir)
{
    int len = 0;
    var (dr, dc) = dir;
    var (r, c) = loc;

    (r, c) = (r + dr, c + dc);
    len++;

    do
    {
        if (r == grid.Length - 1)
        {
            return ((r, c), len);
        }
        
        foreach (var (direction, gate) in dirs)
        {
            if ((direction.dr, direction.dc) == (-dr, -dc))
                continue;
            if (grid[r + direction.dr][c + direction.dc] == gate && len > 2)
            {
                return ((r + 2 * direction.dr, c + 2 * direction.dc), len + 2);
            }
            if (grid[r + direction.dr][c + direction.dc] == '.' || grid[r + direction.dr][c + direction.dc] == gate)
            {
                (r,c) = (r + direction.dr, c + direction.dc);
                (dr, dc) = direction;
                len++;
                break;
            }
        }
    } while (true);
}

Dictionary<(int, int), int> memo = new();

MaxLen((0, 1)).Dump1();


var distinct = segments.Distinct().ToList();
foreach (var d in distinct.ToList().Select(x => (start: (r0: x.end.r1, c0: x.end.c1), end: (r1: x.start.r0, c1: x.start.c0), x.len)))
{
    distinct.Add(d);
}

var lookup = distinct.ToLookup(x => x.start);

var visited = ImmutableHashSet<(int,int)>.Empty;

int max = 0;

void LongWalk((int,int) loc, ImmutableHashSet<(int,int)> visited, int len)
{
    if (loc == (140,139))
    {
        if (len > max)
        {
            max = len;
            (max, len).Dump();
        }
        return;
    }
    
    visited = visited.Add(loc);
    foreach (var next in lookup[loc].OrderByDescending(x => x.len))
    {
        if (visited.Contains(next.end)) continue;
        LongWalk(next.end,visited,len+next.len);
    }
}

LongWalk((0,1),ImmutableHashSet<(int,int)>.Empty,0);

max.Dump2();


int MaxLen((int,int) loc)
{
    if (memo.ContainsKey(loc)) return memo[loc];
    var (r,c) = loc;
    if (r == grid.Length - 1)
    {
        return 0;
    }
    else
    {
        int max = 0;
        foreach (var next in segments.Where(x => x.start == (r,c)))
        {
            var n = next.len + MaxLen(next.end);
            if (n > max) max = n;
        }
        
        memo[loc] = max;
        return max;
    }
}

#if CHECKED
}
#endif
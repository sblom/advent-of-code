<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>RawScape.Wintellect.PowerCollections</NuGetReference>
  <NuGetReference>RegExtract</NuGetReference>
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
var lines = @"1,0,1~1,2,1   <- A
0,0,2~2,0,2   <- B
0,2,3~2,2,3   <- C
0,0,4~0,2,4   <- D
2,0,5~2,2,5   <- E
0,1,6~2,1,6   <- F
1,1,8~1,1,9   <- G".GetLines();
#endif

#if CHECKED
checked{
#endif

AoC._outputs = new DumpContainer[] {new(), new()};
Util.HorizontalRun("Part 1,Part 2",AoC._outputs).Dump();

#endregion

var grid = lines.Select(x => x.ToCharArray()).ToArray();

List<((int dr, int dc) dir, char gate)> dirs = [((0,1),'>'), ((0,-1),'<'), ((1,0),'v'), ((-1,0),'^')];
var visited = new HashSet<(int,int)>();

var frontier = new Queue<(int, int)>();
frontier.Enqueue((0, 1));

Dictionary<(int,int),List<((int,int),int)>> edges = new();

while (frontier.TryDequeue(out (int r, int c) loc)){
    int len = 0;
    var (r,c) = loc;

    while (true)
    {
        var next = Next((r,c)).SingleOrDefault();
        if (next != (0,0))
        {
            if (r == grid.Length - 1)
            {
                edges[loc] = new();
                edges[loc].Add(((r, c), len));
                goto next_loc;
            }

            (r,c) = next;
            len++;
        }
        else
        {
            foreach (var ((dr,dc), gate) in dirs)
            {
                if (grid[r + dr][c + dc] == gate)
                {
                    (r,c) = (r + 2 * dr, c + 2 * dc);
                    len += 3;
                    break;
                }
            }
            
            if (!edges.ContainsKey(loc)) edges[loc] = new();

            foreach (var ((dr,dc), gate) in dirs)
            {
                if (grid[r + dr][c + dc] == gate)
                {
                    frontier.Enqueue((r + 2 * dr, c + 2 * dc));
                    edges[loc].Add(((r, c), len));
                }
            }
        }
    }
    next_loc:;
}

edges.Dump();

((int r, int c) end, int len) Walk((int r, int c) loc, (int dr, int dc) dir)
{
    int len = 0;
    var (dr, dc) = dir;
    var (r, c) = loc;

    do
    {
        foreach (var (direction, gate) in dirs)
        {
            if ((direction.dr, direction.dc) == (-dr, -dc))
                continue;
            if (grid[r + dr][c + dc] == gate)
            {
                return ((r + 2 * dr, c + 2 * dc), len + 2);
            }
            if (grid[r + dr][c + dc] == '.')
            {
                (r,c) = (r + dr, c + dc);
                (dr, dc) = direction;
                break;
            }
        }
    } while (true);
}

IEnumerable<(int r, int c)> Next((int r,int c) loc)
{
    var (r,c) = loc;
    visited.Add((r,c));
    foreach (var ((dr, dc), gate) in dirs)
    {
        if (r + dr < 0 || r + dr >= grid.Length || c + dc < 0 || c + dc >= grid[0].Length)
        {
            continue;
        }
        
        if (grid[r+dr][c+dc] == gate)
        {
            yield break;
        }
        if (grid[r+dr][c+dc] == '.' && !visited.Contains((r + dr, c + dc)))
        {
            yield return (r + dr, c + dc);
        }
    }    
}

#if CHECKED
}
#endif

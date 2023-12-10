<Query Kind="Statements">
  <NuGetReference>LinqToRanges</NuGetReference>
  <NuGetReference>RegExtract</NuGetReference>
  <Namespace>LinqToRanges</Namespace>
  <Namespace>RegExtract</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>

#region Preamble

#load "..\Lib\Utils"
#load "..\Lib\BFS"

//#define TEST
//#define CHECKED

#if !TEST
var lines = await AoC.GetLinesWeb();
#else
var lines = @"FF7FSF7F7F7F7F7F---7
L|LJ||||||||||||F--J
FL-7LJLJ||||||LJL-77
F--JF--7||LJLJIF7FJ-
L---JF-JLJIIIIFJLJJ7
|F|F-JF---7IIIL7L|7|
|FFJF7L7F-JF7IIL---7
7-L-JL7||F7|L7F-7F7|
L.L7LFJ|||||FJL7||LJ
L7JLJL-JLJLJL--JLJ.L".GetLines();
#endif

#if CHECKED
checked{
#endif

#endregion

var gr = lines.Select(x => x.ToCharArray()).ToArray();
var grid = new char[lines.Count(), lines.First().Length];
for (int i = 0; i < gr.Length; i++)
{
    for (int j = 0; j < gr[0].Length; j++)
    {
        grid[i,j] = gr[i][j];
    }
}

//grid.Dump();

var (x0,y0) = (0,0);

for (int i = 0; i < gr.Length; i++)
{
    for (int j = 0; j < gr[0].Length; j++)
    {
        if (grid[i, j] == 'S')
        {
            x0 = j;
            y0 = i;
        }
    }
}

var (x, y) = (x0,y0);
#if TEST
var (dx, dy) = (-1,0);
#else
var (dx, dy) = (0,-1);
#endif


List<(int x,int y)> path = new();
HashSet<(int x,int y)> pathhash = new();

do {
    path.Add((x, y));
    var dirs = GetDirs(grid[y + dy, x + dx]);
    (x, y) = (x + dx, y + dy);
    var nextdir = dirs.Where(x => x != (-dx, -dy)).Single();
    (dx, dy) = (nextdir.Item1, nextdir.Item2);
} while (grid[y,x] != 'S');

(path.Count()/2).Dump("Part 1");

pathhash = path.ToHashSet();

int inside = 0;

for (int i = 0; i < gr.Length; i++)
{
    for (int j = 0; j < gr[0].Length; j++)
    {
        if (!pathhash.Contains((j,i)))
        {
            int yi = i;
            int crossings = 0;
            while (yi > 0)
            {
                yi--;
                if (pathhash.Contains((j,yi)))
                {                    
                    crossings += grid[yi,j] switch
                    {
                        'F' or 'L' or '-' => 1,
                        _ => 0
                    };
                }
            }
            if (crossings % 2 == 1)
            {
                inside++;
            }            
        }
    }
}

inside.Dump("Part 2");

int A2 = 0;
for (int i = 0; i < pathhash.Count; i++)
{
    A2 += path[i].x * path[(i+1) % pathhash.Count].y - path[i].y * path[(i+1) % pathhash.Count].x;
}

(A2 / 2 - path.Count / 2 + 1).Dump("Pick's Theorem + Shoelace Formula");

(int, int)[] GetDirs(char ch)
{
    return ch switch
    {
        '|' => [(0, -1), (0, 1)],
        '-' => [(-1, 0), (1, 0)],
        'L' => [(0, -1), (1, 0)],
        'J' or 'S' => [(0, -1), (-1, 0)],
        '7' => [(0, 1), (-1, 0)],
        'F' => [(0, 1), (1, 0)],
        _ => []
    };
}

#if CHECKED
}
#endif
